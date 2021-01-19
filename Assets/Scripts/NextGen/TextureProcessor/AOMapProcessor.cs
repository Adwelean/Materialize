using System;
using System.Collections;
using System.IO;
using System.Threading;

using Assets.Scripts.NextGen.Extensions;
using Assets.Scripts.NextGen.ShaderWrapper;
using Assets.Scripts.NextGen.TextureSettings;

using Cysharp.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.NextGen.TextureProcessor
{    
    public class AOMapProcessor : TextureProcessorBase<AOMapSettingsWrapper>
    {

        public AOMapProcessor(AOMapSettingsWrapper textureSettings)
            : base(textureSettings)
        {
        }

        public int ImageSizeX { get; set; }
        public int ImageSizeY { get; set; }

        private RenderTexture CreateRenderTexture(int? width = default, int? height = default, int depth = 0, RenderTextureFormat renderTextureFormat = RenderTextureFormat.RGHalf)
            => new RenderTexture(width ?? ImageSizeX, height ?? ImageSizeY, depth, renderTextureFormat, RenderTextureReadWrite.Linear) { wrapMode = TextureWrapMode.Repeat };

        public override async UniTask<TextureContext> Process(TextureContext textureContext)
        {
            Texture2D diffuseMap = default;
            Texture2D normalMap = default;
            RenderTexture heightMapHD = default;

            if (textureContext.ProcessorPhase >= ProcessorPhase.AOMapProcessed)
                return textureContext;

            if (!textureContext.Textures.TryGetValueAs(TypeMap.Diffuse, out diffuseMap))
                throw new ArgumentNullException($"[Phase: {textureContext.ProcessorPhase}] Missing Diffuse texture!");

            if (!textureContext.Textures.TryGetValueAs(TypeMap.Normal, out normalMap))
                throw new ArgumentNullException($"[Phase: {textureContext.ProcessorPhase}] Missing NormalMap texture!");

            if (!textureContext.Textures.TryGetValueAs(TypeMap.HeightHD, out heightMapHD))
                throw new ArgumentNullException($"[Phase: {textureContext.ProcessorPhase}] Missing heightMapHD texture!");

            ImageSizeX = diffuseMap.width;
            ImageSizeY = diffuseMap.height;

            var blitShader = new BlitShaderWrapper();

            blitShader.ImageSize = new Vector4(ImageSizeX, ImageSizeY, 0, 0);

            var blendedAoMap = await CreateBlendedAOMapFromNormalDepth(blitShader, normalMap, heightMapHD);
            var aoMap = CreateAOMap(blendedAoMap);

            textureContext.Textures.Add(TypeMap.AO, aoMap);
            textureContext.ProcessorPhase = ProcessorPhase.AOMapProcessed;

            return textureContext;
        }

        public async UniTask<RenderTexture> CreateBlendedAOMapFromNormalDepth(BlitShaderWrapper blitShader, Texture2D normalMap, RenderTexture heightMap)
        {
            var blendedAoMap = CreateRenderTexture();
            var workingAoMap = CreateRenderTexture();

            blitShader.Spread = Settings.Spread;
            blitShader.MainTex = normalMap;
            blitShader.HeightTex = heightMap;
            blitShader.BlendTex = blendedAoMap;
            blitShader.Depth = Settings.Depth;

            var yieldCountDown = 5;

            for (var i = 1; i < 100; i++)
            {
                blitShader.BlendAmount = 1.0f / i;
                blitShader.Progress = i / 100.0f;

                Graphics.Blit(normalMap, workingAoMap, blitShader.Material, 7);
                Graphics.Blit(workingAoMap, blendedAoMap);

                yieldCountDown -= 1;

                if (yieldCountDown > 0) continue;

                yieldCountDown = 5;

                await UniTask.Delay(1);
                await UniTask.Yield(PlayerLoopTiming.Update);
            }           

            return blendedAoMap;
        }

        public Texture2D CreateAOMap(RenderTexture blendedAoMap)
        {
            var blitShader = new BlitAOShaderWrapper();
            var tempMap = CreateRenderTexture(ImageSizeX, ImageSizeY, renderTextureFormat: RenderTextureFormat.ARGB32);

            blitShader.FinalBias = Settings.FinalBias;
            blitShader.FinalContrast = Settings.FinalContrast;
            blitShader.MainTex = blendedAoMap;
            blitShader.AOBlend = Settings.Blend;

            Graphics.Blit(blendedAoMap, tempMap, blitShader.Material, 8);

            var aoMap = new Texture2D(tempMap.width, tempMap.height, TextureFormat.ARGB32, true, true);
            aoMap.ReadPixels(new Rect(0, 0, tempMap.width, tempMap.height), 0, 0);
            aoMap.Apply();

            return aoMap;
        }
    }
}

using System;

using Assets.Scripts.NextGen.Extensions;
using Assets.Scripts.NextGen.ShaderWrapper;
using Assets.Scripts.NextGen.TextureSettings;

using Cysharp.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.NextGen.TextureProcessor
{
    public class EdgeMapProcessor : TextureProcessorBase<EdgeMapSettingsWrapper>
    {
        public EdgeMapProcessor(EdgeMapSettingsWrapper textureSettings)
            : base(textureSettings)
        {
        }

        private int ImageSizeX { get; set; }
        private int ImageSizeY { get; set; }

        private RenderTexture CreateRenderTexture(int? width = default, int? height = default, int depth = 0, RenderTextureFormat renderTextureFormat = RenderTextureFormat.ARGB32)
            => new RenderTexture(width ?? ImageSizeX, height ?? ImageSizeY, depth, renderTextureFormat, RenderTextureReadWrite.Linear) { wrapMode = TextureWrapMode.Repeat };

        public override UniTask<TextureContext> Process(TextureContext textureContext)
        {
            Texture2D diffuseMap = default;
            Texture2D normalMap = default;

            if (textureContext.ProcessorPhase >= ProcessorPhase.EdgeMapProcessed)
                return UniTask.FromResult(textureContext);

            if (!textureContext.Textures.TryGetValueAs(TypeMap.Diffuse, out diffuseMap))
                throw new ArgumentNullException($"[Phase: {textureContext.ProcessorPhase}] Missing Diffuse texture!");

            if (!textureContext.Textures.TryGetValueAs(TypeMap.Metalic, out normalMap))
                throw new ArgumentNullException($"[Phase: {textureContext.ProcessorPhase}] Missing NormalMap texture!");

            ImageSizeX = diffuseMap.width;
            ImageSizeY = diffuseMap.height;
            
            var blurMap0 = CreateRenderTexture();
            var blurMap1 = CreateRenderTexture();
            var blurMap2 = CreateRenderTexture();
            var blurMap3 = CreateRenderTexture();
            var blurMap4 = CreateRenderTexture();
            var blurMap5 = CreateRenderTexture();
            var blurMap6 = CreateRenderTexture();

            ApplyBlurMapFromNormalMapToBlitMaterial(normalMap, blurMap0, blurMap1, blurMap2, blurMap3, blurMap4, blurMap5, blurMap6);

            var edgeMap = CreateEdgeMap(diffuseMap, normalMap, blurMap0, blurMap1, blurMap2, blurMap3, blurMap4, blurMap5, blurMap6);

            textureContext.Textures.Add(TypeMap.Edge, edgeMap);
            textureContext.ProcessorPhase = ProcessorPhase.EdgeMapProcessed;

            return UniTask.FromResult(textureContext);
        }

        public void ApplyBlurMapFromNormalMapToBlitMaterial(in Texture2D normalMap, RenderTexture blurMap0, RenderTexture blurMap1, RenderTexture blurMap2, RenderTexture blurMap3, RenderTexture blurMap4, RenderTexture blurMap5, RenderTexture blurMap6)
        {
            var tempMap = CreateRenderTexture();

            var blitShader = new BlitShaderWrapper { ImageSize = new Vector4(ImageSizeX, ImageSizeY, 0, 0) };

            blitShader.BlurContrast = Settings.Blur0Contrast;
            blitShader.MainTex = normalMap;

            // Apply to diffuse map
            Graphics.Blit(normalMap, blurMap0, blitShader.Material, 0);

            // Setup and Apply blur
            blitShader.BlurContrast = 1.0f;
            
            // Blur the image 1
            blitShader.MainTex = blurMap0;
            blitShader.BlurSamples = 4;
            blitShader.BlurSpread = 1.0f;
            blitShader.BlurDirection = new Vector4(1, 0, 0, 0);
            Graphics.Blit(blurMap0, tempMap, blitShader.Material, 1);

            blitShader.MainTex = tempMap;
            blitShader.BlurDirection = new Vector4(0, 1, 0, 0);
            Graphics.Blit(tempMap, blurMap1, blitShader.Material, 1);


            // Blur the image 2
            blitShader.MainTex = blurMap1;
            blitShader.BlurSpread = 2.0f;
            blitShader.BlurDirection = new Vector4(1, 0, 0, 0);
            Graphics.Blit(blurMap1, tempMap, blitShader.Material, 1);

            blitShader.MainTex = tempMap;
            blitShader.BlurDirection = new Vector4(0, 1, 0, 0);
            Graphics.Blit(tempMap, blurMap2, blitShader.Material, 1);


            // Blur the image 3
            blitShader.MainTex = blurMap2;
            blitShader.BlurSpread = 4.0f;
            blitShader.BlurDirection = new Vector4(1, 0, 0, 0);
            Graphics.Blit(blurMap2, tempMap, blitShader.Material, 1);

            blitShader.MainTex = tempMap;
            blitShader.BlurDirection = new Vector4(0, 1, 0, 0);
            Graphics.Blit(tempMap, blurMap3, blitShader.Material, 1);


            // Blur the image 4
            blitShader.MainTex = blurMap3;
            blitShader.BlurSpread = 8.0f;
            blitShader.BlurDirection = new Vector4(1, 0, 0, 0);
            Graphics.Blit(blurMap3, tempMap, blitShader.Material, 1);

            blitShader.MainTex = tempMap;
            blitShader.BlurDirection = new Vector4(0, 1, 0, 0);
            Graphics.Blit(tempMap, blurMap4, blitShader.Material, 1);


            // Blur the image 5
            blitShader.MainTex = blurMap4;
            blitShader.BlurSpread = 16.0f;
            blitShader.BlurDirection = new Vector4(1, 0, 0, 0);
            Graphics.Blit(blurMap4, tempMap, blitShader.Material, 1);

            blitShader.MainTex = tempMap;
            blitShader.BlurDirection = new Vector4(0, 1, 0, 0);
            Graphics.Blit(tempMap, blurMap5, blitShader.Material, 1);


            // Blur the image 6
            blitShader.MainTex = blurMap5;
            blitShader.BlurSpread = 32.0f;
            blitShader.BlurDirection = new Vector4(1, 0, 0, 0);
            Graphics.Blit(blurMap5, tempMap, blitShader.Material, 1);

            blitShader.MainTex = tempMap;
            blitShader.BlurDirection = new Vector4(0, 1, 0, 0);
            Graphics.Blit(tempMap, blurMap6, blitShader.Material, 1);
        }

        public Texture2D CreateEdgeMap(in Texture2D diffuseTexture, in Texture2D normalMap, RenderTexture blurMap0, RenderTexture blurMap1, RenderTexture blurMap2, RenderTexture blurMap3, RenderTexture blurMap4, RenderTexture blurMap5, RenderTexture blurMap6)
        {
            var tempMap = CreateRenderTexture();

            var edgeShader = new BlitEdgeShaderWrapper
            {
                Blur0Weight = Settings.Blur0Weight * Settings.Blur0Weight * Settings.Blur0Weight,
                Blur1Weight = Settings.Blur1Weight * Settings.Blur1Weight * Settings.Blur1Weight,
                Blur2Weight = Settings.Blur2Weight * Settings.Blur2Weight * Settings.Blur2Weight,
                Blur3Weight = Settings.Blur3Weight * Settings.Blur3Weight * Settings.Blur3Weight,
                Blur4Weight = Settings.Blur4Weight * Settings.Blur4Weight * Settings.Blur4Weight,
                Blur5Weight = Settings.Blur5Weight * Settings.Blur5Weight * Settings.Blur5Weight,
                Blur6Weight = Settings.Blur6Weight * Settings.Blur6Weight * Settings.Blur6Weight,
                EdgeAmount = Settings.EdgeAmount,
                CreviceAmount = Settings.CreviceAmount,
                Pinch = Settings.Pinch,
                Pillow = Settings.Pillow,
                FinalContrast = Settings.FinalContrast,
                FinalBias = Settings.FinalBias,

                MainTex = normalMap,
                BlurTex0 = blurMap0,
                BlurTex1 = blurMap1,
                BlurTex2 = blurMap2,
                BlurTex3 = blurMap3,
                BlurTex4 = blurMap4,
                BlurTex5 = blurMap5,
                BlurTex6 = blurMap6
            };

            // Save low fidelity for texture 2d
            Graphics.Blit(blurMap0, tempMap, edgeShader.Material, 6);

            var edgeMap =  new Texture2D(tempMap.width, tempMap.height, TextureFormat.ARGB32, true, true);
            edgeMap.ReadPixels(new Rect(0, 0, tempMap.width, tempMap.height), 0, 0);
            edgeMap.Apply();

            return edgeMap;
        }
    }
}

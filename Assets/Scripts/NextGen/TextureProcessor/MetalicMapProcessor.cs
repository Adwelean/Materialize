using System;

using Assets.Scripts.NextGen.Extensions;
using Assets.Scripts.NextGen.ShaderWrapper;
using Assets.Scripts.NextGen.TextureSettings;

using Cysharp.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.NextGen.TextureProcessor
{
    public class MetalicMapProcessor : TextureProcessorBase<MetalicMapSettingsWrapper>
    {
        public MetalicMapProcessor(MetalicMapSettingsWrapper textureSettings)
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

            if (textureContext.ProcessorPhase >= ProcessorPhase.MetalicMapProcessed)
                return UniTask.FromResult(textureContext);

            if (!textureContext.Textures.TryGetValueAs(TypeMap.Diffuse, out diffuseMap))
                throw new ArgumentNullException($"[Phase: {textureContext.ProcessorPhase}] Missing Diffuse texture!");

            var blitShader = new BlitShaderWrapper { ImageSize = new Vector4(ImageSizeX, ImageSizeY, 0, 0) };

            ImageSizeX = diffuseMap.width;
            ImageSizeY = diffuseMap.height;

            var blurMap = CreateRenderTexture();
            var overlayBlurMap = CreateRenderTexture();

            ApplyBlurMapFromDiffuseMapToBlitMaterial(ref blitShader, diffuseMap, blurMap, overlayBlurMap);

            var metalicMap = CreateMetalicMap(diffuseMap, blurMap, overlayBlurMap);

            textureContext.Textures.Add(TypeMap.Metalic, metalicMap);
            textureContext.ProcessorPhase = ProcessorPhase.MetalicMapProcessed;

            return UniTask.FromResult(textureContext);
        }

        public void ApplyBlurMapFromDiffuseMapToBlitMaterial(ref BlitShaderWrapper blitShader, in Texture2D diffuseTexture, RenderTexture blurMap, RenderTexture overlayBlurMap)
        {
            var tempMap = CreateRenderTexture();

            blitShader.BlurContrast = 1.0f;
            blitShader.BlurSpread = 1.0f;

            // Blur the image 1
            blitShader.BlurSamples = Settings.BlurSize;
            blitShader.BlurDirection = new Vector4(1, 0, 0, 0);

            /*if (Settings.UseAdjustedDiffuse)
            {
                if (Settings.BlurSize == 0)
                    Graphics.Blit(_diffuseMap, _tempMap);
                else
                    Graphics.Blit(_diffuseMap, _tempMap, _blitMaterial, 1);
            }
            else*/
            {
                if (Settings.BlurSize == 0)
                    Graphics.Blit(diffuseTexture, tempMap);
                else
                    Graphics.Blit(diffuseTexture, tempMap, blitShader.Material, 1);
            }

            blitShader.BlurDirection = new Vector4(0, 1, 0, 0);

            if (Settings.BlurSize == 0)
                Graphics.Blit(tempMap, blurMap);
            else
                Graphics.Blit(tempMap, blurMap, blitShader.Material, 1);

            //ThisMaterial.SetTexture("_BlurTex", _blurMap);

            // Blur the image for overlay
            blitShader.BlurSamples = Settings.OverlayBlurSize;
            blitShader.BlurDirection = new Vector4(1, 0, 0, 0);

            //Graphics.Blit(Settings.UseAdjustedDiffuse ? _diffuseMap : _diffuseMapOriginal, tempMap, blitShader.Material, 1);
            Graphics.Blit(diffuseTexture, tempMap, blitShader.Material, 1);

            blitShader.BlurDirection = new Vector4(0, 1, 0, 0);
            Graphics.Blit(tempMap, overlayBlurMap, blitShader.Material, 1);

            //ThisMaterial.SetTexture("_OverlayBlurTex", _overlayBlurMap);
        }

        public Texture2D CreateMetalicMap(in Texture2D diffuseTexture, RenderTexture blurMap, RenderTexture overlayBlurMap)
        {
            var metalicShader = new BlitMetallicShaderWrapper();
            var tempMap = CreateRenderTexture();

            //metalicShader.ImageSize = new Vector4(ImageSizeX, ImageSizeY, 0, 0);

            metalicShader.MetalColor = Settings.MetalColor;
            metalicShader.SampleUV = new Vector4(Settings.SampleUv.x, Settings.SampleUv.y, 0, 0);

            metalicShader.HueWeight = Settings.HueWeight;
            metalicShader.SatWeight = Settings.SatWeight;
            metalicShader.LumWeight = Settings.LumWeight;

            metalicShader.MaskLow = Settings.MaskLow;
            metalicShader.MaskHigh = Settings.MaskHigh;

            //metalicShader.Slider = _slider;

            metalicShader.BlurOverlay = Settings.BlurOverlay;

            metalicShader.FinalContrast = Settings.FinalContrast;

            metalicShader.FinalBias = Settings.FinalBias;

            metalicShader.BlurTex = blurMap;

            metalicShader.OverlayBlurTex = overlayBlurMap;


            //Graphics.Blit(Settings.UseAdjustedDiffuse ? _diffuseMap : _diffuseMapOriginal, _tempMap, _metallicBlitMaterial, 0);
            Graphics.Blit(diffuseTexture, tempMap, metalicShader.Material, 0);


            var metallicMap = new Texture2D(tempMap.width, tempMap.height, TextureFormat.ARGB32, true, true);
            metallicMap.ReadPixels(new Rect(0, 0, tempMap.width, tempMap.height), 0, 0);
            metallicMap.Apply();

            return metallicMap;
        }
    }
}

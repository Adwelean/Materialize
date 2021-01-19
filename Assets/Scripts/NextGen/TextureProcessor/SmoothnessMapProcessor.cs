using System;

using Assets.Scripts.NextGen.Extensions;
using Assets.Scripts.NextGen.ShaderWrapper;
using Assets.Scripts.NextGen.TextureSettings;

using Cysharp.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.NextGen.TextureProcessor
{
    public class SmoothnessMapProcessor : TextureProcessorBase<SmoothnessMapSettingsWrapper>
    {
        public SmoothnessMapProcessor(SmoothnessMapSettingsWrapper textureSettings)
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
            Texture2D metalicMap = default;

            if (textureContext.ProcessorPhase >= ProcessorPhase.SmoothnessMapProcessed)
                return UniTask.FromResult(textureContext);

            if (!textureContext.Textures.TryGetValueAs(TypeMap.Diffuse, out diffuseMap))
                throw new ArgumentNullException($"[Phase: {textureContext.ProcessorPhase}] Missing Diffuse texture!");

            if (!textureContext.Textures.TryGetValueAs(TypeMap.Metalic, out metalicMap))
                throw new ArgumentNullException($"[Phase: {textureContext.ProcessorPhase}] Missing MetalicMap texture!");

            ImageSizeX = diffuseMap.width;
            ImageSizeY = diffuseMap.height;

            var blurMap = CreateRenderTexture();
            var overlayBlurMap = CreateRenderTexture();

            ApplyBlurMapFromDiffuseMapToBlitMaterial(diffuseMap, blurMap, overlayBlurMap);

            var smoothnessMap = CreateSmoothnessMap(diffuseMap, metalicMap, blurMap, overlayBlurMap);

            textureContext.Textures.Add(TypeMap.Smoothness, smoothnessMap);
            textureContext.ProcessorPhase = ProcessorPhase.SmoothnessMapProcessed;

            return UniTask.FromResult(textureContext);
        }

        public void ApplyBlurMapFromDiffuseMapToBlitMaterial(in Texture2D diffuseTexture, RenderTexture blurMap, RenderTexture overlayBlurMap)
        {
            var tempMap = CreateRenderTexture();

            var blitShader = new BlitShaderWrapper { ImageSize = new Vector4(ImageSizeX, ImageSizeY, 0, 0) };

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
        }

        public Texture2D CreateSmoothnessMap(in Texture2D diffuseTexture, in Texture2D metallicMap, RenderTexture blurMap, RenderTexture overlayBlurMap)
        {
            var smoothnessShader = new BlitSmoothnessShaderWrapper();

            var tempMap = CreateRenderTexture();

            //smoothnessShader.ImageSize = new Vector4(_imageSizeX, _imageSizeY, 0, 0);

            smoothnessShader.MetallicTex = metallicMap;


            smoothnessShader.BlurTex = blurMap;

            smoothnessShader.OverlayBlurTex = overlayBlurMap;

            smoothnessShader.MetalSmoothness = Settings.MetalSmoothness;

            smoothnessShader.UseSample1 = Settings.UseSample1 ? 1 : 0;
            smoothnessShader.SampleColor1 = Settings.SampleColor1;
            smoothnessShader.SampleUV1 = new Vector4(Settings.SampleUv1.x, Settings.SampleUv1.y, 0, 0);
            smoothnessShader.HueWeight1 = Settings.HueWeight1;
            smoothnessShader.SatWeight1 = Settings.SatWeight1;
            smoothnessShader.LumWeight1 = Settings.LumWeight1;
            smoothnessShader.MaskLow1 = Settings.MaskLow1;
            smoothnessShader.MaskHigh1 = Settings.MaskHigh1;
            smoothnessShader.Sample1Smoothness = Settings.Sample1Smoothness;

            smoothnessShader.UseSample2 = Settings.UseSample2 ? 1 : 0;
            smoothnessShader.SampleColor2 = Settings.SampleColor2;
            smoothnessShader.SampleUV2 = new Vector4(Settings.SampleUv2.x, Settings.SampleUv2.y, 0, 0);
            smoothnessShader.HueWeight2 = Settings.HueWeight2;
            smoothnessShader.SatWeight2 = Settings.SatWeight2;
            smoothnessShader.LumWeight2 = Settings.LumWeight2;
            smoothnessShader.MaskLow2 = Settings.MaskLow2;
            smoothnessShader.MaskHigh2 = Settings.MaskHigh2;
            smoothnessShader.Sample2Smoothness = Settings.Sample2Smoothness;

            smoothnessShader.UseSample3 = Settings.UseSample3 ? 1 : 0;
            smoothnessShader.SampleColor3 = Settings.SampleColor3;
            smoothnessShader.SampleUV3 = new Vector4(Settings.SampleUv3.x, Settings.SampleUv3.y, 0, 0);
            smoothnessShader.HueWeight3 = Settings.HueWeight3;
            smoothnessShader.SatWeight3 = Settings.SatWeight3;
            smoothnessShader.LumWeight3 = Settings.LumWeight3;
            smoothnessShader.MaskLow3 = Settings.MaskLow3;
            smoothnessShader.MaskHigh3 = Settings.MaskHigh3;
            smoothnessShader.Sample3Smoothness = Settings.Sample3Smoothness;


            //smoothnessShader.Invert = Settings.Invert ? 1 = 0;
            smoothnessShader.BaseSmoothness = Settings.BaseSmoothness;

            smoothnessShader.BlurOverlay = Settings.BlurOverlay;
            smoothnessShader.FinalContrast = Settings.FinalContrast;
            smoothnessShader.FinalBias = Settings.FinalBias;


            Graphics.Blit(diffuseTexture, tempMap, smoothnessShader.Material, 0);

            var smoothnessMap = new Texture2D(tempMap.width, tempMap.height, TextureFormat.ARGB32, true, true);
            smoothnessMap.ReadPixels(new Rect(0, 0, tempMap.width, tempMap.height), 0, 0);
            smoothnessMap.Apply();

            return smoothnessMap;
        }
    }
}

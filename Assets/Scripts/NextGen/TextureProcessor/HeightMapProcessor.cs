using System;

using Assets.Scripts.NextGen.Extensions;
using Assets.Scripts.NextGen.ShaderWrapper;
using Assets.Scripts.NextGen.TextureSettings;

using Cysharp.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.NextGen.TextureProcessor
{
    public class HeightMapProcessor : TextureProcessorBase<HeightMapSettingsWrapper>
    {
        public HeightMapProcessor(HeightMapSettingsWrapper textureSettings)
            : base(textureSettings)
        {
        }

        private int ImageSizeX { get; set; }
        private int ImageSizeY { get; set; }

        private RenderTexture CreateRenderTexture(int? width = default, int? height = default, int depth = 0, RenderTextureFormat renderTextureFormat = RenderTextureFormat.RFloat)
            => new RenderTexture(width ?? ImageSizeX, height ?? ImageSizeY, depth, renderTextureFormat, RenderTextureReadWrite.Linear) { wrapMode = TextureWrapMode.Repeat };

        public override UniTask<TextureContext> Process(TextureContext textureContext)
        {
            Texture2D diffuseMap = default;

            if (textureContext.ProcessorPhase >= ProcessorPhase.HeightMapProcessed)
                return UniTask.FromResult(textureContext);

            if (!textureContext.Textures.TryGetValueAs(TypeMap.Diffuse, out diffuseMap))
                throw new ArgumentNullException($"[Phase: {textureContext.ProcessorPhase}] Missing Diffuse texture!");

            ImageSizeX = diffuseMap.width;
            ImageSizeY = diffuseMap.height;

            var blitShader = new BlitShaderWrapper { ImageSize = new Vector4(ImageSizeX, ImageSizeY, 0, 0) };

            var tempBlurMap = CreateRenderTexture();
            var blurMap0 = CreateRenderTexture();
            var blurMap1 = CreateRenderTexture();
            var blurMap2 = CreateRenderTexture();
            var blurMap3 = CreateRenderTexture();
            var blurMap4 = CreateRenderTexture();
            var blurMap5 = CreateRenderTexture();
            var blurMap6 = CreateRenderTexture();

            var blurredMap = CreateBlurredMapFromDiffuseMap(ref blitShader, diffuseMap, tempBlurMap, blurMap0, blurMap1, blurMap2, blurMap3, blurMap4, blurMap5, blurMap6);
            var (heightMap, heightMapHD) = CreateHeightMap(ref blitShader, blurMap0, blurMap1, blurMap2, blurMap3, blurMap4, blurMap5, blurMap6, blurredMap);

            textureContext.Textures.Add(TypeMap.Height, heightMap);
            textureContext.Textures.Add(TypeMap.HeightHD, heightMapHD);
            textureContext.ProcessorPhase = ProcessorPhase.HeightMapProcessed;

            return UniTask.FromResult(textureContext);
        }

        private RenderTexture CreateBlurredMapFromDiffuseMap(ref BlitShaderWrapper blitShader, in Texture2D diffuseMap, RenderTexture tempBlurMap, RenderTexture blurMap0, RenderTexture blurMap1, RenderTexture blurMap2, RenderTexture blurMap3, RenderTexture blurMap4, RenderTexture blurMap5, RenderTexture blurMap6)
        {
            var blitSampleShader = new BlitSampleShaderWrapper();

            var avgMap = CreateRenderTexture(256, 256);
            var avgTempMap = CreateRenderTexture(256, 256);

            // Setup shader
            blitSampleShader.IsolateSample1 = Settings.IsolateSample1 ? 1 : 0;
            blitSampleShader.UseSample1 = Settings.UseSample1 ? 1 : 0;
            blitSampleShader.SampleColor1 = Settings.SampleColor1;
            blitSampleShader.SampleUV1 = new Vector4(Settings.SampleUv1.x, Settings.SampleUv1.y, 0, 0);
            blitSampleShader.HueWeight1 = Settings.HueWeight1;
            blitSampleShader.SatWeight1 = Settings.SatWeight1;
            blitSampleShader.LumWeight1 = Settings.LumWeight1;
            blitSampleShader.MaskLow1 = Settings.MaskLow1;
            blitSampleShader.MaskHigh1 = Settings.MaskHigh1;
            blitSampleShader.Sample1Height = Settings.Sample1Height;
            blitSampleShader.IsolateSample2 = Settings.IsolateSample2 ? 1 : 0;
            blitSampleShader.UseSample2 = Settings.UseSample2 ? 1 : 0;
            blitSampleShader.SampleColor2 = Settings.SampleColor2;
            blitSampleShader.SampleUV2 = new Vector4(Settings.SampleUv2.x, Settings.SampleUv2.y, 0, 0);
            blitSampleShader.HueWeight2 = Settings.HueWeight2;
            blitSampleShader.SatWeight2 = Settings.SatWeight2;
            blitSampleShader.LumWeight2 = Settings.LumWeight2;
            blitSampleShader.MaskLow2 = Settings.MaskLow2;
            blitSampleShader.MaskHigh2 = Settings.MaskHigh2;
            blitSampleShader.Sample2Height = Settings.Sample2Height;
            blitSampleShader.SampleBlend = 0.0f;
            blitSampleShader.FinalContrast = Settings.FinalContrast;
            blitSampleShader.FinalBias = Settings.FinalBias;

            // Apply to diffuse map
            Graphics.Blit(diffuseMap, blurMap0, blitSampleShader.Material, 0);

            // Setup and Apply blur
            //blitShader.ImageSize = new Vector4(imageSizeX, imageSizeY, 0, 0);
            blitShader.BlurContrast = 1.0f;

            var extraSpread = (blurMap0.width + blurMap0.height) * 0.5f / 1024.0f;
            var spread = 1.0f;

            // Blur the image 1
            blitShader.BlurSamples = 4;
            blitShader.BlurSpread = spread;
            blitShader.BlurDirection = new Vector4(1, 0, 0, 0);
            Graphics.Blit(blurMap0, tempBlurMap, blitShader.Material, 1);
            blitShader.BlurDirection = new Vector4(0, 1, 0, 0);
            Graphics.Blit(tempBlurMap, blurMap1, blitShader.Material, 1);

            spread += extraSpread;

            // Blur the image 2
            blitShader.BlurSpread = spread;
            blitShader.BlurDirection = new Vector4(1, 0, 0, 0);
            Graphics.Blit(blurMap1, tempBlurMap, blitShader.Material, 1);
            blitShader.BlurDirection = new Vector4(0, 1, 0, 0);
            Graphics.Blit(tempBlurMap, blurMap2, blitShader.Material, 1);

            spread += 2 * extraSpread;

            // Blur the image 3
            blitShader.BlurSpread = spread;
            blitShader.BlurDirection = new Vector4(1, 0, 0, 0);
            Graphics.Blit(blurMap2, tempBlurMap, blitShader.Material, 1);
            blitShader.BlurDirection = new Vector4(0, 1, 0, 0);
            Graphics.Blit(tempBlurMap, blurMap3, blitShader.Material, 1);

            spread += 4 * extraSpread;

            // Blur the image 4
            blitShader.BlurSpread = spread;
            blitShader.BlurDirection = new Vector4(1, 0, 0, 0);
            Graphics.Blit(blurMap3, tempBlurMap, blitShader.Material, 1);
            blitShader.BlurDirection = new Vector4(0, 1, 0, 0);
            Graphics.Blit(tempBlurMap, blurMap4, blitShader.Material, 1);

            spread += 8 * extraSpread;

            // Blur the image 5
            blitShader.BlurSpread = spread;
            blitShader.BlurDirection = new Vector4(1, 0, 0, 0);
            Graphics.Blit(blurMap4, tempBlurMap, blitShader.Material, 1);
            blitShader.BlurDirection = new Vector4(0, 1, 0, 0);
            Graphics.Blit(tempBlurMap, blurMap5, blitShader.Material, 1);

            spread += 16 * extraSpread;

            // Blur the image 6
            blitShader.BlurSpread = spread;
            blitShader.BlurDirection = new Vector4(1, 0, 0, 0);
            Graphics.Blit(blurMap5, tempBlurMap, blitShader.Material, 1);
            blitShader.BlurDirection = new Vector4(0, 1, 0, 0);
            Graphics.Blit(tempBlurMap, blurMap6, blitShader.Material, 1);

            // Average Color
            blitShader.BlurSamples = 32;
            blitShader.BlurSpread = 64.0f * extraSpread;
            blitShader.BlurDirection = new Vector4(1, 0, 0, 0);
            Graphics.Blit(blurMap6, avgTempMap, blitShader.Material, 1);
            blitShader.BlurDirection = new Vector4(0, 1, 0, 0);
            Graphics.Blit(avgTempMap, avgMap, blitShader.Material, 1);

            return avgMap;
        }

        private (Texture2D heightMap, RenderTexture heightMapHD) CreateHeightMap(ref BlitShaderWrapper blitShader, RenderTexture blurMap0, RenderTexture blurMap1, RenderTexture blurMap2, RenderTexture blurMap3, RenderTexture blurMap4, RenderTexture blurMap5, RenderTexture blurMap6, RenderTexture avgMap)
        {
            
            blitShader.FinalContrast = Settings.FinalContrast;
            blitShader.FinalBias = Settings.FinalBias;

            var realGain = Settings.FinalGain;

            if (realGain < 0.0f)
                realGain = Mathf.Abs(1.0f / (realGain - 1.0f));
            else
                realGain = realGain + 1.0f;

            blitShader.FinalGain = realGain;
            blitShader.HeightFromNormal = 0.0f;
            blitShader.Blur0Weight = Settings.Blur0Weight;
            blitShader.Blur1Weight = Settings.Blur1Weight;
            blitShader.Blur2Weight = Settings.Blur2Weight;
            blitShader.Blur3Weight = Settings.Blur3Weight;
            blitShader.Blur4Weight = Settings.Blur4Weight;
            blitShader.Blur5Weight = Settings.Blur5Weight;
            blitShader.Blur6Weight = Settings.Blur6Weight;
            blitShader.Blur0Contrast = Settings.Blur0Contrast;
            blitShader.Blur1Contrast = Settings.Blur1Contrast;
            blitShader.Blur2Contrast = Settings.Blur2Contrast;
            blitShader.Blur3Contrast = Settings.Blur3Contrast;
            blitShader.Blur4Contrast = Settings.Blur4Contrast;
            blitShader.Blur5Contrast = Settings.Blur5Contrast;
            blitShader.Blur6Contrast = Settings.Blur6Contrast;
            blitShader.BlurTex0 = blurMap0;
            blitShader.BlurTex1 = blurMap1;
            blitShader.BlurTex2 = blurMap2;
            blitShader.BlurTex3 = blurMap3;
            blitShader.BlurTex4 = blurMap4;
            blitShader.BlurTex5 = blurMap5;
            blitShader.BlurTex6 = blurMap6;
            blitShader.AvgTex = avgMap;

            var tempHeightMap = CreateRenderTexture(renderTextureFormat: RenderTextureFormat.ARGB32);

            // Save low fidelity for texture 2d
            Graphics.Blit(blurMap0, tempHeightMap, blitShader.Material, 2);

            var heightMap = new Texture2D(tempHeightMap.width, tempHeightMap.height, TextureFormat.ARGB32, true, true);
            heightMap.ReadPixels(new Rect(0, 0, tempHeightMap.width, tempHeightMap.height), 0, 0);
            heightMap.Apply();

            var heightMapHD = CreateRenderTexture(tempHeightMap.width, tempHeightMap.height, renderTextureFormat: RenderTextureFormat.RHalf);
            Graphics.Blit(blurMap0, heightMapHD, blitShader.Material, 2);

            return (heightMap, heightMapHD);
        }
    }
}

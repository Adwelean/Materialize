//using Assets.Scripts.NextGen.TextureSettings;

//using Cysharp.Threading.Tasks;

//using UnityEngine;

//namespace Assets.Scripts.NextGen.TextureProcessor
//{
//    public class HeightMapProcessorOld : TextureProcessorBase<HeightMapSettingsWrapper>
//    {
//        #region ShaderProperties
//        private const float blurScale = 1.0f;
//        private readonly int blurScaleId = Shader.PropertyToID("_BlurScale");
//        private readonly int imageSize = Shader.PropertyToID("_ImageSize");
//        private readonly int isolate = Shader.PropertyToID("_Isolate");
//        private readonly int blur0Weight = Shader.PropertyToID("_Blur0Weight");
//        private readonly int blur1Weight = Shader.PropertyToID("_Blur1Weight");
//        private readonly int blur2Weight = Shader.PropertyToID("_Blur2Weight");
//        private readonly int blur3Weight = Shader.PropertyToID("_Blur3Weight");
//        private readonly int blur4Weight = Shader.PropertyToID("_Blur4Weight");
//        private readonly int blur5Weight = Shader.PropertyToID("_Blur5Weight");
//        private readonly int blur6Weight = Shader.PropertyToID("_Blur6Weight");
//        private readonly int blur0Contrast = Shader.PropertyToID("_Blur0Contrast");
//        private readonly int blur1Contrast = Shader.PropertyToID("_Blur1Contrast");
//        private readonly int blur2Contrast = Shader.PropertyToID("_Blur2Contrast");
//        private readonly int blur3Contrast = Shader.PropertyToID("_Blur3Contrast");
//        private readonly int blur4Contrast = Shader.PropertyToID("_Blur4Contrast");
//        private readonly int blur5Contrast = Shader.PropertyToID("_Blur5Contrast");
//        private readonly int blur6Contrast = Shader.PropertyToID("_Blur6Contrast");
//        private readonly int finalGain = Shader.PropertyToID("_FinalGain");
//        private readonly int finalContrast = Shader.PropertyToID("_FinalContrast");
//        private readonly int finalBias = Shader.PropertyToID("_FinalBias");
//        private readonly int slider = Shader.PropertyToID("_Slider");
//        private readonly int blurTex0 = Shader.PropertyToID("_BlurTex0");
//        private readonly int heightFromNormal = Shader.PropertyToID("_HeightFromNormal");
//        private readonly int blurTex1 = Shader.PropertyToID("_BlurTex1");
//        private readonly int blurTex2 = Shader.PropertyToID("_BlurTex2");
//        private readonly int blurTex3 = Shader.PropertyToID("_BlurTex3");
//        private readonly int blurTex4 = Shader.PropertyToID("_BlurTex4");
//        private readonly int blurTex5 = Shader.PropertyToID("_BlurTex5");
//        private readonly int blurTex6 = Shader.PropertyToID("_BlurTex6");
//        private readonly int avgTex = Shader.PropertyToID("_AvgTex");
//        private readonly int spread = Shader.PropertyToID("_Spread");
//        private readonly int spreadBoost = Shader.PropertyToID("_SpreadBoost");
//        private readonly int samples = Shader.PropertyToID("_Samples");
//        private readonly int mainTex = Shader.PropertyToID("_MainTex");
//        private readonly int blendTex = Shader.PropertyToID("_BlendTex");
//        private readonly int isNormal = Shader.PropertyToID("_IsNormal");
//        private readonly int blendAmount = Shader.PropertyToID("_BlendAmount");
//        private readonly int progress = Shader.PropertyToID("_Progress");
//        private readonly int isolateSample1 = Shader.PropertyToID("_IsolateSample1");
//        private readonly int useSample1 = Shader.PropertyToID("_UseSample1");
//        private readonly int sampleColor1 = Shader.PropertyToID("_SampleColor1");
//        private readonly int sampleUv1 = Shader.PropertyToID("_SampleUV1");
//        private readonly int hueWeight1 = Shader.PropertyToID("_HueWeight1");
//        private readonly int satWeight1 = Shader.PropertyToID("_SatWeight1");
//        private readonly int lumWeight1 = Shader.PropertyToID("_LumWeight1");
//        private readonly int maskLow1 = Shader.PropertyToID("_MaskLow1");
//        private readonly int maskHigh1 = Shader.PropertyToID("_MaskHigh1");
//        private readonly int sample1Height = Shader.PropertyToID("_Sample1Height");
//        private readonly int isolateSample2 = Shader.PropertyToID("_IsolateSample2");
//        private readonly int useSample2 = Shader.PropertyToID("_UseSample2");
//        private readonly int sampleColor2 = Shader.PropertyToID("_SampleColor2");
//        private readonly int sampleUv2 = Shader.PropertyToID("_SampleUV2");
//        private readonly int hueWeight2 = Shader.PropertyToID("_HueWeight2");
//        private readonly int satWeight2 = Shader.PropertyToID("_SatWeight2");
//        private readonly int lumWeight2 = Shader.PropertyToID("_LumWeight2");
//        private readonly int maskLow2 = Shader.PropertyToID("_MaskLow2");
//        private readonly int maskHigh2 = Shader.PropertyToID("_MaskHigh2");
//        private readonly int sample2Height = Shader.PropertyToID("_Sample2Height");
//        private readonly int sampleBlend = Shader.PropertyToID("_SampleBlend");
//        private readonly int blurContrast = Shader.PropertyToID("_BlurContrast");
//        private readonly int blurSamples = Shader.PropertyToID("_BlurSamples");
//        private readonly int blurSpread = Shader.PropertyToID("_BlurSpread");
//        private readonly int blurDirection = Shader.PropertyToID("_BlurDirection");
//        #endregion ShaderProperties

//        public HeightMapProcessorOld(HeightMapSettingsWrapper textureSettings)
//            : base(textureSettings)
//        {
//        }

//        private RenderTexture CreateRenderTexture(int width, int height, int depth = 0, RenderTextureFormat renderTextureFormat = RenderTextureFormat.RFloat)
//            => new RenderTexture(width, height, depth, renderTextureFormat, RenderTextureReadWrite.Linear) { wrapMode = TextureWrapMode.Repeat };

//        public override UniTask<Texture2D> Process(Texture2D diffuseMap, Texture2D _, Texture2D __)
//        {
//            var blitMaterial = new Material(Shader.Find("Hidden/Blit_Shader"));

//            var imageSizeX = diffuseMap.width;
//            var imageSizeY = diffuseMap.height;

//            var tempBlurMap = CreateRenderTexture(imageSizeX, imageSizeY);
//            var blurMap0 = CreateRenderTexture(imageSizeX, imageSizeY);
//            var blurMap1 = CreateRenderTexture(imageSizeX, imageSizeY);
//            var blurMap2 = CreateRenderTexture(imageSizeX, imageSizeY);
//            var blurMap3 = CreateRenderTexture(imageSizeX, imageSizeY);
//            var blurMap4 = CreateRenderTexture(imageSizeX, imageSizeY);
//            var blurMap5 = CreateRenderTexture(imageSizeX, imageSizeY);
//            var blurMap6 = CreateRenderTexture(imageSizeX, imageSizeY);

//            var bluredMap = CreateBlurredMapFromDiffuseMap(ref blitMaterial, diffuseMap, imageSizeX, imageSizeY, tempBlurMap, blurMap0, blurMap1, blurMap2, blurMap3, blurMap4, blurMap5, blurMap6);
//            var heightMap = CreateHeightMap(ref blitMaterial, imageSizeX, imageSizeY, blurMap0, blurMap1, blurMap2, blurMap3, blurMap4, blurMap5, blurMap6, bluredMap);

//            return UniTask.FromResult(heightMap);
//        }

//        private RenderTexture CreateBlurredMapFromDiffuseMap(ref Material blitMaterial, in Texture2D diffuseMap, in int imageSizeX, in int imageSizeY, RenderTexture tempBlurMap, RenderTexture blurMap0, RenderTexture blurMap1, RenderTexture blurMap2, RenderTexture blurMap3, RenderTexture blurMap4, RenderTexture blurMap5, RenderTexture blurMap6)
//        {
//            var blitMaterialSample = new Material(Shader.Find("Hidden/Blit_Sample"));

//            var avgMap = CreateRenderTexture(256, 256);
//            var avgTempMap = CreateRenderTexture(256, 256);

//            // Setup shader
//            blitMaterialSample.SetInt(this.isolateSample1, Settings.IsolateSample1 ? 1 : 0);
//            blitMaterialSample.SetInt(this.useSample1, Settings.UseSample1 ? 1 : 0);
//            blitMaterialSample.SetColor(this.sampleColor1, Settings.SampleColor1);
//            blitMaterialSample.SetVector(this.sampleUv1, new Vector4(Settings.SampleUv1.x, Settings.SampleUv1.y, 0, 0));
//            blitMaterialSample.SetFloat(this.hueWeight1, Settings.HueWeight1);
//            blitMaterialSample.SetFloat(this.satWeight1, Settings.SatWeight1);
//            blitMaterialSample.SetFloat(this.lumWeight1, Settings.LumWeight1);
//            blitMaterialSample.SetFloat(this.maskLow1, Settings.MaskLow1);
//            blitMaterialSample.SetFloat(this.maskHigh1, Settings.MaskHigh1);
//            blitMaterialSample.SetFloat(this.sample1Height, Settings.Sample1Height);
//            blitMaterialSample.SetInt(this.isolateSample2, Settings.IsolateSample2 ? 1 : 0);
//            blitMaterialSample.SetInt(this.useSample2, Settings.UseSample2 ? 1 : 0);
//            blitMaterialSample.SetColor(this.sampleColor2, Settings.SampleColor2);
//            blitMaterialSample.SetVector(this.sampleUv2, new Vector4(Settings.SampleUv2.x, Settings.SampleUv2.y, 0, 0));
//            blitMaterialSample.SetFloat(this.hueWeight2, Settings.HueWeight2);
//            blitMaterialSample.SetFloat(this.satWeight2, Settings.SatWeight2);
//            blitMaterialSample.SetFloat(this.lumWeight2, Settings.LumWeight2);
//            blitMaterialSample.SetFloat(this.maskLow2, Settings.MaskLow2);
//            blitMaterialSample.SetFloat(this.maskHigh2, Settings.MaskHigh2);
//            blitMaterialSample.SetFloat(this.sample2Height, Settings.Sample2Height);
//            blitMaterialSample.SetFloat(this.sampleBlend, 0.0f);
//            blitMaterialSample.SetFloat(this.finalContrast, Settings.FinalContrast);
//            blitMaterialSample.SetFloat(this.finalBias, Settings.FinalBias);

//            // Apply to diffuse map
//            Graphics.Blit(diffuseMap, blurMap0, blitMaterialSample, 0);

//            // Setup and Apply blur
//            blitMaterial.SetVector(this.imageSize, new Vector4(imageSizeX, imageSizeY, 0, 0));
//            blitMaterial.SetFloat(this.blurContrast, 1.0f);

//            var extraSpread = (blurMap0.width + blurMap0.height) * 0.5f / 1024.0f;
//            var spread = 1.0f;

//            // Blur the image 1
//            blitMaterial.SetInt(this.blurSamples, 4);
//            blitMaterial.SetFloat(this.blurSpread, spread);
//            blitMaterial.SetVector(this.blurDirection, new Vector4(1, 0, 0, 0));
//            Graphics.Blit(blurMap0, tempBlurMap, blitMaterial, 1);
//            blitMaterial.SetVector(this.blurDirection, new Vector4(0, 1, 0, 0));
//            Graphics.Blit(tempBlurMap, blurMap1, blitMaterial, 1);

//            spread += extraSpread;

//            // Blur the image 2
//            blitMaterial.SetFloat(this.blurSpread, spread);
//            blitMaterial.SetVector(this.blurDirection, new Vector4(1, 0, 0, 0));
//            Graphics.Blit(blurMap1, tempBlurMap, blitMaterial, 1);
//            blitMaterial.SetVector(this.blurDirection, new Vector4(0, 1, 0, 0));
//            Graphics.Blit(tempBlurMap, blurMap2, blitMaterial, 1);

//            spread += 2 * extraSpread;

//            // Blur the image 3
//            blitMaterial.SetFloat(this.blurSpread, spread);
//            blitMaterial.SetVector(this.blurDirection, new Vector4(1, 0, 0, 0));
//            Graphics.Blit(blurMap2, tempBlurMap, blitMaterial, 1);
//            blitMaterial.SetVector(this.blurDirection, new Vector4(0, 1, 0, 0));
//            Graphics.Blit(tempBlurMap, blurMap3, blitMaterial, 1);

//            spread += 4 * extraSpread;

//            // Blur the image 4
//            blitMaterial.SetFloat(this.blurSpread, spread);
//            blitMaterial.SetVector(this.blurDirection, new Vector4(1, 0, 0, 0));
//            Graphics.Blit(blurMap3, tempBlurMap, blitMaterial, 1);
//            blitMaterial.SetVector(this.blurDirection, new Vector4(0, 1, 0, 0));
//            Graphics.Blit(tempBlurMap, blurMap4, blitMaterial, 1);

//            spread += 8 * extraSpread;

//            // Blur the image 5
//            blitMaterial.SetFloat(this.blurSpread, spread);
//            blitMaterial.SetVector(this.blurDirection, new Vector4(1, 0, 0, 0));
//            Graphics.Blit(blurMap4, tempBlurMap, blitMaterial, 1);
//            blitMaterial.SetVector(this.blurDirection, new Vector4(0, 1, 0, 0));
//            Graphics.Blit(tempBlurMap, blurMap5, blitMaterial, 1);

//            spread += 16 * extraSpread;

//            // Blur the image 6
//            blitMaterial.SetFloat(this.blurSpread, spread);
//            blitMaterial.SetVector(this.blurDirection, new Vector4(1, 0, 0, 0));
//            Graphics.Blit(blurMap5, tempBlurMap, blitMaterial, 1);
//            blitMaterial.SetVector(this.blurDirection, new Vector4(0, 1, 0, 0));
//            Graphics.Blit(tempBlurMap, blurMap6, blitMaterial, 1);

//            // Average Color
//            blitMaterial.SetInt(this.blurSamples, 32);
//            blitMaterial.SetFloat(this.blurSpread, 64.0f * extraSpread);
//            blitMaterial.SetVector(this.blurDirection, new Vector4(1, 0, 0, 0));
//            Graphics.Blit(blurMap6, avgTempMap, blitMaterial, 1);
//            blitMaterial.SetVector(this.blurDirection, new Vector4(0, 1, 0, 0));
//            Graphics.Blit(avgTempMap, avgMap, blitMaterial, 1);

//            return avgMap;
//        }

//        private Texture2D CreateHeightMap(ref Material blitMaterial, in int imageSizeX, in int imageSizeY, RenderTexture blurMap0, RenderTexture blurMap1, RenderTexture blurMap2, RenderTexture blurMap3, RenderTexture blurMap4, RenderTexture blurMap5, RenderTexture blurMap6, RenderTexture avgMap)
//        {
//            blitMaterial.SetVector(this.imageSize, new Vector4(imageSizeX, imageSizeY, 0, 0));
//            blitMaterial.SetFloat(this.finalContrast, Settings.FinalContrast);
//            blitMaterial.SetFloat(this.finalBias, Settings.FinalBias);

//            var realGain = Settings.FinalGain;

//            if (realGain < 0.0f)
//                realGain = Mathf.Abs(1.0f / (realGain - 1.0f));
//            else
//                realGain = realGain + 1.0f;

//            blitMaterial.SetFloat(this.finalGain, realGain);
//            blitMaterial.SetFloat(this.heightFromNormal, 0.0f);
//            blitMaterial.SetFloat(this.blur0Weight, Settings.Blur0Weight);
//            blitMaterial.SetFloat(this.blur1Weight, Settings.Blur1Weight);
//            blitMaterial.SetFloat(this.blur2Weight, Settings.Blur2Weight);
//            blitMaterial.SetFloat(this.blur3Weight, Settings.Blur3Weight);
//            blitMaterial.SetFloat(this.blur4Weight, Settings.Blur4Weight);
//            blitMaterial.SetFloat(this.blur5Weight, Settings.Blur5Weight);
//            blitMaterial.SetFloat(this.blur6Weight, Settings.Blur6Weight);
//            blitMaterial.SetFloat(this.blur0Contrast, Settings.Blur0Contrast);
//            blitMaterial.SetFloat(this.blur1Contrast, Settings.Blur1Contrast);
//            blitMaterial.SetFloat(this.blur2Contrast, Settings.Blur2Contrast);
//            blitMaterial.SetFloat(this.blur3Contrast, Settings.Blur3Contrast);
//            blitMaterial.SetFloat(this.blur4Contrast, Settings.Blur4Contrast);
//            blitMaterial.SetFloat(this.blur5Contrast, Settings.Blur5Contrast);
//            blitMaterial.SetFloat(this.blur6Contrast, Settings.Blur6Contrast);
//            blitMaterial.SetTexture(this.blurTex0, blurMap0);
//            blitMaterial.SetTexture(this.blurTex1, blurMap1);
//            blitMaterial.SetTexture(this.blurTex2, blurMap2);
//            blitMaterial.SetTexture(this.blurTex3, blurMap3);
//            blitMaterial.SetTexture(this.blurTex4, blurMap4);
//            blitMaterial.SetTexture(this.blurTex5, blurMap5);
//            blitMaterial.SetTexture(this.blurTex6, blurMap6);
//            blitMaterial.SetTexture(this.avgTex, avgMap);

//            var tempHeightMap = CreateRenderTexture(imageSizeX, imageSizeY, renderTextureFormat: RenderTextureFormat.ARGB32);

//            // Save low fidelity for texture 2d
//            Graphics.Blit(blurMap0, tempHeightMap, blitMaterial, 2);

//            var heightMap = new Texture2D(tempHeightMap.width, tempHeightMap.height, TextureFormat.ARGB32, true, true);
//            heightMap.ReadPixels(new Rect(0, 0, tempHeightMap.width, tempHeightMap.height), 0, 0);
//            heightMap.Apply();

//            //var hdHeightMap = CreateRenderTexture(tempHeightMap.width, tempHeightMap.height, renderTextureFormat: RenderTextureFormat.RHalf);
//            //Graphics.Blit(blurMap0, hdHeightMap, BlitMaterial, 2);

//            return heightMap;
//        }
//    }
//}

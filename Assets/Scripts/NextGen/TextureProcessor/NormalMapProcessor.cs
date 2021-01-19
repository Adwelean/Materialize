using System;

using Assets.Scripts.NextGen.Extensions;
using Assets.Scripts.NextGen.ShaderWrapper;
using Assets.Scripts.NextGen.TextureSettings;

using Cysharp.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.NextGen.TextureProcessor
{
    public class NormalMapProcessor : TextureProcessorBase<NormalMapSettingsWrapper>
    {
        public NormalMapProcessor(NormalMapSettingsWrapper textureSettings)
            : base(textureSettings)
        {
        }

        private int ImageSizeX { get; set; }
        private int ImageSizeY { get; set; }

        private RenderTexture CreateRenderTexture(int? width = default, int? height = default, int depth = 0, RenderTextureFormat renderTextureFormat = RenderTextureFormat.ARGBHalf)
            => new RenderTexture(width ?? ImageSizeX, height ?? ImageSizeY, depth, renderTextureFormat, RenderTextureReadWrite.Linear) { wrapMode = TextureWrapMode.Repeat };

        public override UniTask<TextureContext> Process(TextureContext textureContext)
        {
            Texture2D diffuseMap = default;
            Texture2D heightMap = default;
            RenderTexture heightMapHD = default;

            if (textureContext.ProcessorPhase >= ProcessorPhase.NormalMapProcessed)
                return UniTask.FromResult(textureContext);

            if (!textureContext.Textures.TryGetValueAs(TypeMap.Diffuse, out diffuseMap))
                throw new ArgumentNullException($"[Phase: {textureContext.ProcessorPhase}] Missing Diffuse texture!");

            if (!textureContext.Textures.TryGetValueAs(TypeMap.Height, out heightMap))
                throw new ArgumentNullException($"[Phase: {textureContext.ProcessorPhase}] Missing HeightMap texture!");

            textureContext.Textures.TryGetValueAs(TypeMap.HeightHD, out heightMapHD);

            ImageSizeX = diffuseMap.width;
            ImageSizeY = diffuseMap.height;

            var blitShader = new BlitShaderWrapper { ImageSize = new Vector4(ImageSizeX, ImageSizeY, 0, 0) };

            var tempBlurMap = CreateRenderTexture();
            var heightBlurMap = CreateRenderTexture(renderTextureFormat: RenderTextureFormat.RHalf);
            var blurMap0 = CreateRenderTexture();
            var blurMap1 = CreateRenderTexture();
            var blurMap2 = CreateRenderTexture();
            var blurMap3 = CreateRenderTexture();
            var blurMap4 = CreateRenderTexture();
            var blurMap5 = CreateRenderTexture();
            var blurMap6 = CreateRenderTexture();

            ApplyBlurredMapFromHeightMapToBlitMaterial(ref blitShader, diffuseMap, heightMap, heightMapHD, tempBlurMap, heightBlurMap, blurMap0, blurMap1, blurMap2, blurMap3, blurMap4, blurMap5, blurMap6);

            var normalMap = CreateNormalMap(ref blitShader, tempBlurMap, heightBlurMap, blurMap0, blurMap1, blurMap2, blurMap3, blurMap4, blurMap5, blurMap6);

            textureContext.Textures.Add(TypeMap.Normal, normalMap);
            textureContext.ProcessorPhase = ProcessorPhase.NormalMapProcessed;

            return UniTask.FromResult(textureContext);
        }

        private void ApplyBlurredMapFromHeightMapToBlitMaterial(
            ref BlitShaderWrapper blitShader,
            in Texture2D diffuseTexture,
            in Texture2D heightMap,
            in RenderTexture heightMapHD,
            RenderTexture tempBlurMap,
            RenderTexture heightBlurMap,
            RenderTexture blurMap0,
            RenderTexture blurMap1,
            RenderTexture blurMap2,
            RenderTexture blurMap3,
            RenderTexture blurMap4,
            RenderTexture blurMap5,
            RenderTexture blurMap6)
        {
            // Blur the height map for normal slope
            blitShader.BlurSpread = 1.0f;
            blitShader.BlurSamples = Settings.SlopeBlur;
            blitShader.BlurDirection = new Vector4(1, 0, 0, 0);
            blitShader.BlurContrast = 1.0f;

            blitShader.Desaturate = 0;
            Graphics.Blit(heightMap, tempBlurMap, blitShader.Material, 1);
            blitShader.LightTex = heightMap;

            blitShader.Desaturate = 0;

            blitShader.BlurDirection = new Vector4(0, 1, 0, 0);
            Graphics.Blit(tempBlurMap, heightBlurMap, blitShader.Material, 1);

            blitShader.BlurSpread = 3.0f;
            blitShader.BlurDirection = new Vector4(1, 0, 0, 0);
            Graphics.Blit(heightBlurMap, tempBlurMap, blitShader.Material, 1);

            blitShader.BlurDirection = new Vector4(0, 1, 0, 0);
            Graphics.Blit(tempBlurMap, heightBlurMap, blitShader.Material, 1);

            blitShader.LightBlurTex = heightBlurMap;

            // Make normal from height
            blitShader.LightRotation = Settings.LightRotation;
            blitShader.ShapeRecognition = Settings.ShapeRecognition;
            blitShader.ShapeBias = Settings.ShapeBias;
            blitShader.DiffuseTex = diffuseTexture;

            blitShader.BlurContrast = Settings.Blur0Contrast;

            if (heightMapHD != default)
                Graphics.Blit(heightMapHD, blurMap0, blitShader.Material, 3);
            else
                Graphics.Blit(heightMap, blurMap0, blitShader.Material, 3);

            var extraSpread = (blurMap0.width + blurMap0.height) * 0.5f / 1024.0f;
            var spread = 1.0f;

            blitShader.BlurContrast = 1.0f;
            blitShader.Desaturate = 0;

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
        }

        private Texture2D CreateNormalMap(ref BlitShaderWrapper blitShader, RenderTexture heightBlurMap, RenderTexture blurMap0, RenderTexture blurMap1, RenderTexture blurMap2, RenderTexture blurMap3, RenderTexture blurMap4, RenderTexture blurMap5, RenderTexture blurMap6, RenderTexture avgMap)
        {
            //blitMaterial.ImageSize = new Vector4(imageSizeX, imageSizeY, 0, 0);

            blitShader.Blur0Weight = Settings.Blur0Weight;
            blitShader.Blur1Weight = Settings.Blur1Weight;
            blitShader.Blur2Weight = Settings.Blur2Weight;
            blitShader.Blur3Weight = Settings.Blur3Weight;
            blitShader.Blur4Weight = Settings.Blur4Weight;
            blitShader.Blur5Weight = Settings.Blur5Weight;
            blitShader.Blur6Weight = Settings.Blur6Weight;
            blitShader.FinalContrast = Settings.FinalContrast;

            //blitMaterial.HeightBlurTex = heightBlurMap; // not exist in shader
            blitShader.BlurTex = heightBlurMap; // BlurTex == HeightBlurTex maybe ?

            blitShader.MainTex = blurMap0;
            blitShader.BlurTex0 = blurMap0;
            blitShader.BlurTex1 = blurMap1;
            blitShader.BlurTex2 = blurMap2;
            blitShader.BlurTex3 = blurMap3;
            blitShader.BlurTex4 = blurMap4;
            blitShader.BlurTex5 = blurMap5;
            blitShader.BlurTex6 = blurMap6;

            blitShader.Angularity = Settings.Angularity;
            blitShader.AngularIntensity = Settings.AngularIntensity;

            var tempNormalMap = CreateRenderTexture(renderTextureFormat: RenderTextureFormat.ARGB32);

            Graphics.Blit(blurMap0, tempNormalMap, blitShader.Material, 4);

            var normalMap = new Texture2D(tempNormalMap.width, tempNormalMap.height, TextureFormat.ARGB32, true, true);
            normalMap.ReadPixels(new Rect(0, 0, tempNormalMap.width, tempNormalMap.height), 0, 0);
            normalMap.Apply();

            return normalMap;
        }
    }
}

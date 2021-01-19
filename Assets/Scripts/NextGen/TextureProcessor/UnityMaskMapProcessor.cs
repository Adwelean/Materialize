using System;

using Assets.Scripts.NextGen.Extensions;
using Assets.Scripts.NextGen.TextureSettings;

using Cysharp.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.NextGen.TextureProcessor
{
    public class UnityMaskMapProcessor : TextureProcessorBase<UnityMaskMapSettings>
    {
        public UnityMaskMapProcessor(UnityMaskMapSettings textureSettings)
            : base(textureSettings)
        {
        }

        public override UniTask<TextureContext> Process(TextureContext textureContext)
        {
            Texture2D diffuseMap = default;
            Texture2D metalicMap = default;
            Texture2D aoMap = default;
            Texture2D smoothnessMap = default;

            if (textureContext.ProcessorPhase >= ProcessorPhase.UnityMaskMapProcessed)
                return UniTask.FromResult(textureContext);

            if (!textureContext.Textures.TryGetValueAs(TypeMap.Diffuse, out diffuseMap))
                throw new ArgumentNullException($"[Phase: {textureContext.ProcessorPhase}] Missing {nameof(diffuseMap)} texture!");

            if (!textureContext.Textures.TryGetValueAs(TypeMap.Diffuse, out metalicMap))
                throw new ArgumentNullException($"[Phase: {textureContext.ProcessorPhase}] Missing {nameof(metalicMap)} texture!");

            if (!textureContext.Textures.TryGetValueAs(TypeMap.Diffuse, out aoMap))
                throw new ArgumentNullException($"[Phase: {textureContext.ProcessorPhase}] Missing {nameof(aoMap)} texture!");

            if (!textureContext.Textures.TryGetValueAs(TypeMap.Diffuse, out smoothnessMap))
                throw new ArgumentNullException($"[Phase: {textureContext.ProcessorPhase}] Missing {nameof(smoothnessMap)} texture!");

            var maskMap = new Texture2D(diffuseMap.width, diffuseMap.height, TextureFormat.RGBA32, false);

            var color = new Color();
            var defaultTexture = new Texture2D(1, 1);

            for (int x = 0; x < maskMap.width; x++)
            {
                for (int y = 0; y < maskMap.height; y++)
                {
                    color.r = metalicMap.GetPixel(x, y).grayscale;
                    color.g = aoMap.GetPixel(x, y).grayscale;
                    color.b = defaultTexture.GetPixel(x, y).grayscale;
                    color.a = smoothnessMap.GetPixel(x, y).grayscale;

                    maskMap.SetPixel(x, y, color);
                }
            }

            maskMap.Apply();

            textureContext.Textures.Add(TypeMap.UnityMask, maskMap);
            textureContext.ProcessorPhase = ProcessorPhase.UnityMaskMapProcessed;

            return UniTask.FromResult(textureContext);
        }
    }
}

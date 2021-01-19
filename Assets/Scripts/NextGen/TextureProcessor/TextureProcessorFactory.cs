using System;

namespace Assets.Scripts.NextGen.TextureProcessor
{
    public static class TextureProcessorFactory
    {
        public static ITextureProcessor CreateProcessor<T>(MaterializeSettings materializeSettings) where T : ITextureProcessor
        {
            switch (typeof(T).Name)
            {
                case nameof(HeightMapProcessor):
                    return new HeightMapProcessor(materializeSettings.HeightMapSettings);

                case nameof(NormalMapProcessor):
                    return new NormalMapProcessor(materializeSettings.NormalMapSettings);

                case nameof(MetalicMapProcessor):
                    return new MetalicMapProcessor(materializeSettings.MetallicSettings);

                case nameof(SmoothnessMapProcessor):
                    return new SmoothnessMapProcessor(materializeSettings.SmoothnessMapSettings);

                case nameof(EdgeMapProcessor):
                    return new EdgeMapProcessor(materializeSettings.EdgeMapSettings);

                case nameof(AOMapProcessor):
                    return new AOMapProcessor(materializeSettings.AOMapSettings);

                case nameof(UnityMaskMapProcessor):
                    return new UnityMaskMapProcessor(materializeSettings.UnityMaskMapSettings);

                default:
                    throw new NotImplementedException();
            }
        }
    }
}

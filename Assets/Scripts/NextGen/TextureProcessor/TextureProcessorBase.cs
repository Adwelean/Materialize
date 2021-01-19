using Assets.Scripts.NextGen.TextureSettings;

using Cysharp.Threading.Tasks;

namespace Assets.Scripts.NextGen.TextureProcessor
{
    public abstract class TextureProcessorBase<TSettings> : ITextureProcessor
        where TSettings : ITextureSettings
    {
        public TextureProcessorBase(TSettings textureSettings)
        {
            Settings = textureSettings;
        }

        private protected TSettings Settings { get; }

        public abstract UniTask<TextureContext> Process(TextureContext textureContext);
    }
}

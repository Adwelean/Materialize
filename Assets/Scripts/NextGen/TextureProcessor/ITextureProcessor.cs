using Cysharp.Threading.Tasks;

namespace Assets.Scripts.NextGen.TextureProcessor
{
    public interface ITextureProcessor
    {
        UniTask<TextureContext> Process(TextureContext textureContext);
    }
}

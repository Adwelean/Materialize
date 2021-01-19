using Assets.Scripts.NextGen.TextureProcessor;

namespace Assets.Scripts.NextGen.GeneratorDescriptor
{
    public interface IGeneratorDescriptor
    {
        string GetName();
        string GetFileName(string origFileName);
        ITextureProcessor GetProcessor(MaterializeSettings materializeSettings);
    }
}

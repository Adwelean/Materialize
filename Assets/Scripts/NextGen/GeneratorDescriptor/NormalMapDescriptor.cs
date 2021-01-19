using Assets.Scripts.NextGen.TextureProcessor;

namespace Assets.Scripts.NextGen.GeneratorDescriptor
{
    public class NormalMapDescriptor : IGeneratorDescriptor
    {
        public const string MapName = "NormalMap";

        public string GetName() => MapName;
        public string GetFileName(string origFileName) => $"{origFileName}_normalMap.png";
        public ITextureProcessor GetProcessor(MaterializeSettings materializeSettings) => TextureProcessorFactory.CreateProcessor<NormalMapProcessor>(materializeSettings);
    }
}

using Assets.Scripts.NextGen.TextureProcessor;

namespace Assets.Scripts.NextGen.GeneratorDescriptor
{
    public class AOMapDescriptor : IGeneratorDescriptor
    {
        public const string MapName = "AmbientOcclusionMap";

        public string GetName() => MapName;
        public string GetFileName(string origFileName) => $"{origFileName}_aoMap.png";
        public ITextureProcessor GetProcessor(MaterializeSettings materializeSettings) => TextureProcessorFactory.CreateProcessor<AOMapProcessor>(materializeSettings);
    }
}

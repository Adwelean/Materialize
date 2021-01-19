
using Assets.Scripts.NextGen.TextureProcessor;

namespace Assets.Scripts.NextGen.GeneratorDescriptor
{
    public class MetalicMapDescriptor : IGeneratorDescriptor
    {
        public const string MapName = "MetalicMap";

        public string GetName() => MapName;
        public string GetFileName(string origFileName) => $"{origFileName}_metalicMap.png";
        public ITextureProcessor GetProcessor(MaterializeSettings materializeSettings) => TextureProcessorFactory.CreateProcessor<MetalicMapProcessor>(materializeSettings);
    }
}

using Assets.Scripts.NextGen.TextureProcessor;

namespace Assets.Scripts.NextGen.GeneratorDescriptor
{
    public class SmoothnessMapDescriptor : IGeneratorDescriptor
    {
        public const string MapName = "SmoothnessMap";

        public string GetName() => MapName;
        public string GetFileName(string origFileName) => $"{origFileName}_smoothnessMap.png";
        public ITextureProcessor GetProcessor(MaterializeSettings materializeSettings) => TextureProcessorFactory.CreateProcessor<SmoothnessMapProcessor>(materializeSettings);
    }
}

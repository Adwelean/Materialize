using Assets.Scripts.NextGen.TextureProcessor;

namespace Assets.Scripts.NextGen.GeneratorDescriptor
{
    public class EdgeMapDescriptor : IGeneratorDescriptor
    {
        public const string MapName = "EdgeMap";

        public string GetName() => MapName;
        public string GetFileName(string origFileName) => $"{origFileName}_edgeMap.png";
        public ITextureProcessor GetProcessor(MaterializeSettings materializeSettings) => TextureProcessorFactory.CreateProcessor<EdgeMapProcessor>(materializeSettings);
    }
}

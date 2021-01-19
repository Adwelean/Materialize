using Assets.Scripts.NextGen.TextureProcessor;

namespace Assets.Scripts.NextGen.GeneratorDescriptor
{
    public class HeightMapDescriptor : IGeneratorDescriptor
    {
        public const string MapName = "HeightMap";

        public string GetName() => MapName;
        public string GetFileName(string origFileName) => $"{origFileName}_heightMap.png";
        public ITextureProcessor GetProcessor(MaterializeSettings materializeSettings) => TextureProcessorFactory.CreateProcessor<HeightMapProcessor>(materializeSettings);
    }
}

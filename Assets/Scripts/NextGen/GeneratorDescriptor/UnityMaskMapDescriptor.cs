using Assets.Scripts.NextGen.TextureProcessor;

namespace Assets.Scripts.NextGen.GeneratorDescriptor
{
    public class UnityMaskMapDescriptor : IGeneratorDescriptor
    {
        public const string MapName = "UnityMaskMap";

        public string GetName() => MapName;
        public string GetFileName(string origFileName) => $"{origFileName}_maskMap.png";
        public ITextureProcessor GetProcessor(MaterializeSettings materializeSettings) => TextureProcessorFactory.CreateProcessor<UnityMaskMapProcessor>(materializeSettings);
    }
}

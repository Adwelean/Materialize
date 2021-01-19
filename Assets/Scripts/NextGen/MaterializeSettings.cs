using Assets.Scripts.NextGen.TextureSettings;

namespace Assets.Scripts.NextGen
{
    public class MaterializeSettings
    {
        public AOMapSettingsWrapper AOMapSettings { get; set; }
        public EdgeMapSettingsWrapper EdgeMapSettings { get; set; }
        public HeightMapSettingsWrapper HeightMapSettings { get; set; }
        public MetalicMapSettingsWrapper MetallicSettings { get; set; }
        public NormalMapSettingsWrapper NormalMapSettings { get; set; }
        public SmoothnessMapSettingsWrapper SmoothnessMapSettings { get; set; }
        public UnityMaskMapSettings UnityMaskMapSettings { get; set; }

        public static MaterializeSettings Default => new MaterializeSettings
        {
            AOMapSettings = new AOMapSettingsWrapper(),
            EdgeMapSettings = new EdgeMapSettingsWrapper(),
            HeightMapSettings = new HeightMapSettingsWrapper(),
            MetallicSettings = new MetalicMapSettingsWrapper(),
            NormalMapSettings = new NormalMapSettingsWrapper(),
            SmoothnessMapSettings = new SmoothnessMapSettingsWrapper(),
            UnityMaskMapSettings = new UnityMaskMapSettings()
        };
    }
}

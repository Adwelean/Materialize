
using UnityEngine;

namespace Assets.Scripts.NextGen.ShaderWrapper
{
    public class BlitMetallicShaderWrapper : ShaderWrapperBase
    {
        public Texture MainTex { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Vector4 MetalColor { get => GetMaterialProperty<Vector4>(); set => SetMaterialProperty(value: value); }
        public Vector4 SampleUV { get => GetMaterialProperty<Vector4>(); set => SetMaterialProperty(value: value); }
        public float HueWeight { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float SatWeight { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float LumWeight { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float MaskLow { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float MaskHigh { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float DiffuseContrast { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float DiffuseBias { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public Texture BlurTex { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Texture OverlayBlurTex { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public float BlurOverlay { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float FinalContrast { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float FinalBias { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float GamaCorrection { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        private protected override Material CreateMaterial() => new Material(Shader.Find("Hidden/Blit_Metallic"));
    }
}

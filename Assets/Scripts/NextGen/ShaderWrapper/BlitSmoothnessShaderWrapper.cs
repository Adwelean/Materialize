
using UnityEngine;

namespace Assets.Scripts.NextGen.ShaderWrapper
{
    public class BlitSmoothnessShaderWrapper : ShaderWrapperBase
    {
        public Texture MainTex { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Texture BlurTex { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Texture OverlayBlurTex { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Texture MetallicTex { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }

        public Vector4 SampleColor1 { get => GetMaterialProperty<Vector4>(); set => SetMaterialProperty(value: value); }
        public Vector4 SampleUV1 { get => GetMaterialProperty<Vector4>(); set => SetMaterialProperty(value: value); }

        public Vector4 SampleColor2 { get => GetMaterialProperty<Vector4>(); set => SetMaterialProperty(value: value); }
        public Vector4 SampleUV2 { get => GetMaterialProperty<Vector4>(); set => SetMaterialProperty(value: value); }

        public Vector4 SampleColor3 { get => GetMaterialProperty<Vector4>(); set => SetMaterialProperty(value: value); }
        public Vector4 SampleUV3 { get => GetMaterialProperty<Vector4>(); set => SetMaterialProperty(value: value); }

        public float MetalSmoothness { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public int UseSample1 { get => GetMaterialProperty<int>(); set => SetMaterialProperty(value: value); }
        public int IsolateSample1 { get => GetMaterialProperty<int>(); set => SetMaterialProperty(value: value); }
        public float HueWeight1 { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float SatWeight1 { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float LumWeight1 { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float MaskLow1 { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float MaskHigh1 { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float Sample1Smoothness { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public int UseSample2 { get => GetMaterialProperty<int>(); set => SetMaterialProperty(value: value); }
        public int IsolateSample2 { get => GetMaterialProperty<int>(); set => SetMaterialProperty(value: value); }
        public float HueWeight2 { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float SatWeight2 { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float LumWeight2 { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float MaskLow2 { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float MaskHigh2 { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float Sample2Smoothness { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public int UseSample3 { get => GetMaterialProperty<int>(); set => SetMaterialProperty(value: value); }
        public int IsolateSample3 { get => GetMaterialProperty<int>(); set => SetMaterialProperty(value: value); }
        public float HueWeight3 { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float SatWeight3 { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float LumWeight3 { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float MaskLow3 { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float MaskHigh3 { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float Sample3Smoothness { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public float BaseSmoothness { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public float BlurOverlay { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public float FinalContrast { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public float FinalBias { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public float Slider { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public float GamaCorrection { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        private protected override Material CreateMaterial() => new Material(Shader.Find("Hidden/Blit_Smoothness"));
    }
}

using UnityEngine;

namespace Assets.Scripts.NextGen.ShaderWrapper
{
    public class BlitEdgeShaderWrapper : ShaderWrapperBase
    {
        public Texture MainTex { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }

        public float BlurScale { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public Vector4 ImageSize { get => GetMaterialProperty<Vector4>(); set => SetMaterialProperty(value: value); }
        public float GamaCorrection { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public Texture BlurTex0 { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Texture BlurTex1 { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Texture BlurTex2 { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Texture BlurTex3 { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Texture BlurTex4 { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Texture BlurTex5 { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Texture BlurTex6 { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }

        public float Contrast { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public float Blur0Weight { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float Blur0Contrast { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public float Blur1Weight { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float Blur1Contrast { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public float Blur2Weight { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float Blur2Contrast { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public float Blur3Weight { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float Blur3Contrast { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public float Blur4Weight { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float Blur4Contrast { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public float Blur5Weight { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float Blur5Contrast { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public float Blur6Weight { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float Blur6Contrast { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public float FinalBias { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float FinalContrast { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public float Pillow { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float Pinch { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public float IsColor { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        public int FlipNormalY { get => GetMaterialProperty<int>(); set => SetMaterialProperty(value: value); }

        public float EdgeAmount { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float CreviceAmount { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        private protected override Material CreateMaterial() => new Material(Shader.Find("Hidden/Blit_Shader")); // Blit_Edge_From_Normal outdated
    }
}


using UnityEngine;

namespace Assets.Scripts.NextGen.ShaderWrapper
{
    public class BlitShaderWrapper : ShaderWrapperBase
    {
        #region # ShaderProperties #
        public Texture MainTex { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Texture HeightTex { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Texture LightTex { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Texture LightBlurTex { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Texture DiffuseTex { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public float BlurScale { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public Vector4 ImageSize { get => GetMaterialProperty<Vector4>(); set => SetMaterialProperty(value: value); }
        public float GamaCorrection { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public Vector4 BlurDirection { get => GetMaterialProperty<Vector4>(); set => SetMaterialProperty(value: value); }
        public int BlurSamples { get => GetMaterialProperty<int>(); set => SetMaterialProperty(value: value); }
        public float BlurSpread { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public Texture BlurTex0 { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Texture BlurTex1 { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Texture BlurTex2 { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Texture BlurTex3 { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Texture BlurTex4 { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Texture BlurTex5 { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public Texture BlurTex6 { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public float Contrast { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float BlurContrast { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
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
        public float Pinch { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float Pillow { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float EdgeAmount { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float CreviceAmount { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float IsColor { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float Spread { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float Depth { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float AOBlend { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public Texture BlendTex { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public float BlendAmount { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float DiffuseContrast { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float DiffuseBias { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public Texture BlurTex { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public float BlurWeight { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public Texture AvgTex { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public float LightMaskPow { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float LightPow { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float DarkMaskPow { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float DarkPow { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float HotSpot { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float DarkSpot { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float FinalContrast { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float FinalBias { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float FinalGain { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float AngularIntensity { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float Angularity { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float ColorLerp { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float Saturation { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float ShapeRecognition { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float LightRotation { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float ShapeBias { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public int FlipNormalY { get => GetMaterialProperty<int>(); set => SetMaterialProperty(value: value); }
        public float HeightFromNormal { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public int Desaturate { get => GetMaterialProperty<int>(); set => SetMaterialProperty(value: value); }
        public float Progress { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        #endregion

        private protected override Material CreateMaterial() => new Material(Shader.Find("Hidden/Blit_Shader"));
    }
}


using UnityEngine;

namespace Assets.Scripts.NextGen.ShaderWrapper
{
    public class BlitAOShaderWrapper : ShaderWrapperBase
    {
        public Texture MainTex { get => GetMaterialProperty<Texture>(); set => SetMaterialProperty(value: value); }
        public float FinalContrast { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float FinalBias { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }
        public float AOBlend { get => GetMaterialProperty<int>(); set => SetMaterialProperty(value: value); }
        public int ImageSize { get => GetMaterialProperty<int>(); set => SetMaterialProperty(value: value); }
        public int Spread { get => GetMaterialProperty<int>(); set => SetMaterialProperty(value: value); }
        public int HeightTex { get => GetMaterialProperty<int>(); set => SetMaterialProperty(value: value); }
        public int BlendTex { get => GetMaterialProperty<int>(); set => SetMaterialProperty(value: value); }
        public int Depth { get => GetMaterialProperty<int>(); set => SetMaterialProperty(value: value); }
        public int BlendAmount { get => GetMaterialProperty<int>(); set => SetMaterialProperty(value: value); }
        public float Progress { get => GetMaterialProperty<float>(); set => SetMaterialProperty(value: value); }

        private protected override Material CreateMaterial() => new Material(Shader.Find("Hidden/Blit_Shader"));
    }
}

using System;
using System.Runtime.CompilerServices;

using UnityEngine;

namespace Assets.Scripts.NextGen.ShaderWrapper
{
    public abstract class ShaderWrapperBase
    {
        public ShaderWrapperBase()
        {
            Material = CreateMaterial();
        }

        public Material Material { get; }

        private protected abstract Material CreateMaterial();

        private protected T GetMaterialProperty<T>([CallerMemberName] string propertyName = "")
        {
            switch (typeof(T).Name)
            {
                case nameof(Single):
                    return (T)Convert.ChangeType(Material.GetFloat(Shader.PropertyToID($"_{propertyName}")), typeof(T));

                case nameof(Int32):
                    return (T)Convert.ChangeType(Material.GetInt(Shader.PropertyToID($"_{propertyName}")), typeof(T));

                case nameof(Texture):
                    return (T)Convert.ChangeType(Material.GetTexture(Shader.PropertyToID($"_{propertyName}")), typeof(T));

                case nameof(Vector2):
                case nameof(Vector4):
                    return (T)Convert.ChangeType(Material.GetVector(Shader.PropertyToID($"_{propertyName}")), typeof(T));

                default:
                    throw new NotImplementedException();
            }
        }

        private protected void SetMaterialProperty([CallerMemberName] string propertyName = "", object value = default)
        {
            switch (value.GetType().Name)
            {
                case nameof(Single):
                    Material.SetFloat(Shader.PropertyToID($"_{propertyName}"), (float)value);
                    break;

                case nameof(Int32):
                    Material.SetInt(Shader.PropertyToID($"_{propertyName}"), (int)value);
                    break;

                case nameof(Texture2D):
                case nameof(RenderTexture):
                    Material.SetTexture(Shader.PropertyToID($"_{propertyName}"), (Texture)value);
                    break;

                case nameof(Vector2):
                case nameof(Vector4):
                    Material.SetVector(Shader.PropertyToID($"_{propertyName}"), (Vector4)value);
                    break;
            }
        }
    }
}

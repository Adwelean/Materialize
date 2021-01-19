using System;
using System.Collections.Generic;

using Assets.Scripts.NextGen.Extensions;

using UnityEngine;

namespace Assets.Scripts.NextGen.TextureProcessor
{
    public enum TypeMap
    {
        Diffuse,
        Height,
        HeightHD,
        Normal,
        Metalic,
        Smoothness,
        Edge,
        AO,
        UnityMask
    }

    public enum ProcessorPhase
    {
        None,
        HeightMapProcessed,
        NormalMapProcessed,
        MetalicMapProcessed,
        SmoothnessMapProcessed,
        EdgeMapProcessed,
        AOMapProcessed,
        UnityMaskMapProcessed
    }

    public class TextureContext
    {
        public ProcessorPhase ProcessorPhase { get; set; } = ProcessorPhase.None;

        public Dictionary<TypeMap, Texture> Textures { get; set; } = new Dictionary<TypeMap, Texture>();

        public Texture GetCurrentMap()
        {
            var typeMap = ProcessorPhase switch
            {
                ProcessorPhase.None => TypeMap.Diffuse,
                ProcessorPhase.HeightMapProcessed => TypeMap.Height,
                ProcessorPhase.NormalMapProcessed => TypeMap.Normal,
                ProcessorPhase.MetalicMapProcessed => TypeMap.Metalic,
                ProcessorPhase.SmoothnessMapProcessed => TypeMap.Smoothness,
                ProcessorPhase.EdgeMapProcessed => TypeMap.Edge,
                ProcessorPhase.AOMapProcessed => TypeMap.AO,
                ProcessorPhase.UnityMaskMapProcessed => TypeMap.UnityMask,
                _ => throw new ArgumentException("Not supported")
            };

            Textures.TryGetValueAs(typeMap, out Texture texture);

            return texture;
        }
    }
}

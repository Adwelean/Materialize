using System;
using System.IO;

using Assets.Scripts.NextGen.GeneratorDescriptor;

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

using UnityEngine;

using HeightMap = Assets.Scripts.NextGen.GeneratorDescriptor.HeightMapDescriptor;
using NormalMap = Assets.Scripts.NextGen.GeneratorDescriptor.NormalMapDescriptor;
using MetalicMap = Assets.Scripts.NextGen.GeneratorDescriptor.MetalicMapDescriptor;
using SmoothnessMap = Assets.Scripts.NextGen.GeneratorDescriptor.SmoothnessMapDescriptor;
using EdgeMap = Assets.Scripts.NextGen.GeneratorDescriptor.EdgeMapDescriptor;
using AOMap = Assets.Scripts.NextGen.GeneratorDescriptor.AOMapDescriptor;
using UnityMaskMap = Assets.Scripts.NextGen.GeneratorDescriptor.UnityMaskMapDescriptor;

using Assets.Scripts.NextGen.TextureProcessor;
using Assets.Scripts.NextGen.Extensions;

namespace Assets.Scripts.NextGen
{
    public class MaterializeManager
    {
        public MaterializeManager(string outputProcessResultPath)
        {
            OutputProcessResultPath = outputProcessResultPath;
        }

        private string OutputProcessResultPath { get; }
        private string ErrorLogFilePath => Path.Combine(OutputProcessResultPath, "error.log");

        public async UniTask<bool> Generate(IUniTaskAsyncEnumerable<string> textureFilesPaths, MaterializeSettings materializeSettings = default)
        {
            bool ret = true;

            await textureFilesPaths.ForEachAwaitAsync(async textureFilePath =>
            {
                try
                {
                    await Generate(textureFilePath, materializeSettings ?? MaterializeSettings.Default);
                }
                catch (Exception ex)
                {
                    ret = false;
                }
            });

            return ret;
        }

        public async UniTask<bool> Generate(string textureFilePath, MaterializeSettings materializeSettings = default)
        {
            if (materializeSettings == default)
                materializeSettings = MaterializeSettings.Default;

            var diffuseTextureFileInfo = new FileInfo(textureFilePath);

            if (!diffuseTextureFileInfo.Exists)
            {
                File.AppendAllText(ErrorLogFilePath, $"[{DateTimeOffset.Now:G}] File {textureFilePath} does not exist.");

                return false;
            }

            var diffuseTextureFileName = diffuseTextureFileInfo.Name.Replace(diffuseTextureFileInfo.Extension, "");
            var diffuseTextureFileRawData = File.ReadAllBytes(textureFilePath);
            var diffuseMap = new Texture2D(2, 2);

            if (!diffuseMap.LoadImage(diffuseTextureFileRawData))
            {
                Debug.LogError($"Cannot load texture data. ({diffuseTextureFileInfo.FullName})");
                File.AppendAllText(ErrorLogFilePath, $"[{DateTimeOffset.Now:G}] Cannot load texture data ({diffuseTextureFileInfo.FullName}).");
                
                return false;
            }

            var textureContext = new TextureContext();
            textureContext.Textures.Add(TypeMap.Diffuse, diffuseMap);

            textureContext = await Generate<HeightMap>(textureContext, diffuseTextureFileInfo.DirectoryName, diffuseTextureFileName, materializeSettings);
            textureContext = await Generate<NormalMap>(textureContext, diffuseTextureFileInfo.DirectoryName, diffuseTextureFileName, materializeSettings);
            textureContext = await Generate<MetalicMap>(textureContext, diffuseTextureFileInfo.DirectoryName, diffuseTextureFileName, materializeSettings);
            textureContext = await Generate<SmoothnessMap>(textureContext, diffuseTextureFileInfo.DirectoryName, diffuseTextureFileName, materializeSettings);
            textureContext = await Generate<EdgeMap>(textureContext, diffuseTextureFileInfo.DirectoryName, diffuseTextureFileName, materializeSettings);
            textureContext = await Generate<AOMap>(textureContext, diffuseTextureFileInfo.DirectoryName, diffuseTextureFileName, materializeSettings);
            textureContext = await Generate<UnityMaskMap>(textureContext, diffuseTextureFileInfo.DirectoryName, diffuseTextureFileName, materializeSettings);

            return await UniTask.FromResult(true);
        }

        private async UniTask<TextureContext> Generate<T>(TextureContext textureContext, string directory, string diffuseMapFileName, MaterializeSettings materializeSettings)
            where T : IGeneratorDescriptor
        {
            IGeneratorDescriptor descriptor = Activator.CreateInstance<T>();

            try
            {
                var processor = descriptor.GetProcessor(materializeSettings);

                textureContext = await processor.Process(textureContext);

                var textureMap = (Texture2D)textureContext.GetCurrentMap();

                File.WriteAllBytes(Path.Combine(directory, descriptor.GetFileName(diffuseMapFileName)), textureMap.EncodeToPNG());
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to generate {descriptor.GetName()} for file {diffuseMapFileName}");
                Debug.LogException(ex);

                throw;
            }

            return textureContext;
        }
    }
}

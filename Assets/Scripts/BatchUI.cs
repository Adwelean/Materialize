using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SFB;
using System.Linq;
using System;
using System.Threading.Tasks;

using Cysharp.Threading.Tasks;
using System.Threading;
using Cysharp.Threading.Tasks.Linq;
using Assets.Scripts.NextGen;

public class BatchUI : MonoBehaviour
{
    public MainGui MainGui;
   // public HeightFromDiffuseGui HeightmapCreator;
    public bool UseInitalLocation = true;
    bool PathIsSet = true;
    //string path = null;
    public bool ProcessPropertyMap;
    // Start is called before the first frame update
    void Start()
    {
        MainGui = FindObjectOfType<MainGui>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// use inital location toggle
    /// </summary>
    /// <param name="value">what the value is</param>
    public void UseInitalLocationToggle(bool value) { UseInitalLocation = value; }

    /// <summary>
    /// Loads all the textures for batching.
    /// </summary>
    public void BatchLoadTextures()
    {
        StartCoroutine(BatchProcessTextures());
    }

    public async UniTask StartBatch(string outputProcessResultPath, string[] filePaths)
    {
        await new MaterializeManager(outputProcessResultPath).Generate(filePaths.ToUniTaskAsyncEnumerable());
    }

    public async UniTask StartBatch(string outputProcessResultPath, string path)
    {
        /*var fileNames = Directory
            .GetFiles(path, "*.*")
            .Where(x => x.EndsWith(".jpg") || x.EndsWith(".png"))
            .Where(fileName => !new string[] { "_ao", "_height", "edge", "smooth", "_metal", "_normal" }.Any(x => fileName.Contains(x)));

        foreach(var fileName in fileNames)
        {
            byte[] data = System.IO.File.ReadAllBytes(fileName);
            Texture2D texture = new Texture2D(2, 2);

            if (texture.LoadImage(data))
            {
                Debug.Log("Start Batch");
                await BatchTextures(texture, fileName);
                Debug.Log("End Batch");
            }
        }*/

        Predicate<string> filter = fileName => fileName.EndsWith(".jpg") || fileName.EndsWith(".png") && !new string[] { "_ao", "_height", "_edge", "_smooth", "_metal", "_normal", "_mask" }.Any(x => fileName.Contains(x));

        var manager = new MaterializeManager(outputProcessResultPath);

        var filesPaths = EnumerateFilesRecursivelyAsync(path, filter);

        await manager.Generate(filesPaths.Take(1));

        //await foreach (var fileName in EnumerateFilesRecursivelyAsync(path, filter))
        /*await EnumerateFilesRecursivelyAsync(path, filter).ForEachAwaitAsync(async fileName =>
        {
            byte[] data = System.IO.File.ReadAllBytes(fileName);
            Texture2D texture = new Texture2D(2, 2);

            if (texture.LoadImage(data))
            {
                var fi = new FileInfo(fileName);

                Debug.Log("Start Batch, dir: " + fi.DirectoryName + ", fileName: " + fi.Name);
                //await BatchTextures(texture, fi.Name, fi.DirectoryName).ToUniTask(this);
                Debug.Log("End Batch");
            }
        });*/
    }

    private IUniTaskAsyncEnumerable<string> EnumerateFilesRecursivelyAsync(string path, Predicate<string> condition)
    {
        return UniTaskAsyncEnumerable.Create<string>(async (writer, token) =>
        {
            try
            {
                await UniTask.Yield();

                TraverseTreeParallelForEach(
                    path, 
                    async (entry) => 
                    { 
                        if (condition.Invoke(entry)) 
                            await writer.YieldAsync(entry); 
                    }
                );

                await UniTask.Yield();
            }
            catch (ArgumentException)
            {
                // Directory not found
            }
        });
    }

    private void TraverseTreeParallelForEach(string root, Action<string> action)
    {
        //Count of files traversed and timer for diagnostic output
        int fileCount = 0;
        var sw = System.Diagnostics.Stopwatch.StartNew();

        // Determine whether to parallelize file processing on each folder based on processor count.
        int procCount = System.Environment.ProcessorCount;

        // Data structure to hold names of subfolders to be examined for files.
        Stack<string> dirs = new Stack<string>();

        if (!Directory.Exists(root))
        {
            throw new ArgumentException();
        }
        dirs.Push(root);

        while (dirs.Count > 0)
        {
            string currentDir = dirs.Pop();
            string[] subDirs = { };
            string[] files = { };

            try
            {
                subDirs = Directory.GetDirectories(currentDir);
            }
            // Thrown if we do not have discovery permission on the directory.
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
                continue;
            }
            // Thrown if another process has deleted the directory after we retrieved its name.
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
                continue;
            }

            try
            {
                files = Directory.GetFiles(currentDir);
            }
            catch (UnauthorizedAccessException e)
            {
                Debug.Log(e.Message);
                continue;
            }
            catch (DirectoryNotFoundException e)
            {
                Debug.Log(e.Message);
                continue;
            }
            catch (IOException e)
            {
                Debug.Log(e.Message);
                continue;
            }

            // Execute in parallel if there are enough files in the directory.
            // Otherwise, execute sequentially.Files are opened and processed
            // synchronously but this could be modified to perform async I/O.
            try
            {
                if (files.Length < procCount)
                {
                    foreach (var file in files)
                    {
                        action(file);
                        fileCount++;
                    }
                }
                else
                {
                    Parallel.ForEach(files, () => 0, (file, loopState, localCount) =>
                    {
                        action(file);
                        return (int)++localCount;
                    },
                    (c) => {
                        Interlocked.Add(ref fileCount, c);
                    });
                }
            }
            catch (AggregateException ae)
            {
                ae.Handle((ex) => {
                    if (ex is UnauthorizedAccessException)
                    {
                        // Here we just output a message and go on.
                        Debug.Log(ex.Message);
                        return true;
                    }
                    // Handle other exceptions here if necessary...

                    return false;
                });
            }

            // Push the subdirectories onto the stack for traversal.
            // This could also be done before handing the files.
            foreach (string str in subDirs)
                dirs.Push(str);
        }

        // For diagnostic purposes.
        Debug.Log($"Processed {fileCount} files in {sw.ElapsedMilliseconds} milliseconds");
    }

    /// <summary>
    /// Processes all the textures and saves them out.
    /// </summary>
    /// <returns> IEnum</returns>
    private IEnumerator BatchProcessTextures()
    {
        var paths = StandaloneFileBrowser.OpenFolderPanel("Texture Files Location", "", false);

        //var path = StandaloneFileBrowser.SaveFilePanel("Texture Directory", "", "","");
        var s = Directory.GetFiles(paths[0], "*.*").Where(g => g.EndsWith(".jpg") || g.EndsWith(".png"));
        foreach (string f in s)
        {
            //BatchProcessTextures(f);

            byte[] Data = System.IO.File.ReadAllBytes(f);
            Texture2D Tex = new Texture2D(2, 2);
            if (Tex.LoadImage(Data))
            {
                yield return StartCoroutine(BatchTextures(Tex, f, paths[0]));

                //MainGui.HeightFromDiffuseGuiScript.StartCoroutine(ProcessHeight());
                //return null;
            }
        }
        //return null;
    }

    /// <summary>
    /// Batch runs all the textures to output them to a file location
    /// </summary>
    /// <param name="T">Texture to output</param>
    /// <param name="name">Name of texture</param>
    /// <returns> IEnum</returns>
    IEnumerator BatchTextures(Texture2D T, string name, string path)
    {
        MainGui.DiffuseMapOriginal = T;
        MainGui.HeightFromDiffuseGuiObject.SetActive(true);
        MainGui.HeightFromDiffuseGuiScript.NewTexture();
        MainGui.HeightFromDiffuseGuiScript.DoStuff();
        //yield return MainGui.HeightFromDiffuseGuiScript.StartCoroutine(MainGui.HeightFromDiffuseGuiScript.ProcessDiffuse());
        yield return new WaitForSeconds(.1f);
        MainGui.HeightFromDiffuseGuiScript.StartProcessHeight();
        MainGui.CloseWindows();
        MainGui.FixSize();
        MainGui.NormalFromHeightGuiObject.SetActive(true);
        MainGui.NormalFromHeightGuiScript.NewTexture();
        MainGui.NormalFromHeightGuiScript.DoStuff();
        //yield return MainGui.NormalFromHeightGuiScript.StartCoroutine(MainGui.NormalFromHeightGuiScript.ProcessHeight());
        yield return new WaitForSeconds(.1f);
        MainGui.NormalFromHeightGuiScript.StartProcessNormal();
        yield return new WaitForEndOfFrame();
        MainGui.CloseWindows();
        MainGui.FixSize();
        MainGui.MetallicMap = new Texture2D(MainGui.HeightMap.width, MainGui.HeightMap.height);
        Color theColor = new Color();
        for (int x = 0; x < MainGui.MetallicMap.width; x++)
        {
            for (int y = 0; y < MainGui.MetallicMap.height; y++)
            {
                theColor.r = 0;
                theColor.g = 0;
                theColor.b = 0;
                theColor.a = 255;
                MainGui.MetallicMap.SetPixel(x, y, theColor);
            }
        }
        // MainGui.MetallicGuiObject.SetActive(true);
        //MainGui.MetallicGuiScript.NewTexture();
        //MainGui.MetallicGuiScript.DoStuff();
        //yield return new WaitForSeconds(.1f);
        //MainGui.MetallicGuiScript.StartCoroutine(MainGui.MetallicGuiScript.ProcessMetallic());
        MainGui.MetallicMap.Apply();
        MainGui.CloseWindows();
        MainGui.FixSize();
        MainGui.SmoothnessGuiObject.SetActive(true);
        MainGui.SmoothnessGuiScript.NewTexture();
        MainGui.SmoothnessGuiScript.DoStuff();
        yield return new WaitForSeconds(.1f);
        MainGui.SmoothnessGuiScript.StartCoroutine(MainGui.SmoothnessGuiScript.ProcessSmoothness());
        MainGui.CloseWindows();
        MainGui.FixSize();
        MainGui.EdgeFromNormalGuiObject.SetActive(true);
        MainGui.EdgeFromNormalGuiScript.NewTexture();
        MainGui.EdgeFromNormalGuiScript.DoStuff();
        yield return new WaitForSeconds(.1f);
        MainGui.EdgeFromNormalGuiScript.StartCoroutine(MainGui.EdgeFromNormalGuiScript.ProcessEdge());
        MainGui.CloseWindows();
        MainGui.FixSize();
        MainGui.AoFromNormalGuiObject.SetActive(true);
        MainGui.AoFromNormalGuiScript.NewTexture();
        MainGui.AoFromNormalGuiScript.DoStuff();
        yield return new WaitForSeconds(.1f);
        MainGui.AoFromNormalGuiScript.StartCoroutine(MainGui.AoFromNormalGuiScript.ProcessAo());

        yield return new WaitForSeconds(.3f);
        
        List<string> names = name.Split( new string[] { "/", "\\" }, StringSplitOptions.None).ToList<string>();

        string defaultName = names[names.Count - 1];
        Debug.Log(defaultName);
        names = defaultName.Split('.').ToList<string>();
        defaultName = names[0];
        string NameWithOutExtension = defaultName;
        defaultName = defaultName + ".mtz";
        
        /*if (UseInitalLocation)
        {
            if (!PathIsSet)
            {
                path = StandaloneFileBrowser.SaveFilePanel("Save Project", MainGui._lastDirectory, defaultName, "mtz");
                //if (path.IsNullOrEmpty()) return;
                PathIsSet = true;
                var lastBar = path.LastIndexOf(MainGui._pathChar);
                MainGui._lastDirectory = path.Substring(0, lastBar + 1);

            }
            else
            {
                /*List<string> PathSplit = path.Split(new string[] { "/", "\\" }, StringSplitOptions.None).ToList<string>();
                //PathSplit[PathSplit.Length - 1]
                PathSplit.RemoveAt(PathSplit.Count - 1);
                //Debug.Log(PathSplit);
                path = string.Join("/" , PathSplit.ToArray());
                path = path+ "/" + defaultName;
                Debug.Log(defaultName);
                //var lastBar = path.LastIndexOf(MainGui._pathChar);
                //MainGui._lastDirectory = path.Substring(0, lastBar + 1);
            }
        }
        else
        {
            path = StandaloneFileBrowser.SaveFilePanel("Save Project", MainGui._lastDirectory, defaultName, "mtz");
            var lastBar = path.LastIndexOf(MainGui._pathChar);
            MainGui._lastDirectory = path.Substring(0, lastBar + 1);
        }*/

        Debug.Log(path);
        MainGui._saveLoadProjectScript.SaveProject(path);
        yield return new WaitForSeconds(1f);
        if (ProcessPropertyMap)
        {
            MainGui.ProcessPropertyMap();
            MainGui.SaveTextureFile(MapType.Property, path, NameWithOutExtension);
        }
        //return null;
    }
}

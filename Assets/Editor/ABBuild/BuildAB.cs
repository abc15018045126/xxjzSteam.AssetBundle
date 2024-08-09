using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.ABBuild
{
    static class BuildAB
    {
        const string workshopDir = @"Assets/ABWork";

        [MenuItem("BuildAB/AutoReset AB Name", false, 30)]
        public static void ResetABNames()
        {
            var abnp = new ABNameProc(workshopDir);
            abnp.Run();
        }

        [MenuItem("BuildAB/AutoClear AB Name", false, 30)]
        public static void ClearAllABNames()
        {
            var ans = AssetDatabase.GetAllAssetBundleNames();
            for (int i = 0; i < ans.Length; i++)
            {
                if (ans[i].StartsWith("abfixed/"))
                {
                    continue;
                }
                EditorUtility.DisplayProgressBar("Clear All AB Names...", ans[i], Mathf.InverseLerp(0, ans.Length - 1, i));
                AssetDatabase.RemoveAssetBundleName(ans[i], true);
            }
            AssetDatabase.Refresh();
            AssetDatabase.RemoveUnusedAssetBundleNames();
            EditorUtility.ClearProgressBar();
        }

        private static void HashAssetBundles(AssetBundleManifest assetBundleManifest, string assetBundleManifestFilePath, string outputPath)
        {
            var inputPath = Path.GetDirectoryName(assetBundleManifestFilePath);
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            else
            {
                Tools.ClearDirectory(outputPath);
            }

            //var mfab = AssetBundle.LoadFromFile(assetBundleManifestFilePath);
            //if (mfab == null)
            //{
            //    EditorUtility.DisplayDialog("", "请先构建AB文件", "Finish", "Close");
            //    return;
            //}
            //var assetBundleManifest = mfab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            //mfab.Unload(false);

            var fileSizeInfo = new Dictionary<string, int>();
            var bundleNames = assetBundleManifest.GetAllAssetBundles();
            for (var i = 0; i < bundleNames.Length; i++)
            {
                var oldPath = Path.Combine(inputPath, bundleNames[i]);
                var hash = assetBundleManifest.GetAssetBundleHash(bundleNames[i]).ToString();
                var newPath = Path.Combine(outputPath, hash);

                FileInfo fi = new FileInfo(oldPath);
                fi.CopyTo(newPath, true);
                fileSizeInfo.Add(hash, (int)fi.Length);
            }
            // copy manifest file
            var manifestFileName = Path.GetFileName(assetBundleManifestFilePath);
            File.Copy(assetBundleManifestFilePath, Path.Combine(outputPath, manifestFileName), true);
            // generate ver file
            using (var ms = new MemoryStream())
            {
                var bw = new BinaryWriter(ms);
                var manifestFileMD5 = Tools.CalculateMD5(new FileStream(assetBundleManifestFilePath, FileMode.Open));
                bw.Write(manifestFileMD5);
                foreach (var kv in fileSizeInfo)
                {
                    bw.Write(kv.Key);
                    bw.Write(kv.Value);
                }
                var verData = ms.ToArray();
                var cd = LZMAHelper.Compress(verData);
                File.WriteAllBytes(Path.Combine(outputPath, "ver"), cd);
            }
        }

        [MenuItem("BuildAB/Build Windows(ForceRebuild)", false, 30)]
        static void BuildStandaloneWindowsForceRebuild()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows &&
                EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows64)
            {
                var str = string.Format("当前是[{0}],请先切换到Windows平台", EditorUserBuildSettings.activeBuildTarget);
                EditorUtility.DisplayDialog("", str, "Close");
                return;
            }
            var outputPath = "../AssetBundles/windows";
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            var abm = BuildPipeline.BuildAssetBundles(outputPath,

                BuildAssetBundleOptions.ChunkBasedCompression |
                BuildAssetBundleOptions.ForceRebuildAssetBundle |
                BuildAssetBundleOptions.DeterministicAssetBundle |
                BuildAssetBundleOptions.DisableLoadAssetByFileNameWithExtension,

                BuildTarget.StandaloneWindows
            );

            if (abm != null)
            {
                var manifestFileName = Path.GetFileNameWithoutExtension(outputPath);
                var manifestFilePath = Path.Combine(outputPath, manifestFileName);
                HashAssetBundles(abm, manifestFilePath, outputPath + ".hash");

                EditorUtility.DisplayDialog("Build StandaloneWindows", "Finish", "Close");
            }
            else
            {
                EditorUtility.DisplayDialog("", "构建AB文件失败", "Close");
            }
        }

        [MenuItem("BuildAB/Build Windows", false, 30)]
        static void BuildStandaloneWindows()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows &&
                EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows64)
            {
                var str = string.Format("当前是[{0}],请先切换到Windows平台", EditorUserBuildSettings.activeBuildTarget);
                EditorUtility.DisplayDialog("", str, "Close");
                return;
            }

            var outputPath = "../AssetBundles/windows";
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            var abm = BuildPipeline.BuildAssetBundles(outputPath,

                BuildAssetBundleOptions.ChunkBasedCompression |
                BuildAssetBundleOptions.DeterministicAssetBundle |
                BuildAssetBundleOptions.DisableLoadAssetByFileNameWithExtension,

                BuildTarget.StandaloneWindows
            );

            if (abm != null)
            {
                var manifestFileName = Path.GetFileNameWithoutExtension(outputPath);
                var manifestFilePath = Path.Combine(outputPath, manifestFileName);
                HashAssetBundles(abm, manifestFilePath, outputPath + ".hash");

                EditorUtility.DisplayDialog("Build StandaloneWindows", "Finish", "Close");
            }
            else
            {
                EditorUtility.DisplayDialog("", "构建AB文件失败", "Close");
            }
        }


        [MenuItem("BuildAB/Build Android", false, 30)]
        static void BuildAndroid()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            {
                EditorUtility.DisplayDialog("", "请先切换到Android平台!", "Close");
                return;
            }
            var outputPath = "../AssetBundles/android";
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            var abm = BuildPipeline.BuildAssetBundles(outputPath,

                BuildAssetBundleOptions.ChunkBasedCompression |
                BuildAssetBundleOptions.DeterministicAssetBundle |
                BuildAssetBundleOptions.DisableLoadAssetByFileNameWithExtension,
                BuildTarget.Android);

            if (abm != null)
            {
                var manifestFileName = Path.GetFileNameWithoutExtension(outputPath);
                var manifestFilePath = Path.Combine(outputPath, manifestFileName);
                HashAssetBundles(abm, manifestFilePath, outputPath + ".hash");
                EditorUtility.DisplayDialog("Build Android", "Finish", "Close");
            }
            else
            {
                EditorUtility.DisplayDialog("", "构建AB文件失败", "Close");
            }
        }

        [MenuItem("BuildAB/Build IOS", false, 30)]
        static void BuildIOS()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
            {
                EditorUtility.DisplayDialog("", "请先切换到IOS平台!", "Close");
                return;
            }
            var outputPath = "../AssetBundles/ios";
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            var abm = BuildPipeline.BuildAssetBundles(outputPath,

                BuildAssetBundleOptions.ChunkBasedCompression |
                BuildAssetBundleOptions.DeterministicAssetBundle |
                BuildAssetBundleOptions.DisableLoadAssetByFileNameWithExtension,
                BuildTarget.iOS);

            if (abm != null)
            {
                var manifestFileName = Path.GetFileNameWithoutExtension(outputPath);
                var manifestFilePath = Path.Combine(outputPath, manifestFileName);
                HashAssetBundles(abm, manifestFilePath, outputPath + ".hash");
                EditorUtility.DisplayDialog("Build IOS", "Finish", "Close");
            }
            else
            {
                EditorUtility.DisplayDialog("", "构建AB文件失败", "Close");
            }
        }
    }
}
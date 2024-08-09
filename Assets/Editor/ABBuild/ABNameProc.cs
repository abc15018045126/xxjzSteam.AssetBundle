using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace Assets.Editor.ABBuild
{
    public class ABNameProc
    {
        private readonly string projectDataPath;
        private readonly string workshopDir;

        private readonly HashSet<string> buildInShader = new HashSet<string>();

        public ABNameProc(string workshopPath)
        {
            projectDataPath = Application.dataPath;
            this.workshopDir = workshopPath;
            this.InitAllProjectAssets();
            this.InitWorkshopAsserts();
            this.InitWorkshopPrefabs();
            this.InitWorkshopPrefabRefs();
            this.InitBuildInShadersList();
        }
        private void InitBuildInShadersList()
        {
            const string GraphicsSettingsAssetPath = "ProjectSettings/GraphicsSettings.asset";
            SerializedObject graphicsManager = new SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath(GraphicsSettingsAssetPath)[0]);
            SerializedProperty alwayIncludeShaders = graphicsManager.FindProperty("m_AlwaysIncludedShaders");

            var e = alwayIncludeShaders.GetEnumerator();
            while (e.MoveNext())
            {
                SerializedProperty p = (SerializedProperty)e.Current;
                var path = AssetDatabase.GetAssetPath(p.objectReferenceValue);
                buildInShader.Add(path);
            }
        }

        readonly Dictionary<string, string> ABNamesDic = new Dictionary<string, string>();
        bool TrySetABName(string assetPath, string abName)
        {
            if (ABNamesDic.TryGetValue(assetPath, out string existedABName) && GetABNameLevel(existedABName) > GetABNameLevel(abName))
            {
                return false;
            }
            if (string.IsNullOrEmpty(abName))
            {
                ABNamesDic[assetPath] = null;
            }
            else
            {
                ABNamesDic[assetPath] = Path.ChangeExtension(abName, null).ToLower();
            }
            return true;
        }

        static int GetABNameLevel(string abName)
        {
            if (abName.StartsWith("abfixed/"))
            {
                return 10000;
            }
            if (abName.StartsWith("global/"))
            {
                return 1000;
            }
            if (abName.StartsWith("d_"))
            {
                var splitIdx = abName.IndexOf('/');
                if (splitIdx >= 0)
                {
                    var str = abName.Substring(2, splitIdx - 2);
                    if (int.TryParse(str, out int ret))
                    {
                        return ret;
                    }
                }
            }
            return 100;
        }

        readonly List<string> allProjectAssets = new List<string>();
        void InitAllProjectAssets()
        {
            var allAssets = AssetDatabase.GetAllAssetPaths();

            foreach (var _path in allAssets)
            {
                if (Path.IsPathRooted(_path) && !_path.StartsWith(projectDataPath))
                {
                    continue;
                }
                if (!_path.StartsWith("Assets/"))
                {
                    continue;
                }
                if (_path.EndsWith(".dll"))
                {
                    continue;
                }
                if (Directory.Exists(_path))
                {
                    continue;
                }
                var type = AssetDatabase.GetMainAssetTypeAtPath(_path);
                if (type == typeof(MonoScript))
                {
                    continue;
                }
                allProjectAssets.Add(_path);
            }

            var guidmap = new Dictionary<string , string>();
            allProjectAssets.ForEach((e) =>
            {
                var guid = AssetDatabase.AssetPathToGUID(e);
                if (guidmap.ContainsKey(guid))
                {
                    var old = guidmap[guid];
                    Debug.LogErrorFormat("guid[{0}] conflict: {0}->{1}",guid, old, e);
                }
                else
                {
                    guidmap.Add(guid, e);
                }
            });
        }

        List<string> workshopAsserts;
        void InitWorkshopAsserts()
        {
            workshopAsserts = allProjectAssets.FindAll((_path) =>
            {
                return _path.StartsWith(workshopDir);
            });
        }

        string[] workshopPrefabs;
        void InitWorkshopPrefabs()
        {
            var prefabList = workshopAsserts.FindAll((e) =>
            {
                return e.EndsWith(".prefab");
            });
            workshopPrefabs = prefabList.ToArray();
        }

        readonly Dictionary<string, HashSet<string>> workshopPrefabAllRefs = new Dictionary<string, HashSet<string>>();
        readonly Dictionary<string, HashSet<string>> dependsRefs = new Dictionary<string, HashSet<string>>();
        readonly HashSet<string> hadProcRefs = new  HashSet<string>();
        void InitWorkshopPrefabRefs()
        {
            string _asset;
            string[] _deps;
            float progress;
            // -- workshopPrefabAllRefs
            for (int i = 0; i < workshopPrefabs.Length; i++)
            {
                _asset = workshopPrefabs[i];
                // -- show progress
                progress = Mathf.InverseLerp(0, workshopPrefabs.Length - 1, i);
                EditorUtility.DisplayProgressBar("Init workshop prefab all dependencies reference...", _asset, progress);
                // -- 
                _deps = AssetDatabase.GetDependencies(_asset);
                _deps = Array.FindAll(_deps, (key) =>
                {
                    if (Path.IsPathRooted(key) && !key.StartsWith(projectDataPath))
                    {
                        return false;
                    }
                    var type = AssetDatabase.GetMainAssetTypeAtPath(key);
                    if (type == typeof(MonoScript))
                    {
                        return false;
                    }
                    return true;
                });
                Tools.Add(workshopPrefabAllRefs, _deps, _asset);
            }
            // -- dependsRefs
            foreach (var kv in workshopPrefabAllRefs)
            {
                dependsRefs.Add(kv.Key, new HashSet<string>(kv.Value));
            }
            EditorUtility.ClearProgressBar();
        }

        void DoPackShaders()
        {
            foreach (var kv in workshopPrefabAllRefs)
            {
                var type = AssetDatabase.GetMainAssetTypeAtPath(kv.Key);
                if (type == typeof(Shader))
                {
                    if (!buildInShader.Contains(kv.Key) && kv.Value.Count >= 1)
                    {
                        UnityEngine.Rendering.VertexAttribute gs;
                        this.TrySetABName(kv.Key, "global/shaderslist");
                        var deps = AssetDatabase.GetDependencies(kv.Key);
                        foreach (var d in deps)
                        {
                            if (AssetDatabase.GetMainAssetTypeAtPath(d) != typeof(Shader))
                            {
                                this.TrySetABName(d, "global/shaderslist_deps");
                            }
                        }
                    }
                    else
                    {
                        this.TrySetABName(kv.Key, null);
                    }
                }
            }
        }


        void DoFonts()
        {
            var workshopFontDir = workshopDir + "/font";
            var fontList = workshopAsserts.FindAll((e) =>
            {
                return e.StartsWith(workshopFontDir);
            });

            for (var i = 0; i < fontList.Count; i++)
            {
                var ap = fontList[i];
                var px = Tools.GetRelativePathWithoutExtension(ap, workshopDir);
                this.TrySetABName(ap, "global/font_common");

                var deps = AssetDatabase.GetDependencies(ap);
                foreach (var d in deps)
                {
                    if (!d.StartsWith(workshopFontDir))
                    {
                        this.TrySetABName(d, "global/font_deps");
                    }
                }
            }
        }

        void DoAudios()
        {
            for (var i = 0; i < workshopAsserts.Count; i++)
            {
                var ap = workshopAsserts[i];
                var type = AssetDatabase.GetMainAssetTypeAtPath(ap);
                if (type == typeof(AudioClip))
                {
                    this.TrySetABName(ap, Tools.GetRelativePathWithoutExtension(ap, workshopDir));
                }
            }
        }

        void DoTilemapData()
        {
            var tilemapdataDir = workshopDir + "/tilemapdata";
            var tilemapdataList = workshopAsserts.FindAll((e) =>
            {
                return e.StartsWith(tilemapdataDir);
            });

            for (var i = 0; i < tilemapdataList.Count; i++)
            {
                var ap = tilemapdataList[i];
                var type = AssetDatabase.GetMainAssetTypeAtPath(ap);
                if (type == typeof(TextAsset))
                {
                    this.TrySetABName(ap, Tools.GetRelativePathWithoutExtension(ap, workshopDir));
                }
            }
        }
		        
		void DoTilemapImage()
        {
            var tilemapimageDir = workshopDir + "/tilemapimage";
            var tilemapimageList = workshopAsserts.FindAll((e) =>
            {
                return e.StartsWith(tilemapimageDir);
            });

            for (var i = 0; i < tilemapimageList.Count; i++)
            {
                var ap = tilemapimageList[i];
                var type = AssetDatabase.GetMainAssetTypeAtPath(ap);
                if (type == typeof(Texture2D))
                {
                    this.TrySetABName(ap, Tools.GetRelativePathWithoutExtension(ap, workshopDir));
                }
            }
        }

        void DoScenes()
        {
            for (var i = 0; i < workshopAsserts.Count; i++)
            {
                var ap = workshopAsserts[i];
                var type = AssetDatabase.GetMainAssetTypeAtPath(ap);
                if (type == typeof(SceneAsset))
                {
                    this.TrySetABName(ap, Tools.GetRelativePathWithoutExtension(ap, workshopDir));
                }
            }
        }

        void DoSpriteAtlas()
        {
            var fileList = workshopAsserts.FindAll((e) =>
            {
                return e.EndsWith(".spriteatlas");
            });
            for (var i = 0; i < fileList.Count; i++)
            {
                var ap = fileList[i];
                hadProcRefs.Add(ap);
                var sa = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(ap);
                if (sa != null)
                {

                    hadProcRefs.Add(ap);
                    var fileName = Path.GetFileNameWithoutExtension(ap);
                    this.TrySetABName(ap, fileName);
                    DoAtlasSprites(AssetDatabase.LoadAssetAtPath<SpriteAtlas>(ap), fileName);
                }
            }
        }

        private void DoAtlasSprites(SpriteAtlas sa, string abName)
        {
            var allPackables = sa.GetPackables();
            foreach (var e in allPackables)
            {
                var e_path = AssetDatabase.GetAssetPath(e);
                if (File.Exists(e_path))
                {
                    TrySetABName(e_path, abName);
                    hadProcRefs.Add(e_path);
                }
                else if (Directory.Exists(e_path))
                {
                    var dirFiles = workshopAsserts.FindAll((e) =>
                    {
                        return e.StartsWith(e_path);
                    });
                    foreach (var df in dirFiles)
                    {
                        TrySetABName(df, abName);
                        hadProcRefs.Add(df);
                    }
                }
            }
        }


        void DoIcons()
        {
            var workshopIconDir = workshopDir + "/icons";
            var fileList = workshopAsserts.FindAll((e) =>
            {
                if (!e.StartsWith(workshopIconDir))
                {
                    return false;
                }
                if (hadProcRefs.Contains(e))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            });

            for (var i = 0; i < fileList.Count; i++)
            {
                var ap = fileList[i];
                var type = AssetDatabase.GetMainAssetTypeAtPath(ap);
                hadProcRefs.Add(ap);
                if (type != typeof(Texture2D))
                {
                    continue;
                }
                var na = Tools.GetRelativePathWithoutExtension(ap, workshopDir);
                na = Path.GetDirectoryName(na).Replace('\\', '/');
                this.TrySetABName(ap, "global/" + na);

                SetTextureAtlasName(ap, na.Replace('/', '_'));
            }
        }

        void DoUiAtlas()
        {
            var workshopIconDir = workshopDir + "/uiatlas";
            var fileList = workshopAsserts.FindAll((e) =>
            {
                if (!e.StartsWith(workshopIconDir))
                {
                    return false;
                }
                if (hadProcRefs.Contains(e))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            });
            for (var i = 0; i < fileList.Count; i++)
            {
                var ap = fileList[i];
                hadProcRefs.Add(ap);
                var type = AssetDatabase.GetMainAssetTypeAtPath(ap);
                if (type != typeof(Texture2D))
                {
                    continue;
                }
                var na = Tools.GetRelativePathWithoutExtension(ap, workshopDir);
                na = Path.GetDirectoryName(na).Replace('\\', '/');
                this.TrySetABName(ap, na);
                SetTextureAtlasName(ap, na.Replace('/', '_'));
            }
        }

        static void SetTextureAtlasName(string texPath, string atlasName)
        {
            var textureImporter = AssetImporter.GetAtPath(texPath) as TextureImporter;
            //textureImporter.spritePackingTag = atlasName;
            textureImporter.SaveAndReimport();
        }


        void DoBackgrounds()
        {
            var workshopBackgroundDir = workshopDir + "/background";
            var backgroundList = workshopAsserts.FindAll((e) =>
            {
                return e.StartsWith(workshopBackgroundDir);
            });
            for (var i = 0; i < backgroundList.Count; i++)
            {
                var ap = backgroundList[i];
                var type = AssetDatabase.GetMainAssetTypeAtPath(ap);
                if (type != typeof(Texture2D))
                {
                    continue;
                }
                this.TrySetABName(ap, Tools.GetRelativePathWithoutExtension(ap, workshopDir));
            }
        }

        void DoSmallmap()
        {
            var workshopSmallmapDir = workshopDir + "/smallmap";
            var smallmapList = workshopAsserts.FindAll((e) =>
            {
                return e.StartsWith(workshopSmallmapDir);
            });
            for (var i = 0; i < smallmapList.Count; i++)
            {
                var ap = smallmapList[i];
                var type = AssetDatabase.GetMainAssetTypeAtPath(ap);
                if (type != typeof(Texture2D))
                {
                    continue;
                }
                this.TrySetABName(ap, Tools.GetRelativePathWithoutExtension(ap, workshopDir));
            }
        }

        void DoPrefabs()
        {
            foreach (var prefab in workshopPrefabs)
            {
                this.TrySetABName(prefab, Tools.GetRelativePathWithoutExtension(prefab, workshopDir));
            }
        }

        public static int GetPackLevel(int count, int maxCount)
        {
            if (count >= maxCount * 0.70f && count > 5) { return 6; }
            if (count >= maxCount * 0.50f && count > 4) { return 5; }
            if (count >= maxCount * 0.30f && count > 3) { return 4; }
            if (count >= maxCount * 0.10f && count > 2) { return 3; }
            if (count >= maxCount * 0.05f && count > 1) { return 2; }
            return count > 0 ? 1 : 0;
        }

        void DoDeps()
        {
            var maxPrefabNum = workshopPrefabs.Length;
            foreach (var kv in dependsRefs)
            {
                var _ref = kv.Value;
                var _refCount = _ref.Count;
                if (_refCount < 2)
                {
                    continue;
                }
                var _dep = kv.Key;
                if (hadProcRefs.Contains(_dep))
                {
                    continue;
                }
                var _depType = AssetDatabase.GetMainAssetTypeAtPath(_dep);

                if (_depType == typeof(MonoScript) || _depType == typeof(Shader) || _depType == typeof(AudioClip))
                {
                    continue;
                }
                var _depFileSize = new FileInfo(_dep).Length;
                if (_depFileSize < 256)
                {
                    continue;
                }
                var _pl = GetPackLevel(_refCount, maxPrefabNum);
                var abn = string.Format("d_{0}/{1}", _pl, Tools.CalculateMD5Hash(Tools.HashSetToString(_ref)));
                this.TrySetABName(_dep, abn);
            }
        }

        void SetupABName()
        {
            for (var i = 0; i < allProjectAssets.Count; i++)
            {
                var _path = allProjectAssets[i];
                var _name = ABNamesDic.ContainsKey(_path) ? ABNamesDic[_path] : null;
                var _asim = AssetImporter.GetAtPath(_path);
                if (_asim.assetBundleName.StartsWith("abfixed/"))
                {
                    continue;
                }
                EditorUtility.DisplayProgressBar("Set AB Name...", _path, Mathf.InverseLerp(0, allProjectAssets.Count - 1, i));
                _asim.SetAssetBundleNameAndVariant(_name, null);
            }
        }

        public void Run()
        {
            this.DoPackShaders();
            this.DoFonts();
            this.DoAudios();

            this.DoSpriteAtlas();

            this.DoIcons();
            this.DoUiAtlas();

            this.DoBackgrounds();
            this.DoSmallmap();
            this.DoScenes();

            this.DoTilemapData();
            this.DoTilemapImage();
            this.DoPrefabs();
            this.DoDeps();

            this.SetupABName();

            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("AssetBunlde", "Reset AB Name Done!", "Close");
        }
    }
}

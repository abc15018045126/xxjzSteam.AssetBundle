using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class TPSpriteRect
{
    public int x;
    public int y;
    public int w;
    public int h;

}

public class TPSpriteSize
{
    public int w;
    public int h;
}

public class TPSprite
{
    public TPSpriteRect frame;
    public bool rotated;
    public bool trimmed;
    public TPSpriteRect spriteSourceSize;
    public TPSpriteSize sourceSize;
}

public class TPAtlasMeta
{
    public string app;
    public string version;
    public string image;
    public TPSpriteSize size;
    public float scale;
    public string smartupdate;
}

public class TPAtlas
{
    public Dictionary<string, TPSprite> frames;

    public TPAtlasMeta meta;

    public Rect GetUnityRect(TPSpriteRect tsr)
    {
        return new Rect(tsr.x, meta.size.h - tsr.y - tsr.h, tsr.w, tsr.h);
    }
}

public class TPAtlasSet
{
    public static void SetSpineTexture(TextureImporter texImport)
    {
        texImport.textureType = TextureImporterType.Sprite;
        texImport.spriteImportMode = SpriteImportMode.Multiple;
        texImport.spritePackingTag = "";
        texImport.alphaSource = TextureImporterAlphaSource.FromInput;
        texImport.alphaIsTransparency = true;
        texImport.mipmapEnabled = false;
        texImport.maxTextureSize = 2048;
        texImport.sRGBTexture = true;
    }

    public static TPAtlasSet CreateFromJsonFile(string filePath)
    {
        var fileExtension = Path.GetExtension(filePath);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
        var rootPath = Path.GetDirectoryName(filePath);
        var rg = new Regex(@"_[0-9]$");

        if (rg.IsMatch(fileNameWithoutExtension))
        {
            var ret = new TPAtlasSet(rootPath);
            for (var i = 0; i < 10; i++)
            {
                var s = fileNameWithoutExtension.Substring(0, fileNameWithoutExtension.Length - 1) + i;
                var p = Path.Combine(rootPath, s + fileExtension);
                if (File.Exists(p))
                {
                    var jsonText = File.ReadAllText(p);
                    var altas = Newtonsoft.Json.JsonConvert.DeserializeObject<TPAtlas>(jsonText);
                    ret.atlasList.Add(altas);
                }
            }
            ret.SetupAltasSprites();
            return ret;
        }
        else
        {
            var jsonText = File.ReadAllText(filePath);
            var altas = Newtonsoft.Json.JsonConvert.DeserializeObject<TPAtlas>(jsonText);
            var ret = new TPAtlasSet(rootPath);
            ret.atlasList.Add(altas);
            ret.SetupAltasSprites();
            return ret;
        }
    }

    public readonly List<TPAtlas> atlasList = new List<TPAtlas>();

    public readonly string rootPath;

    private List<Sprite> allSprites = null;

    public TPAtlasSet(string rootPath)
    {
        this.rootPath = rootPath;
    }

    public void SetupAltasSprites()
    {
        foreach (var atlas in atlasList)
        {
            var imgPath = Path.Combine(rootPath, atlas.meta.image);
            var ti = AssetImporter.GetAtPath(imgPath) as TextureImporter;
            SetSpineTexture(ti);

            var smdl = new List<SpriteMetaData>(atlas.frames.Count);
            foreach (var kv in atlas.frames)
            {
                var tpsprite = kv.Value;
                var smd = new SpriteMetaData();
                smd.name = kv.Key;
                smd.rect = atlas.GetUnityRect(tpsprite.frame);
                smd.border = Vector4.zero;
                smd.alignment = (int)SpriteAlignment.Custom;

                var centerX = tpsprite.sourceSize.w * 0.5f;
                var centerY = tpsprite.sourceSize.h * 0.5f;

                var _px = centerX - tpsprite.spriteSourceSize.x;
                var _py = centerY - tpsprite.spriteSourceSize.y;
                smd.pivot = new Vector2(_px / tpsprite.frame.w, (1.0f - _py / tpsprite.frame.h));
                smdl.Add(smd);
            }

            ti.spritesheet = smdl.ToArray();
            AssetDatabase.ImportAsset(imgPath, ImportAssetOptions.ForceUpdate);
            AssetDatabase.SaveAssets();
        }
    }

    public Sprite GetSprite(string spriteName)
    {
        if (allSprites == null)
        {
            allSprites = new List<Sprite>();
            foreach (var atlas in atlasList)
            {
                var imgPath = Path.Combine(rootPath, atlas.meta.image);
                var sprites = AssetDatabase.LoadAllAssetsAtPath(imgPath);
                for (var i = 0; i < sprites.Length; i++)
                {
                    if (sprites[i] is Sprite sp)
                    {
                        allSprites.Add(sp);
                    }
                }
            }
        }
        for (var i = allSprites.Count - 1; i >= 0; i--)
        {
            if (allSprites[i].name == spriteName)
            {
                return allSprites[i];
            }
        }
        return null;
    }
}
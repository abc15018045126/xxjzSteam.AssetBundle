using Game.Tilemap;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CQ2DTilemapLoader
{
    //private static Transform EnsureRenderListNode(GameTilemap mapNode)
    //{
    //    var oldRenderListNode = mapNode.transform.Find("renderList");
    //    if (oldRenderListNode != null)
    //    {
    //        GameObject.Destroy(oldRenderListNode.gameObject);
    //    }

    //    var renderListNodeObj = new GameObject("renderList");
    //    var renderListNode = renderListNodeObj.transform;
    //    renderListNode.SetParent(mapNode.transform, false);

    //    renderListNode.localRotation = Quaternion.Euler(-Mathf.Asin(2.0f / 3.0f) * Mathf.Rad2Deg, 0, 0);
    //    var offset_y = 2 * 100;
    //    var offset_z = Mathf.Sqrt(5) * 100;
    //    renderListNode.localPosition = new Vector3(0, -offset_y, offset_z);

    //    return renderListNode;
    //}

    public static void Load(string path, GameTilemap mapNode, string sceneName)
    {
        using (var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            var br = new BinaryReader(fs);
            byte dataExistedFlag = br.ReadByte();
            if ((dataExistedFlag & (1 << 0)) != 0)
            {
                int rmdCount = br.ReadInt32();
                Transform renderListNode = null;
                for (var i = 0; i < rmdCount; i++)
                {
                    if (renderListNode == null)
                    {
                        //renderListNode = EnsureRenderListNode(mapNode);
                        renderListNode = mapNode.transform;
                    }
                    ReadMeshRender(br, renderListNode, sceneName, sceneName + "_" + i);
                }
            }
            if ((dataExistedFlag & (1 << 1)) != 0)
            {
                var orientationType = (TilemapOrientationType)br.ReadByte();
                if (orientationType == TilemapOrientationType.Orthogonal)
                {
                    if (!(mapNode is GameTilemapOrthogonal))
                    {
                        Debug.LogError("地图脚本类型不匹配:GameTilemapOrthogonal");
                        return;
                    }
                }
                else if (orientationType == TilemapOrientationType.Staggered)
                {
                    //if (!(mapNode is GameTilemapStaggered))
                    //{
                    //    Debug.LogError("地图脚本类型不匹配:GameTilemapStaggered");
                    //    return;
                    //}
                    Debug.LogError("地图脚本类型不支持:GameTilemapStaggered");
                    return;
                }
                mapNode.colsCount = br.ReadInt32();
                mapNode.rowsCount = br.ReadInt32();
                mapNode.tileWidth = br.ReadInt32();
                mapNode.tileHeight = br.ReadInt32();
                mapNode.blockData = br.ReadBytes(mapNode.colsCount * mapNode.rowsCount);

                var objLayerCount = br.ReadInt32();
                var objData = new List<TilemapObjectData>();
                for (var i = 0; i < objLayerCount; i++)
                {
                    var layerName = br.ReadString();
                    var objCount = br.ReadInt32();
                    for (var k = 0; k < objCount; k++)
                    {
                        objData.Add(ReadObjectData(br, layerName));
                    }
                }
                mapNode.objectsData = objData.ToArray();
            }
        }
    }

    public static void LoadDataOnly(string path, GameTilemap mapNode, string sceneName)
    {
        using (var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            var br = new BinaryReader(fs);
            byte dataExistedFlag = br.ReadByte();
            if ((dataExistedFlag & (1 << 0)) != 0)
            {
                int rmdCount = br.ReadInt32();
                for (var i = 0; i < rmdCount; i++)
                {
                    ReadMeshRender(br, null, sceneName, sceneName + "_" + i);
                }
            }
            if ((dataExistedFlag & (1 << 1)) != 0)
            {
                var orientationType = (TilemapOrientationType)br.ReadByte();
                if (orientationType == TilemapOrientationType.Orthogonal)
                {
                    if (!(mapNode is GameTilemapOrthogonal))
                    {
                        Debug.LogError("地图脚本类型不匹配:GameTilemapOrthogonal");
                        return;
                    }
                }
                else if (orientationType == TilemapOrientationType.Staggered)
                {
                    //if (!(mapNode is GameTilemapStaggered))
                    //{
                    //    Debug.LogError("地图脚本类型不匹配:GameTilemapStaggered");
                    //    return;
                    //}
                    Debug.LogError("地图脚本类型不支持:GameTilemapStaggered");
                    return;
                }
                mapNode.colsCount = br.ReadInt32();
                mapNode.rowsCount = br.ReadInt32();
                mapNode.tileWidth = br.ReadInt32();
                mapNode.tileHeight = br.ReadInt32();
                mapNode.blockData = br.ReadBytes(mapNode.colsCount * mapNode.rowsCount);

                var objLayerCount = br.ReadInt32();
                var objData = new List<TilemapObjectData>();
                for (var i = 0; i < objLayerCount; i++)
                {
                    var layerName = br.ReadString();
                    var objCount = br.ReadInt32();
                    for (var k = 0; k < objCount; k++)
                    {
                        objData.Add(ReadObjectData(br, layerName));
                    }
                }
                mapNode.objectsData = objData.ToArray();
            }
        }
    }

    private static TilemapObjectData ReadObjectData(BinaryReader br, string layerName)
    {
        var objData = new TilemapObjectData();
        objData.layerName = layerName;
        objData.Id = br.ReadInt32();
        objData.objectType = br.ReadByte();
        objData.objectName = br.ReadString();
        objData.typeStr = br.ReadString();
        objData.x = br.ReadSingle();
        objData.y = br.ReadSingle() * 1.5f;
        objData.w = br.ReadSingle();
        objData.h = br.ReadSingle() * 1.5f;

        var propNum = br.ReadUInt16();
        for (var i = 0; i < propNum; i++)
        {
            var k = br.ReadString();
            var v = br.ReadString();
            objData.properties.Add(k, v);
        }
        return objData;
    }

    private static Mesh ReadMeshRender(BinaryReader br, Transform renderListNode, string sceneName, string meshName)
    {
        var matName = br.ReadString();
        int quadsCount = br.ReadInt32();
        var verts = new Vector3[quadsCount * 4];
        var uvs = new Vector2[quadsCount * 4];
        var indices = new int[quadsCount * 6];

        for (var quadIndex = 0; quadIndex < quadsCount; quadIndex++)
        {
            var layerIndex = br.ReadInt32();

            var _xVertMin = br.ReadInt32();
            var _yVertMin = br.ReadInt32();
            var _xVertMax = br.ReadInt32();
            var _yVertMax = br.ReadInt32();


            var xVertMin = (_xVertMin - 0.5f) * 0.01f;
            var yVertMin = (_yVertMin - 0.5f) * 0.01f * 1.5f;
            var xVertMax = (_xVertMax + 0.5f) * 0.01f;
            var yVertMax = (_yVertMax + 0.5f) * 0.01f * 1.5f;

            var layerValue = -layerIndex * 0.01f;

            verts[quadIndex * 4 + 0] = new Vector3(xVertMin, layerValue, yVertMin);
            verts[quadIndex * 4 + 1] = new Vector3(xVertMin, layerValue, yVertMax);
            verts[quadIndex * 4 + 2] = new Vector3(xVertMax, layerValue, yVertMax);
            verts[quadIndex * 4 + 3] = new Vector3(xVertMax, layerValue, yVertMin);

            var xUv0 = br.ReadSingle();
            var yUv0 = br.ReadSingle();

            var xUv1 = br.ReadSingle();
            var yUv1 = br.ReadSingle();

            var xUv2 = br.ReadSingle();
            var yUv2 = br.ReadSingle();

            var xUv3 = br.ReadSingle();
            var yUv3 = br.ReadSingle();


            uvs[quadIndex * 4 + 0] = new Vector2(xUv0, yUv0);
            uvs[quadIndex * 4 + 1] = new Vector2(xUv1, yUv1);
            uvs[quadIndex * 4 + 2] = new Vector2(xUv2, yUv2);
            uvs[quadIndex * 4 + 3] = new Vector2(xUv3, yUv3);

            indices[quadIndex * 6 + 0] = br.ReadUInt16();
            indices[quadIndex * 6 + 1] = br.ReadUInt16();
            indices[quadIndex * 6 + 2] = br.ReadUInt16();
            indices[quadIndex * 6 + 3] = br.ReadUInt16();
            indices[quadIndex * 6 + 4] = br.ReadUInt16();
            indices[quadIndex * 6 + 5] = br.ReadUInt16();
        }

        var mesh = new Mesh
        {
            vertices = verts,
            uv = uvs,
            triangles = indices
        };

        // -- 设置法线
        //var normals = new Vector3[verts.Length];
        //for (var i = 0; i < normals.Length; i++)
        //{
        //    normals[i] = new Vector3(0, 1, 0);
        //}
        //mesh.normals = normals;

        if (renderListNode != null)
        {
            var go = new GameObject(meshName);
            go.layer = LayerMask.NameToLayer("Ground");
            go.transform.SetParent(renderListNode, false);
            var mf = go.AddComponent<MeshFilter>();
            mf.mesh = mesh;

            var mr = go.AddComponent<MeshRenderer>();
            mr.material = LoadTilesetMaterial(sceneName, matName);
        }
        return mesh;
    }

    static Material LoadTilesetMaterial(string sceneName, string matName)
    {
        string path;
        if (matName.StartsWith(sceneName))
        {
            path = string.Format(@"Assets/TileMap/{0}/{1}.mat", sceneName, matName);
        }
        else
        {
            path = string.Format(@"Assets/TileMap/tileset/{0}.mat", matName);
        }
        var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        return mat;
    }
}

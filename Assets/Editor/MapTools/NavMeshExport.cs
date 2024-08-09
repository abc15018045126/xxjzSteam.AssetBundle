using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class NavMeshExport
{
    public static void Export()
    {
        var outputPath = "../ExportNavMesh";
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        string tmpPath = string.Format("{0}/{1}.obj", outputPath, SceneManager.GetActiveScene().name);
        var tmpNavMeshTriangulation = NavMesh.CalculateTriangulation();
        StreamWriter tmpStreamWriter = new StreamWriter(tmpPath);

        //顶点
        for (int i = 0; i < tmpNavMeshTriangulation.vertices.Length; i++)
        {
            tmpStreamWriter.WriteLine("v  " + tmpNavMeshTriangulation.vertices[i].x + " " + tmpNavMeshTriangulation.vertices[i].y + " " + tmpNavMeshTriangulation.vertices[i].z);
        }

        tmpStreamWriter.WriteLine("g pPlane1");

        //索引
        for (int i = 0; i < tmpNavMeshTriangulation.indices.Length;)
        {
            tmpStreamWriter.WriteLine("f " + (tmpNavMeshTriangulation.indices[i] + 1) + " " + (tmpNavMeshTriangulation.indices[i + 1] + 1) + " " + (tmpNavMeshTriangulation.indices[i + 2] + 1));
            i = i + 3;
        }

        tmpStreamWriter.Flush();
        tmpStreamWriter.Close();
        EditorUtility.DisplayDialog("ExportNavMesh Success", "Finish", "Close");
    }


    public static void ExportTilemapData()
    {
        var outputPath = "../ExportNavMesh";
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        string tmpPath = string.Format("{0}/{1}_data.tmx", outputPath, SceneManager.GetActiveScene().name);
        var tmpNavMeshTriangulation = NavMesh.CalculateTriangulation();

        var mapCols = 100;
        var mapRows = 80;

        var tilewidth = 48;
        var tileheight = 32;

        var sb = new StringBuilder();

        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF - 8\"?>");

        sb.Append("<map version=\"1.2\" tiledversion =\"1.2.1\" orientation =\"orthogonal\" renderorder=\"right - down\" ");
        sb.AppendFormat("width=\"{0}\" height=\"{1}\" ", mapCols, mapRows);
        sb.AppendFormat("tilewidth = \"{0}\" tileheight = \"{1}\" ", tilewidth, tileheight);
        sb.AppendLine("infinite = \"0\" nextlayerid = \"20\" nextobjectid = \"80\">");

        sb.AppendLine(" <tileset firstgid=\"1\" name=\"data\" tilewidth=\"48\" tileheight=\"32\" tilecount=\"2\" columns=\"2\">");
        sb.AppendLine("  <image source=\"dataTile0.png\" width=\"96\" height=\"32\"/>");
        sb.AppendLine(" </tileset>");


        sb.AppendFormat(" <layer id=\"19\" name=\"block\" width=\"{0}\" height=\"{1}\">", mapCols, mapRows);
        sb.AppendLine();

        sb.AppendLine("  <data encoding=\"csv\">");

        //TODO
        for (var r = 0; r < mapRows; r++)
        {
            for (var c = 0; c < mapCols; c++)
            {
                if (c > 0 || r > 0) sb.Append(",");
                var p = new Vector3(c * 0.48f, 0.0f, r * 0.48f);
                if (NavMesh.SamplePosition(p, out NavMeshHit hit, 0.24f, -1))
                {
                    sb.Append(1);
                }
                else
                {
                    sb.Append(0);
                }
            }
            sb.AppendLine();
        }


        sb.AppendLine("  </data>");
        sb.AppendLine(" </layer>");
        sb.AppendLine("</map>");

        File.WriteAllText(tmpPath, sb.ToString(), Encoding.UTF8);
        //StreamWriter tmpStreamWriter = new StreamWriter(tmpPath);
        //tmpStreamWriter.Write();
        //tmpStreamWriter.Flush();
        //tmpStreamWriter.Close();
        EditorUtility.DisplayDialog("ExportNavMesh Success", "Finish", "Close");
    }

}
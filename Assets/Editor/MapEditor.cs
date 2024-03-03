using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class MapEditor
{
#if UNITY_EDITOR

    const string TILEMAP_OUTPUT_PATH = "Assets/Resources/Map";

    // % (Ctrl) # (Shift) & (alt)
    [MenuItem("Tools/GenerateMap")]
    private static void GenerateMap()
    {
        GameObject[] gameObjects = Resources.LoadAll<GameObject>("Prefabs/Map");
        if (Directory.Exists(TILEMAP_OUTPUT_PATH) == false)
            Directory.CreateDirectory(TILEMAP_OUTPUT_PATH);

        foreach (GameObject go in gameObjects)
        {
            Tilemap tmBase = Util.FindChild<Tilemap>(go, "Tilemap_Base", true);
            Tilemap tm = Util.FindChild<Tilemap>(go, "Tilemap_Collision", true);
            if (tm == null)
            {
                Debug.LogError($"Not found Tilemap_Collision in current asset {go.name}");
                return;
            }

            using (var writer = File.CreateText($"{TILEMAP_OUTPUT_PATH}/{go.name}.txt"))
            {
                int xMin = tmBase.cellBounds.xMin;
                int yMin = tmBase.cellBounds.yMin;
                int xMax = tmBase.cellBounds.xMax;
                int yMax = tmBase.cellBounds.yMax;

                writer.WriteLine(xMin);
                writer.WriteLine(xMax);
                writer.WriteLine(yMin);
                writer.WriteLine(yMax);

                for (int y = yMax; y >= yMin; y--)
                {
                    for (int x = xMin; x <= xMax; x++)
                    {
                        TileBase tb = tm.GetTile(new Vector3Int(x, y));
                        if (tb != null)
                            writer.Write(1);
                        else
                            writer.Write(0);
                    }
                    writer.WriteLine();
                }
            }
        }

        Debug.Log("Compeleted Generate Map");
    }
#endif
}

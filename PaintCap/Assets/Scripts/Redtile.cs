using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class Redtile : Tile {

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/RedTile")]

    public static void CreateRedTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Red Tile", "New RedTile", "asset", "Save treeTile", "Assets");
        if (path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<Redtile>(), path);
    }
#endif
}

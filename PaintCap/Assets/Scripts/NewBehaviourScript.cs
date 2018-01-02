using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NewBehaviourScript : MonoBehaviour {
    private static int mouseCount = 0;

    public Tilemap tileMap;
    public TileBase painterTile;

    // Use this for initialization
    void Start () {
        Debug.Log("Startup NewBehaviourScript");
	}
	
	// Update is called once per frame
	void Update () {
        
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
                Debug.Log("Touched");
        }
        if (Input.GetMouseButtonDown(0))
        {
            mouseCount++;
            //GridLayout gridLayout = transform.parent.GetComponent<GridLayout>();
            Vector3Int cellPosition = tileMap.LocalToCell(Input.mousePosition);
            Debug.Log("Moused " + mouseCount + " " + cellPosition + " " + Input.mousePosition);
			Debug.Log("tilemap bounds: " + tileMap.size + " "  + tileMap.localBounds.center + " " + tileMap.localBounds.min);
            tileMap.SetTile(new Vector3Int(0, 0, 0), painterTile);
        }
    }

}

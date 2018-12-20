using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PaintCap
{
	public class TileManager : MonoBehaviour {

		public Tile[] tiles;

		public Tile getTileByType(TileType type) 
		{
			return tiles[(int)type];
		}

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}

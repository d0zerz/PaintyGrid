using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using PaintCap;

namespace PaintCap
{
	public class BoardState {
		private Vector2Int boardDimensions;
		private Tilemap backgroundMap;
		private TileManager tileManager;
		private TileState[,] boardState;

		public BoardState(Tilemap bgMap, TileManager tileManager) {
			backgroundMap = bgMap;
			this.tileManager = tileManager;
		}

		public void paintBoardState() {
			Debug.Log ("Painting board state");

			// paint background tilemap
			backgroundMap.ClearAllTiles ();
			for (int x = 0; x < boardDimensions.x; x++) {
				for (int y = 0; y < boardDimensions.y; y++) {
					TileType type = boardState [x, y].getTileBackground ();
					Tile tile = tileManager.getTileByType (type);
					backgroundMap.SetTile(new Vector3Int(x, y, 0), tile);
				}
			}
		}

		public void initBoard(Vector2Int board) {
			Debug.Log ("Init board");
			boardDimensions = board;
			initBoardRandomly ();
			DrawLine (new Vector3Int (-100, -100, 1), new Vector3Int (100, 100, 1), Color.cyan, 1);
		}
			
		private void initBoardRandomly() {
			boardState = new TileState[boardDimensions.x, boardDimensions.y];
			System.Random rnd = new System.Random();
			for (int x = 0; x < boardDimensions.x; x++) {
				for (int y = 0; y < boardDimensions.y; y++) {
					boardState [x, y] = new TileState((TileType)rnd.Next(1, 4));
				}
			}
		}

		void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
		{
			GameObject myLine = new GameObject();
			myLine.transform.position = start;
			myLine.AddComponent<LineRenderer>();
			LineRenderer lr = myLine.GetComponent<LineRenderer>();
			lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
			lr.SetColors(color, color);
			lr.SetWidth(1f, 1f);
			lr.SetPosition(0, start);
			lr.SetPosition(1, end);
			//GameObject.Destroy(myLine, duration);
		}

	}
}
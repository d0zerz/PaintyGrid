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
			//backgroundMap.ClearAllTiles ();
			for (int x = 0; x < boardDimensions.x; x++) {
				for (int y = 0; y < boardDimensions.y; y++) {
					TileType type = boardState [x, y].getTileBackground ();
					Tile tile = tileManager.getTileByType (type);
					backgroundMap.SetTile(new Vector3Int(x, y, 0), tile);
				}
			}
		}

        public void setTile(Vector3Int position, TileType tileType)
        {
            TileState state = new TileState(tileType);
            boardState[position.x, position.y] = state;
        }

        public void setTileToBlack(Vector3Int position)
        {
            TileState state = new TileState(TileType.BLACK_TILE);
            boardState[position.x, position.y] = state;
        }

        public void initBoard(Vector2Int board) {
			Debug.Log ("Init board jack");
			boardDimensions = board;
			initBoardRandomly ();
			//DrawLine (new Vector3Int (-100, -100, 1), new Vector3Int (100, 100, 1), Color.cyan, 1);
		}
			
		private void initBoardRandomly() {
			boardState = new TileState[boardDimensions.x, boardDimensions.y];
			System.Random rnd = new System.Random();
			for (int x = 0; x < boardDimensions.x; x++) {
				for (int y = 0; y < boardDimensions.y; y++) {
					boardState [x, y] = new TileState((TileType)rnd.Next(0, 3));
				}
			}
		}
	}
}
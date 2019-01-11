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
			for (int x = 0; x < boardDimensions.x; x++) {
				for (int y = 0; y < boardDimensions.y; y++) {
                    Tile tile = boardState[x, y].getGameTile().getTile();
					backgroundMap.SetTile(new Vector3Int(x, y, 0), tile);
				}
			}
		}

        public TileState getTileState(Vector3 pos)
        {
            Vector3Int boardStatePos = Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            return boardState[boardStatePos.x, boardStatePos.y];
        }

        public TileState getNearestMatch(Vector3 pos, Color color)
        {
            float lowestColorDistance = float.MaxValue;
            TileState bestMatch = null;
            foreach(var state in getNearbyTiles(pos)) {
                Vector3 tileMiddle = state.getTileMiddle();
                Color tileColor = state.getGameTile().getTileColor();
                float distanceFromTile = Vector3.Distance(tileMiddle, pos);
                float colorDistance = this.colorDistance(color, tileColor);
                if (colorDistance < lowestColorDistance)
                {
                    lowestColorDistance = colorDistance;
                    bestMatch = state;
                }
            }
            Debug.Log(string.Format("best color dist {0}", lowestColorDistance));
            return bestMatch;
        }

        private float colorDistance(Color c1, Color c2)
        {
            return Mathf.Sqrt(
                Mathf.Pow(c1.r - c2.r, 2) +
                Mathf.Pow(c1.g - c2.g, 2) +
                Mathf.Pow(c1.b - c2.b, 2)
                );
        }

        // Taken from https://stackoverflow.com/questions/2103368/color-logic-algorithm
        // Is broken.
        private float colorDistanceExperimental(Color c1, Color c2)
        {
            float r = (c1.r - c2.r) * 256;
            float g = (c1.g - c2.g) * 256;
            float b = (c1.b - c2.b) * 256;

            float rmean = (c1.r * 256 + c2.r * 256) / 2;
            
            float weightR = 2 + rmean / 256;
            float weightG = 4.0f;
            float weightB = 2 + (255 - rmean) / 256;
            return Mathf.Sqrt(weightR * r * r + weightG * g * g + weightB * b * b);
        }

        private List<TileState> getNearbyTiles(Vector3 pos)
        {
            Vector3Int boardStatePos = Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            int xPos = boardStatePos.x;
            int yPos = boardStatePos.y;

            List<TileState> tileStates = new List<TileState>();
            addTileStateSafe(tileStates, xPos, yPos);
            addTileStateSafe(tileStates, xPos, yPos - 1);
            addTileStateSafe(tileStates, xPos, yPos + 1);
            addTileStateSafe(tileStates, xPos - 1, yPos + 1);
            addTileStateSafe(tileStates, xPos - 1, yPos);
            addTileStateSafe(tileStates, xPos - 1, yPos - 1);
            addTileStateSafe(tileStates, xPos + 1, yPos + 1);
            addTileStateSafe(tileStates, xPos + 1, yPos);
            addTileStateSafe(tileStates, xPos + 1, yPos + 1);
            return tileStates;
        }

        private void addTileStateSafe(List<TileState> listToAdd, int xPos, int yPos)
        {
            if (xPos >=0 && xPos < boardDimensions.x && yPos >= 0 && yPos < boardDimensions.y)
            {
                listToAdd.Add(boardState[xPos, yPos]);
            }
        }


        //public void setTile(Vector3Int position, TileType tileType)
        //{
        //    TileState state = new TileState(tileManager.getTileByType(tileType));
        //    boardState[position.x, position.y] = state;
        //}

        public void initBoard(Vector2Int board) {
			Debug.Log ("Init board jack");
			boardDimensions = board;
			initBoardRandomly ();
		}
			
		private void initBoardRandomly() {
			boardState = new TileState[boardDimensions.x, boardDimensions.y];
			for (int x = 0; x < boardDimensions.x; x++) {
				for (int y = 0; y < boardDimensions.y; y++) {
					boardState [x, y] = new TileState(tileManager.getRandomTile(), x, y);
				}
			}
		}
	}
}

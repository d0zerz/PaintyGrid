using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using PaintCap;

namespace PaintCap
{
	public class BoardState {

        private const float MIN_DAMAGE_THRESHOLD = .5f;

		private Vector2Int boardDimensions;
		
        private TileManager tileManager;
		private TileState[,] boardState;

        private Vector2Int endPosition;
        private List<Vector2Int> capturedPositions = new List<Vector2Int>();

		public BoardState(TileManager tileManager) {
			this.tileManager = tileManager;
		}

		public void paintBoardState() {
			Debug.Log ("Painting board state");
			// paint background tilemap
			for (int x = 0; x < boardDimensions.x; x++) {
				for (int y = 0; y < boardDimensions.y; y++) {
                    TileState tileState = boardState[x, y];
                    Tile tile = tileState.getGameTile().getTile();
                    tileManager.setBackgroundTile(tile, x, y);
                    if (tileState.modifierTile != null)
                    {
                        tileManager.setModifierTile(tileState.modifierTile, x, y);
                    }
				}
			}
		}

        public Vector2Int getBoardDimensions()
        {
            return boardDimensions;
        }

        public TileState getTileState(Vector3 pos)
        {
            Vector3Int boardStatePos = Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            try
            {
                return boardState[boardStatePos.x, boardStatePos.y];
            }
            catch(Exception)
            {
                return null;
            }
        }

        public TileCapture processBombDrop(Vector3 pos, Color color)
        {
            float highestDamageValue = 0;
            TileState bestMatch = null;

            foreach(var state in getNearbyTiles(pos)) {
                // if state is capped, or there's no captured tile adjacent
                if (state.isCapped() || !hasCapAdjacent(state.getTilePosition()))
                {
                    continue;
                }
                Vector2 tileMiddle = state.getTileMiddle();
                Color tileColor = state.getGameTile().getTileColor();
                float distanceFromTile = Vector2.Distance(tileMiddle, pos);
                float colorDistance = this.colorDistance(color, tileColor);
                //TODO: make this better
                float curDamageValue = ((1f - colorDistance) + Mathf.Max(0f, (1f - distanceFromTile))) / 2f;

                if (curDamageValue > highestDamageValue && curDamageValue > MIN_DAMAGE_THRESHOLD)
                {
                    Debug.Log(string.Format("Candidate lowest dist [{0}] colorMatch [{1}] ", distanceFromTile, colorDistance));
                    highestDamageValue = curDamageValue;
                    bestMatch = state;
                }
            }
            Debug.Log(string.Format("Best dmg val {0}", highestDamageValue));
            return new TileCapture(bestMatch, highestDamageValue);
        }

        private bool hasCapAdjacent(Vector2Int pos)
        {
            foreach(var state in getNearbyTiles(pos))
            {
                if (state.isCapped())
                {
                    return true;
                }
            }
            return false;
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

        private List<TileState> getNearbyTiles(Vector2 pos)
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

        public void initBoard() {
            switch(LevelSelector.currentLevel)
            {
                case 1:
                    initLevel1();
                    break;
                case 2:
                    initLevel2();
                    break;
                default:
                    break;
            }
		}

        public void initLevel1()
        {
            initLineBoardRandomly(6, 10);
            List<Vector2Int> capPoints = new List<Vector2Int>
            {
                new Vector2Int(0,0),
                new Vector2Int(1,0),
                new Vector2Int(0,1),
                new Vector2Int(1,1)
            };
            initCapPoints(capPoints);
            initModifiers(new Vector2Int(5,9));
        }

        public void initLevel2()
        {
            initLineBoardRandomly(6, 10);
            List<Vector2Int> capPoints = new List<Vector2Int>
            {
                new Vector2Int(0,0)
            };
            initCapPoints(capPoints);
            initModifiers(new Vector2Int(3, 7));
        }

        public Vector2Int getEndpos()
        {
            return endPosition;
        }

        public void initModifiers(Vector2Int winningPos)
        {
            TileState state = boardState[winningPos.x , winningPos.y];
            state.isFinalTile = true;
            state.modifierTile = tileManager.levelWinningTile;
            endPosition = winningPos;
        }

        public List<Vector2Int> getCapturedPositions()
        {
            return capturedPositions;
        }

        public void initCapPoints(List<Vector2Int> capPositions)
        {
            foreach (var capPos in capPositions)
            {
                setCapPos(capPos.x, capPos.y);
            }
        }

        public void setCapPos(int x, int y)
        {
            TileState state = boardState[x, y];
            state.captureFully();
            tileManager.drawTileCapture(state);
            capturedPositions.Add(new Vector2Int(x, y));
        }

        private void initLineBoardRandomly(int width, int height)
        {
            Debug.Log("Init line board");
            boardDimensions = new Vector2Int(width, height);
            boardState = new TileState[boardDimensions.x, boardDimensions.y];
            for (int x = 0; x < boardDimensions.x; x++)
            {
                for (int y = 0; y < boardDimensions.y; y++)
                {
                    boardState[x, y] = new TileState(tileManager.getRandomTile(), x, y);
                }
            }
        }

        private void initSquareBoardRandomly(int size) {
            Debug.Log("Init square board randomly jack " + size);
            boardDimensions = new Vector2Int(size, size);
            boardState = new TileState[boardDimensions.x, boardDimensions.y];
			for (int x = 0; x < boardDimensions.x; x++) {
				for (int y = 0; y < boardDimensions.y; y++) {
					boardState [x, y] = new TileState(tileManager.getRandomTile(), x, y);
				}
			}
		}

        private void initBoardDiagonal()
        {

        }
	}
}

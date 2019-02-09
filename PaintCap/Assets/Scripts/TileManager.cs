using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PaintCap
{
    public class TileManager : MonoBehaviour {

        private const int NUM_COLOR_TILES = 6;
        public RedTile redTile;
        public BlueTile blueTile;
        public GreenTile greenTile;
        public BlueRedTile blueRedTile;
        public GreenBlueTile greenBlueTile;
        public RedGreenTile redGreenTile;
        public BorderWhite borderWhiteTile;

        public AnimatedTile animatedCap;

        public Tilemap backgroundGrid;
        public Tilemap capturedGrid;
        public Tilemap modifierGrid;

        public Tile[] partialCapTiles;
        public Tile fullCapTile;
        public Tile levelWinningTile;

        System.Random rnd = new System.Random();

        public void setBackgroundTile(Tile tile, int x, int y)
        {
            backgroundGrid.SetTile(new Vector3Int(x, y, 0), tile);
        }

        public void setModifierTile(Tile tile, int x, int y)
        {
            modifierGrid.SetTile(new Vector3Int(x, y, 0), tile);
        }

        public void drawTileCapture(TileState state) // int x, int y, float capPct)
        {
            float numTiles = partialCapTiles.Length;
            float pctSlices = 1f / numTiles;
            float capPct = state.getCapPercent();
            int x = state.getTilePosition().x;
            int y = state.getTilePosition().y;

            if (capPct >= 1f)
            {
                capturedGrid.SetTile(new Vector3Int(x, y, 0), fullCapTile);
            }
            else
            {
                int tileNum = Mathf.FloorToInt(capPct / pctSlices);
                capturedGrid.SetTile(new Vector3Int(x, y, 0), partialCapTiles[tileNum]);
            }
            
        }

        public GameTile getTileByType(TileType type) 
		{
			switch(type)
            {
                case TileType.RED_TILE: 
                    return redTile;
                case TileType.BLUE_TILE:
                    return blueTile;
                case TileType.GREEN_TILE:
                    return greenTile;
                case TileType.BLUE_RED_TILE:
                    return blueRedTile;
                case TileType.RED_GREEN_TILE:
                    return redGreenTile;
                case TileType.GREEN_BLUE_TILE:
                    return greenBlueTile;
                default:
                    throw new System.Exception();
            }
		}

        public GameTile getRandomTile()
        {
            return getTileByType((TileType)rnd.Next(0, NUM_COLOR_TILES));
        }

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}

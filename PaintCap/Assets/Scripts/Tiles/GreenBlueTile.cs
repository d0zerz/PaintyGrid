using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

namespace PaintCap
{
    public class GreenBlueTile : MonoBehaviour, GameTile
    {
        public Color tileColor;
        public Tile tile;

        public Tile getTile()
        {
            return tile;
        }

        public Color getTileColor()
        {
            return tileColor;
        }

        public TileType getTileType()
        {
            return TileType.GREEN_BLUE_TILE;
        }
    }
}
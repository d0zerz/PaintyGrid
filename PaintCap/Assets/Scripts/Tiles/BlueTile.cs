using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

namespace PaintCap
{
    public class BlueTile : MonoBehaviour, GameTile
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
            return TileType.BLUE_TILE;
        }
    }
}
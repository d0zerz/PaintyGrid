using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

namespace PaintCap
{
    public interface GameTile
    {
        Tile getTile();

        Color getTileColor();

        TileType getTileType();
    }
}

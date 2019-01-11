using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

namespace PaintCap
{
	public class TileState
	{
		private GameTile backgroundTile;
        private Vector2 tilePos;

		public TileState (GameTile backgroundTile, int x, int y)
		{
			this.backgroundTile = backgroundTile;
            this.tilePos = new Vector2(x, y);
		}

		public GameTile getGameTile() {
			return backgroundTile;
		}

        public Vector2 getTilePosition() {
            return tilePos;
        }

        public Vector2 getTileMiddle()
        {
            return tilePos + new Vector2(.5f, .5f);
        }
    }
}


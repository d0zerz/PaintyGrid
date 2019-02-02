using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

namespace PaintCap
{
	public class TileState
	{
        private const float TOTAL_CAP_REQUIRED = 2f;
		private GameTile backgroundTile;
        private Vector2Int tilePos;
        public Tile modifierTile;
        public bool isFinalTile = false;

        private float capAmount = 0f;

        public void addCaptureAmount(float amount)
        {
            capAmount += amount;
        }

        public void captureFully()
        {
            capAmount = getCapRequired();
        }

        public bool isCapped()
        {
            return getCapPercent() >= 1f;
        }

        public float getCapPercent()
        {
            return capAmount > getCapRequired() ? 1 : capAmount / getCapRequired();
        }

        public float getCapRequired()
        {
            if (isFinalTile)
            {
                return TOTAL_CAP_REQUIRED * 10;
            }
            return TOTAL_CAP_REQUIRED;
        }

		public TileState (GameTile backgroundTile, int x, int y)
		{
			this.backgroundTile = backgroundTile;
            this.tilePos = new Vector2Int(x, y);
		}

		public GameTile getGameTile() {
			return backgroundTile;
		}

        public Vector2Int getTilePosition() {
            return tilePos;
        }

        public Vector2 getTileMiddle()
        {
            return tilePos + new Vector2(.5f, .5f);
        }
    }

    public class TileCapture
    {
        public TileCapture(TileState tileState, float capAmount)
        {
            this.tileState = tileState;
            this.capAmount = capAmount;
        }

        public TileState tileState { get; set; }
        public float capAmount { get; set; }
    }
}


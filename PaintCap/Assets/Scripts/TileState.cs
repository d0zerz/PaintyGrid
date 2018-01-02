using System;

namespace PaintCap
{
	public class TileState
	{
		private TileType tileBackground;

		public TileState (TileType tileType)
		{
			this.tileBackground = tileType;
		}

		public TileType getTileBackground() {
			return tileBackground;
		}
	}
}


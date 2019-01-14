using System.Collections.Generic;
using System;
using UnityEngine;

namespace PaintCap
{
	public class BombManager : MonoBehaviour
	{
		List<ColorBomb> bombs = new List<ColorBomb>();
        public TileManager tileManager;

		public BombManager ()
		{
		}

		public void addBomb(Vector3 position, TileState targetTile, Color bombColor) {
			GameObject newBombGameObj = new GameObject();
			ColorBomb cb = newBombGameObj.AddComponent<ColorBomb> ();
			cb.createBomb(position, targetTile, bombColor, tileManager);
			bombs.Add (cb);
		}

		void Awake () {       

		}

		void Update () {
			
		}
	}

	public class ColorBomb : MonoBehaviour {
        private static float BAD_TILE_ALPHA = .4f;
        private static float BOMB_FADE_TIME_S = 1f;  // 3s
        private static float THETA_SCALE = 0.08f;        //Set lower to add more points
		private static float CIRCLE_RADIUS = 0.1f;
		private static int CIRCLE_POINTS = (int)((2.0f * Mathf.PI) / THETA_SCALE); 
		private static float ACCEL_PER_SECOND = .5f;

		private Vector3 initialPos;
		private Vector3? endPos;
		private Color startColor;
		private LineRenderer colorRenderer;
        private LineRenderer backgroundRenderer;

		private float curSpeed = ACCEL_PER_SECOND;
		private Vector2? endDirection;
        private TileState endTile;
        private TileManager tileManager;

        void Awake()
        {
            GameObject newGameObj = new GameObject();
            backgroundRenderer = newGameObj.AddComponent<LineRenderer>();
            GameObject colorGameObj = new GameObject();
            colorRenderer = colorGameObj.AddComponent<LineRenderer>();
        }

        public void createBomb(Vector3 initialPos, TileState endTile, Color color, TileManager tileManager)
		{
			this.initialPos = initialPos;
            this.endTile = endTile;
            this.startColor = color;
            this.tileManager = tileManager;

            if (endTile != null)
            {
                this.endPos = endTile.getTileMiddle();
                endDirection = new Vector2(endPos.Value.x - initialPos.x, endPos.Value.y - initialPos.y);
            } else
            {
                startColor.a = BAD_TILE_ALPHA;
            }

            setLr(backgroundRenderer, new Color(90,90,90,BAD_TILE_ALPHA), 9, .25f);
            setLr(colorRenderer, startColor, 10, .2f);

            colorRenderer.transform.position = initialPos;
            backgroundRenderer.transform.position = initialPos;
        }

        private void setLr(LineRenderer lr, Color color, int sortOrd, float width)
        {
            lr.startColor = color;
            lr.endColor = color;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startWidth = width;
            lr.endWidth = width;
            lr.generateLightingData = false;
            lr.sortingOrder = sortOrd;
            lr.positionCount = CIRCLE_POINTS;
            lr.useWorldSpace = false;
            drawCircle(lr);
        }

        private void fadeBomb(LineRenderer lr, float fadePct)
        {
            Color curColor = lr.startColor;
            Color newColor = new Color(curColor.r, curColor.g, curColor.b, curColor.a - fadePct);
            lr.startColor = newColor;
            lr.endColor = newColor;
        }

        private void fadeBombs()
        {
            float fadePct = Time.deltaTime / BOMB_FADE_TIME_S;
            // fade the bomb away
            fadeBomb(colorRenderer, fadePct);
            fadeBomb(backgroundRenderer, fadePct);
            if (colorRenderer.startColor.a <= 0)
            {
                DestroyEverything();
            }
        }

        void Update ()
        {
            // bomb didn't have any matches, fade out
            if (endTile == null)
            {
                fadeBombs();
            }
            else
            {
                curSpeed += ACCEL_PER_SECOND * Time.deltaTime;
                float moveAmount = Time.deltaTime * curSpeed;
                moveTowardsEnd(moveAmount);
            }
        }


        private void moveTowardsEnd(float moveAmount)
        {
            moveCircles(moveAmount);

            if (isAtEndPoint())
            {
                Vector2Int coords = Vector2Int.FloorToInt(initialPos);
                tileManager.setCapturedTile(tileManager.borderWhiteTile.tile, coords.x, coords.y);
                DestroyEverything();
            }
        }

        private void DestroyEverything()
        {
            Destroy(backgroundRenderer.gameObject);
            Destroy(colorRenderer.gameObject);
            Destroy(gameObject);
        }

        void moveCircles(float moveAmount)
        {
            moveTowardsEndPos(moveAmount, colorRenderer);
            moveTowardsEndPos(moveAmount, backgroundRenderer);
        }

		void moveTowardsEndPos(float moveAmount, LineRenderer lr) {
            Transform tran = lr.transform;
			Vector3 newPos = new Vector3(
                tran.position.x + (moveAmount * endDirection.Value.x),
                tran.position.y + (moveAmount * endDirection.Value.y),
                tran.position.z
			);
			tran.position = newPos;
		}

		private bool isAtEndPoint() {
            if (!endPos.HasValue)
            {
                return true;
            }

			Vector3 curPos = colorRenderer.transform.position;
			return isDimensionDone (endDirection.Value.x, curPos.x, endPos.Value.x) &&
				   isDimensionDone (endDirection.Value.y, curPos.y, endPos.Value.y);
			
		}

		private bool isDimensionDone(float endDir, float cur, float end) {
			return (endDir >= 0 && cur >= end || endDir < 0 && cur < end);
		}

		//TODO: this should be elsewhere
		private void drawCircle(LineRenderer lr) {
			Vector3 pos;
			float theta = 0f;
            Transform tran = lr.transform;
			for(int i = 0; i < CIRCLE_POINTS; i++){          
				theta += (2.0f * Mathf.PI * THETA_SCALE);         
				float x = CIRCLE_RADIUS * Mathf.Cos(theta);
				float y = CIRCLE_RADIUS * Mathf.Sin(theta);          
				x += tran.position.x;
				y += tran.position.y;
				pos = new Vector3(x, y, 20);
				lr.SetPosition(i, pos);
			}
		}
	}
}


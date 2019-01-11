using System.Collections.Generic;
using System;
using UnityEngine;

namespace PaintCap
{
	public class BombManager : MonoBehaviour
	{
		List<ColorBomb> bombs = new List<ColorBomb>();

		public BombManager ()
		{
		}

		public void addBomb(Vector3 position, Vector3 targetPos, Color bombColor) {
			GameObject newBombGameObj = new GameObject();
			ColorBomb cb = newBombGameObj.AddComponent<ColorBomb> ();
			cb.createBomb(position, targetPos, bombColor);
			bombs.Add (cb);
		}

		void Awake () {       

		}

		void Update () {
			
		}
	}

	public class ColorBomb : MonoBehaviour {
		private static float THETA_SCALE = 0.08f;        //Set lower to add more points
		private static float CIRCLE_RADIUS = 0.1f;
		private static int CIRCLE_POINTS = (int)((2.0f * Mathf.PI) / THETA_SCALE); 
		private static float TIME_ACTIVE = 3f;
		private static float ACCEL_PER_SECOND = .3f;

		public Vector3 initialPos;
		public Vector3 endPos;
		private Color startColor;
		private LineRenderer colorRenderer;
        private LineRenderer backgroundRenderer;

		private float curSpeed = 0f;
		private Vector2 endDirection;

        void Awake()
        {
            GameObject newGameObj = new GameObject();
            backgroundRenderer = newGameObj.AddComponent<LineRenderer>();
            GameObject colorGameObj = new GameObject();
            colorRenderer = colorGameObj.AddComponent<LineRenderer>();
        }

        public void createBomb(Vector3 initialPos, Vector3 endPos, Color color)
		{
			this.initialPos = initialPos;
			this.endPos = endPos;
			this.startColor = color;
            Debug.Log(string.Format("Drawing circle at {0} ", initialPos));
            setLr(backgroundRenderer, Color.gray, 9, .25f);
            setLr(colorRenderer, color, 10, .2f);
			endDirection = new Vector2 (endPos.x - initialPos.x, endPos.y - initialPos.y);

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

		void Update () {
			curSpeed += ACCEL_PER_SECOND * Time.deltaTime;
			float moveAmount = Time.deltaTime * curSpeed;
            moveCircles(moveAmount);

            if (isAtEndPoint ()) {
                Destroy(backgroundRenderer.gameObject);
                Destroy(colorRenderer.gameObject);
                Destroy (gameObject);
			}
		}

        void moveCircles(float moveAmount)
        {
            moveTowardsEndPos(moveAmount, colorRenderer);
            moveTowardsEndPos(moveAmount, backgroundRenderer);
        }

		void moveTowardsEndPos(float moveAmount, LineRenderer lr) {
            Transform tran = lr.transform;
			Vector3 newPos = new Vector3(
                tran.position.x + (moveAmount * endDirection.x),
                tran.position.y + (moveAmount * endDirection.y),
                tran.position.z
			);
			tran.position = newPos;
		}

		private bool isAtEndPoint() {
			Vector3 curPos = colorRenderer.transform.position;
			return isDimensionDone (endDirection.x, curPos.x, endPos.x) &&
				   isDimensionDone (endDirection.y, curPos.y, endPos.y);
			
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


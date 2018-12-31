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

		public void addBomb(Vector3 position, Color bombColor) {
			GameObject newBombGameObj = new GameObject();
			ColorBomb cb = newBombGameObj.AddComponent<ColorBomb> ();
			cb.drawBomb (position, new Vector3(0,0,0), bombColor);
			bombs.Add (cb);
		}

		void Awake () {       

		}

		void Update () {
			
		}
	}

	public class ColorBomb : MonoBehaviour {
		private static float THETA_SCALE = 0.05f;        //Set lower to add more points
		private static float CIRCLE_RADIUS = 0.1f;
		private static int CIRCLE_POINTS = (int)((2.0f * Mathf.PI) / THETA_SCALE); 
		private static float TIME_ACTIVE = 3f;
		private static float ACCEL_PER_SECOND = .3f;

		public Vector3 initialPos;
		public Vector3 endPos;
		private Color startColor;
		private LineRenderer lr;

		private float curSpeed = 0f;
		private Vector2 endDirection;

		public void drawBomb(Vector3 initialPos, Vector3 endPos, Color color)
		{
			this.initialPos = initialPos;
			this.endPos = endPos;
			this.startColor = color;
			lr = gameObject.AddComponent<LineRenderer>();
			lr.startColor = color;
			lr.endColor = color;
			lr.material = new Material(Shader.Find("Sprites/Default"));
			lr.startWidth = 0.35f;
			lr.endWidth = 0.35f;
			lr.generateLightingData = false;
			lr.sortingOrder = 10;
			lr.positionCount = CIRCLE_POINTS;
			lr.useWorldSpace = false;
			drawCircle ();
			endDirection = new Vector2 (endPos.x - initialPos.x, endPos.y - initialPos.y);

			this.transform.position = initialPos;
		}

		void Update () {
			curSpeed += ACCEL_PER_SECOND * Time.deltaTime;
			float moveAmount = Time.deltaTime * curSpeed;
			moveTowardsEndPos (moveAmount);
			if (isAtEndPoint ()) {
				Destroy (gameObject);
			}

		}

		void moveTowardsEndPos(float moveAmount) {
			Vector3 newPos = new Vector3(
				this.transform.position.x + (moveAmount * endDirection.x), 
				this.transform.position.y + (moveAmount * endDirection.y),
				this.transform.position.z
			);
			
			this.transform.position = newPos;
		}

		private bool isAtEndPoint() {
			Vector3 curPos = this.transform.position;
			return isDimensionDone (endDirection.x, curPos.x, endPos.x) &&
				   isDimensionDone (endDirection.y, curPos.y, endPos.y);
			
		}

		private bool isDimensionDone(float endDir, float cur, float end) {
			return (endDir >= 0 && cur >= end || endDir < 0 && cur < end);
		}

		//TODO: this should be elsewhere
		private void drawCircle() {
			Debug.Log(string.Format("Drawing circle at {0} ", initialPos));
			Vector3 pos;
			float theta = 0f;
			for(int i = 0; i < CIRCLE_POINTS; i++){          
				theta += (2.0f * Mathf.PI * THETA_SCALE);         
				float x = CIRCLE_RADIUS * Mathf.Cos(theta);
				float y = CIRCLE_RADIUS * Mathf.Sin(theta);          
				x += gameObject.transform.position.x;
				y += gameObject.transform.position.y;
				pos = new Vector3(x, y, 20);
				lr.SetPosition(i, pos);
			}
		}
	}
}


using System;
using UnityEngine;

namespace PaintCap
{
	public class ActiveColorManager : MonoBehaviour
	{
		public LineRenderer solidColorLine;
        public Transform colorLinesTransform;
		public Camera uiCamera;

		private float timer = 0.0f;
		private float curTimeInCycle = 0.0f;
		private const float WAIT_TIME = 0.01f;
		private const float CYCLE_TIME = 12f; 

		//TODO: make dyamic from colorLine's initial length
		private const float LINE_LENGTH = 10f; 

		private const float ONE_THIRD = 1f / 3f;
        private const float ONE_SIXTH = 1f / 6f;
        private const float TWO_THIRDS = 2f / 3f;

        private Vector3 origLinePoint;

        public ActiveColorManager ()
		{
        }

        void Awake()
        {
            origLinePoint = colorLinesTransform.position;
        }

		//Update is called every frame.
		void Update()
		{
			timer += Time.deltaTime;
			curTimeInCycle += Time.deltaTime;
			if (curTimeInCycle > CYCLE_TIME) {
				curTimeInCycle = curTimeInCycle - CYCLE_TIME;
			}

			if (timer > WAIT_TIME)
			{
				timer = timer - WAIT_TIME;
			}
			float cyclePct = getCyclePct ();
			moveColorLineToPct (cyclePct);
			changeSolidColor (cyclePct);
		}

		public void handleResChange() {
			Vector2 camTopLeft = uiCamera.ScreenToWorldPoint(new Vector2 (uiCamera.rect.xMin, uiCamera.rect.yMin));
			Vector2 solidLinePos = solidColorLine.transform.position;

		}

		public Color getCurColor() {
			return getCurColor(getCyclePct());
		}

		private void moveColorLineToPct(float pct) 
		{
			float lineXPos = LINE_LENGTH * pct;
			Vector3 curPos = colorLinesTransform.position;
			Vector3 newPos = new Vector3(origLinePoint.x + lineXPos, curPos.y, curPos.z);
            colorLinesTransform.position = newPos;
		}

		private void changeSolidColor(float pct)
		{
			Color color = getCurColor (pct);
			solidColorLine.startColor = color;
			solidColorLine.endColor = color; 
		}

		private Color getCurColor(float pct) {
			float rVal=0f;
			float bVal=0f;
			float gVal=0f;

            // Color vals stay at 100% in the 1/6th range surrounding their peak for more vibrant colors! pretty! aayyay!
            // From 0 - 1/6 - 1/3 - 3/6 - 2/3 - 5/6 - 0
            //     R=1        B=1         G=1
            float distFromThird = Math.Abs(ONE_THIRD - pct);
            float distFrom2Third = Math.Abs(TWO_THIRDS - pct);
            float distFrom0 = pct >= (1f/2f) ? 1f - pct : pct;

            //Debug.Log(string.Format("pct {3}, 0 {0} 1 {1} 2 {2} ", distFrom0, distFromThird, distFrom2Third, pct));
            //TODO:  there's got to be a better way?
            if (distFrom0 <= ONE_SIXTH )
            {
                rVal = 1;
                if (pct >= 1 - ONE_SIXTH)
                {
                    bVal = distFrom0 / ONE_SIXTH;
                }
                else
                {
                    gVal = distFrom0 / ONE_SIXTH;
                }
            }
            else if (distFromThird <= ONE_SIXTH)
            {
                gVal = 1;
                if (pct > ONE_THIRD)
                {
                    bVal = distFromThird / ONE_SIXTH;
                }
                else
                {
                    rVal = distFromThird / ONE_SIXTH;
                }
            }
            else
            {
                bVal = 1;
                if (pct > TWO_THIRDS)
                {
                    rVal = distFrom2Third / ONE_SIXTH;
                }
                else
                {
                    gVal = distFrom2Third / ONE_SIXTH;
                }
            }
            //Debug.Log(string.Format("pct {3} r {0} g {1} b {2}", rVal, gVal, bVal, pct));
            return new Color(rVal, gVal, bVal, 1f);
		}
			

		private float getCyclePct()
		{
			return curTimeInCycle / CYCLE_TIME;
		}
	}
}

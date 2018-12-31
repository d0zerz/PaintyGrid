using System;
using UnityEngine;

namespace PaintCap
{
	public class ActiveColorManager : MonoBehaviour
	{
		public LineRenderer colorLine;
		public LineRenderer solidColorLine;

		private float timer = 0.0f;
		private float curTimeInCycle = 0.0f;
		private const float WAIT_TIME = 0.01f;
		private const float CYCLE_TIME = 8f; 

		//TODO: make dyamic from colorLine's length
		private const float LINE_LENGTH = 6f; 

		private const float ONE_THIRD = 1f/3f;
		private const float TWO_THIRDS = 2f/3f;

		public ActiveColorManager ()
		{
			
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

		public Color getCurColor() {
			return getCurColor(getCyclePct());
		}

		private void moveColorLineToPct(float pct) 
		{
			float lineXPos = LINE_LENGTH * pct;
			Vector3 curPos = colorLine.transform.position;
			Vector3 newPos = new Vector3(lineXPos, curPos.y, curPos.z);
			colorLine.transform.position = newPos;
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

			if (pct < ONE_THIRD) {
				rVal = 1 - (pct / ONE_THIRD);
				bVal = pct / ONE_THIRD; 
			}
			else if (pct < TWO_THIRDS) {
				float relPct = pct - ONE_THIRD;
				bVal = 1 - relPct / ONE_THIRD; 
				gVal = relPct / ONE_THIRD;
			}
			else {
				float relPct = pct - TWO_THIRDS;
				gVal = 1- relPct / ONE_THIRD;
				rVal = relPct / ONE_THIRD;
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

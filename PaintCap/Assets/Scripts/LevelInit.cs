using UnityEngine;
using System.Collections;

namespace PaintCap
{
    public class LevelInitData
    {
        public Vector3 cameraEndPos;
    }

    public class LevelInit : MonoBehaviour
    {
        public Camera mainCam;

        private bool initializingLevel = false;
        private Vector3 targetLevelLoadPosStart;
        private Vector3 targetLevelLoadPosEnd;
        private float timeThroughInit = 0f;
        private const float INIT_ANIMATION_TIME = 1f;
        private const float INIT_PAUSE_TIME = .2f;
        private const float INIT_TOTAL_TIME = INIT_PAUSE_TIME + INIT_ANIMATION_TIME;

        private const float INIT_SMOOTH_SPEED = .2f;

        public void initLevel()
        {
            // move camera to endpos
            targetLevelLoadPosEnd = new Vector3(1, 1, mainCam.transform.position.z);
            targetLevelLoadPosStart = mainCam.transform.position;
            initializingLevel = true;
        }

        public bool isInitializing()
        {
            return initializingLevel;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (initializingLevel)
            {
                //move camera to bottom left over a few seconds

                timeThroughInit += Time.deltaTime;
                if (timeThroughInit > INIT_TOTAL_TIME)
                {
                    initializingLevel = false;
                }
                else
                {
                    float pctThroughAnimation = timeThroughInit < INIT_PAUSE_TIME ? 0 : (timeThroughInit - INIT_PAUSE_TIME) / INIT_ANIMATION_TIME;
                    Vector3 smoothedPos = Vector3.Lerp(targetLevelLoadPosStart, targetLevelLoadPosEnd, pctThroughAnimation);
                    mainCam.transform.position = smoothedPos;
                }
            }
        }
    }

}
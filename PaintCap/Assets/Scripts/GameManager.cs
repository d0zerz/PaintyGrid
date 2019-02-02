using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 

namespace PaintCap
{
	public class GameManager : MonoBehaviour
	{
	    public static GameManager instance = null;  //Static instance of GameManager which allows it to be accessed by any other script.

		public Tilemap backgroundTilemap;
		public TileManager tileManager;
		public ActiveColorManager acm;
		public BombManager bombManager;
        public ScreenChange screenChange;
        public Camera mainCam;
        public Camera uiCam;

        private BoardState boardState;
		private int gameCounter;
		private Vector3? lastMousePos = null;

	    //Awake is always called before any Start functions
	    void Awake()
	    {
	        //Check if instance already exists
			if (instance == null) {
				//if not, set instance to this
				instance = this;
				boardState = new BoardState (tileManager);
			}

	        //If instance already exists and it's not this:
	        else if (instance != this)

	            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
	            Destroy(gameObject);

	        //Sets this to not be destroyed when reloading scene
	        DontDestroyOnLoad(gameObject);
	        InitGame();
	    }

	    void InitGame()
	    {
	        Debug.Log("init game");
			boardState.initBoard();
			boardState.paintBoardState ();
            screenChange.OnOrientationChange.AddListener(ResolutionChange);
            screenChange.OnResolutionChange.AddListener(ResolutionChange);
        }

        void ResolutionChange()
        {
            Rect uiRect = uiCam.rect;
            Vector2 mainWorldMin = uiCam.ViewportToWorldPoint(new Vector3(uiRect.xMin, uiRect.yMin));
            Vector2 mainWorldMax = uiCam.ViewportToWorldPoint(new Vector3(uiRect.xMax, uiRect.yMax));

            Rect mainCamRect = mainCam.rect;
            Vector2 resolution = new Vector2(Screen.width, Screen.height);
            DeviceOrientation orientation = Input.deviceOrientation;
            Debug.Log(string.Format("Resolution change [{0},{1}] : {2}",resolution.x, resolution.y, orientation));
            Debug.Log(string.Format("UI Rect{0} {1} {2} {3}  |||  mainCamRec{4} {5} {6} {7}", mainWorldMin.x, mainWorldMin.y, mainWorldMax.x, mainWorldMax.y, 
                mainCamRect.xMin, mainCamRect.xMax, mainCamRect.yMin, mainCamRect.yMax));
        }

        //Update is called every frame.
        void Update()
        {
            gameCounter++;
            if (gameCounter % 100 == 0)
            {
                //DrawTopColors();
                //Debug.Log(string.Format("gameCounter {0}", gameCounter));
            }
			if (!Input.GetKey("left ctrl") && !Input.GetMouseButton(1)) {
				lastMousePos = null;
			}
			if (Input.GetKey("left ctrl") || Input.GetMouseButton(1))
			{
				if (lastMousePos.HasValue)
                {
                    Vector2 posDiff = mainCam.ScreenToWorldPoint(lastMousePos.Value) - mainCam.ScreenToWorldPoint(Input.mousePosition);
                    moveMainCam(posDiff);
                    Debug.Log(string.Format("PosDiff {0} ", posDiff));
                }
                //Debug.Log(string.Format("Co-ords of right click is [X: {0} Y: {1}]", pointClicked.x, pointClicked.y));
                lastMousePos = Input.mousePosition;
			}
            else if (Input.GetMouseButtonDown(0))
            {
                Vector3 pointClicked = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                TileState tileState = boardState.getTileState(pointClicked);
                if (tileState != null)
                {
                    Color color = tileState.getGameTile().getTileColor();
                    TileCapture bestTileMatch = boardState.processBombDrop(pointClicked, acm.getCurColor());
                    bombManager.addBomb(pointClicked, bestTileMatch, acm.getCurColor());
                    Debug.Log(string.Format("Co-ords of mouse is [X: {0} Y: {1}] {2}", pointClicked.x, pointClicked.y, color.ToString()));
                }
            }
        }

        private const float OUT_OF_BOUNDS_VISUAL_GIVE = .1f;
        private void moveMainCam(Vector2 posDiff)
        {
            Rect mainCamRect = mainCam.rect;
            Vector2 mainWorldMin = mainCam.ViewportToWorldPoint(new Vector3(mainCamRect.xMin, mainCamRect.yMin));
            Vector2 mainWorldMax = mainCam.ViewportToWorldPoint(new Vector3(mainCamRect.xMax, mainCamRect.yMax));

            Vector3 curPos = mainCam.transform.position; // mainCam.ScreenToWorldPoint(posDiff) // .transform.position;
            Debug.Log(string.Format("xMin{0} yMin{1} xMax{2} yMax{3}", mainWorldMin.x, mainWorldMin.y, mainWorldMax.x, mainWorldMax.y));
            if (mainWorldMin.x < -1f * OUT_OF_BOUNDS_VISUAL_GIVE && posDiff.x < 0)
            {
                posDiff.x = 0;
            }
            if (mainWorldMin.y < -1f * OUT_OF_BOUNDS_VISUAL_GIVE && posDiff.y < 0)
            {
                posDiff.y = 0;
            }
            if (mainWorldMax.x > boardState.getBoardDimensions().x + OUT_OF_BOUNDS_VISUAL_GIVE && posDiff.x > 0)
            {
                posDiff.x = 0;
            }
            if (mainWorldMax.y > boardState.getBoardDimensions().y + OUT_OF_BOUNDS_VISUAL_GIVE + .5f && posDiff.y > 0)
            {
                posDiff.y = 0;
            }
            mainCam.transform.position = new Vector3(curPos.x + posDiff.x, curPos.y + posDiff.y, curPos.z);
        }
    }
}

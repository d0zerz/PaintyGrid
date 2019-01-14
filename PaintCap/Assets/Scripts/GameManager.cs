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
		public Transform mainCam;
		public ActiveColorManager acm;
		public BombManager bombManager;

        private BoardState boardState;

		private int gameCounter;

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
			boardState.initBoard(new Vector2Int(20,20));
			boardState.paintBoardState ();
			//camera.position = new Vector3(10, 10, -10);
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

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pointClicked = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                TileState tileState = boardState.getTileState(pointClicked);
                Color color = tileState.getGameTile().getTileColor();
                TileState bestTileMatch = boardState.getNearestMatch(pointClicked, acm.getCurColor());
                bombManager.addBomb(pointClicked, bestTileMatch, acm.getCurColor());
                Debug.Log(string.Format("Co-ords of mouse is [X: {0} Y: {1}] {2}", pointClicked.x, pointClicked.y, color.ToString()));
            }
        }
	}
}
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

using System.Collections.Generic;       //Allows us to use Lists. 

namespace PaintCap
{
	public class GameManager : MonoBehaviour
	{
	    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

		public Tilemap backgroundTilemap;
		public TileManager tileManager;
		public Transform camera;

	    private int redVal = 0;
        private long gameCounter = 0;
        private BoardState boardState;

	    public int getRedVal()
	    {
	        return redVal;
	    }

	    //Awake is always called before any Start functions
	    void Awake()
	    {
	        //Check if instance already exists
			if (instance == null) {
				//if not, set instance to this
				instance = this;
				boardState = new BoardState (backgroundTilemap, tileManager);
			}

	        //If instance already exists and it's not this:
	        else if (instance != this)

	            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
	            Destroy(gameObject);

	        //Sets this to not be destroyed when reloading scene
	        DontDestroyOnLoad(gameObject);

	        //Call the InitGame function to initialize the first level 
	        InitGame();
	    }

	    void InitGame()
	    {
	        Debug.Log("init game");
			boardState.initBoard(new Vector2Int(20,20));
			boardState.paintBoardState ();
			camera.position = new Vector3(10, 10, -10);
	    }


        void UpdateActiveColor()
        {
            redVal++;
            if (redVal > 255) redVal = 0;

        }

        void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
        {
            GameObject myLine = new GameObject();
            myLine.transform.position = start;
            myLine.AddComponent<LineRenderer>();
            LineRenderer lr = myLine.GetComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("UI/Default"));
            lr.startWidth = 5f;
            lr.endWidth = 5f;
            lr.startColor = color;
            lr.endColor = color;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
            lr.useWorldSpace = false;
//            GameObject.Destroy(myLine, duration);
        }

        void DrawTopColors()
        {
            Color color = new Color(.5F, .5F, .5F);


            int xPos = Screen.width / 2;
            int yPos = Screen.height;
            Debug.Log(string.Format("drawLine xPos {0} yPos {1}", xPos, yPos));
            DrawLine (new Vector3Int (xPos - 50, yPos, -1), new Vector3Int (xPos + 50, yPos - 100, -1), Color.cyan, 5000f);
        }

        //Update is called every frame.
        void Update()
	    {
            gameCounter++;
            if (gameCounter % 100 == 0)
            {
                DrawTopColors();
                Debug.Log(string.Format("gameCounter {0}", gameCounter));
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector3Int pos = Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                Debug.Log(string.Format("Co-ords of mouse is [X: {0} Y: {1}]", pos.x, pos.y));
                boardState.setTileToBlack(pos);
                boardState.paintBoardState();
            }
        }
	}
}
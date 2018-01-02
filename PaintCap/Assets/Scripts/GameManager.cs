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

	    //Update is called every frame.
	    void Update()
	    {
	        redVal++;
	        if (redVal > 255) redVal = 0;
	    }
	}
}
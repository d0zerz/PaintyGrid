using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    private static int mouseCount = 0;
    
	// Use this for initialization
	void Start () {
        Debug.Log("Startup NewBehaviourScript");
	}
	
	// Update is called once per frame
	void Update () {
        
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
                Debug.Log("Touched");
        }
        if (Input.GetMouseButtonDown(0))
        {
            mouseCount++;
            Debug.Log("Moused " + mouseCount);
        }
        if (Input.GetMouseButtonDown(0))
        {
            mouseCount++;
            Debug.Log("Moused " + mouseCount + " " + Input.mousePosition);
        }
    }

}

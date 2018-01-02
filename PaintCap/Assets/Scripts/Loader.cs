﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaintCap
{
	public class Loader : MonoBehaviour {

	    public GameObject gameManager;

	    void Awake () {
	        if (GameManager.instance == null)

	            //Instantiate gameManager prefab
	            Instantiate(gameManager);
	    }
	}
}
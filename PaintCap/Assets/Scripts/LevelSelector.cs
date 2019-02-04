using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour {
    public static int currentLevel = 1;
    public void LoadScene(int level)
    {
        currentLevel = level;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}

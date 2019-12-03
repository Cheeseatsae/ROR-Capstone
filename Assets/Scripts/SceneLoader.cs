﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadPlayScene()
    {
        SceneManager.LoadScene(1);
        Jukebox.instance.SetProgressionVariable(4);
        LevelManager.instance.Pause();
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(0);


    }
}

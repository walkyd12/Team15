using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {



    public void loadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
        if (PF_stuff.loggedin == 0)
            PF_stuff.login("Hello");
    }

    public void leaderBoard(string string_name)
    {
        PF_stuff.getleaderboard("hello");
        print(PF_stuff.LB);

    }

    public void login(string x)
    {
        PF_stuff.login("hello");
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

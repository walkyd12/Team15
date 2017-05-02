﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public static string LS = "";

    public void loadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }

    public void leaderBoard(string string_name)
    {
        PF_stuff.getleaderboard("hello");
        LS = "Rank    Player ID                    Points\n";
        for(int i=0;i<10;i++)
        {
            LS=LS+PF_stuff.LB[i].Position.ToString()+"          "+PF_stuff.LB[i].PlayFabId.ToString()+" "+PF_stuff.LB[i].StatValue.ToString()+"\n";
        }
        print(LS);

        LS = LS + "\nYour Player ID: " + PF_stuff.UserId;
    }

    public void login(string x)
    {
        if (!PF_stuff.loggedin)
        {
            PF_stuff.login("hello");

        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

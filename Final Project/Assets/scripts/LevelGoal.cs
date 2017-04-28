using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour {
    public static int points;
    public static int wins;
    public Text leaderboard;
    bool levelComplete = false;
    int bonesLeft;

    //use this goal variable to choose the type of goal for the level
    // 0 is for collect all bones
    public int goal = 0;
    // Use this for initialization
    void Start () {
		if(goal == 0)
        {
            countBones();
        }
	}
	
	// Update is called once per frame
	void Update () {
        //countBones();
	}
    void countBones()
    {
        int bonesLeftPrevious = bonesLeft;
        GameObject[] getCount;
        getCount = GameObject.FindGameObjectsWithTag("bones");
        bonesLeft = getCount.Length;
        string p = bonesLeft.ToString();
        string t = string.Format("{1} {0}", p, "Bones Left: ");
        this.GetComponent<GUIText>().text = t;

    }
    public void updateBones()
    {
        bonesLeft--;
        string p = bonesLeft.ToString();
        string t = string.Format("{1} {0}", p, "Bones Left: ");
        this.GetComponent<GUIText>().text = t;
        if (bonesLeft == 0)
        {
            levelComplete = true;
            
            points += 10;
            wins++;
            Dictionary<string, int> updates = new Dictionary<string, int>();
            updates.Add("points", LevelGoal.points);
            updates.Add("wins", LevelGoal.wins);
            PF_stuff.UpdateUserStatistics(updates);
            changeLevel();


        }
    }

    void changeLevel()
    {
        Scene s = SceneManager.GetActiveScene();
        print(s.name);
        if(s.name == "simple_level")
        {
            loadLevel("empty_level");
        }
    }
    public void loadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
}

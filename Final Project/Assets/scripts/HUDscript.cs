using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class HUDscript : MonoBehaviour {
    public ball Hugo;
    public ball Chihuahua;
    public ball StBernard;
     ball HugoPrefab;
     ball ChihuahuaPrefab;
     ball StBernardPrefab;
    int currentCharacter;
    // Use this for initialization
    void Start () {
        currentCharacter = 0;
        switchCharacter();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void resetLevel(string levelName)
    {
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }

    public void switchCharacter()
    {
        currentCharacter++;
        if (currentCharacter == 1)
        {
            print("111");

            HugoPrefab = Resources.Load("prefabs/Hugo") as ball;
            Hugo = ball.Instantiate(HugoPrefab, transform.position, transform.rotation);
            print("111111");
        }
        else if (currentCharacter == 2)
        {
            print("222");

            ChihuahuaPrefab = Resources.Load("prefabs/Chihuahua") as ball;
            Chihuahua = ball.Instantiate(ChihuahuaPrefab, transform.position, transform.rotation);
        }
        else
        {
            print("333");

            StBernardPrefab = Resources.Load("prefabs/StBernard") as ball;
            StBernard = ball.Instantiate(StBernardPrefab, transform.position, transform.rotation);
            currentCharacter = 0;
            print(currentCharacter);
        }
    }
}

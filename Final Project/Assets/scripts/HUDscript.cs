using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class HUDscript : MonoBehaviour {

    public ball HugoPrefab;
    public ball ChihuahuaPrefab;
    public ball StBernardPrefab;
    ball Hugo;
    ball Chihuahua;
    ball StBernard;
    Vector3 center;
    Vector3 storage;
    int currentCharacter;
    ball currentBall;
    bool outOfThrows;
    // Use this for initialization
    void Start () {
        currentCharacter = 0;

         center = new Vector3(0, 0, 0);
        storage = new Vector3(300, 300, 0);
        StBernard = Instantiate(StBernardPrefab, center, transform.rotation);
        Hugo = Instantiate(HugoPrefab, center, transform.rotation);
        Chihuahua = Instantiate(ChihuahuaPrefab, center, transform.rotation);
        storeAllCharacters();
        switchCharacter();
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public ball getCurrentBall()
    {
        return currentBall;
    }

    public void resetLevel(string levelName)
    {
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }

    public void switchCharacter()
    {

        bool switched = false;
        int counter = 0;
        if (outOfThrows == false)
        {
            while (switched == false)
            {
                currentCharacter++;
                if (currentCharacter == 1)
                {
                    if (StBernard.getHoldAllowed() == true)
                    {
                        storeCharacter(StBernard);
                    }
                    if (Hugo.getStorage() == true)
                    {
                        centerCharacter(Hugo);
                        currentBall = Hugo;
                        switched = true;
                    }

                }
                else if (currentCharacter == 2)
                {
                    if (Hugo.getHoldAllowed() == true)
                    {
                        storeCharacter(Hugo);
                    }
                    if (Chihuahua.getStorage() == true)
                    {
                        centerCharacter(Chihuahua);
                        currentBall = Chihuahua;
                        switched = true;
                    }

                }
                else
                {
                    if (Chihuahua.getHoldAllowed() == true )
                    {
                        storeCharacter(Chihuahua);
                    }
                    if (StBernard.getStorage() == true)
                    {
                        centerCharacter(StBernard);
                        currentBall = StBernard;
                        switched = true;
                    }
                    currentCharacter = 0;
                }
                counter++;

                if (counter > 3)
                {
                    switched = true;
                    outOfThrows = true;
                }

            }
        }
}
    


    void centerCharacter(ball b)
    {
        b.transform.position = center;
        b.setStorage(0);
        
    }
    void storeCharacter(ball b)
    {
        b.transform.position = storage;
        b.setStorage(1);
        

    }
    void storeAllCharacters()
    {
        storeCharacter(Hugo);
        storeCharacter(Chihuahua);
        storeCharacter(StBernard);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dogbone : MonoBehaviour {
    LevelGoal lg;
    public bool isEaten = false;
    //GUIText gt;
	// Use this for initialization
	void Start () {
        lg = FindObjectOfType<LevelGoal>();
        //gt = lg.GetComponent<GUIText>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider c)
    {
        //trigger for collecting bones
        
        if (c.tag == "ball")
        {
            isEaten = true;
            Destroy(gameObject);
        }
        else if (c.tag == "tiger")
        {
            isEaten = true;
            Destroy(gameObject);
        }

        LevelGoal.points += 5;
        lg.updateBones();
    
    }

}

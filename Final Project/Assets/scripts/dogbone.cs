using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dogbone : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider c)
    {
        //ADD CODE HERE FOR ADDING SCORE
        print("trigger working");
        if (c.tag == "ball")
        {
            Destroy(gameObject);
        }
    
    }
}

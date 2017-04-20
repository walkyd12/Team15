using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeHydrant : MonoBehaviour {

    public object makeHyd()
    {
        object hyd = Instantiate(Resources.Load("firehydrantPF"));
        return hyd;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

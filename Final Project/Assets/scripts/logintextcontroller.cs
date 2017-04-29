using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class logintextcontroller : MonoBehaviour {

    public Text text;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (PF_stuff.loggedin)
            text.text = "Login Success!";

    }
}

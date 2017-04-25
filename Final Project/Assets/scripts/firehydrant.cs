using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class firehydrant : MonoBehaviour {
    public bool smashed = false;
    int score = 0;
    Animator ani;

    void OnCollisionEnter(Collision c)
    {
       
        smashed = true;
    }
    void OnTriggerStay(Collider other)
    {

        if (smashed == true)
        {
           
            Vector3 dir = new Vector3(1000, 0, 0);
            other.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Force);
       

    }
}

    private void resultCallback2(UpdatePlayerStatisticsResult obj)
    {
        throw new NotImplementedException();
    }

    void Start()
    {
        ani = GetComponent<Animator>();
    }
    void Update()
    {
        if(smashed == true)
        {
            ani.SetBool("isSmashed", true);
            
        }
    }
}

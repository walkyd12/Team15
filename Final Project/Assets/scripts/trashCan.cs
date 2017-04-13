using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashCan : MonoBehaviour
{
    bool smashed = false;

    void OnCollisionEnter(Collision c)
    {
        
        smashed = true;
    }
    void OnTriggerStay(Collider other)
    {

        if (smashed == true)
        {
            
            Vector3 dir = new Vector3(500, -1000, 0);
            other.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Force);
        }
    }
    void Start()
    {

    }
    void Update()
    {

    }
}

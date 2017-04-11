using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashCan : MonoBehaviour
{
    bool smashed = false;

    void OnCollisionEnter(Collision c)
    {
        print("smashing!");
        smashed = true;
    }
    void OnTriggerStay(Collider other)
    {

        if (smashed == true)
        {
            print("aaaaaa");
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

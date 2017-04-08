using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firehydrant : MonoBehaviour {
    bool smashed = false;
    Animator ani;

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
            Vector3 dir = new Vector3(1000, 0, 0);
            other.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Force);
        }
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

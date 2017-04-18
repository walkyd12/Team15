using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashCan : MonoBehaviour
{
    public bool smashed = false;
    public trashCan can;


    void OnTriggerStay(Collider other)
    {
        smashed = true;

        Vector3 positionOfTrash = gameObject.transform.position;
        Vector3 positionOfBall = other.transform.position;
        Vector3 dir = positionOfTrash - positionOfBall;
        float mag = dir.magnitude;
        dir = dir.normalized;
        dir = dir * 2000 * (1/(mag));
        print(mag);

        other.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Force);
         
    }
    void Start()
    {

    }
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashCan : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
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

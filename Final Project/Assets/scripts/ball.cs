using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour {
    Vector3 target;
    Vector3 prevTarget;
    public Rigidbody rb;
    Vector3[] lastFive;
    int fiveC = 0;
    public float SZP = 0;
    bool holdAllowed = true;
    bool released = false;

	void Start () {
        target = transform.position;
        prevTarget = transform.position;
        rb = GetComponent<Rigidbody>();
        lastFive = new Vector3[5];
        
    }

    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
    void OnCollisionEnter(Collision c)
    {

        rb.velocity = Vector3.Reflect(rb.velocity, Vector3.right);
        
    }
    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "holdarea")
        {
            holdAllowed = true;
        }
    }
    void OnTriggerExit(Collider c)
    {
        if (c.tag == "holdarea")
        {
            holdAllowed = false;
        }
    }
   
    void Update()
    {
        holdThrowHandler();
    }
   public bool getReleased()
    {
        return released;
    }

    void holdThrowHandler()
    {
        if (holdAllowed == true)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                prevTarget = target;
                target = GetWorldPositionOnPlane(Input.mousePosition, SZP);
                target.z = SZP;
                transform.position = target;
                if (fiveC >= 5)
                {
                    fiveC = 0;
                    lastFive[fiveC] = target - prevTarget;
                }
                else
                {
                    lastFive[fiveC] = target - prevTarget;
                    fiveC++;
                }

            }
            if (Input.GetMouseButtonUp(0))
            {
                release();
            }

        }
        else
        {
            if (released == false)
            {
                release();
            }
        }
    }
    void release()
    {
        rb.velocity = (calThrow(lastFive)) * 25;
        released = true;
    }

    Vector3 calThrow(Vector3[] array)
    {
        Vector3 reVec = new Vector3(0,0,0);
        for(int x=0; x<5; x++)
        {
            reVec = reVec + array[x];
        }
        reVec.x = reVec.x / 5;
        reVec.y = reVec.y / 5;
        reVec.z = SZP;
        if (reVec.x > 50)
        {
            reVec.x = 50;
        }
        if (reVec.y > 50)
        {
            reVec.y = 50;
        }
        if (reVec.x < -50)
        {
            reVec.x = -50;
        }
        if (reVec.y < -50)
        {
            reVec.y = -50;
        }

        return reVec;
    }
 
}

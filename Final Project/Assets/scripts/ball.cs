using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    Vector3 target;
    Vector3 prevTarget;
    public Rigidbody rb;
    public SphereCollider rc;
    Vector3[] lastFive;
    int fiveC = 0;
    public float SZP = 0; //use this variable to lock character on z axis
    bool holdAllowed = true;
    bool released = false;
    bool holding = false;
    public LayerMask collisionMask;
    void Start()
    {
        target = transform.position;
        prevTarget = transform.position;
        rb = GetComponent<Rigidbody>();
        rc = GetComponent<SphereCollider>();
        lastFive = new Vector3[5];
    }

    void Update()
    {
        //bounce();  //calculates bounce
        holdThrowHandler(); //handles logic behind throwing ball
    }

    //calculates bounce, might end up getting rid of this
    void bounce()
    {
        Ray ray = new Ray(transform.position, rb.velocity);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Time.deltaTime * (rb.velocity.magnitude) + .08f, collisionMask))
        {
            print("asdf");
            Vector3 reflectDir = Vector3.Reflect(rb.velocity, hit.normal);
            rb.velocity = reflectDir;
        }
    }
    //handles logic involved in throwing ball
    void holdThrowHandler()
    {
        prevTarget = target;
        target = GetWorldPositionOnPlane(Input.mousePosition, SZP);
        target.z = SZP;
        if (holdAllowed == true)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                grab(target);
            }
            if (holding == true)
            {
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
    //called when clicking, only grabs if mouse is over character
    void grab(Vector3 tar)
    {
        if (rc.bounds.Contains(tar))
        {
            holding = true;
            released = false;
        }
    }

    //called when the ball should be released. (when the ball is let go by player, or moved out of hold zone)
    void release()
    {
        rb.velocity = (calThrow(lastFive)) * 25;
        float spinDir = Random.Range(-.5f, .5f);
        Vector3 v = new Vector3(0, 0, spinDir);
        rb.AddTorque(v);
        released = true;
        holding = false;
    }

    //returns if ball is release or not
    public bool getReleased()
    {
        return released;
    }

    //handles calculation of direction and speed of throw
    Vector3 calThrow(Vector3[] array)
    {
        Vector3 reVec = new Vector3(0, 0, 0);
        for (int x = 0; x < 5; x++)
        {
            reVec = reVec + array[x];
        }
        reVec.x = reVec.x / 5;
        reVec.y = reVec.y / 5;
        reVec.z = SZP;
        if (reVec.x > 10f)
        {
            reVec.x = 10f;
        }
        if (reVec.y > 10f)
        {
            reVec.y = 10f;
        }
        if (reVec.x < -10f)
        {
            reVec.x = -10f;
        }
        if (reVec.y < -10f)
        {
            reVec.y = -10f;
        }

        return reVec;
    }

    //returns mouse position relative to world space
    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

   //this is called when entering collider of trigger
    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "holdarea")
        {
            holdAllowed = true;
        }
    }
    //this is called when leaving collider of trigger
    void OnTriggerExit(Collider c)
    {
        if (c.tag == "holdarea")
        {
            holdAllowed = false;
        }
    }
}

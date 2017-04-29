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
    bool smoothed = false;
    int smoothCounter = 0;
    float time = .1f;
    public LayerMask collisionMask;
    bool inStorage;
    //currentCharacter: set to 1 for hugo, 2  for chihuahua, 3 for StBernard
    public int currentCharacter; 

    public void changeCurrentCharacter()
    {
        if(currentCharacter == 3)
        {
            currentCharacter = 0;
        }
        currentCharacter++;     
    }
    void Start()
    {
        target = transform.position;
        prevTarget = transform.position;
        rb = GetComponent<Rigidbody>();
        rc = GetComponent<SphereCollider>();
        lastFive = new Vector3[5];
        //changeCurrentCharacter();
    }

    void Update()
    {
        
        //bounce();  //calculates bounce
        if (inStorage == false)
        {
            holdThrowHandler(); //handles logic behind throwing ball
        }
    }
    public void setStorage(int x)
    {
        if (x == 0)
        {
            inStorage = false;
            holdAllowed = true;
            time = .1f;
        }
        else
        {
            inStorage = true;
            holdAllowed = true;
        }
    }
    //calculates bounce, might end up getting rid of this
    void bounce()
    {
        Ray ray = new Ray(transform.position, rb.velocity);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Time.deltaTime * (rb.velocity.magnitude) + .08f, collisionMask))
        {
            
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
                if (holdAllowed == true)
                {
                    grab(target);
                }
            }
            if (holding == true)
            {
                //float step = rb.velocity.magnitude * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target, 3);
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
        rb.velocity = (calThrow(lastFive)) * 15;
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
    public bool getHoldAllowed()
    {
        return holdAllowed;
    }
    public bool getStorage()
    {
        return inStorage;
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
           // holdAllowed = true;
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
    public void allowHold()
    {
        holdAllowed = true;
    }

    public void setSmooth()
    {
        smoothed = true;
    }
    public bool getSmooth()
    {
        return smoothed;
    }
    public int getSmoothCounter()
    {
        return smoothCounter;
    }
    public void addToSmoothCounter()
    {
        smoothCounter++;
    }
    public float lowerTime(float decrementBy)
    {
        if (time > .007)
        {
            time = time - decrementBy;
        }
        else if(time > .002)
        {
            time = time - .001f;
        }
        return time;
    }
    public float getTime()
    {
        return time;
    }
}

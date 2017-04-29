using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraBehavior : MonoBehaviour {
    Vector3 ballLocation;
    ball currentBall;
    public HUDscript HUD;
    float diffX;
    float diffY;
    float zoom = 13;
    public Camera thisCamera;
    
    Vector3 velocity = Vector3.zero;

    void Start () {
        thisCamera = GetComponent<Camera>();
       
	}
	
	void LateUpdate () {
        currentBall = HUD.getCurrentBall();
        ballLocation = currentBall.transform.position;
        bool smoothed = currentBall.getSmooth();
        Vector3 smooth;
        float time = currentBall.getTime();
        if (currentBall.getHoldAllowed() == false)
        {
            time = currentBall.lowerTime(.006f);
            if (smoothed == false)
            {
                currentBall.setSmooth();
            }
            smooth = (Vector3.SmoothDamp(transform.position, ballLocation, ref velocity, time));
            smooth.z = -10;
            transform.position = smooth;
        }
        else
        {
            transform.position = new Vector3(0, 0, -10);
        }
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            changeZoomLevel(thisCamera.fieldOfView - 1);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            changeZoomLevel(thisCamera.fieldOfView + 1);
        }
    }

  
    
    Vector3 trackBall(ball b)
    {
        Vector3 pos = b.transform.position;
        return pos;
    }

    public void changeZoomLevel(float desiredFieldOfView)
    {
        thisCamera.fieldOfView = desiredFieldOfView;   
    }
}

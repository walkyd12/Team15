using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraBehavior : MonoBehaviour {
    Vector3 ballLocation;
    public ball currentBall;
    float diffX;
    float diffY;
    float zoom = 13;
    bool ballIsReleased;
    public Camera thisCamera;
    Vector3 velocity = Vector3.zero;

    // Use this for initialization
    void Start () {
        thisCamera = GetComponent<Camera>();

	}
	
	// Update is called once per frame
	void Update () {
        //changeZoomLevel(zoom);
        ballLocation = trackBall(currentBall);
        if (ballIsReleased == true)
        {
            
            diffX = transform.position.x - ballLocation.x;
            diffY = transform.position.y - ballLocation.y;
            float currentX = transform.position.x;
            float currentY = transform.position.y;
            Vector3 posChange = transform.position;
            
            Vector3 smooth = (Vector3.SmoothDamp(transform.position, ballLocation, ref velocity, .1f));
            smooth.z = -10;
            transform.position = smooth;
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
        ballIsReleased = b.getReleased();
        return pos;
    }

    public void changeZoomLevel(float desiredFieldOfView)
    {

        thisCamera.fieldOfView = desiredFieldOfView;
        
    }
}

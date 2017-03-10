using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraBehavior : MonoBehaviour {
    Vector3 ballLocation;
    public ball currentBall;
    float diffX;
    float diffY;
    float zoom = -10;
    bool ballIsReleased;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //UpdateZoom();
        ballLocation = trackBall(currentBall);
        print(ballIsReleased);
        if (ballIsReleased == true)
        {
            diffX = transform.position.x - ballLocation.x;
            diffY = transform.position.y - ballLocation.y;
            float currentX = transform.position.x;
            float currentY = transform.position.y;
            Vector3 posChange = transform.position;
            Vector3 velocity = Vector3.zero;
            Vector3 smooth = (Vector3.SmoothDamp(transform.position, ballLocation, ref velocity, .1f));
            smooth.z = zoom;
            transform.position = smooth;
        }

    }
    
    Vector3 trackBall(ball b)
    {
        Vector3 pos = b.transform.position;
        ballIsReleased = b.getReleased();
            return pos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraBehavior : MonoBehaviour {
    Vector3 ballLocation;
    public GameObject currentBall;
    float diffX;
    float diffY;
    float zoom = -10;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //UpdateZoom();
        ballLocation = trackBall(currentBall);
        diffX = transform.position.x - ballLocation.x;
        diffY = transform.position.y - ballLocation.y;
        float currentX = transform.position.x;
        float currentY = transform.position.y;
        Vector3 posChange = transform.position;
        Vector3 velocity = Vector3.zero;
        Vector3 smooth = (Vector3.SmoothDamp(transform.position, ballLocation, ref velocity, .5f));
        smooth.z = zoom;
        transform.position = smooth;

    }
    
    Vector3 trackBall(GameObject ball)
    {
        Vector3 pos = ball.transform.position;
            return pos;
    }
}

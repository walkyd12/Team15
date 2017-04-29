using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class bounce : MonoBehaviour
{

    void OnCollisionEnter(Collision c)
    {
        // force is how forcefully we will push the player away from the enemy.
        float force = 300;

        // If the object we hit is the enemy
        if (c.gameObject.tag == "ball")
        {

            // Calculate Angle Between the collision point and the player
            //Vector3 dir = c.contacts[0].point - transform.position;

            //This returns the normal line from the collsion
            //as in the line that points directly out of the block where the player hit
            Vector3 dir = c.contacts[0].normal;
            //  Normalize it  (not the same thing as a normal line)
            dir = dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player
            c.gameObject.GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Impulse);
        }
    }
}
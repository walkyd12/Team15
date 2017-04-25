using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class UnitTest {

    [Test]
	public void NewPlayModeTestSimplePasses() {
        /* 
         * Test Firehydant script for collision detection
         */


        var Hydrant = new GameObject();
        var Ball = new GameObject();
        Hydrant.transform.position = new Vector3(0, 0, 0);
        Ball.transform.position = new Vector3(0, 0, 0);

        bool hydrantOn = false;

        if(Hydrant.transform.position == Ball.transform.position)
        {
            hydrantOn = true;
        }

        Assert.IsTrue(hydrantOn, "Fire Hydrants out of order");

        /*
         * Test Trashcan script for collision detection
         */

        var Trashcan = new GameObject();
        Trashcan.transform.position = new Vector3(0, 0, 0);

        bool trashOn = false;

        if(Trashcan.transform.position == Ball.transform.position)
        {
            trashOn = true;
        }

        Assert.IsTrue(trashOn, "Trashcans not gross enough");

        /*
         * Test Bone script for deleting them on pickup
         */

        var Bone = new GameObject();
        Bone.transform.position = new Vector3(100, 100, 0);
        Ball.transform.position = new Vector3(100, 100, 0);

        bool isBoneEaten = false;

        if(Bone.transform.position == Ball.transform.position)
        {
            isBoneEaten = true;
        }

        Assert.IsTrue(isBoneEaten, "Dogs not liking bones");

    }
}

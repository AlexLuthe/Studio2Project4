using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour {

    public GameObject myCam;
    public Vector3 offset;
    public float smoothSpeed;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        FollowCamera();
    }

    public void FollowCamera()
    {
        Vector3 desiredPosition = myCam.transform.localPosition + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.localPosition, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.localPosition = smoothedPosition;

        //Quaternion desiredRotation = myCam.transform.localRotation;
        //Quaternion smoothedRotation = Quaternion.Lerp(transform.localRotation, desiredRotation, smoothSpeed * Time.deltaTime);
        //transform.localRotation = smoothedRotation;
    }
}

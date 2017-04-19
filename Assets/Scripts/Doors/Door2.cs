using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door2 : MonoBehaviour {

    public Animator anim2;
    public Animation anima2;
    public AnimationClip animClip2;
    public AudioSource audSource2;
    bool animPlayed2 = false;
    public float maxReach2 = 1.0f;

	//Update is called once per frame
	void Update () {
		if (Input.GetAxis("Interact") > 0 && !animPlayed2)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, maxReach2) && hitInfo.collider.GetComponent<Door>())
            {
                if (anim2 && animClip2)
                {
                    anim2.enabled = true;
                }
                if (anima2)
                {
                    anima2.Play();
                    audSource2.Play();
                }                    
                Debug.Log("Door Open");
            }
        }
	}
}

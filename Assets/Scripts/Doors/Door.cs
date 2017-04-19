using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public Animator anim;
    public Animation anima;
    public AnimationClip animClip;
    public AudioSource audSource;
    bool animPlayed = false;
    public float maxReach = 1.0f;

	//Update is called once per frame
	void Update () {
		if (Input.GetAxis("Interact") > 0 && !animPlayed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, maxReach) && hitInfo.collider.GetComponent<Door>())
            {
                if (anim && animClip)
                {
                    anim.enabled = true;
                }
                if (anima)
                {
                    anima.Play();
                    audSource.Play();
                }                    
                Debug.Log("Door Open");
            }
        }
	}
}

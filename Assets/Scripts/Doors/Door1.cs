using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door1 : MonoBehaviour {

    public Animator anim1;
    public Animation anima1;
    public AnimationClip animClip1;
    public AudioSource audSource1;
    bool animPlayed1 = false;
    public float maxReach = 1.0f;

	//Update is called once per frame
	void Update () {
		if (Input.GetAxis("Interact") > 0 && !animPlayed1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, maxReach) && hitInfo.collider.GetComponent<Door>())
            {
                if (anim1 && animClip1)
                {
                    anim1.enabled = true;
                }
                if (anima1)
                {
                    anima1.Play();
                    audSource1.Play();
                }                    
                Debug.Log("Door Open");
            }
        }
	}
}

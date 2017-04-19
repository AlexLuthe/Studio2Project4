using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodMorning : MonoBehaviour {

    public Animator anim;
    public Animator anim1;
    public Animation anima;
    public Animation anima1;
    public AnimationClip animClip;
    public AnimationClip animClip1;
    public AudioSource audSource;
    public AudioSource audSource1;
    bool animPlayed = false;
    public float maxReach = 1.0f;
    public Transform myTransform;

	//Update is called once per frame
	void Update () {
        myTransform = this.transform;
        anima.Play();
        audSource.Play();
        transform.position = new Vector3(21.76f, 13.401f, 50.92f);
        transform.rotation = Quaternion.Euler(21.76f, 13.401f, 50.92f);
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
                    transform.position = new Vector3(26.808f, 13.58f, 51.267f);
                    transform.rotation = Quaternion.Euler(0f, 90f, 0f);

                }
            }
        }
    }                             
}


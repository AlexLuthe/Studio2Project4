using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milk : MonoBehaviour {

    public Animator anim;
    public AnimationClip animClip;
    public bool grabbingMilk = false;
    public bool placingMilk = false;
    public Vector3 newPos;
    EventListener eventListener;

    void Start()
    {
        eventListener = FindObjectOfType<EventListener>();
    }

    // Update is called once per frame
    void Update () {
        if (grabbingMilk)
            GrabMilk();
        else if (placingMilk)
            PlaceMilk();
	}

    public void GrabMilk()
    {
        transform.parent = Camera.main.transform;
        transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x + eventListener.xRotationModifier,
            Camera.main.transform.eulerAngles.y + eventListener.yRotationModifier,
            Camera.main.transform.eulerAngles.z + eventListener.zRotationModifier);
        eventListener.heldObject = gameObject;

        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(eventListener.xPickupModifier, eventListener.yPickupModifier, eventListener.zPickupModifier), Time.deltaTime * 4);

        if (Vector3.Distance(transform.localPosition, new Vector3(eventListener.xPickupModifier, eventListener.yPickupModifier, eventListener.zPickupModifier)) < 1)
            grabbingMilk = false;
    }

    public void PlaceMilk()
    {
        transform.parent = null;

        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 4);

        if (Vector3.Distance(transform.position, newPos) < 1)
            placingMilk = false;
    }
}

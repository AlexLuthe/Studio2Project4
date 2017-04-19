using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionObject : MonoBehaviour {

    [System.Serializable]
    public struct ObjectInfo
    {
        public Animator anim;
        public AnimationClip animClip;
        public AudioSource[] audioSource;
        public float[] delay;
        public string name;
        public bool interactType; // True for pick up, false to interact 
        public bool held;
        public GameObject objToHide;
        public GameObject obj;
        public float timer;
    }

    EventListener eventListener;
    public ObjectInfo objectInfo;
    public int[] missionProg = new int[2];

    private void Start()
    {
        eventListener = GameObject.FindObjectOfType<EventListener>();
        objectInfo.obj = this.gameObject;
        objectInfo.anim = this.GetComponent<Animator>();
    }

    public void objectListener(ObjectInfo objectInfo, GameObject heldObject, Vector3 rotationModifier)
    {
        // Do stuff
        if (objectInfo.interactType && heldObject == null)
        {
            StartCoroutine(GrabObject(objectInfo, rotationModifier));
        }
        else
        {
            if (objectInfo.anim && objectInfo.animClip)
            {
                objectInfo.anim.enabled = true;
            }

            if (objectInfo.audioSource.Length > 0)
            {
                for (int index = 0; index < objectInfo.audioSource.Length; ++index)
                {
                    objectInfo.audioSource[index].PlayDelayed(objectInfo.delay[index]);
                }
            }
            objectInfo.obj.GetComponent<BoxCollider>().enabled = false;
        }
        Debug.Log(objectInfo.name);
    }

    IEnumerator GrabObject(ObjectInfo objectInfo, Vector3 rotationModifier)
    {
        if (eventListener.heldObject == null)
        {
            if (objectInfo.anim && objectInfo.animClip)
            {
                objectInfo.anim.enabled = true;
                yield return new WaitForSeconds(objectInfo.animClip.length);
            }
            else
                yield return null;

            // Grab
            objectInfo.obj.transform.parent = Camera.main.transform;
            //objectInfo.obj.transform.localPosition = new Vector3(xPickupModifier, yPickupModifier, zPickupModifier);
            objectInfo.obj.transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x + rotationModifier.x,
                Camera.main.transform.eulerAngles.y + rotationModifier.y,
                Camera.main.transform.eulerAngles.z + rotationModifier.z);
            eventListener.heldObject = objectInfo.obj;
            if (eventListener.heldObject.GetComponent<MeshRenderer>())
                eventListener.heldObject.GetComponent<MeshRenderer>().enabled = true;

            if (eventListener.heldObject.GetComponentInChildren<MeshRenderer>())
                foreach (MeshRenderer rend in eventListener.heldObject.GetComponentsInChildren<MeshRenderer>())
                    rend.enabled = true;

            if (objectInfo.objToHide)
                objectInfo.objToHide.SetActive(false);
        }
    }
}

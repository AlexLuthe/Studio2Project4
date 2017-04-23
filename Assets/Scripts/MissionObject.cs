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
        public GameObject[] objsToHide;
        public GameObject[] objsToShow; // For those fancy effects you kids seem to want
        public GameObject obj;
        public float timer;
        public int mission;
    }

    EventListener eventListener;
    public ObjectInfo objectInfo;
    public int[] missionProg = new int[2];

    public virtual void Start()
    {
        eventListener = GameObject.FindObjectOfType<EventListener>();
        objectInfo.obj = this.gameObject;
        objectInfo.anim = this.GetComponent<Animator>();
    }

    public virtual void objectListener(ObjectInfo objInfo, GameObject heldObject, Vector3 rotationModifier)
    {
        // Do stuff
        if (objInfo.interactType && heldObject == null)
        {
            StartCoroutine(GrabObject(objInfo, rotationModifier));
        }
        else
        {
            /*if (objectInfo.anim && objectInfo.animClip)
            {
                objectInfo.anim.enabled = true;
            }*/

            if (objectInfo.audioSource.Length > 0 && eventListener.missionProg[missionProg[0]] == missionProg[1] + 1 && missionProg[0] == objInfo.mission)
            {
                for (int index = 0; index < objectInfo.audioSource.Length; ++index)
                {
                    if (objectInfo.audioSource[index])
                        objectInfo.audioSource[index].PlayDelayed(objectInfo.delay[index]);
                }
            }
            objInfo.obj.GetComponent<BoxCollider>().enabled = false;
            if (GetComponent<MeshRenderer>())
            {
                GetComponent<MeshRenderer>().enabled = true;
                Color colour = GetComponent<MeshRenderer>().material.color;
                GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(colour.r, colour.g, colour.b, 1));
            }

            if (GetComponentInChildren<MeshRenderer>())
                foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
                {
                    rend.enabled = true;
                    Color colour = rend.material.color;
                    rend.material.SetColor("_Color", new Color(colour.r, colour.g, colour.b, 1));
                }
            if (objInfo.objsToHide.Length > 0)
                foreach (GameObject objToHide in objInfo.objsToHide)
                {
                    if (objToHide)
                        objToHide.SetActive(false);
                }
            if (objInfo.objsToShow.Length > 0)
                foreach (GameObject objToShow in objInfo.objsToShow)
                {
                    if (objToShow)
                        objToShow.SetActive(true);
                }
        }
    }

    IEnumerator GrabObject(ObjectInfo objInfo, Vector3 rotationModifier)
    {
        if (eventListener.heldObject == null)
        {
            /*if (objectInfo.anim && objectInfo.animClip)
            {
                objectInfo.anim.enabled = true;
                yield return new WaitForSeconds(objectInfo.animClip.length);
            }*/
            //else
            yield return null;

            // Grab
            objInfo.obj.transform.parent = Camera.main.transform;
            //objectInfo.obj.transform.localPosition = new Vector3(xPickupModifier, yPickupModifier, zPickupModifier);
            objInfo.obj.transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x + rotationModifier.x,
                Camera.main.transform.eulerAngles.y + rotationModifier.y,
                Camera.main.transform.eulerAngles.z + rotationModifier.z);
            eventListener.heldObject = objInfo.obj;
            if (eventListener.heldObject.GetComponent<MeshRenderer>())
                eventListener.heldObject.GetComponent<MeshRenderer>().enabled = true;

            if (eventListener.heldObject.GetComponentInChildren<MeshRenderer>())
                foreach (MeshRenderer rend in eventListener.heldObject.GetComponentsInChildren<MeshRenderer>())
                    rend.enabled = true;
            if (objInfo.objsToHide.Length > 0)
                foreach (GameObject objToHide in objInfo.objsToHide)
                {
                    if (objToHide)
                        objToHide.SetActive(false);
                }
            if (objInfo.objsToShow.Length > 0)
                foreach (GameObject objToShow in objInfo.objsToShow)
                {
                    if (objToShow)
                        objToShow.SetActive(true);
                }
        }
    }
}

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

    public void objectListener(ObjectInfo objInfo, GameObject heldObject, Vector3 rotationModifier)
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

            if (objectInfo.audioSource.Length > 0)
            {
                for (int index = 0; index < objectInfo.audioSource.Length; ++index)
                {
                    objectInfo.audioSource[index].PlayDelayed(objInfo.delay[index]);
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
            if (objInfo.objToHide)
                objInfo.objToHide.SetActive(false);
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
            if (objInfo.objToHide)
                objInfo.objToHide.SetActive(false);

        }
    }
}

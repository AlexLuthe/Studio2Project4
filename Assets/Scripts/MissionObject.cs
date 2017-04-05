using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionObject : MonoBehaviour {

    [System.Serializable]
    public struct ObjectInfo
    {
        public AnimationClip animClip;
        public AudioClip audioClip;
        public string name;
        public bool interactType; // True for pick up, false to interact 
        public bool held;
        public GameObject obj;
        public Vector3 heldPos;
    }

    public ObjectInfo objectInfo;
    public int[] missionProg = new int[2];

    private void Start()
    {
        objectInfo.obj = this.gameObject;
    }

    private void Update()
    {
        if (objectInfo.held)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, objectInfo.heldPos, Time.deltaTime);
        }
    }

}

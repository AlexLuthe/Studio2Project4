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
        public GameObject obj;
    }

    public ObjectInfo objectInfo;
    public int[] missionProg = new int[2];

    private void Start()
    {
        objectInfo.obj = this.gameObject;
    }

}

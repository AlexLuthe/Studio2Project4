using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionObject : MonoBehaviour {

    [System.Serializable]
    public struct ObjectInfo
    {
        public AnimationClip animClip;
        public AudioClip audioClip;
    }

    public ObjectInfo objectInfo;
    public int[] missionProg = new int[2];

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderOutline : MonoBehaviour {

    public EventListener eventListener;
    public MissionObject[] missionObjects;

	// Use this for initialization
	void Start ()
    {
        missionObjects = FindObjectsOfType<MissionObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {		
        foreach (MissionObject mobj in missionObjects)
        {
            for (int mObjIndex = 0; mObjIndex < eventListener.missionProg.Length; ++mObjIndex)
            {
                if (mobj.missionProg[1] == eventListener.missionProg[mObjIndex])
                {
                    mobj.gameObject.layer = 8;
                    foreach (Transform child in mobj.gameObject.GetComponentInChildren<Transform>())
                    {
                        child.gameObject.layer = 8;
                    }
                }
                else
                {
                    mobj.gameObject.layer = 0;
                    foreach (Transform child in mobj.gameObject.GetComponentInChildren<Transform>())
                    {
                        child.gameObject.layer = 0;
                    }
                }
            }
        }
	}
}

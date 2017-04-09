using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Analytics;

[System.Serializable]
public class Event : UnityEvent<MissionObject.ObjectInfo>
{
    
}

public class EventListener : MonoBehaviour {

    public int[] missionProg;
	public float[] missionTimers = {0,0,0,0,0};
    public Event m_event;
    [Range(-3,3)]
    public float xPickupModifier, yPickupModifier, zPickupModifier, xRotationModifier, yRotationModifier, zRotationModifier;
    public GameObject heldObject;

    bool startedGame = false;

	// Use this for initialization
	void Start () {
        if (m_event == null)
            m_event = new Event();

        m_event.AddListener(objectListener);
	}
	
	// Update is called once per frame
	void Update () {

        for (int index = 0; index < missionTimers.Length; ++index)
        {
            if (missionTimers[index] > 0)
                missionTimers[index] -= Time.deltaTime;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Input.GetKeyDown(KeyCode.E) && m_event != null)
        {
            Physics.Raycast(ray, out hitInfo);
            if (hitInfo.collider.gameObject.GetComponent<MissionObject>())
            {
                MissionObject hitObject = hitInfo.collider.gameObject.GetComponent<MissionObject>();

                int missionIndex = hitObject.missionProg[0];
                int missionProgress = hitObject.missionProg[1];

                for (int mIndex = 0; mIndex < missionProg.Length; ++mIndex)
                {
                    if (missionIndex == mIndex && missionProgress == missionProg[missionIndex])
                    {
						if (missionTimers [missionIndex] <= 0) {
							if (heldObject) {
								Destroy (heldObject);
								heldObject = null;
                                if (hitObject.GetComponent<MeshRenderer>())
                                    hitObject.GetComponent<MeshRenderer>().enabled = true;

                                if (hitObject.GetComponentInChildren<MeshRenderer>())
                                    foreach (MeshRenderer rend in hitObject.GetComponentsInChildren<MeshRenderer>())
                                        rend.enabled = true;
                            }
							++missionProg [missionIndex];
							missionTimers [missionIndex] = hitObject.objectInfo.timer;
							m_event.Invoke (hitObject.objectInfo);

                            if (!startedGame)
                            {
                                startedGame = true;
                                Analytics.CustomEvent("First Object", new Dictionary<string, object>
                                {
                                    {"Cereal", missionProg[0]},
                                    {"Coffee", missionProg[1]},
                                    {"Shower", missionProg[2]},
                                    {"Toilet", missionProg[3]},
                                    {"Music", missionProg[4]}
                                });
                            }
						}
                    }
                }
            }
        }
        if (heldObject)
        {
            heldObject.transform.localPosition = Vector3.Lerp(heldObject.transform.localPosition, new Vector3(xPickupModifier, yPickupModifier, zPickupModifier), Time.deltaTime * 4);
        }
    }

    void objectListener(MissionObject.ObjectInfo objectInfo)
    {
        // Do stuff
        if (objectInfo.interactType)
        {
            // Grab
            objectInfo.obj.transform.parent = Camera.main.transform;
            //objectInfo.obj.transform.localPosition = new Vector3(xPickupModifier, yPickupModifier, zPickupModifier);
            objectInfo.obj.transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x + xRotationModifier, 
                Camera.main.transform.eulerAngles.y + yRotationModifier, 
                Camera.main.transform.eulerAngles.z + zRotationModifier);
            heldObject = objectInfo.obj;
        }
        else
        {
            objectInfo.anim.enabled = true;
        }
        Debug.Log(objectInfo.name);
    }
}

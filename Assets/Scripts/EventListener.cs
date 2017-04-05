using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Event : UnityEvent<MissionObject.ObjectInfo>
{
    
}

public class EventListener : MonoBehaviour {

    public int[] missionProg;
    public float[] missionTimers;
    public Event m_event;
    [Range(-3,3)]
    public float xPickupModifier, yPickupModifier, zPickupModifier, xRotationModifier, yRotationModifier, zRotationModifier;
    public GameObject heldObject;

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
                if (!hitObject.GetComponent<MeshRenderer>().enabled)
                    hitObject.GetComponent<MeshRenderer>().enabled = true;
                int missionIndex = hitObject.missionProg[0];
                int missionProgress = hitObject.missionProg[1];

                for (int mIndex = 0; mIndex < missionProg.Length; ++mIndex)
                {
                    if (missionIndex == mIndex && missionProgress == missionProg[missionIndex] && missionTimers[missionIndex] <= 0)
                    {
                        if (heldObject)
                        {
                            Destroy(heldObject);
                            heldObject = null;
                        }
                        ++missionProg[missionIndex];
                        missionTimers[missionIndex] = hitObject.objectInfo.timer;
                        m_event.Invoke(hitObject.objectInfo);

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
        Debug.Log(objectInfo.name);
    }
}

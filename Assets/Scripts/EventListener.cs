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
    public Event m_event;

	// Use this for initialization
	void Start () {
        if (m_event == null)
            m_event = new Event();

        m_event.AddListener(Ping);
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Input.GetKeyDown(KeyCode.E) && m_event != null)
        {
        Physics.Raycast(ray, out hitInfo);
            if (hitInfo.collider.gameObject.GetComponent<MissionObject>())
            {
                MissionObject hitObject = hitInfo.collider.gameObject.GetComponent<MissionObject>();
                int xBanana = hitObject.missionProg[0];
                int yBanana = hitObject.missionProg[1];

                for (int missionIndex = 0; missionIndex < missionProg.Length; ++missionIndex)
                {
                    if (xBanana == missionIndex && yBanana == missionProg[missionIndex])
                        m_event.Invoke(hitObject.objectInfo);
                }
            }
        }
    }

    void Ping(MissionObject.ObjectInfo objectInfo)
    {
        // Do stuff
        Debug.Log("Doing Stuff");
    }
}

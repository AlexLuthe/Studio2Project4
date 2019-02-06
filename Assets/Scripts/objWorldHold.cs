using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objWorldHold : MonoBehaviour {

    RaycastShooting raycastShooting;

	// Use this for initialization
	void Start ()
    {
        raycastShooting = GameObject.FindGameObjectWithTag("Player").GetComponent<RaycastShooting>();
	}

    public void DestroyIfNoChild()
    {
        if (raycastShooting.fromPoint.transform.parent != this.gameObject)
        {
            Destroy(this.gameObject);
        }
    }
}

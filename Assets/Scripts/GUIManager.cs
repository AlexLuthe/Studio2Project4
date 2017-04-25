using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUIManager : MonoBehaviour {

    public EventSystem es;
    private GameObject StoreSelected;

	// Use this for initialization
	void Start () {
        StoreSelected = es.firstSelectedGameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (es.currentSelectedGameObject != StoreSelected)
        {
            if (es.currentSelectedGameObject == null)
                es.SetSelectedGameObject(StoreSelected);

            else
                StoreSelected = es.currentSelectedGameObject;
        }
		
	}
}

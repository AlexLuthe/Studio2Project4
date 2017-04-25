using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetUp : MissionObject {

    public GameObject playerGO;
    public CharacterController characterController;
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpsController;

    public override void Start()
    {
        base.Start();
        playerGO = GameObject.Find("FPSController");
        characterController = GameObject.FindObjectOfType<CharacterController>();
        fpsController = GameObject.FindObjectOfType<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
    }

    public override void objectListener(ObjectInfo objInfo, GameObject heldObject, Vector3 rotationModifier)
    {
        base.objectListener(objInfo, heldObject, rotationModifier);

        playerGO.transform.eulerAngles = new Vector3(0, 0, 0);

        characterController.enabled = true;
        fpsController.enabled = true;
        playerGO.GetComponent<Animation>().enabled = false;
    }
}
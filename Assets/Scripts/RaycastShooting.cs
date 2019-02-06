using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RaycastShooting : MonoBehaviour {

    public float shootingDistance;
    public Camera playerCam;
    public Animator playerAnim;
    public Animator crosshairAnim;
    public Animator endGameFade;
    public GameObject crosshair;
    public GameObject activateableObjs;
    public bool lookingAt;
    public bool activateCross;
    public GameObject blinkEyes;
    public float blinkRate;

    public GameObject holdingArm;
    public GameObject objWorldHolder;
    public GameObject fromPoint;
    public float lerpSpeed = 1.5f;
    public bool itemPickedUp;

    [Header("Wake Up Mission")]
    public LayerMask alarmClock;
    public GameObject alarmClockObj;
    public bool wokenUp;

    [Header("Toilet Mission")]
    public bool hasPeed = false;
    public float peeTimer;
    public float peeReady;
    public ToiletListener toiletListener;
    public ShowerListener showerListener;
    public LayerMask toiletBowl;
    public LayerMask toiletFlush;
    public LayerMask toiletWipe;
    public LayerMask toiletWash;
    public LayerMask toiletSoap;

    [Header("Kitchen Missions")]
    //Neutral Pickups
    public bool milkPickedUp;
    //Cereal Mssion
    public bool bowlPickedUp;        
    public bool spoonPickedUp;
    public bool cerealPickedUp;
    public bool milkPoured;
    public bool cerealPoured;
    public bool spoonPutIn;
    public bool cerealMade;
    //Coffee Mission
    public bool mugPickedUp;
    public bool coffeeMade;

    [Header("Keys Mission")]
    public Text cleanOrDirty;
    public Text nakedOrClothed;
    public Text bustingOrRelieved;
    public Text hungryOrSatisfied;
    public Text lethargicOrInvigorated;
    public bool showerOn;
    public bool clean;
    public bool clothed;
    public bool relieved;
    public bool satisfied;
    public bool invigorated;
    public LayerMask keys;
    public LayerMask endGameDoor;
    public bool keysCollected;

    // Use this for initialization
    void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        toiletListener = GameObject.Find("Go To Toilet").GetComponent<ToiletListener>();
        showerListener = GameObject.Find("Shower").GetComponent<ShowerListener>();
        wokenUp = false;
    }

    private void FixedUpdate()
    {
        RaycastCheck();
        RaycastingAlways();
        EyesBlinking();
        activateableObjs = GameObject.FindGameObjectWithTag("Activateable");
        endGameStats();
    }

    public void RaycastCheck()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Raycasting();
        }
    }

    public void Raycasting()
    {
        //Debug.Log("Trying to Hit");
        float x = Screen.width / 2;
        float y = Screen.height / 2;

        Ray ray = playerCam.ScreenPointToRay(new Vector2(x, y));
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shootingDistance))
        {
            if (hit.collider.gameObject.name == "Alarm Clock")
            {
                playerAnim.Play("Get Out Of Bed");
                GameObject.FindGameObjectWithTag("AlarmSound").GetComponent<AudioSource>().enabled = false;
                GameObject.FindGameObjectWithTag("AlarmText").GetComponent<Animator>().enabled = false;
                GameObject.FindGameObjectWithTag("AlarmText").GetComponent<Text>().enabled = true;
                GameObject.Find("Alarm Clock").tag = "Untagged";
                StartCoroutine(WaitForWakeUp(4.5f));
            }

            if (hit.collider.gameObject.name == "Go To Toilet" && !hasPeed && toiletListener.toiletLidOpen)
            {
                toiletListener.toiletPee.SetActive(true);
                GameObject.Find("Go To Toilet").GetComponent<AudioSource>().Play();
                hasPeed = true;
                peeTimer = 0.0f;
                toiletListener.gameObject.tag = "Untagged";
                relieved = true;
            }

            if (hit.collider.gameObject.name == "Flush")
            {
                toiletListener.toiletPee.SetActive(false);
                toiletListener.flushWater.Play();
                toiletListener.flushSound.Play();
            }

            if (hit.collider.gameObject.name == "Wipe")
            {
                toiletListener.toiletWipe.GetComponent<AudioSource>().Play();
            }

            if (hit.collider.gameObject.name == "Apply Soap")
            {
                toiletListener.soapSound.Play();
                toiletListener.handSoap.Play();
            }

            if (hit.collider.gameObject.name == "Wash Hands")
            {
                toiletListener.toiletWash.GetComponent<AudioSource>().Play();
                toiletListener.ActiveWater(toiletListener.toiletWash.GetComponent<AudioSource>().clip.length);
            }

            if (hit.collider.gameObject.name == "Shower On")
            {
                showerOn = !showerOn;
                showerListener.showerWaterOn = !showerListener.showerWaterOn;
                showerListener.showerWater.SetActive(showerListener.showerWaterOn ? true : false);                
            }

            if (hit.collider.gameObject.name == "Wash Self")
            {
                if(showerOn)
                    clean = true;
                showerListener.washSelf.GetComponent<AudioSource>().Play();
            }

            if (hit.collider.gameObject.name == "Dry Self")
            {
                showerListener.drySelf.GetComponent<AudioSource>().Play();
            }

            if (hit.collider.gameObject.name == "Put On Clothes")
            {
                showerListener.putOnClothesSound.Play();
                showerListener.putOnClothes.SetActive(false);
                showerListener.putOnShoes.SetActive(false);
                clothed = true;
            }

            if (hit.collider.gameObject.name == "Bowl Pickup" && !itemPickedUp)
            {
                if(cerealMade && spoonPutIn)
                {
                    GameObject bowlPickUp = GameObject.Find("Bowl Pickup");
                    fromPoint = bowlPickUp;
                    bowlPickUp.GetComponent<BoxCollider>().enabled = false;
                    StartCoroutine(PickUpObject());
                    StartCoroutine(FillHands(0.5f));
                    bowlPickUp.GetComponent<BowlListener>().milkLiquid.SetActive(false);
                    bowlPickUp.GetComponent<BowlListener>().cerealFlakes.SetActive(false);
                    bowlPickUp.GetComponent<BowlListener>().cerealMilkFlakes.SetActive(false);
                    satisfied = true;
                }
                else if(!cerealMade)
                {
                    GameObject bowlPickUp = GameObject.Find("Bowl Pickup");
                    fromPoint = bowlPickUp;
                    bowlPickUp.GetComponent<BoxCollider>().enabled = false;
                    StartCoroutine(PickUpObject());
                    StartCoroutine(FillHands(0.5f));
                    bowlPickedUp = true;
                }
            }

            if (spoonPickedUp && hit.collider.gameObject.name == "Bowl Pickup")
            {
                GameObject bowlPickUp = GameObject.Find("Bowl Pickup");
                GameObject spoonPickUp = GameObject.Find("BigSpoon");
                bowlPickUp.GetComponent<BowlListener>().spoon.SetActive(true);
                spoonPutIn = true;
                spoonPickedUp = false;
                itemPickedUp = false;
                Destroy(spoonPickUp.gameObject);
            }            

            if (hit.collider.gameObject.name == "Milk Pickup" && !itemPickedUp)
            {                
                Debug.Log("Milk Picked Up");
                GameObject milkPickUp = GameObject.Find("Milk Pickup");
                fromPoint = milkPickUp;
                milkPickUp.GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(PickUpObject());
                StartCoroutine(FillHands(0.5f));
                milkPickedUp = true;
            }

            if (hit.collider.gameObject.name == "Cereal Box" && !itemPickedUp)
            {
                GameObject cerealPickUp = GameObject.Find("Cereal Box");
                fromPoint = cerealPickUp;
                cerealPickUp.GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(PickUpObject());
                StartCoroutine(FillHands(0.5f));
                cerealPickedUp = true;
            }

            if (hit.collider.gameObject.name == "BigSpoon" && !itemPickedUp)
            {
                GameObject spoonPickUp = GameObject.Find("BigSpoon");
                fromPoint = spoonPickUp;
                spoonPickUp.GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(PickUpObject());
                StartCoroutine(FillHands(0.5f));
                spoonPickedUp = true;
            }

            if (milkPickedUp && hit.collider.gameObject.name == "Bowl Pickup")
            {
                if (cerealPoured)
                {
                    GameObject bowlPickUp = GameObject.Find("Bowl Pickup");
                    bowlPickUp.GetComponent<BowlListener>().milkLiquid.SetActive(true);
                    bowlPickUp.GetComponent<BowlListener>().cerealFlakes.SetActive(false);
                    bowlPickUp.GetComponent<BowlListener>().cerealMilkFlakes.SetActive(true);
                    cerealMade = true;
                }

                else if (!cerealPoured)
                {
                    GameObject bowlPickUp = GameObject.Find("Bowl Pickup");
                    bowlPickUp.GetComponent<BowlListener>().milkLiquid.SetActive(true);
                    milkPoured = true;
                }
            }

            if (cerealPickedUp && hit.collider.gameObject.name == "Bowl Pickup")
            {
                if (!milkPoured)
                {
                    GameObject bowlPickUp = GameObject.Find("Bowl Pickup");
                    bowlPickUp.GetComponent<BowlListener>().cerealFlakes.SetActive(true);
                    cerealPoured = true;
                }

                else if (milkPoured)
                {
                    GameObject bowlPickUp = GameObject.Find("Bowl Pickup");
                    bowlPickUp.GetComponent<BowlListener>().cerealMilkFlakes.SetActive(true);
                    cerealMade = true;
                }
            }

            if (hit.collider.gameObject.name == "Mug Pickup" && !itemPickedUp)
            {
                if (coffeeMade)
                {
                    GameObject mugPickUp = GameObject.Find("Mug Pickup");
                    fromPoint = mugPickUp;
                    mugPickUp.GetComponent<BoxCollider>().enabled = false;
                    StartCoroutine(PickUpObject());
                    StartCoroutine(FillHands(0.5f));
                    mugPickUp.GetComponent<MugListener>().coffeeWithMilk.SetActive(false);                    
                    mugPickedUp = true;
                    invigorated = true;
                }
                else if (!coffeeMade)
                {
                    GameObject mugPickUp = GameObject.Find("Mug Pickup");
                    fromPoint = mugPickUp;
                    mugPickUp.GetComponent<BoxCollider>().enabled = false;
                    StartCoroutine(PickUpObject());
                    StartCoroutine(FillHands(0.5f));
                    mugPickedUp = true;
                    GameObject.Find("Coffee Machine").tag = "Activateable";
                }
            }

            if (mugPickedUp && hit.collider.gameObject.name == "Coffee Machine")
            {
                GameObject mugPickUp = GameObject.Find("Mug Pickup");
                StartCoroutine(CupToCoffeeMachine());
                StartCoroutine(CoffeeMachineActivate(5.0f));
            }

            if (milkPickedUp && hit.collider.gameObject.name == "Mug Pickup")
            {
                GameObject mugPickUp = GameObject.Find("Mug Pickup");
                mugPickUp.GetComponent<MugListener>().coffeeNoMilk.SetActive(false);
                mugPickUp.GetComponent<MugListener>().coffeeWithMilk.SetActive(true);
                coffeeMade = true;
            }


            if (hit.collider.gameObject.name == "EndGameKeys" && !itemPickedUp)
            {
                GameObject endGameKeys = GameObject.Find("EndGameKeys");
                fromPoint = endGameKeys;
                endGameKeys.GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(PickUpObject());
                StartCoroutine(FillHands(0.5f));
                keysCollected = true;
                GameObject.Find("EndGameDoor").tag = "Activateable";
            }

            if (hit.collider.gameObject.name == "EndGameDoor" && keysCollected)
            {
                crosshair.SetActive(false);
                endGameFade.Play("FadeToWhite");
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().enabled = false;
                GameObject.FindGameObjectWithTag("Player").GetComponent<RaycastShooting>().enabled = false;
                Destroy(GameObject.Find("EndGameKeys"));
                Debug.Log("FadingOut");
                StartCoroutine(LoadSceneAfterDelay("Menu", 35.0f));
            }
        }
    }

    public void RaycastingAlways()
    {
        //Debug.Log("Trying to Hit");
        float x = Screen.width / 2;
        float y = Screen.height / 2;

        Ray ray = playerCam.ScreenPointToRay(new Vector2(x, y));
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shootingDistance) && hit.transform.gameObject.tag == "Activateable")
        {
            StartCoroutine(WaitForCrosshair(0.2f));
        }
        else if (activateCross)
        {
            StartCoroutine(WaitForCrosshairDeact(0.2f));
        }

        if (Physics.Raycast(ray, out hit, shootingDistance) && hit.transform.gameObject.tag != "Activateable" && itemPickedUp && Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Dropping");
            StartCoroutine(PutDownObject());
        }
    }

    //DO NOT PUT WAITFORSECONDS HERE
    public IEnumerator PickUpObject()
    {
        while (fromPoint.transform.localPosition != holdingArm.transform.localPosition)
        {
            fromPoint.gameObject.tag = "Untagged";
            fromPoint.transform.parent = holdingArm.transform;
            fromPoint.transform.localPosition = Vector3.Lerp(fromPoint.transform.localPosition, holdingArm.transform.localPosition, lerpSpeed * Time.deltaTime);
            fromPoint.transform.localRotation = Quaternion.Lerp(fromPoint.transform.localRotation, holdingArm.transform.localRotation, lerpSpeed * Time.deltaTime);
            fromPoint.GetComponent<Rigidbody>().useGravity = false;
            fromPoint.GetComponent<Rigidbody>().isKinematic = true;
            yield return null;          
        }
    }
    public IEnumerator PutDownObject()
    {
        fromPoint.gameObject.tag = "Activateable";
        fromPoint.GetComponent<BoxCollider>().enabled = true;
        fromPoint.GetComponent<Rigidbody>().useGravity = true;
        fromPoint.GetComponent<Rigidbody>().isKinematic = false;
        fromPoint.transform.parent = null;
        bowlPickedUp = false;
        mugPickedUp = false;
        milkPickedUp = false;
        spoonPickedUp = false;
        cerealPickedUp = false;
        itemPickedUp = false;

        yield return null;
    }

    IEnumerator FillHands(float time)
    {
        yield return new WaitForSeconds(time);
        itemPickedUp = true;
    }

    IEnumerator WaitForCrosshair(float time)
    {
        crosshairAnim.Play("Activateable");
        yield return new WaitForSeconds(time);
        activateCross = true;
    }
    IEnumerator WaitForCrosshairDeact(float time)
    {
        crosshairAnim.Play("Deactivateable");
        yield return new WaitForSeconds(time);
        activateCross = false;
    }

    IEnumerator WaitForWakeUp(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<Animator>().enabled = false;
        wokenUp = true;
    }

    public IEnumerator LoadSceneAfterDelay(string sceneName, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator CoffeeMachineActivate(float time)
    {
        GameObject mugPickUp = GameObject.Find("Mug Pickup");
        GameObject coffeeMachine = GameObject.Find("Coffee Machine");
        coffeeMachine.tag = "Untagged";
        coffeeMachine.GetComponent<BoxCollider>().enabled = false;
        mugPickUp.GetComponent<MugListener>().coffeeLiquid1.SetActive(true);
        mugPickUp.GetComponent<MugListener>().coffeeLiquid2.SetActive(true);
        mugPickUp.GetComponent<MugListener>().coffeeGrindingSound.Play();
        yield return new WaitForSeconds(time);
        fromPoint.GetComponent<BoxCollider>().enabled = true;
        fromPoint.gameObject.tag = "Activateable";
        mugPickUp.GetComponent<MugListener>().coffeeGrindingSound.Stop();
        mugPickUp.GetComponent<MugListener>().coffeeLiquid1.SetActive(false);
        mugPickUp.GetComponent<MugListener>().coffeeLiquid2.SetActive(false);
        mugPickUp.GetComponent<MugListener>().coffeeNoMilk.SetActive(true);        
    }

    IEnumerator CupToCoffeeMachine()
    {
        GameObject coffeeMachine = GameObject.Find("CupHolder");
        fromPoint.gameObject.tag = "Untagged";
        coffeeMachine.tag = "Untagged";
        itemPickedUp = false;
        mugPickedUp = false;
        while (fromPoint.transform.localPosition != coffeeMachine.transform.localPosition)
        { 
            fromPoint.transform.parent = coffeeMachine.transform;
            fromPoint.transform.localPosition = Vector3.Lerp(fromPoint.transform.localPosition, coffeeMachine.transform.localPosition, lerpSpeed * Time.deltaTime);
            fromPoint.transform.localRotation = Quaternion.Lerp(fromPoint.transform.localRotation, coffeeMachine.transform.localRotation, lerpSpeed * Time.deltaTime);
            fromPoint.GetComponent<Rigidbody>().useGravity = false;
            fromPoint.GetComponent<Rigidbody>().isKinematic = true;
            yield return null;
        }
    }

    public void EyesBlinking()
    {
        float randomNum = Random.Range(0.0f, 100.0f);
        if (randomNum <= blinkRate && wokenUp)
        {
            Debug.Log("Below 0.10f");
            blinkEyes.GetComponent<Animator>().Play("Blink");
        }
    }

    public void endGameStats()
    {
        cleanOrDirty.text = (clean ? "Clean," : "Dirty,");
        nakedOrClothed.text = (clothed ? "clothed," : "naked,");
        bustingOrRelieved.text = (relieved ? "relieved," : "busting,");
        hungryOrSatisfied.text = (satisfied ? "satisfied," : "hungry,");
        lethargicOrInvigorated.text = (invigorated ? "invigorated." : "lethargic.");
    }
}
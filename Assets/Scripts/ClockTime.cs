using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockTime : MonoBehaviour {

    public Text clockTime;
    public float secondsValue;
    public float minuteValue;
    public float hourValue;
    public bool AMorPM = true;

	// Use this for initialization
	void Start ()
    {
        hourValue = 7;
        minuteValue = (int)00;
	}
	
	// Update is called once per frame
	void Update ()
    {
        clockTime.text = hourValue + ":" + minuteValue.ToString("00") + (AMorPM ? "AM" : "PM");	
	}

    void FixedUpdate()
    {
        Minutes();
        Hours();
        DayHalfSwap();
    }

    void Minutes()
    {
        secondsValue += (Time.deltaTime);
        if (secondsValue >= 60)
        {
            minuteValue++;
            secondsValue = 0;
        }
    }

    void Hours()
    {
        if(minuteValue >= 60)
        {
            hourValue++;
            minuteValue = 0;
        }
    }

    void DayHalfSwap()
    {
        if(hourValue >= 13)
        {            
            AMorPM = !AMorPM;
            hourValue = 1;
        }
    }
}

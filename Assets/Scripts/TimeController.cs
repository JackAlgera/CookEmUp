using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour {

    public static TimeController instance;

    public float initXScale;
    public float currentXScale;

    public GameObject bar;

    public float currentTime;
    public float maxTime;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        initXScale = bar.transform.localScale.x;
        currentXScale = initXScale;

        currentTime = maxTime;
    }
    
    void Update () {
        if(currentTime >= maxTime)
        {
            currentTime = maxTime;
        }

        if(currentTime < 0)
        {
            currentTime = 0;
        }

        currentTime -= Time.deltaTime;

        UpdateBar();
	}

    public void UpdateBar()
    {
        Vector3 temp = bar.transform.localScale;
        temp.x = (currentTime / maxTime) * initXScale;

        bar.transform.localScale = temp;
    }

    public void FinishOrder()
    {
        currentTime += maxTime * 0.1f; // Increase time by 10% of max time
    }

    public void WrongOrder()
    {
        currentTime -= maxTime * 0.1f;
    }
}

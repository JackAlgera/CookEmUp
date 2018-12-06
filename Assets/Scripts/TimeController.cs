using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDifficulty { Beginner, Easy, Normal, Fast, Hard, Insane}

public class TimeController : MonoBehaviour {

    public static TimeController instance;

    public float initXScale;
    public float currentXScale;

    public GameObject bar;

    public float currentTime;
    public float maxTime;

    public float[] difficultyTimeChanges;
    public float[] difficultyValues; // Increases time by X% of maxtime when finishing an order -> Better : time goes down faster

    public float totTime;
    public EDifficulty currentDifficulty;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        totTime = 0f;
        currentDifficulty = EDifficulty.Beginner;

        initXScale = bar.transform.localScale.x;
        currentXScale = initXScale;

        currentTime = maxTime;
    }
    
    void Update () {
        totTime += Time.deltaTime;
        CheckDifficultyTime();

        if(currentTime >= maxTime)
        {
            currentTime = maxTime;
        }

        if(currentTime < 0)
        {
            currentTime = 0;
        }

        currentTime -= Time.deltaTime * difficultyValues[(int)currentDifficulty];

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

    public void CheckDifficultyTime()
    {
        if(((int)currentDifficulty < sizeof(EDifficulty)) && totTime > difficultyTimeChanges[(int) currentDifficulty])
        {
            currentDifficulty = (EDifficulty)((int)currentDifficulty + 1);
        }
    }
}

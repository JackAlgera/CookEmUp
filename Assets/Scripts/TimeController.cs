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

    public float difficultyTimeChanges;
    public float difficultyValues; // Time goes down X times faster

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

    private void Start()
    {
        UpdateGameDifficulty();
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

        currentTime -= Time.deltaTime * difficultyValues;

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
        if(((int)currentDifficulty < sizeof(EDifficulty)) && totTime > difficultyTimeChanges)
        {
            currentDifficulty = (EDifficulty)((int)currentDifficulty + 1);
            UpdateGameDifficulty();
        }
    }

    public void UpdateGameDifficulty()
    {
        int extraIngredientsToSpawn = 0;
        float timeBTWOrders = 0f;
        int numberOfIngredientsInORder = 2;
        //float descentSpeed = 2f;

        switch (currentDifficulty)
        {
            case EDifficulty.Beginner:
                extraIngredientsToSpawn = 0;
                timeBTWOrders = 2f;
                numberOfIngredientsInORder = 4;
                difficultyTimeChanges = 10f;
                difficultyValues = 1f;
                break;

            case EDifficulty.Easy:
                extraIngredientsToSpawn = 0;
                timeBTWOrders = 4f;
                numberOfIngredientsInORder = 4;
                difficultyTimeChanges = 15f;
                difficultyValues = 1.2f;
                break;

            case EDifficulty.Normal:
                extraIngredientsToSpawn = 0;
                timeBTWOrders = 3f;
                numberOfIngredientsInORder = 4;
                difficultyTimeChanges = 20f;
                difficultyValues = 1.5f;
                break;

            case EDifficulty.Fast:
                extraIngredientsToSpawn = 0;
                timeBTWOrders = 2f;
                numberOfIngredientsInORder = 4;
                difficultyTimeChanges = 25f;
                difficultyValues = 2f;
                break;

            case EDifficulty.Hard:
                extraIngredientsToSpawn = 0;
                timeBTWOrders = 1f;
                numberOfIngredientsInORder = 7;
                difficultyTimeChanges = 30f;
                difficultyValues = 2.5f;
                break;

            case EDifficulty.Insane:
                extraIngredientsToSpawn = 0;
                timeBTWOrders = 0.5f;
                numberOfIngredientsInORder = 8;
                difficultyTimeChanges = 40f;
                difficultyValues = 3f;
                break;

            default:
                break;
        }
        extraIngredientsToSpawn = 0;
        ListOfOrders.instance.extraIngredientsToSpawn = extraIngredientsToSpawn;
        GameController.instance.timeBTWOrders = timeBTWOrders;
        GameController.instance.numberOfIngredientsInORder = numberOfIngredientsInORder;
    }
}

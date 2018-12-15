using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EDifficulty { Beginner, Easy, Normal, Fast, Hard, Insane}

public class TimeController : MonoBehaviour {

    public static TimeController instance;

    public GameObject GameOverPanel;

    public float initXScale;
    public float currentXScale;

    public GameObject bar;

    public float currentTime;
    public float maxTime;

    public float difficultyTimeChanges;
    public float difficultyValues; // Time goes down X times faster

    public float totTime;
    public EDifficulty currentDifficulty;

    public Text currentLevel;

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
        switch (GameController.instance.currentGameState)
        {
            case GameStates.Playing:
                totTime += Time.deltaTime;
                CheckDifficultyTime();

                if (currentTime >= maxTime)
                {
                    currentTime = maxTime;
                }

                if (currentTime < 0)
                {
                    currentTime = 0;
                    GameOverPanel.SetActive(true);
                    GameController.instance.LoseGame();
                }

                currentTime -= Time.deltaTime * difficultyValues;
                UpdateBar();
                break;

            case GameStates.Paused:
                break;

            case GameStates.Lost:
                break;
            default:
                break;
        }


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

    public void ResetTimer()
    {
        currentTime = maxTime;
        currentDifficulty = EDifficulty.Beginner;
        totTime = 0f;
        currentXScale = initXScale;
        UpdateBar();
        UpdateGameDifficulty();
        currentLevel.text = "" + ((int)currentDifficulty);
    }

    public void CheckDifficultyTime()
    {
        if(((int)currentDifficulty < sizeof(EDifficulty)) && totTime > difficultyTimeChanges)
        {
            currentDifficulty = (EDifficulty)((int)currentDifficulty + 1);
            UpdateGameDifficulty();
            currentLevel.text = "" + ((int)currentDifficulty);
        }
    }

    public void UpdateGameDifficulty()
    {
        int extraIngredientsToSpawn = 0;
        float timeBTWOrders = 0f;
        int minIngredientsInOrder = 1;
        int maxIngredientsInOrder = 2;
        //float descentSpeed = 2f;

        switch (currentDifficulty)
        {
            case EDifficulty.Beginner:
                extraIngredientsToSpawn = 0;
                timeBTWOrders = 3.5f;

                minIngredientsInOrder = 2;
                maxIngredientsInOrder = 2;

                difficultyTimeChanges = 10f;
                difficultyValues = 1f;
                break;

            case EDifficulty.Easy:
                extraIngredientsToSpawn = 0;
                timeBTWOrders = 3.5f;

                minIngredientsInOrder = 2;
                maxIngredientsInOrder = 4;

                difficultyTimeChanges = 20f;
                difficultyValues = 1.1f;
                break;

            case EDifficulty.Normal:
                extraIngredientsToSpawn = 0;
                timeBTWOrders = 3.5f;

                minIngredientsInOrder = 2;
                maxIngredientsInOrder = 5;

                difficultyTimeChanges = 40f;
                difficultyValues = 1.2f;
                break;

            case EDifficulty.Fast:
                extraIngredientsToSpawn = 0;
                timeBTWOrders = 4f;

                minIngredientsInOrder = 4;
                maxIngredientsInOrder = 5;

                difficultyTimeChanges = 60f;
                difficultyValues = 1.3f;
                break;

            case EDifficulty.Hard:
                extraIngredientsToSpawn = 1;
                timeBTWOrders = 4f;

                minIngredientsInOrder = 4;
                maxIngredientsInOrder = 6;

                difficultyTimeChanges = 80f;
                difficultyValues = 1.4f;
                break;

            case EDifficulty.Insane:
                extraIngredientsToSpawn = 2;
                timeBTWOrders = 4f;

                minIngredientsInOrder = 6;
                maxIngredientsInOrder = 8;

                difficultyTimeChanges = 120f;
                difficultyValues = 1.5f;
                break;

            default:
                break;
        }
        extraIngredientsToSpawn = 0;
        ListOfOrders.instance.extraIngredientsToSpawn = extraIngredientsToSpawn;
        GameController.instance.timeBTWOrders = timeBTWOrders;
        GameController.instance.minIngredientsInOrder = minIngredientsInOrder;
        GameController.instance.maxIngredientsInOrder = maxIngredientsInOrder;
    }
}

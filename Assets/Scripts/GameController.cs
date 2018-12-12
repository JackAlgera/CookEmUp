using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameStates { Playing, Paused, Lost}

public class GameController : MonoBehaviour {

    public static GameController instance;

    public GameStates currentGameState;

    public GameObject listOfOrders;
    public GameObject[] ingredientPrefabs;
    public Vector3[] spawnPositions;
    private GameObject ingredientHolder;
    public float timeBTWIngredientSpawn;
    //private float currentTimeBTWIngredientSpawn;

    public float timeBTWOrders;
    private float currentTimeBTWOrders;
    public int score;
    public Text scoreText;

    public float descentSpeed;
    public float sizeOfIngredient;
    public int numberOfIngredientsInORder;

    // Variables to spawn ingredients
    private Stack<Ingredients> ingredientsToSpawnBox1 = new Stack<Ingredients>();
    private Stack<Ingredients> ingredientsToSpawnBox2 = new Stack<Ingredients>();
    private Stack<Ingredients> ingredientsToSpawnBox3 = new Stack<Ingredients>();
    private Stack<Ingredients> ingredientsToSpawnBox4 = new Stack<Ingredients>();
    private Stack<Ingredients> ingredientsToSpawnBox5 = new Stack<Ingredients>();

    private bool canSpawnBox1 = true;
    private bool canSpawnBox2 = true;
    private bool canSpawnBox3 = true;
    private bool canSpawnBox4 = true;
    private bool canSpawnBox5 = true;

    void Awake () {
        if(instance == null)
        {
            instance = this;
        }

        currentGameState = GameStates.Playing;

        listOfOrders = GameObject.Find("ListOfOrders");
        ingredientHolder = GameObject.Find("IngredientHolder");

        GameObject spawnPosS = GameObject.Find("SpawnPositions");
        spawnPositions = new Vector3[spawnPosS.transform.childCount];

        for (int i = 0; i < spawnPositions.Length; i++)
        {
            spawnPositions[i] = spawnPosS.transform.GetChild(i).position;
        }

        currentTimeBTWOrders = timeBTWOrders / 2f;
        //currentTimeBTWIngredientSpawn = timeBTWIngredientSpawn;    
	}
	
	void Update () {
        // Ingredient Spawning
        /*
        currentTimeBTWIngredientSpawn -= Time.deltaTime;
        if(currentTimeBTWIngredientSpawn <= 0)
        {
            SpawnIngredients();
            currentTimeBTWIngredientSpawn += timeBTWIngredientSpawn;
        }
        */

        // Spawn Ingredients
        SpawnIngredient();

        // Order spawning
        currentTimeBTWOrders -= Time.deltaTime;
        if(currentTimeBTWOrders <= 0 || listOfOrders.transform.childCount == 0)
        {
            CreateNewOrder(UnityEngine.Random.Range(Mathf.FloorToInt(numberOfIngredientsInORder / 2f) , numberOfIngredientsInORder));
            currentTimeBTWOrders = timeBTWOrders;
        }

        // Collision with ingredients
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = new Vector2((Input.mousePosition.x / Screen.width) * 9f - 4.5f , (Input.mousePosition.y / Screen.height) * 16f - 8f);
            Collider2D touchedIngredient = Physics2D.OverlapPoint(mousePos);
            if (touchedIngredient != null && touchedIngredient.tag == "Ingredient")
            {
                Ingredients type = touchedIngredient.GetComponent<Ingredient>().ingredientType;
                if(listOfOrders.transform.childCount > 0)
                {
                    listOfOrders.GetComponent<ListOfOrders>().CheckOrder(type);
                }

                touchedIngredient.GetComponent<Ingredient>().ClickDestroy();
            }
        }

        /*
		if(Input.touchCount > 0)
        {
            foreach (Touch newTouch in Input.touches)
            {
                if(newTouch.phase == TouchPhase.Began)
                {
                    Collider2D touchedIngredient = Physics2D.OverlapPoint(newTouch.position);
                    if(touchedIngredient != null)
                    {
                        Ingredients type = touchedIngredient.gameObject.GetComponent<Ingredient>().ingredientType;

                    }
                }
            }
        }
        */
    }

    public void RestartGame()
    {
        CheckHighScore(score);

        score = 0;
        UpdateScore();

        listOfOrders.GetComponent<ListOfOrders>().ResetOrders();

        ingredientsToSpawnBox1.Clear();
        ingredientsToSpawnBox2.Clear();
        ingredientsToSpawnBox3.Clear();
        ingredientsToSpawnBox4.Clear();
        ingredientsToSpawnBox5.Clear();

        canSpawnBox1 = true;
        canSpawnBox2 = true;
        canSpawnBox3 = true;
        canSpawnBox4 = true;
        canSpawnBox5 = true;

        ListOfOrders.instance.ResetOrders();

        foreach (Transform ingredient in ingredientHolder.transform)
        {
            ingredient.GetComponent<Ingredient>().ClickDestroy();
        }

        TimeController.instance.RestartGame();

        currentGameState = GameStates.Playing;
    }

    public void GoToMainMenu()
    {
        CheckHighScore(score);
    }

    public void CreateNewOrder(int orderSize)
    {
        listOfOrders.GetComponent<ListOfOrders>().CreateOrder(orderSize);
    }

    public void SpawnIngredients()
    {
        int nbrOfIngredientsToSpawn = UnityEngine.Random.Range(2, 5);
        int[] spawnLocations = new int[nbrOfIngredientsToSpawn];
        for (int i = 0; i < spawnLocations.Length; i++)
        {
            spawnLocations[i] = -1;
        }

        for (int i = 0; i < spawnLocations.Length; i++) // Spawn ingredients on different locations
        {
            int newSpawnLocation = UnityEngine.Random.Range(0, spawnPositions.Length);
            while (Array.IndexOf(spawnLocations, newSpawnLocation) != -1)
            {
                newSpawnLocation = UnityEngine.Random.Range(0, spawnPositions.Length);
            }

            spawnLocations[i] = newSpawnLocation;
            Vector3 randSpawnPos = spawnPositions[newSpawnLocation];
            GameObject newIngredient = Instantiate(ingredientPrefabs[UnityEngine.Random.Range(0, ingredientPrefabs.Length)], randSpawnPos, Quaternion.identity);
            newIngredient.transform.parent = ingredientHolder.transform;
        }
    }

    public void AddIngredientToSpawn(Ingredients ingre)
    {
        switch (ingre)
        {
            case Ingredients.Bread:
                ingredientsToSpawnBox1.Push(ingre);
                break;
            case Ingredients.Spaghetti:
                ingredientsToSpawnBox1.Push(ingre);
                break;
            case Ingredients.Cheese:
                ingredientsToSpawnBox2.Push(ingre);
                break;
            case Ingredients.Wine:
                ingredientsToSpawnBox2.Push(ingre);
                break;
            case Ingredients.Tomatoes:
                ingredientsToSpawnBox3.Push(ingre);
                break;
            case Ingredients.Lettuce:
                ingredientsToSpawnBox3.Push(ingre);
                break;
            case Ingredients.Beef:
                ingredientsToSpawnBox4.Push(ingre);
                break;
            case Ingredients.Chicken:
                ingredientsToSpawnBox4.Push(ingre);
                break;
            case Ingredients.Fish:
                ingredientsToSpawnBox5.Push(ingre);
                break;
            case Ingredients.Eggs:
                ingredientsToSpawnBox5.Push(ingre);
                break;
                /*
            case Ingredients.Spaghetti:
                ingredientsToSpawnBox1.Push(ingre);
                break;
            case Ingredients.Sausage:
                ingredientsToSpawnBox1.Push(ingre);
                break;
            default:
                break;
                */
        }
    }

    public void SpawnIngredient()
    {
        if(canSpawnBox1 && ingredientsToSpawnBox1.Count != 0)
        {
            Ingredients ingredientToSpawn = ingredientsToSpawnBox1.Pop();

            GameObject newIngredient = Instantiate(ingredientPrefabs[(int)ingredientToSpawn], spawnPositions[0], Quaternion.identity); // Spawn bread at the first position
            newIngredient.GetComponent<Ingredient>().descentSpeed = descentSpeed;
            newIngredient.transform.parent = ingredientHolder.transform;
            canSpawnBox1 = false;
        }

        if (canSpawnBox2 && ingredientsToSpawnBox2.Count != 0)
        {
            Ingredients ingredientToSpawn = ingredientsToSpawnBox2.Pop();

            GameObject newIngredient = Instantiate(ingredientPrefabs[(int)ingredientToSpawn], spawnPositions[1], Quaternion.identity); // Spawn bread at the first position
            newIngredient.GetComponent<Ingredient>().descentSpeed = descentSpeed;
            newIngredient.transform.parent = ingredientHolder.transform;
            canSpawnBox2 = false;
        }
        if (canSpawnBox3 && ingredientsToSpawnBox3.Count != 0)
        {
            Ingredients ingredientToSpawn = ingredientsToSpawnBox3.Pop();

            GameObject newIngredient = Instantiate(ingredientPrefabs[(int)ingredientToSpawn], spawnPositions[2], Quaternion.identity); // Spawn bread at the first position
            newIngredient.GetComponent<Ingredient>().descentSpeed = descentSpeed;
            newIngredient.transform.parent = ingredientHolder.transform;
            canSpawnBox3 = false;
        }
        if (canSpawnBox4 && ingredientsToSpawnBox4.Count != 0)
        {
            Ingredients ingredientToSpawn = ingredientsToSpawnBox4.Pop();

            GameObject newIngredient = Instantiate(ingredientPrefabs[(int)ingredientToSpawn], spawnPositions[3], Quaternion.identity); // Spawn bread at the first position
            newIngredient.GetComponent<Ingredient>().descentSpeed = descentSpeed;
            newIngredient.transform.parent = ingredientHolder.transform;
            canSpawnBox4 = false;
        }
        if (canSpawnBox5 && ingredientsToSpawnBox5.Count != 0)
        {
            Ingredients ingredientToSpawn = ingredientsToSpawnBox5.Pop();

            GameObject newIngredient = Instantiate(ingredientPrefabs[(int)ingredientToSpawn], spawnPositions[4], Quaternion.identity); // Spawn bread at the first position
            newIngredient.GetComponent<Ingredient>().descentSpeed = descentSpeed;
            newIngredient.transform.parent = ingredientHolder.transform;
            canSpawnBox5 = false;
        }
    }

    public void SetAbleToSpawnAtPosition(int position, bool able)
    {
        switch (position)
        {
            case 1:
                canSpawnBox1 = able;
                break;

            case 2:
                canSpawnBox2 = able;
                break;

            case 3:
                canSpawnBox3 = able;
                break;

            case 4:
                canSpawnBox4 = able;
                break;

            case 5:
                canSpawnBox5 = able;
                break;
            default:
                break;
        }
    }

    public void CheckTouch(Vector3 touchPosition)
    {

    }

    public void UpdateScore()
    {
        scoreText.text = "" + score;
    }

    public void IncreaseScore()
    {
        score += 1;
        UpdateScore();
    }

    public void IncreaseScoreAfterOrder(int orderSize)
    {
        score += orderSize * 5;
        UpdateScore();
    }

    public void ActivateObject(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }

    public void PauseGame()
    {
        if(Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }

    public void PauseState()
    {
        currentGameState = GameStates.Paused;
    }
    public void PlayingState()
    {
        currentGameState = GameStates.Playing;
    }
    public void LostState()
    {
        currentGameState = GameStates.Lost;
    }

    public void CheckHighScore(int newHighScore)
    {
        if (newHighScore > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", newHighScore);
        }
    }

    public void LoseGame()
    {
        CheckHighScore(score);
        PauseGame();
        currentGameState = GameStates.Lost;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController instance;
    public GameObject listOfOrders;
    public GameObject[] ingredientPrefabs;
    public Vector3[] spawnPositions;
    private GameObject ingredientHolder;
    public float timeBTWIngredientSpawn;
    private float currentTimeBTWIngredientSpawn;

    public float timeBTWOrders;
    private float currentTimeBTWOrders;

    public float ingredientDescentSpeed = 2f;

    public int score;
    public Text scoreText;

    public float descentSpeed;
    public float sizeOfIngredient;
    public int difficulty;

    // Variables to spawn ingredients
    private Stack<GameObject> ingredientsToSpawnBox1 = new Stack<GameObject>();
    private Stack<GameObject> ingredientsToSpawnBox2 = new Stack<GameObject>();
    private Stack<GameObject> ingredientsToSpawnBox3 = new Stack<GameObject>();
    private Stack<GameObject> ingredientsToSpawnBox4 = new Stack<GameObject>();
    private Stack<GameObject> ingredientsToSpawnBox5 = new Stack<GameObject>();

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

        listOfOrders = GameObject.Find("ListOfOrders");
        ingredientHolder = GameObject.Find("IngredientHolder");

        GameObject spawnPosS = GameObject.Find("SpawnPositions");
        spawnPositions = new Vector3[spawnPosS.transform.childCount];

        for (int i = 0; i < spawnPositions.Length; i++)
        {
            spawnPositions[i] = spawnPosS.transform.GetChild(i).position;
        }

        currentTimeBTWOrders = timeBTWOrders / 2f;
        currentTimeBTWIngredientSpawn = timeBTWIngredientSpawn;    
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
            CreateNewOrder(UnityEngine.Random.Range(Mathf.FloorToInt(difficulty / 2f) , difficulty));
            currentTimeBTWOrders = timeBTWOrders;
        }

        // Collision with ingredients
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = new Vector2((Input.mousePosition.x / Screen.width) * 9f - 4.5f , (Input.mousePosition.y / Screen.height) * 16f - 8f);
            Collider2D touchedIngredient = Physics2D.OverlapPoint(mousePos);
            if (touchedIngredient != null)
            {
                Ingredients type = touchedIngredient.transform.parent.GetComponent<Ingredient>().ingredientType;
                if(listOfOrders.transform.childCount > 0)
                {
                    listOfOrders.GetComponent<ListOfOrders>().CheckOrder(type);
                }

                touchedIngredient.transform.parent.GetComponent<Ingredient>().ClickDestroy();
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

        foreach (Transform ingredient in ingredientHolder.transform)
        {
            ingredient.GetComponent<Ingredient>().ClickDestroy();
        }
    }

    public void GoToMainMenu()
    {
        CheckHighScore(score);
        SceneManager.LoadScene("MainMenu");
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
        GameObject newIngredient = null;

        switch (ingre)
        {
            case Ingredients.Bread:
                newIngredient = Instantiate(ingredientPrefabs[(int)ingre], spawnPositions[0], Quaternion.identity); // Spawn bread at the first position
                newIngredient.GetComponent<Ingredient>().descentSpeed = 0f;
                newIngredient.transform.parent = ingredientHolder.transform;
                ingredientsToSpawnBox1.Push(newIngredient);
                break;
            case Ingredients.Tomato:
                newIngredient = Instantiate(ingredientPrefabs[(int)ingre], spawnPositions[1], Quaternion.identity); // Spawn bread at the first position
                newIngredient.GetComponent<Ingredient>().descentSpeed = 0f;
                newIngredient.transform.parent = ingredientHolder.transform;
                ingredientsToSpawnBox2.Push(newIngredient);
                break;
            case Ingredients.Lettuce:
                newIngredient = Instantiate(ingredientPrefabs[(int)ingre], spawnPositions[2], Quaternion.identity); // Spawn bread at the first position
                newIngredient.GetComponent<Ingredient>().descentSpeed = 0f;
                newIngredient.transform.parent = ingredientHolder.transform;
                ingredientsToSpawnBox3.Push(newIngredient);
                break;
            default:
                break;
        }
    }

    public void SpawnIngredient()
    {
        if(canSpawnBox1 && ingredientsToSpawnBox1.Count != 0)
        {
            GameObject ingredientToSpawn = ingredientsToSpawnBox1.Pop();
            ingredientToSpawn.GetComponent<Ingredient>().descentSpeed = descentSpeed;
            canSpawnBox1 = false;
        }

        if (canSpawnBox2 && ingredientsToSpawnBox2.Count != 0)
        {
            GameObject ingredientToSpawn = ingredientsToSpawnBox2.Pop();
            ingredientToSpawn.GetComponent<Ingredient>().descentSpeed = descentSpeed;
            canSpawnBox2 = false;
        }
        if (canSpawnBox3 && ingredientsToSpawnBox3.Count != 0)
        {
            GameObject ingredientToSpawn = ingredientsToSpawnBox3.Pop();
            ingredientToSpawn.GetComponent<Ingredient>().descentSpeed = descentSpeed;
            canSpawnBox3 = false;
        }
        if (canSpawnBox4 && ingredientsToSpawnBox4.Count != 0)
        {
            GameObject ingredientToSpawn = ingredientsToSpawnBox4.Pop();
            ingredientToSpawn.GetComponent<Ingredient>().descentSpeed = descentSpeed;
            canSpawnBox4 = false;
        }
        if (canSpawnBox5 && ingredientsToSpawnBox5.Count != 0)
        {
            GameObject ingredientToSpawn = ingredientsToSpawnBox5.Pop();
            ingredientToSpawn.GetComponent<Ingredient>().descentSpeed = descentSpeed;
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

    public void CheckHighScore(int newHighScore)
    {
        if (newHighScore > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", newHighScore);
        }
    }
}

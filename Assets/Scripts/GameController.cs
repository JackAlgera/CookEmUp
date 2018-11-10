using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController instance;
    public GameObject listOfOrders;
    public GameObject[] ingredientPrefabs;
    public Vector3[] spawnPositions;
    private GameObject ingredientHolder;
    public float timeBTWIngredientSpawn;
    private float currentTimeBTWIngredientSpawn;

    public float timeBTWOrders;
    private float currentTimeBTWOders;

    public float ingredientDescentSpeed = 2f;

    public int score;
    public Text scoreText;

    public int difficulty;

	void Awake () {
        if(instance == null)
        {
            instance = this;
        }

        ResetGame();

        listOfOrders = GameObject.Find("ListOfOrders");
        ingredientHolder = GameObject.Find("IngredientHolder");

        GameObject spawnPosS = GameObject.Find("SpawnPositions");
        spawnPositions = new Vector3[spawnPosS.transform.childCount];

        for (int i = 0; i < spawnPositions.Length; i++)
        {
            spawnPositions[i] = spawnPosS.transform.GetChild(i).position;
        }

        currentTimeBTWOders = timeBTWOrders / 2f;
        currentTimeBTWIngredientSpawn = timeBTWIngredientSpawn;    
	}
	
	void Update () {
        // Ingredient Spawning
        currentTimeBTWIngredientSpawn -= Time.deltaTime;
        if(currentTimeBTWIngredientSpawn <= 0)
        {
            SpawnIngredients();
            currentTimeBTWIngredientSpawn += timeBTWIngredientSpawn;
        }

        // Orders
        currentTimeBTWOders -= Time.deltaTime;
        if(currentTimeBTWOders <= 0)
        {
            CreateNewOrder(UnityEngine.Random.Range(Mathf.FloorToInt(difficulty / 5f) , difficulty));
            currentTimeBTWOders += timeBTWOrders;
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

    public void ResetGame()
    {
        score = 0;
        UpdateScore();
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour {

    public int orderSize;
    public int currentOrderIndex;
    public float symbolSize;
    private float currentSymbolPossition = 0f;
    public float orderTime;

    public Ingredients[] listOfIngredients;

    void Start () {
        currentOrderIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
        orderTime -= Time.deltaTime;
	}

    public void GenerateRandomOrder(GameObject[] gameIngredientsSymbols)
    {
        listOfIngredients = new Ingredients[orderSize];

        for (int i = 0; i < orderSize; i++)
        {
            int randomIngrePos = Random.Range(0, gameIngredientsSymbols.Length);

            GameObject newIngredient = Instantiate(gameIngredientsSymbols[randomIngrePos], transform.position, Quaternion.identity);
            newIngredient.transform.parent = transform;
            newIngredient.transform.Translate(new Vector3(currentSymbolPossition, 0, 0));
            currentSymbolPossition += symbolSize;

            // Spawn required ingredients for order
            listOfIngredients[i] = ((Ingredients)randomIngrePos);
        }
        SpawnIngredients();
    }

    public void SpawnIngredients()
    {
        foreach(Ingredients i in listOfIngredients)
        {
            GameController.instance.AddIngredientToSpawn(i);
        }

        // Plus random ingredients
        int nbrOfRandomIngre = Random.Range(1, 2);
        for (int i = 0; i < nbrOfRandomIngre; i++)
        {
            GameController.instance.AddIngredientToSpawn((Ingredients) Random.Range(0, sizeof(Ingredients)));
        }
    }

    public void CheckOrder(Ingredients type)
    {
        if(transform.GetChild(currentOrderIndex).GetComponent<Symbol>().ingredientType == type)
        {
            transform.GetChild(currentOrderIndex).GetComponent<Animator>().SetTrigger("GrayOut");
            currentOrderIndex++;
            GameController.instance.IncreaseScore();
        }
        else
        {
            if(listOfIngredients != null)
            {
                SpawnIngredients();
            }
            ResetOrder();
        }
    }

    public void ResetOrder()
    {
        for (int i = 0; i < currentOrderIndex; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetTrigger("Reset");

        }
        currentOrderIndex = 0;
    }

    public bool FinishedOrder()
    {
        return (currentOrderIndex == orderSize);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}

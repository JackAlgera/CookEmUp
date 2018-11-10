using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour {

    public int orderSize;
    public int currentOrderIndex;
    public float symbolSize;
    private float currentSymbolPossition = 0f;
    public float orderTime;

    void Start () {
        currentOrderIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
        orderTime -= Time.deltaTime;
	}

    public void GenerateRandomOrder(GameObject[] ingredients)
    {
        for (int i = 0; i < orderSize; i++)
        {
            GameObject newIngredient = Instantiate(ingredients[Random.Range(0, ingredients.Length)], transform.position, Quaternion.identity);
            newIngredient.transform.parent = transform;
            newIngredient.transform.Translate(new Vector3(currentSymbolPossition, 0, 0));
            currentSymbolPossition += symbolSize;
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

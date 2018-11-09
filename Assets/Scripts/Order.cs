using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour {

    public int orderSize;
    public float symbolSize;
    private float currentSymbolPossition = 0f;

    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
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

}

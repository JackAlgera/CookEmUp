using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfOrders : MonoBehaviour {

    public GameObject Order;
    public GameObject[] gameIngredients;

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void CreateOrder(int orderSize)
    {
        GameObject newOrder = Instantiate(Order, transform.position, Quaternion.identity);
        newOrder.transform.parent = transform;
        newOrder.GetComponent<Order>().orderSize = orderSize;
        newOrder.GetComponent<Order>().GenerateRandomOrder(gameIngredients);
    }
}

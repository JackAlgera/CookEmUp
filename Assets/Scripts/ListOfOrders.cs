using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfOrders : MonoBehaviour {

    public GameObject Order;
    public GameObject[] gameIngredients;
    public float distanceBTWOrders;
    public GameObject listOfFinishedOrders;

    void Start () {
        listOfFinishedOrders = GameObject.Find("ListOfFinishedOrders");
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void CreateOrder(int orderSize)
    {
        GameObject newOrder = Instantiate(Order, transform.position, Quaternion.identity);
        newOrder.transform.Translate(new Vector3(0, -distanceBTWOrders * (float)transform.childCount, 0));
        newOrder.transform.parent = transform;
        newOrder.GetComponent<Order>().orderSize = orderSize;
        newOrder.GetComponent<Order>().GenerateRandomOrder(gameIngredients);
    }

    public void CheckOrder(Ingredients type)
    {
        transform.GetChild(0).GetComponent<Order>().CheckOrder(type);
        if(transform.GetChild(0).GetComponent<Order>().FinishedOrder())
        {
            GameController.instance.IncreaseScoreAfterOrder(transform.GetChild(0).GetComponent<Order>().orderSize);
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("Destroy");
            transform.GetChild(0).transform.parent = listOfFinishedOrders.transform;
            foreach (Transform order in transform)
            {
                order.Translate(new Vector3(0, distanceBTWOrders, 0));
            }
        }
    }
}

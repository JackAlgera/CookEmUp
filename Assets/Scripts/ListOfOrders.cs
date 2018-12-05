using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfOrders : MonoBehaviour {

    public static ListOfOrders instance;

    public GameObject Order;
    public GameObject[] gameIngredientsSymbols;
    public float distanceBTWOrders;
    public GameObject listOfFinishedOrders;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start () {
        listOfFinishedOrders = GameObject.Find("ListOfFinishedOrders");
	}

    public void ResetOrders()
    {
        foreach (Transform order in transform)
        {
            order.GetComponent<Animator>().SetTrigger("Destroy");
        }
    }

    public void CreateOrder(int orderSize)
    {
        GameObject newOrder = Instantiate(Order, transform.position, Quaternion.identity);
        newOrder.transform.Translate(new Vector3(0, -distanceBTWOrders * (float)transform.childCount, 0));
        newOrder.transform.parent = transform;
        newOrder.transform.GetChild(0).GetComponent<Order>().orderSize = orderSize;
        newOrder.transform.GetChild(0).GetComponent<Order>().GenerateRandomOrder(gameIngredientsSymbols);
    }

    public void CheckOrder(Ingredients type)
    {
        transform.GetChild(0).GetChild(0).GetComponent<Order>().CheckOrder(type);
        if(transform.GetChild(0).GetChild(0).GetComponent<Order>().FinishedOrder())
        {
            GameController.instance.IncreaseScoreAfterOrder(transform.GetChild(0).GetChild(0).GetComponent<Order>().orderSize);
            transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("Destroy");
            transform.GetChild(0).parent = listOfFinishedOrders.transform;

            IEnumerator currentOrderToMoveCoroutine;

            foreach (Transform order in transform)
            {
                currentOrderToMoveCoroutine = MoveUpOrder(order);
                StartCoroutine(currentOrderToMoveCoroutine);
                //order.transform.GetChild(0).GetComponent<Animator>().SetTrigger("MoveUp");
            }

            // Increase time limit
            TimeController.instance.FinishOrder();
        }
    }

    IEnumerator MoveUpOrder(Transform order)
    {
        yield return new WaitForSeconds(0.2f);
        order.Translate(new Vector3(0, ListOfOrders.instance.distanceBTWOrders, 0));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfOrders : MonoBehaviour {

    public static ListOfOrders instance;

    public GameObject Order;
    public GameObject[] gameIngredientsSymbols;
    public float distanceBTWOrders;
    public GameObject listOfFinishedOrders;

    public Stack<FutureOrder> ordersToSpawn = new Stack<FutureOrder>();

    public int extraIngredientsToSpawn;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        extraIngredientsToSpawn = 0;
    }

    void Start () {
        listOfFinishedOrders = GameObject.Find("ListOfFinishedOrders");
	}

    private void Update()
    {
        if(ordersToSpawn.Count != 0 && transform.childCount < 3)
        {
            PlaceOrder();
        }
    }

    public void ResetOrders()
    {
        while(transform.childCount > 0)
        {
            transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("Destroy");
            transform.GetChild(0).parent = listOfFinishedOrders.transform;
        }
    }

    public void CreateOrder(int orderSize)
    {
        FutureOrder newOrder = new FutureOrder
        {
            orderSize = orderSize,
            extraIngredientsToSpawn = extraIngredientsToSpawn
        };
        ordersToSpawn.Push(newOrder);
    }

    public void PlaceOrder()
    {
        FutureOrder orderToServe = ordersToSpawn.Pop();

        GameObject newOrder = Instantiate(Order, transform.position, Quaternion.identity);
        newOrder.transform.Translate(new Vector3(0, -distanceBTWOrders * (float)transform.childCount, 0));
        newOrder.transform.parent = transform;

        newOrder.transform.GetChild(0).GetComponent<Order>().InitialSetup(orderToServe.orderSize, orderToServe.extraIngredientsToSpawn, gameIngredientsSymbols);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientBubble : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ingredient")
        {
            transform.parent.GetComponent<Ingredient>().descentSpeed = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ingredient" && transform.position.y > collision.transform.position.y)
        {
            transform.parent.GetComponent<Ingredient>().descentSpeed = GameController.instance.descentSpeed;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientStopper : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ingredient")
        {
            collision.transform.parent.GetComponent<Ingredient>().descentSpeed = 0f;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosition : MonoBehaviour {

    public int position;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ingredient")
        {
            GameController.instance.SetAbleToSpawnAtPosition(position, true);
        }
    }
}

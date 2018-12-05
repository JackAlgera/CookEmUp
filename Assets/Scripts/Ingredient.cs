using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour {

    public Ingredients ingredientType;
    public float descentSpeed;

	void FixedUpdate () {
        Vector3 temp = transform.position;
        temp.y -= descentSpeed * Time.deltaTime;
        transform.position = temp;
	}

    public void ClickDestroy()
    {
        if(descentSpeed > 0)
        {
            transform.GetChild(0).tag = "Untagged";
        }
        gameObject.GetComponent<Animator>().SetTrigger("Destroy");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}

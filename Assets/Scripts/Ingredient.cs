using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour {

    public Ingredients ingredientType;
    public float descentSpeed;

	void Start () {
	}
	
	void FixedUpdate () {
        Vector3 temp = transform.position;
        temp.y -= descentSpeed * Time.deltaTime;
        transform.position = temp;
	}

    public void ClickDestroy()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Destroy");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}

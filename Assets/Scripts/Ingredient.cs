using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour {

    public Ingredients ingredientType;
    public float descentSpeed;

    public float maxRotTime;
    public float currentRotTime;

    public float almostRottenTime;
    public bool isRotten = false;

    private void Awake()
    {
        currentRotTime = maxRotTime;
        almostRottenTime = maxRotTime / 3;
    }

    void FixedUpdate () {
        UpdateRotTime();

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

    public void UpdateRotTime()
    {
        currentRotTime -= Time.deltaTime;

        if(currentRotTime < 0)
        {
            gameObject.GetComponent<Animator>().SetTrigger("Destroy");
            // TODO : Rot animation
        }

        if (currentRotTime < almostRottenTime && !isRotten)
        {
            // TODO : Change to be rotten
            isRotten = true;
        }
    }
}

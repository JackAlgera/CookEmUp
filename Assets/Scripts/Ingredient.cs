using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour {

    public Ingredients ingredientType;
    public float descentSpeed;
    public GameObject flies;
    public Animator anim;

    public float maxRotTime;
    public float currentRotTime;

    public float almostRottenTime;
    public bool isRotten = false;

    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
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
        anim.SetTrigger("Destroy");
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
            anim.SetTrigger("Destroy");
        }

        if (currentRotTime < almostRottenTime && !isRotten)
        {
            SpawnFlies();
            anim.SetTrigger("Rot");
            isRotten = true;
        }
    }

    public void SpawnFlies()
    {
        GameObject rottenEffect = Instantiate(flies, transform.position, Quaternion.identity);
        rottenEffect.transform.parent = transform;
    }
}

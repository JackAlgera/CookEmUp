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
    public Rigidbody2D rb;

    public float almostRottenTime;
    public bool isRotten = false;
    public bool isDestroying = false;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        maxRotTime *= Random.Range(0.7f, 1.3f);
        anim = gameObject.GetComponent<Animator>();
        currentRotTime = maxRotTime;
        almostRottenTime = maxRotTime / 3;
        //rb.velocity = Vector3.down * descentSpeed;
    }

    void FixedUpdate () {
        UpdateRotTime();
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

        if(currentRotTime < 0 && !isDestroying)
        {
            isDestroying = true;
            SpawnAgain();
            ClickDestroy();
        }

        if (currentRotTime < almostRottenTime && !isRotten)
        {
            anim.SetTrigger("Rot");
            isRotten = true;
        }
    }

    public void SpawnFlies()
    {
        GameObject rottenEffect = Instantiate(flies, transform.position, Quaternion.identity);
        rottenEffect.transform.parent = transform;
    }

    public void SpawnAgain()
    {
        GameController.instance.AddIngredientToSpawn(ingredientType);
    }
}

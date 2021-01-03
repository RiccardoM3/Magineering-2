using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockResource : Resource
{

    public int maxHealth = 10;
    public int minHealth = 6;

    private int health;

    // Start is called before the first frame update
    void Start()
    {
        health = Random.Range(minHealth, maxHealth);
    }

    public void breakRock()
    {
        /*Debug.Log("Initial Health: " + health.ToString());*/
        base.lootResource();

        if (health > 1)
        {
            health -= 1;
            /*Debug.Log("Health: " + health.ToString());*/
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
}

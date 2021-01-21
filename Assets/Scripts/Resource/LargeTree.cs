using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeTree : Resource
{

    public int maxHealth;
    public int minHealth;
    public int treeId;

    private int health;

    public void Start()
    {
        health = Random.Range(minHealth, maxHealth);
    }
    public bool breakTree()
    {
        /*Debug.Log("Initial Health: " + health.ToString());*/
        base.lootResource();

        if (health > 1)
        {
            health -= 1;
            /*Debug.Log("Health: " + health.ToString());*/
            return false;
        } else
        {
            /*Debug.Log("Breaking tree");*/
            /*Destroy(this.gameObject);*/
            return true;
        }

    }

    public void reset()
    {
        health = Random.Range(minHealth, maxHealth);
    }
}
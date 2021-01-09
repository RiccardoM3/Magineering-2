using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pickaxe : MonoBehaviour
{
    private RaycastHit hit; 
    private Animator animator;

    public float range = 3f;
    private bool isSwinging;

    void Start()
    {
        animator = GetComponent<Animator>();
        isSwinging = false;
    }


    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !isSwinging)
        {
            animator.Play("PickaxeSwing");
            isSwinging = true;
        }
    }

    void ToolSwingCallback()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, range))
        {
            // Did we click something minable?
            if (hit.collider.gameObject.tag == "Minable")
            {
                GameObject minableObject = hit.collider.gameObject;

                RockResource rock = minableObject.GetComponentInParent<RockResource>();
                rock.breakRock();
            }
        }
    }

    void ToolSwingFinish()
    {
        isSwinging = false;
    }
}
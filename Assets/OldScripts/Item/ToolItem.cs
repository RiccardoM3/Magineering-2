using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "Inventory/Tool")]
public class ToolItem : Item
{
    public GameObject toolPrefab;
    private GameObject tool;
    public override void Hover()
    {
        base.Hover();
        tool = Instantiate(toolPrefab, GameObject.Find("MainCamera").transform);
    }

    public override void Unhover()
    {
        base.Unhover();
        if (tool != null)
        {
            Destroy(tool);
            tool = null;
        }
    }
}

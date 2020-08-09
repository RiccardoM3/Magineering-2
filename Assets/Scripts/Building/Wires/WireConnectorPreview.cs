using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireConnectorPreview : MonoBehaviour
{
    private GameObject firstNode;
    private GameObject secondNode;

    // Start is called before the first frame update
    void Start()
    {
        Blueify(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (firstNode && secondNode) {
            Vector3 center = Vector3.Lerp(firstNode.transform.position, secondNode.transform.position, 0.5f);
            Vector3 newScale = transform.localScale;
            newScale.y = Vector3.Distance(firstNode.transform.position, secondNode.transform.position) / 2;
            transform.position = center;
            transform.localScale = newScale;
            transform.up = firstNode.transform.position - secondNode.transform.position;
        }
        
    }

    public void SetFirstNode(GameObject node) {
        this.firstNode = node;
    }

    public void SetSecondNode(GameObject node) {
        this.secondNode = node;
    }

    private void Blueify(GameObject model) {

        if (model.GetComponent<Renderer>() != null) {
            Color baseColor = model.GetComponent<Renderer>().material.color;
            baseColor.r = 0.35f;
            baseColor.b = 200;
            baseColor.g = 0.35f;
            model.GetComponent<Renderer>().material.color = baseColor;
        }

        foreach (Transform child in model.transform) {
            Blueify(child.gameObject);
        }
    }
}

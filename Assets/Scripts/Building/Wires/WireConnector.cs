using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireConnector : MonoBehaviour
{
    public GameObject wireConnectorPrefab;

    public WireNode firstWireNode;
    public WireNode secondWireNode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignWireNodes(WireNode nodeOne, WireNode nodeTwo) {
        this.firstWireNode = nodeOne;
        this.secondWireNode = nodeTwo;
    }

    public void CreateWireConnector(WireNode nodeOne, WireNode nodeTwo) {
        Vector3 center = Vector3.Lerp(nodeOne.transform.position, nodeTwo.transform.position, 0.5f);
        GameObject wireConnector = Instantiate(wireConnectorPrefab, center, new Quaternion());
        Vector3 newScale = wireConnector.transform.localScale;
        newScale.y = Vector3.Distance(nodeOne.transform.position, nodeTwo.transform.position) / 2;
        wireConnector.transform.localScale = newScale;
        wireConnector.transform.up = nodeOne.transform.position - nodeTwo.transform.position;
        wireConnector.GetComponent<WireConnector>().AssignWireNodes(nodeOne, nodeTwo);
    }

    public void SubdivideNodes(WireNode newNode) {
        firstWireNode.RemoveConnection(secondWireNode);
        firstWireNode.AddConnection(newNode);
        secondWireNode.RemoveConnection(firstWireNode);
        secondWireNode.AddConnection(newNode);
        newNode.AddConnection(firstWireNode);
        newNode.AddConnection(secondWireNode);
        CreateWireConnector(firstWireNode.GetComponent<WireNode>(), newNode);
        CreateWireConnector(newNode, secondWireNode.GetComponent<WireNode>());
        Destroy(this.gameObject);
    }
}

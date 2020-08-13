using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireNode : MonoBehaviour
{
    public NodeType nodeType = NodeType.Connector;
    public List<WireNode> connectedNodes = new List<WireNode>();
    public WireSystem attachedWireSystem;
    public float powerContribution = 0;
    public float receivedPower = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddConnection(WireNode node) {
        this.connectedNodes.Add(node);
    }

    public void RemoveConnection(WireNode node) {
        this.connectedNodes.Remove(node);
    }
}

public enum NodeType {
    Generator,
    Connector,
    Consumer
}
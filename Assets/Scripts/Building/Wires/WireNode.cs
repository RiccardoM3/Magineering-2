﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireNode : MonoBehaviour
{
    public NodeType nodeType = NodeType.Connector;
    public List<WireNode> connectedNodes = new List<WireNode>();
    public WireSystem attachedWireSystem;

    private float powerContribution = 0;
    private float contributionPerFrame = 0;

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
    Input,
    Connector,
    Output
}
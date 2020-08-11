using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireSystem
{
    public List<WireNode> wireNodes = new List<WireNode>();
    public WireItem wireType;

    public void AddWireNode(WireNode wireNode) {
        wireNodes.Add(wireNode);
        wireNode.attachedWireSystem = this;
    }

    public void MergeWireSystem(WireSystem wireSystem) {
        for (int i = 0; i < wireSystem.wireNodes.Count; i++) {
            AddWireNode(wireSystem.wireNodes[i]);
        }
    }
}

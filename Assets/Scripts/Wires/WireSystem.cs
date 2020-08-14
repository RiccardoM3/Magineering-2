using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireSystem
{
    public List<WireNode> wireNodes = new List<WireNode>();
    public WireItem wireType;

    private float collectedPower = 0;

    public void AddWireNode(WireNode wireNode) {
        wireNodes.Add(wireNode);
        wireNode.attachedWireSystem = this;
    }

    public void MergeWireSystem(WireSystem wireSystem) {
        for (int i = 0; i < wireSystem.wireNodes.Count; i++) {
            AddWireNode(wireSystem.wireNodes[i]);
        }
    }

    public void CollectPower() {
        collectedPower = 0;
        for (int i = 0; i < wireNodes.Count; i++) {
            if (wireNodes[i].nodeType == NodeType.Generator) {
                collectedPower += wireNodes[i].powerContribution;
            }
        }
    }

    public void PushPower() {
        //Debug.Log("Pushing " + collectedPower);

        //Find all nodes which are comsuming power and sort them by required power in ascending order
        List<WireNode> consumerNodes = new List<WireNode>();
        for (int i = 0; i < wireNodes.Count; i++) {
            if (wireNodes[i].nodeType == NodeType.Consumer) {
                consumerNodes.Add(wireNodes[i]);
            }
        }

        consumerNodes.Sort((n1, n2) => n1.powerContribution.CompareTo(n2.powerContribution));

        //loop through each consumer node and give it power
        while (consumerNodes.Count > 0) {
            float maxSupplyPower = collectedPower / consumerNodes.Count;
            float requiredPower = consumerNodes[0].powerContribution / Time.deltaTime;

            if (requiredPower <= maxSupplyPower) {
                consumerNodes[0].receivedPower = requiredPower;
                collectedPower -= requiredPower;
            } else {
                consumerNodes[0].receivedPower = maxSupplyPower;
                collectedPower -= maxSupplyPower;
            }

            consumerNodes.RemoveAt(0);
        }
    }
}

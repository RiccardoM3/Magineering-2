using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*wires are placed by placing 2 nodes
nodes prioritise snapoints to other nodes
nodes also snap to any wire
nodes by default are placed on a surface when there isnt a snappoint
clicking e toggles the mode to a freemode placement
in freemode placement, scroll brings it forward/back.it is placed at a fixed distance from the camera.these can be placed in 3d
all wires can be placed crossing / going through each other and buildings
nodes are always valid*/
public class WireNode : MonoBehaviour
{
    //public WireNetwork wireNetwork;
    public NodeType nodeType = NodeType.Connector;
    public delegate void OnPullPower();
    public event OnPullPower PullEvents;
    public delegate void OnPushPower();
    public event OnPushPower PushEvents;
    public List<WireNode> connectedNodes = new List<WireNode>();

    private WireItem wireItem;
    private float powerContribution = 0;
    private float contributionPerFrame = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (nodeType == NodeType.Input) {
            PullPower();
        }

        if (nodeType == NodeType.Output) {
            PushPower();
        }
    }

    private void PullPower() {
        PullEvents?.Invoke();
    }

    private void PushPower() {
        PushEvents?.Invoke();
    }

    public void UnsubscribeAll() {
        if (PullEvents != null) {
            foreach (var method in PullEvents.GetInvocationList()) {
                PullEvents -= (method as OnPullPower);
            }
        }

        if (PushEvents != null) {
            foreach (var method in PushEvents.GetInvocationList()) {
                PushEvents -= (method as OnPushPower);
            }
        }
    }

    public void AddConnection(WireNode node) {
        this.connectedNodes.Add(node);
    }

    public void RemoveConnection(WireNode node) {
        this.connectedNodes.Remove(node);
    }

    public void SetItem(WireItem wire) {
        this.wireItem = wire;
    }
}

public enum NodeType {
    Input,
    Connector,
    Output
}
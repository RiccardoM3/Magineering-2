using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireManager : MonoBehaviour
{
    #region Singleton
    public static WireManager instance;

    void Awake() {
        if (instance != null) {
            Debug.LogWarning("More than one instance of WireManager found!");
            return;
        }
        instance = this;
    }
    #endregion

    public GameObject wireNodePreview;
    public GameObject wireConnectorPrefab;
    public GameObject wireConnectorPreviewPrefab;

    private State state;
    private GameObject firstWireNodePreview;
    private GameObject secondWireNodePreview;
    private GameObject wireConnectorPreview;
    private WireItem currentWireType;
    private float minLength = 0.15f;
    private float maxLength = 10f;
    private bool isValid;
    private bool isPlacingWires = false;
    private List<WireSystem> wireSystems = new List<WireSystem>();

    private enum State {
        Inactive,
        FirstNode,
        SecondNode
    }

    // Start is called before the first frame update
    void Start()
    {
        TimeTicker.OnTick += delegate (object sender, TimeTicker.OnTickEventArgs e) {

            for (int i = 0; i < wireSystems.Count; i++) {
                wireSystems[i].CollectPower();
                wireSystems[i].PushPower();
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckIfActiveStateChanged()) {
            return;
        }

        if (isPlacingWires) {
            PlaceWires();
        }
    }

    private bool CheckIfActiveStateChanged() {
        if (isPlacingWires && state == State.FirstNode && Input.GetMouseButtonDown(1)) {
            StopPlacingWires();
            state = State.Inactive;
            return true;
        }
        else if (!isPlacingWires && state == State.Inactive && Input.GetMouseButtonDown(0)) {
            StartPlacingWires(InventoryController.instance.activeSlot.item as WireItem);
            return true;
        }

        return false;
    }

    private void PlaceWires() {
        CheckValidity();
        UpdateValidity();
        if (Input.GetMouseButtonDown(0) && state == State.FirstNode) {
            PauseFirstNodePreview();
            secondWireNodePreview = Instantiate(wireNodePreview);
            secondWireNodePreview.GetComponent<WireNodePreview>().SetRaycastRange(firstWireNodePreview.GetComponent<WireNodePreview>().GetRaycastRange());
            wireConnectorPreview = Instantiate(wireConnectorPreviewPrefab);
            wireConnectorPreview.GetComponent<WireConnectorPreview>().SetFirstNode(firstWireNodePreview);
            wireConnectorPreview.GetComponent<WireConnectorPreview>().SetSecondNode(secondWireNodePreview);
            state = State.SecondNode;
        }
        else if (state == State.SecondNode) {

            if (Input.GetMouseButtonDown(0) && isValid) {
                FinalisePlacement();
            }

            if (Input.GetMouseButtonDown(1)) {
                ResumeFirstNodePreview();
                Destroy(secondWireNodePreview);
                Destroy(wireConnectorPreview);
                state = State.FirstNode;
            }
        }
    }

    public void StartPlacingWires(WireItem wire) {
        currentWireType = wire;
        isPlacingWires = true;
        isValid = true;
        state = State.FirstNode;
        firstWireNodePreview = Instantiate(wireNodePreview);
    }

    public void StopPlacingWires() {
        isPlacingWires = false;
        DestroyAllPreviews();
    }

    private void CheckValidity() {
        isValid = true;

        //check length of wire
        if (wireConnectorPreview != null && (wireConnectorPreview.transform.localScale.y < minLength / 2 || wireConnectorPreview.transform.localScale.y > maxLength / 2)) {
            isValid = false;
            return;
        }

        if (firstWireNodePreview != null && secondWireNodePreview != null) {
            WireNodePreview wireNodeOne = firstWireNodePreview.GetComponent<WireNodePreview>();
            WireNodePreview wireNodeTwo = secondWireNodePreview.GetComponent<WireNodePreview>();

            //check that 2 snapped nodes arent connected
            if (wireNodeOne.isSnapped && wireNodeTwo.isSnapped) {
                WireNode snappedNodeOne = wireNodeOne.snappedNode.GetComponent<WireNode>();
                WireNode snappedNodeTwo = wireNodeTwo.snappedNode.GetComponent<WireNode>();
                if (snappedNodeOne.connectedNodes.Contains(snappedNodeTwo)) {
                    isValid = false;
                    return;
                }
            }

            //check that nodes arent on the same connector
            if (wireNodeOne.isSubdividing && wireNodeTwo.isSubdividing) {
                if (wireNodeOne.subdividingConnector == wireNodeTwo.subdividingConnector) {
                    isValid = false;
                    return;
                }
            }

            if (wireNodeOne.isSubdividing && wireNodeTwo.isSnapped) {
                WireNode snappedNode = wireNodeTwo.snappedNode.GetComponent<WireNode>();
                if (wireNodeOne.subdividingConnector.firstWireNode == snappedNode || wireNodeOne.subdividingConnector.secondWireNode == snappedNode) {
                    isValid = false;
                    return;
                }
            }

            if (wireNodeTwo.isSubdividing && wireNodeOne.isSnapped) {
                WireNode snappedNode = wireNodeOne.snappedNode.GetComponent<WireNode>();
                if (wireNodeTwo.subdividingConnector.firstWireNode == snappedNode || wireNodeTwo.subdividingConnector.secondWireNode == snappedNode) {
                    isValid = false;
                    return;
                }
            }
        }
    }

    private void UpdateValidity() {
        if (isValid) {
            Blueify(firstWireNodePreview);
            Blueify(secondWireNodePreview);
            Blueify(wireConnectorPreview);
        }
        else {
            Redify(firstWireNodePreview);
            Redify(secondWireNodePreview);
            Redify(wireConnectorPreview);
        }
    }

    public void PauseFirstNodePreview() {
        firstWireNodePreview.GetComponent<WireNodePreview>().setPause(true);
    }
    public void ResumeFirstNodePreview() {
        firstWireNodePreview.GetComponent<WireNodePreview>().setPause(false);
        firstWireNodePreview.GetComponent<WireNodePreview>().SetRaycastRange(secondWireNodePreview.GetComponent<WireNodePreview>().GetRaycastRange());
    }
    
    public void FinalisePlacement() {
        GetAndLinkNodes();
        InventoryController.instance.activeSlot.SubtractAmount(1);
        InventoryController.instance.hotbarContainer.SaveItems();

        StopPlacingWires();
        if (InventoryController.instance.activeSlot.item != null) {
            WireItem item = InventoryController.instance.activeSlot.item as WireItem;
            StartPlacingWires(item);

        }
    }

    public void GetAndLinkNodes() {
        GameObject firstWireNode = firstWireNodePreview.GetComponent<WireNodePreview>().GetOrCreateNodeGameObject();
        GameObject secondWireNode = secondWireNodePreview.GetComponent<WireNodePreview>().GetOrCreateNodeGameObject();
        CreateWireConnector(firstWireNode.GetComponent<WireNode>(), secondWireNode.GetComponent<WireNode>());
        WireNode firstWireNodeScript = firstWireNode.GetComponent<WireNode>();
        WireNode secondtWireNodeScript = secondWireNode.GetComponent<WireNode>();
        firstWireNodeScript.AddConnection(secondtWireNodeScript);
        secondtWireNodeScript.AddConnection(firstWireNodeScript);
        UpdateWireSystems(firstWireNodeScript, secondtWireNodeScript);
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

    public void DestroyAllPreviews() {
        Destroy(firstWireNodePreview);
        Destroy(secondWireNodePreview);
        Destroy(wireConnectorPreview);
    }

    private void UpdateWireSystems(WireNode nodeOne, WireNode nodeTwo) {

        if (nodeOne.attachedWireSystem == null && nodeTwo.attachedWireSystem == null) {
            WireSystem wireSystem = new WireSystem();
            wireSystem.AddWireNode(nodeOne);
            wireSystem.AddWireNode(nodeTwo);
            wireSystems.Add(wireSystem);
        }

        else if (nodeOne.attachedWireSystem == null && nodeTwo.attachedWireSystem != null) {
            nodeTwo.attachedWireSystem.AddWireNode(nodeOne);
        }

        else if (nodeOne.attachedWireSystem != null && nodeTwo.attachedWireSystem == null) {
            nodeOne.attachedWireSystem.AddWireNode(nodeTwo);
        }

        else if (nodeOne.attachedWireSystem != null && nodeTwo.attachedWireSystem != null) {
            if (nodeOne.attachedWireSystem != nodeTwo.attachedWireSystem) {
                WireSystem systemToMerge = nodeTwo.attachedWireSystem;
                nodeOne.attachedWireSystem.MergeWireSystem(systemToMerge);
                wireSystems.Remove(systemToMerge);
            }
        }
    }

    private void Blueify(GameObject model) {

        if (model == null) {
            return;
        }

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

    private void Redify(GameObject model) {

        if (model == null) {
            return;
        }

        if (model.GetComponent<Renderer>() != null) {
            Color baseColor = model.GetComponent<Renderer>().material.color;
            baseColor.r = 200f;
            baseColor.b = 0.35f;
            baseColor.g = 0.35f;
            model.GetComponent<Renderer>().material.color = baseColor;
        }

        foreach (Transform child in model.transform) {
            Redify(child.gameObject);
        }
    }
}

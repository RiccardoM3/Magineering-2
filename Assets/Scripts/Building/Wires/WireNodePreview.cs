using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireNodePreview : MonoBehaviour
{
    public GameObject wireNodePrefab;
    public bool isSnapped = false;
    public GameObject snappedNode;
    public bool isSubdividing = false;
    public WireConnector subdividingConnector;

    private Vector3 spawnPos;
    private bool paused = false;
    private float raycastRange = 8f;
    private float changeDistanceSpeed = 5f;


    // Start is called before the first frame update
    void Start() {
        Blueify(gameObject);
        GetPosition();
    }

    // Update is called once per frame
    void Update() {
        if (InventoryController.instance.isActive || paused) {
            return;
        }
        
        HandleInput();
        GetPosition();
    }

    public void setPause(bool value) {
        this.paused = value;
    }

    private void HandleInput() {
        
        if (Input.GetKey(KeyCode.E)) {
            raycastRange += Time.deltaTime * changeDistanceSpeed;
        }
        if (Input.GetKey(KeyCode.Q)) {
            raycastRange -= Time.deltaTime * changeDistanceSpeed;
        }
        raycastRange = Mathf.Clamp(raycastRange, 0.75f, 8);
    }

    public float GetRaycastRange() {
        return this.raycastRange;
    }

    public void SetRaycastRange(float range) {
        this.raycastRange = range;
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

    private void GetPosition() {

        RaycastHit hit;
        var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        int buildLayerMask = 1 << LayerMask.NameToLayer("Build");
        int terrainlayerMask = 1 << LayerMask.NameToLayer("Terrain");
        
        if (Physics.Raycast(ray, out hit, raycastRange, terrainlayerMask | buildLayerMask)) {

            //if raycast hit something,
            if (hit.transform.tag == "WireNode") {
                spawnPos = hit.transform.position;
                isSnapped = true;
                snappedNode = hit.transform.gameObject;
                isSubdividing = false;
                subdividingConnector = null;
            }
            else if (hit.transform.tag == "Wire") {
                spawnPos = hit.point - hit.normal * hit.transform.localScale.x / 2;
                isSnapped = false;
                snappedNode = null;
                isSubdividing = true;
                subdividingConnector = hit.transform.GetComponent<WireConnector>();
            }
            else {
                spawnPos = hit.point;
                isSnapped = false;
                snappedNode = null;
                isSubdividing = false;
                subdividingConnector = null;
            }
        } else {
            //raycast didn't hit
            spawnPos = Camera.main.transform.position + ray.direction * raycastRange;
            isSnapped = false;
            snappedNode = null;
            isSubdividing = false;
            subdividingConnector = null;
        }

        gameObject.transform.position = spawnPos;
    }

    public GameObject GetOrCreateNodeGameObject() {
        if (isSnapped) {
            return snappedNode;
        } else if (isSubdividing) {
            GameObject newNode = Instantiate(wireNodePrefab, this.transform.position, this.transform.rotation);
            subdividingConnector.SubdivideNodes(newNode.GetComponent<WireNode>());
            return newNode;
        } else {
            return Instantiate(wireNodePrefab, this.transform.position, this.transform.rotation);
        }
    }
}

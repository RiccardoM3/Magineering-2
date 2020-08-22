using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    public GameObject building;
    public bool canBeFreePlaced = true;
    public bool snapCanRotate;
    public List<float> snapRotations = new List<float>();
    public List<string> snapTags = new List<string>();

    private Vector3 initialOffset;
    private Vector3 spawnPos;
    private float maxPlacementRange = 20f;
    private float rotationSpeed = 50f;
    private float rotationOffset = 0f;
    private bool isSnapped = false;
    private bool isValid = true;
    private Quaternion snappedRotation;
    private float maxInclineAngle = 30; //max angle of incline;
    private List<Collider> collidingObjects = new List<Collider>();
    private GameObject activeSnapPoint;

    // Start is called before the first frame update
    void Start()
    {
        initialOffset = transform.position;
        Blueify(gameObject);
        GetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (InventoryController.instance.isActive) {
            return;
        }

        if (Input.GetKey(KeyCode.E)) {
            rotationOffset += Time.deltaTime * rotationSpeed;
        }
        if (Input.GetKey(KeyCode.Q)) {
            if (!isSnapped) {
                rotationOffset -= Time.deltaTime * rotationSpeed;
            } else if (snapCanRotate) {
                //TODO
            }
            
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            rotationOffset = 0;
        }

        GetPosition();
        GetRotation();
        

        if (Input.GetMouseButtonDown(0) && isValid)
        {
            FinalisePlacement();
        }
    }

    private void Blueify(GameObject model)
    {
        
        if (model.GetComponent<Renderer>() != null)
        {
            Color baseColor = model.GetComponent<Renderer>().material.color;
            baseColor.r = 0.35f;
            baseColor.b = 200;
            baseColor.g = 0.35f;
            model.GetComponent<Renderer>().material.color = baseColor;
        }

        foreach (Transform child in model.transform)
        {
            Blueify(child.gameObject);
        }
    }

    private void Redify(GameObject model) {
        if (model.GetComponent<Renderer>() != null) {
            Color baseColor = model.GetComponent<Renderer>().material.color;
            baseColor.r = 200;
            baseColor.b = 0.35f;
            baseColor.g = 0.35f;
            model.GetComponent<Renderer>().material.color = baseColor;
        }

        foreach (Transform child in model.transform) {
            Redify(child.gameObject);
        }
    }

    public void FinalisePlacement()
    {
        Instantiate(building, transform.position, transform.rotation);
        InventoryController.instance.activeSlot.SubtractAmount(1);
        InventoryController.instance.hotbarContainer.saveItems();

        if (isSnapped) {
           activeSnapPoint.SetActive(false);
        }

        Destroy(this.gameObject);
    }

    //Returns the distance to fire a raycast such that the horizontal distance will equal maxPlacementRange
    public float CalcRaycastDistance() {
        float cameraInclineAngle = Camera.main.transform.rotation.eulerAngles.x * Mathf.PI / 180;
        return maxPlacementRange / Mathf.Cos(cameraInclineAngle);
    }

    private void GetPosition() {

        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        float raycastRange = CalcRaycastDistance();

        isSnapped = false;
        isValid = true;

        //fire raycast in snap points layer
        if (!Input.GetKey(KeyCode.LeftControl) || !canBeFreePlaced) {
            int snapPointlayerMask = 1 << LayerMask.NameToLayer("SnapPoints");
            RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, snapPointlayerMask);
            if (hits.Length > 0) {          //if the raycast hit something
                int index = 0;
                bool found = false;
                while (index < hits.Length && !found) {       //loop through all intersection hits

                    hit = hits[index];
                    for (int i = 0; i < snapTags.Count; i++) {          //check if the hit was a valid snaptag
                        if (hit.transform.tag == snapTags[i]) {
                            isSnapped = true;
                            spawnPos = hit.transform.parent.Find("SpawnPoint").transform.position + initialOffset;
                            snappedRotation = hit.transform.parent.Find("SpawnPoint").transform.rotation;
                            activeSnapPoint = hit.transform.parent.gameObject;
                            UpdateValidity(hit);
                            found = true;   //break outter loop when first valid hit is found
                        }
                    }

                    index++;
                }
            }
        }

        if (!isSnapped) {
            //fire raycast in camera direction
            int buildLayerMask = 1 << LayerMask.NameToLayer("Build");
            int terrainLayerMask = 1 << LayerMask.NameToLayer("Terrain");

            if (Physics.Raycast(ray, out hit, raycastRange, buildLayerMask | terrainLayerMask)) {
                spawnPos = hit.point + initialOffset;
                UpdateValidity(hit);
            }
            else {
                //did not hit anything. fire a new raycast down from the endposition, since this is the furthest position you can place at
                Vector3 rayEndPoint = Camera.main.transform.position + ray.direction * raycastRange;

                if (Physics.Raycast(rayEndPoint, Vector3.down, out hit, buildLayerMask | terrainLayerMask)) {
                    spawnPos = hit.point + initialOffset;
                    UpdateValidity(hit);
                }
            }
            if (!canBeFreePlaced) {
                isValid = false;
                UpdateColour();
            }
        }

        gameObject.transform.position = spawnPos;
    }

    public void GetRotation() {
        if (isSnapped) {
            transform.rotation = snappedRotation;
        }
        else {
            Vector3 initialRotation = transform.eulerAngles;
            transform.LookAt(new Vector3(InventoryController.instance.transform.position.x, transform.position.y, InventoryController.instance.transform.position.z));
            transform.rotation = Quaternion.Euler(initialRotation.x, transform.eulerAngles.y + rotationOffset, initialRotation.z);
        }
    }

    public void UpdateValidity(RaycastHit hit) {
        
        if (isValid && !isSnapped) {
            if (hit.normal.normalized.y < Mathf.Sin((90 - maxInclineAngle) * Mathf.PI / 180)) {
                isValid = false;
            }

            if (collidingObjects.Count > 0) {
                isValid = false;
            }
        }

        UpdateColour();
    }

    public void UpdateColour() {
        if (isValid) {
            Blueify(this.gameObject);
        }
        else {
            Redify(this.gameObject);
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Build")) {
            collidingObjects.Add(other);
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Build")) {
            collidingObjects.Remove(other);
        }
    }
}

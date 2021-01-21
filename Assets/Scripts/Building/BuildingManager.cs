using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    #region Singleton
    public static BuildingManager instance;

    void Awake() {
        if (instance != null) {
            Debug.LogWarning("More than one instance of WireManager found!");
            return;
        }
        instance = this;
    }
    #endregion

    private GameObject buildingPreview;
    private bool isBuilding = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {

        if (buildingPreview != null) {
            if (isBuilding && Input.GetMouseButtonDown(1)) {
                isBuilding = false;
                buildingPreview.SetActive(false);
            }
            else if (!isBuilding && Input.GetMouseButtonDown(0)) {
                isBuilding = true;
                buildingPreview.SetActive(true);
            }
        }

    }

    public void StartBuilding(GameObject preview) {
        isBuilding = true;
        buildingPreview = Instantiate(preview);
    }

    public void StopBuilding() {
        isBuilding = false;
        if (buildingPreview != null) {
            Destroy(buildingPreview);
        }
    }

}

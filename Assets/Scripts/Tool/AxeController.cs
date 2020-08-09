using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AxeController : MonoBehaviour
{
    // Terrains, Hit
    private Terrain terrain;            // Derived from hit...GetComponent<Terrain>
    private RaycastHit hit;                // For hit. methods
    private Animator animator;

    // Tree, GameManager
    private ResourceManager rMgr;    // Resource manager script
    public float respawnTimer;            // Duration of terrain tree respawn timer
    public float range = 3f;
    private bool isSwinging;
    public RuntimeAnimatorController treeFallAnimator;

    void Start()
    {

        if (respawnTimer <= 0)
        {
            Debug.Log("respawnTimer unset in Inspector, using quick test value: 15");
            respawnTimer = 15;
        }

        animator = GetComponent<Animator>();
        terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        rMgr = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ResourceManager>();
        isSwinging = false;
    }


    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !isSwinging)
        {
            animator.Play("ToolSwing");
            isSwinging = true;
        }
    }

    void ToolSwingCallback()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, range))
        {
            // Did we click a tree?
            if (hit.collider.gameObject.tag == "Tree")
            {
                GameObject tree = hit.collider.gameObject;

                LargeTree treeComponent = tree.GetComponent<LargeTree>();
                if (treeComponent.breakTree())
                {
                    HarvestWood(treeComponent.treeId, tree.transform.position, tree);
                }
            }
        }
    }

    void ToolSwingFinish()
    {
        isSwinging = false;
    }

    private bool CheckRecentUsage(int _treeIndex)
    {
        bool beenUsed = false;

        for (int cnt = 0; cnt < rMgr.managedTrees.Count; cnt++)
        {
            if (rMgr.managedTrees[cnt].treeIndex == _treeIndex)
            {
                Debug.Log("Tree has been used recently");
                beenUsed = true;
            }
        }

        return beenUsed;
    }


    private void HarvestWood(int treeIndex, Vector3 treePos, GameObject collider)
    {
        if (!CheckRecentUsage(treeIndex))
        {
            //Spawn fallen tree
            int treePrototypeIndex = terrain.terrainData.GetTreeInstance(treeIndex).prototypeIndex;
            GameObject fallenTree = Instantiate(terrain.terrainData.treePrototypes[treePrototypeIndex].prefab, treePos, Quaternion.identity);
            Animator animator = fallenTree.AddComponent<Animator>();
            animator.runtimeAnimatorController = treeFallAnimator;
            Destroy(fallenTree, 1.84f);

            // Add this terrain tree to our resource manager
            var trees = new List<TreeInstance>(terrain.terrainData.treeInstances);
            TreeInstance treeBackup = trees[treeIndex];
            rMgr.AddTerrainTree(treeIndex, Time.time + respawnTimer, treeBackup, collider);
            collider.SetActive(false);

            //Remove the set the tree the terraindata to an empty class
            trees[treeIndex] = new TreeInstance();
            terrain.terrainData.treeInstances = trees.ToArray();
        }
    }
}
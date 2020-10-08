using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{

    public GameObject treeCollider;

    // Define List component
    public class QM_Tree
    {
        public int treeIndex { get; set; }
        public float respawnTime { get; set; }
        public TreeInstance treeBackup { get; set; }
        public GameObject collider { get; set; }

        // Constructor
        public QM_Tree(int _treeINDEX, float _respawnTime, TreeInstance _treeBackup, GameObject _collider)
        {
            treeIndex = _treeINDEX;
            respawnTime = _respawnTime;
            treeBackup = _treeBackup;
            collider = _collider;
        }

    }

    // Tree Harvest script access
    public List<QM_Tree> managedTrees = new List<QM_Tree>();

    void Start()
    {
        // Scan for tree to respawn every 5 seconds
        InvokeRepeating("RespawnTrees", 5, 5);

        //Uncomment this command to permanently remove all trees
        //terrain.terrainData.treeInstances = new List<TreeInstance>().ToArray();

        //Initialise all tree colliders
        Terrain terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        int treeCount = terrain.terrainData.treeInstances.Length;
        for (int cnt = 0; cnt < treeCount; cnt++)
        {
            Vector3 thisTreePos = Vector3.Scale(terrain.terrainData.treeInstances[cnt].position, terrain.terrainData.size) + terrain.transform.position;

            Instantiate(treeCollider, GameObject.Find("Trees").transform);
            LargeTree treeScript = treeCollider.GetComponent<LargeTree>();
            treeScript.treeId = cnt;
            treeCollider.transform.position = thisTreePos;
        }
    }


    private void RespawnTrees()
    {
        if (managedTrees.Count == 0)
            return;

        // Loop through all managed trees
        for (int cnt = 0; cnt < managedTrees.Count; cnt++)
        {
            //Check if the inidividual tree is ready to respawn
            if (managedTrees[cnt].respawnTime < Time.time)
            {
                //Get the terrain's trees:
                Terrain terrain = (Terrain)GameObject.FindObjectOfType(typeof(Terrain));
                var trees = new List<TreeInstance>(terrain.terrainData.treeInstances);

                //Reload the old tree into the tree index
                trees[managedTrees[cnt].treeIndex] = managedTrees[cnt].treeBackup;
                terrain.terrainData.treeInstances = trees.ToArray();
                managedTrees[cnt].collider.GetComponent<LargeTree>().reset();
                managedTrees[cnt].collider.SetActive(true);

                //Remove the tree from managed trees
                managedTrees.RemoveAt(cnt);
                terrain.Flush();    //Refresh the terrain just in case
                return;
            }

        }
    }


    public void AddTerrainTree(int _treeIDX, float _respawnTime, TreeInstance _treeBackup, GameObject _collider)
    {
        managedTrees.Add(new QM_Tree(_treeIDX, _respawnTime, _treeBackup, _collider));
    }
}
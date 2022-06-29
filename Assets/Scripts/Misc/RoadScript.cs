using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoadScript : MonoBehaviour
{
    [SerializeField] List<GameObject> pathBlocks;
    public bool RoadModified = false;

    public void ClearPath(int index)
    {
        Destroy(pathBlocks[index]);
    }

    static bool what = false;

    public void BakeNavMesh()
    {
        if (what) return;
        what = true;
        pathBlocks[0].GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoadScript : MonoBehaviour
{
    [SerializeField] List<GameObject> pathBlocks;
    [SerializeField] GameObject streetLamp, lamp;
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

    public void DeleteLamp()
    {
        Destroy(streetLamp);
    }

    private void Update()
    {
        if (lamp == null) return;

        if (480 < GameController.instance.CurrentTime && GameController.instance.CurrentTime < 1200 && lamp.activeSelf)
            lamp.SetActive(false);
        else if ((480 >= GameController.instance.CurrentTime || GameController.instance.CurrentTime >= 1200) && !lamp.activeSelf)
            lamp.SetActive(true);
    }
}

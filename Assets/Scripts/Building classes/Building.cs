using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimulationNS
{
    public abstract class Building : MonoBehaviour
    {
        [SerializeField]
        string name;
        [SerializeField]
        Transform BuildingCenter;

        [SerializeField]
        Vector2Int gridDimensions;


        public Vector3 GetBuildingCenter() { return BuildingCenter.position; }
        public Vector2Int GetGridDimensions() { return gridDimensions; }

    }
}
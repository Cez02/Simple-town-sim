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


        public Vector3 GetBuildingCenter() { return BuildingCenter.position; }


    }
}
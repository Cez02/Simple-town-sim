using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimulationNS {
    public class SchoolBuilding : WorkingBuilding
{
        new public static List<SchoolBuilding> buildings = new List<SchoolBuilding>();

        // Start is called before the first frame update
        void Start()
        {
            buildings.Add(this);
        }

    }
}
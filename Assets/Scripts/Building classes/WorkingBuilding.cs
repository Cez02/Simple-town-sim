using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimulationNS;
using System;

namespace SimulationNS
{
    public class WorkingBuilding : Building
    {

        [System.Serializable]
        public struct Shift
        {
            public Vector2Int shiftStart, shiftEnd;
        }

        [SerializeField]
        List<Shift> ShiftTimes;

        public Shift GetRandomShift()
        {
            if (ShiftTimes.Count == 0) throw new UninitializedSimulationStructuresException("Working building has no shifts.");

            System.Random shiftIndex = new System.Random();
            return ShiftTimes[shiftIndex.Next(ShiftTimes.Count)];
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
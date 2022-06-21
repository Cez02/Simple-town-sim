using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimulationNS;

public class Child : Person
{
    [SerializeField]
    WorkingBuilding BuildingSchool;

    void InitializeChildEQ()
    {
        if (BuildingSchool == null) throw new UninitializedSimulationStructuresException(
                                               "Uninitialized school building in child.");

        //Here we initialize the child's school day

        /*
        eventQueue = new Queue<MoveEvent>();
        var SchoolShift = BuildingSchool.GetShift(0);

        eventQueue.Enqueue(
            new MoveEvent(
                MoveEvent.midnightTime + new TimeSpan(0, 0, 0),
                MoveEvent.midnightTime + new TimeSpan(7, 0, 0),
         */      

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

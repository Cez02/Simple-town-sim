using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimulationNS;

public class Child : Person
{
    [SerializeField]
    WorkingBuilding BuildingSchool;

    override public void PrepareEventQueue()
    {
        if (BuildingSchool == null) throw new UninitializedSimulationStructuresException(
                                               "Uninitialized school building in child.");

        //Here we initialize the child's school day
        eventQueue = new Queue<MoveEvent>();
        //var SchoolShift = BuildingSchool.GetShift(0);

        eventQueue.Enqueue(
            new MoveEvent(
                GameController.TimeToSeconds(0, 0),
                GameController.TimeToSeconds(7, 0),
                dwelling));

        eventQueue.Enqueue(
            new MoveEvent(
                GameController.TimeToSeconds(8, 0),
                GameController.TimeToSeconds(15, 20),
                BuildingSchool));

        eventQueue.Enqueue(
            new MoveEvent(
                GameController.TimeToSeconds(15, 20),
                GameController.TimeToSeconds(23, 59),
                dwelling));




    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
            

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

    }
}

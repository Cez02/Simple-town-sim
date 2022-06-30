using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimulationNS;

public class Child : Person
{
    [SerializeField]
    SchoolBuilding BuildingSchool;

    override public void PrepareEventQueue()
    {
        if (customEventQueueInitialized)
        {
            CopyCustomQueue();
            return;
        }


        if (BuildingSchool == null) throw new UninitializedSimulationStructuresException(
                                               "Uninitialized school building in child.");

        //Here we initialize the child's school day
        eventQueue = new Queue<MoveEvent>();
        var schoolShift = BuildingSchool.GetRandomShift();

        eventQueue.Enqueue(
            new MoveEvent(
                GameController.TimeToSeconds(0, 0),
                GameController.TimeToSeconds(schoolShift.shiftStart),
                dwelling));

   

        eventQueue.Enqueue(
            new MoveEvent(
                GameController.TimeToSeconds(schoolShift.shiftStart),
                GameController.TimeToSeconds(schoolShift.shiftEnd),
                BuildingSchool));

        eventQueue.Enqueue(
            new MoveEvent(
                GameController.TimeToSeconds(schoolShift.shiftEnd),
                GameController.TimeToSeconds(23, 59),
                dwelling));
        

    }

    public void PrepareChild(string name_, int age_, DwellingBuilding house, SchoolBuilding whereSchool_)
    {
        name = name_;
        age = age_;
        BuildingSchool = whereSchool_;
        dwelling = house;
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

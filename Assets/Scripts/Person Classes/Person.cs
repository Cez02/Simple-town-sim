using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SimulationNS
{
    [System.Serializable]
    public class MoveEvent{

        public static DateTime midnightTime = new DateTime(2022, 1, 1, 0, 0, 0);

        public DateTime expectedArrival { get; private set; }
        public DateTime expectedLeave { get; private set; }
        public Building destination { get; private set; }

        public MoveEvent(DateTime _expectedArrival, DateTime _expectedLeave, Building destination_)
        {
            expectedArrival = _expectedArrival;
            expectedLeave = _expectedLeave;
            destination = destination_;

        }
    }

    public abstract class Building : MonoBehaviour
    {
        [SerializeField]
        string name;
        List<Vector3> EntrancePositions;
    }

    public abstract class Person : MonoBehaviour
    {
        [SerializeField]
        protected string name;
        [SerializeField]
        protected int age;

        

        protected Queue<MoveEvent> eventQueue;


    }
}
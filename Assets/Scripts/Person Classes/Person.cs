using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

namespace SimulationNS
{
    [System.Serializable]
    public class MoveEvent{

        public float expectedStart;
        public float expectedEnd;
        public Building destination;

        public MoveEvent(float _expectedStart, float _expectedEnd, Building destination_)
        {
            expectedStart = _expectedStart;
            expectedEnd = _expectedEnd;
            destination = destination_;

        }
    }

    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Person : MonoBehaviour
    {

        //=================================
        // Variables
        //=================================

        [Header("Custom event queue variables")]
        public bool customEventQueueInitialized = false;

        public List<MoveEvent> customEventQueue = new List<MoveEvent>();

        [Header("Standard variables")]
        [SerializeField]
        protected string name;
        [SerializeField]
        protected int age;
        [SerializeField]
        protected DwellingBuilding dwelling;


        protected Queue<MoveEvent> eventQueue = new Queue<MoveEvent>();



        protected MoveEvent currentEvent;

        // Path finding variables

        NavMeshAgent thisNMA;

        //=================================
        // Event methods
        //=================================

        //Prepares new event queue for given simulation day
        public abstract void PrepareEventQueue();

        protected void CopyCustomQueue()
        {

            if (customEventQueue == null || customEventQueue.Count == 0)
                throw new UninitializedSimulationStructuresException("Custom queue is empty.");

            foreach (var x in customEventQueue)
            {
                eventQueue.Enqueue(x);
            }
        }

        //helper method
        public static float distance2D(Vector3 a, Vector3 b)
        {
            return (a.x - b.x) * (a.x - b.x) + (a.z - b.z) * (a.z - b.z);
        }

        public bool Moving = true;
        public float dis = 0f;
        public Vector3 first, second;
        //if person is in target building, then do nothing
        //otherwise we must go to said building
        public void HandleEvent()
        {
            if (currentEvent != null)
            {
                first = transform.position;
                second = currentEvent.destination.GetBuildingCenter();
                dis = distance2D(transform.position, currentEvent.destination.GetBuildingCenter());
            }
            if (currentEvent == null || distance2D(transform.position, currentEvent.destination.GetBuildingCenter()) < 0.2f)
            {
                Moving = false;
                thisNMA.isStopped = true;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                return;
            }

            if (thisNMA.destination != currentEvent.destination.transform.position)
            {
                Moving = true;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                thisNMA.isStopped = false;
                thisNMA.SetDestination(currentEvent.destination.GetBuildingCenter());
            }
        } 

        //updates the current event executed by person
        public void UpdateEvents()
        {
            if(currentEvent != null)
            {
                if (currentEvent.expectedEnd <= GameController.instance.CurrentTime)
                    currentEvent = null;
                else
                    return;
            }

            //skip events that we perhaps missed
            while (eventQueue.Count > 0 && eventQueue.Peek().expectedEnd <= GameController.instance.CurrentTime)
                eventQueue.Dequeue();

            if (eventQueue.Count == 0) return;

            var nextEvent = eventQueue.Peek();
            if(nextEvent.expectedStart <= GameController.instance.CurrentTime)
            {
                currentEvent = nextEvent;
            }
        }

        public void ResetPersonPosition()
        {
            Debug.Log("Data:");
            Debug.Log(transform.position);
            transform.position = dwelling.GetBuildingCenter();
            Debug.Log(transform.position);
        }

        public virtual void Start()
        {
            GameController.PrepareSimulation += ResetPersonPosition;
            thisNMA = GetComponent<NavMeshAgent>();
            GameController.NewDay += PrepareEventQueue;
            GameController.HandleSecond += UpdateEvents;
            GameController.HandleSecond += HandleEvent;
        }

        public virtual void Update()
        {
            if (Time.timeScale > 1.0f)
            {
                CheckSteeringTargetPosition();
            }

        }

        protected float _distanceStearingTarget;

        protected void CheckSteeringTargetPosition()
        {
            float distanceST = Vector3.Distance(thisNMA.transform.position, thisNMA.steeringTarget);
            if (distanceST <= 0.5f) //distance to next edge on nav mesh
            {
                if (_distanceStearingTarget < distanceST)
                {
                    thisNMA.transform.position = thisNMA.steeringTarget;
                }
                else
                {
                    _distanceStearingTarget = distanceST;
                }
            }
        }


        private void OnApplicationQuit()
        {
            GameController.NewDay -= PrepareEventQueue;
            GameController.HandleSecond -= UpdateEvents;
            GameController.HandleSecond -= HandleEvent;
            GameController.PrepareSimulation -= ResetPersonPosition;
        }
    }
}
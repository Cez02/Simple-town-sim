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


        protected Queue<MoveEvent> eventQueue;



        protected MoveEvent currentEvent;

        // Path finding variables

        NavMeshAgent thisNMA;

        bool moving = false;


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
        float distance2D(Vector3 a, Vector3 b)
        {
            return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
        }

        //if person is in target building, then do nothing
        //otherwise we must go to said building
        public void HandleEvent()
        {
            if (currentEvent == null || distance2D(transform.position, currentEvent.destination.GetBuildingCenter()) < 0.2f)
            {
                thisNMA.isStopped = true;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                return;
            }

            if (thisNMA.destination != currentEvent.destination.transform.position)
            {
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



        public virtual void Start()
        {
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
            if (distanceST <= 0.1f) //distance to next edge on nav mesh
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
        }
    }
}
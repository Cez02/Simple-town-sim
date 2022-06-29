using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimulationNS
{
    public class EmployedAdult : Adult
    {
        [SerializeField]
        protected WorkingBuilding placeOfEmployment;

        public override void PrepareEventQueue()
        {
            if (customEventQueueInitialized)
            {
                CopyCustomQueue();
                return;
            }


            if (placeOfEmployment == null) throw new UninitializedSimulationStructuresException(
                                                   "Uninitialized place of employment in employed adult.");

            eventQueue = new Queue<MoveEvent>();
            var workShift = placeOfEmployment.GetRandomShift();

            eventQueue.Enqueue(
                new MoveEvent(
                    GameController.TimeToSeconds(0, 0),
                    GameController.TimeToSeconds(workShift.shiftStart),
                    dwelling));

            eventQueue.Enqueue(
                new MoveEvent(
                GameController.TimeToSeconds(workShift.shiftStart),
                GameController.TimeToSeconds(workShift.shiftEnd),
                    placeOfEmployment));

            eventQueue.Enqueue(
                new MoveEvent(
                    GameController.TimeToSeconds(workShift.shiftEnd),
                    GameController.TimeToSeconds(23, 59),
                    dwelling));

        }

        public void PrepareAdult(string name_, int age_, DwellingBuilding house, WorkingBuilding whereWork_)
        {
            name = name_;
            age = age_;
            placeOfEmployment = whereWork_;
            dwelling = house;
        }

        public void Start()
        {
            base.Start();
        }

        public void Update()
        {
            base.Update();
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimulationNS
{
    public class Adult : Person
    {
        [SerializeField]
        protected List<Child> Children;

        public override void PrepareEventQueue()
        {
            if (customEventQueueInitialized)
            {
                CopyCustomQueue();
                return;
            }

            throw new UninitializedSimulationStructuresException("Adult entity with no event queue.");

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
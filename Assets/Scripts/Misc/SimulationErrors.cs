using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimulationNS
{
    public class GenericSimulationException : System.Exception
    {
        public GenericSimulationException() { }

        public GenericSimulationException(string message) : base(message) { }
    }

    public class UninitializedSimulationStructuresException : GenericSimulationException
    {
        public UninitializedSimulationStructuresException() { }

        public UninitializedSimulationStructuresException(string message) : base(message) { }
    }
}
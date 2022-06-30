using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

namespace SimulationNS
{
    public class GeneratePeople : MonoBehaviour
    {

        [Header("Settings")]
        [SerializeField] int numberOfPeople;
        [SerializeField] Transform whereToSpawn;

        List<Transform> People = new List<Transform>();

        [Header("Assets")]
        [SerializeField] Transform EmployedAdult;
        [SerializeField] Transform Child;

        void GenerateChild()
        {
            var x = Instantiate(Child);
            x.position = whereToSpawn.position;

            //prepare position

            NavMeshHit hit;
            if (!NavMesh.SamplePosition(x.position, out hit, 1.0f, NavMesh.AllAreas))
            {
                Debug.LogError("Navmesh sample position not found");
            }

            x.position = hit.position;

            x.GetComponent<NavMeshAgent>().enabled = true;

            var personData = x.GetComponent<Child>();

            SchoolBuilding dest = SchoolBuilding.buildings[0];
            for (int i = 1; i < SchoolBuilding.buildings.Count; i++)
            {
                if (Vector3.Distance(transform.position, dest.transform.position) > Vector3.Distance(transform.position, WorkingBuilding.buildings[i].transform.position))
                    dest = SchoolBuilding.buildings[i];
            }

            string name = GameController.instance.names[GameController.RandomNumberGenerator.Next()% GameController.instance.names.Length];
            personData.PrepareChild(name, 20 + GameController.RandomNumberGenerator.Next()%70, GetComponent<DwellingBuilding>(), dest);
            personData.PrepareEventQueue();
            People.Add(x);
        }

        void GeneratePerson()
        {
            var x = Instantiate(EmployedAdult);
            x.position = whereToSpawn.position;

            //prepare position

            NavMeshHit hit;
            if(!NavMesh.SamplePosition(x.position, out hit, 1.0f, NavMesh.AllAreas)){
                Debug.LogError("Navmesh sample position not found");
            }

            x.position = hit.position;

            x.GetComponent<NavMeshAgent>().enabled = true;

            var personData = x.GetComponent<EmployedAdult>();

            WorkingBuilding dest = WorkingBuilding.buildings[0];
            for(int i = 1; i<WorkingBuilding.buildings.Count; i++)
            {
                if (Vector3.Distance(transform.position, dest.transform.position) > Vector3.Distance(transform.position, WorkingBuilding.buildings[i].transform.position))
                    dest = WorkingBuilding.buildings[i];
            }


            string name = GameController.instance.names[GameController.RandomNumberGenerator.Next() % GameController.instance.names.Length];
            personData.PrepareAdult(name, 20 + GameController.RandomNumberGenerator.Next() % 70, GetComponent<DwellingBuilding>(), dest);
            personData.PrepareEventQueue();
            People.Add(x);

        }

        public void GenPeople()
        {
            for (int i = 0; i < numberOfPeople; i++)
            {
                if (GameController.RandomNumberGenerator.Next() % 3 <= 1)
                    GeneratePerson();
                else
                    GenerateChild();
            }
        }


        private void Start()
        {

        }

        private void OnApplicationQuit()
        {
            //GameController.PrepareSimulation -= GenPeople;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
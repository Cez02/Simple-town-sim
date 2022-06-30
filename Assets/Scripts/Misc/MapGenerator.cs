using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using TMPro;
using UnityEngine.AI;

namespace SimulationNS
{
    public class MapGenerator : MonoBehaviour
    {
        public static MapGenerator Instance;

        //===========================
        // Map generation variables
        //===========================

        [Header("Map generation settings")]
        [SerializeField] Vector2Int mapSize;
        [SerializeField] string seed;
        [SerializeField] int maxPopulation;
        private int seedValue;

        const int MinimumDistanceBetweenKeyBuildings = 3;



        //===========================
        // Map generation assets
        //===========================

        [Header("Map generation assets")]
        [SerializeField] Transform roadFragmentPrefab;
        [SerializeField] Transform grassFragmentPrefab, buildingBasePrefab;
        [SerializeField] List<BuildingData> Buildings;


        //===========================
        // Map generation data
        //===========================

        // position and union-find index
        List<KeyValuePair<Vector2Int, int>> keyBuildings = new List<KeyValuePair<Vector2Int, int>>();
        List<Vector2Int> schoolCoords = new List<Vector2Int>();
        List<Vector2Int> dwellingBuildings = new List<Vector2Int>();
        List<DwellingBuilding> dwellingBuildingsData = new List<DwellingBuilding>();
        List<int> groups = new List<int>();

        public enum BuildingType
        {
            House,
            School,
            Skyscraper
        }

        [System.Serializable]
        class BuildingData
        {
            public Transform buildingObject;
            public BuildingType buildingType;

            public BuildingData(BuildingType bt)
            {
                buildingType = bt;
            }
        }

        List<List<int>> roadMap = new List<List<int>>();
        List<List<Transform>> roadMapPrefabs = new List<List<Transform>>();
        int ssCounter = 2;

        List<List<bool>> visitedMap = new List<List<bool>>();

        System.Random ran;


        //===========================
        // Map generation methods
        //===========================

        private int StringSeedToInt(string s)
        {
            int MOD = 1000000007;
            int seedConstant = 0;
            long res = 0;

            foreach(var c in s)
            {
                res += (int)c;
                res %= MOD;
            }

            return (int)res;
        }


        // union find functions

        int Find(int x)
        {
            if (groups[x] == x) return x;
            else
            {
                groups[x] = Find(groups[x]);
                return groups[x];
            }
        }

        // building placement methods

        bool PositionFarAway(Vector2Int pos) 
        { 
            foreach(var k in keyBuildings)
            {
                int dis = Mathf.Abs(pos.x - k.Key.x) + Mathf.Abs(pos.y - k.Key.y);

                if (dis < MinimumDistanceBetweenKeyBuildings) return false;
            }
            return true;
        }

        void PlaceBuilding()
        {
            //walk with bfs until far enough

            Vector2Int currentPos = new Vector2Int(ran.Next() % mapSize.x, ran.Next() % mapSize.y);

            while (!PositionFarAway(currentPos))
            {
                currentPos = new Vector2Int(ran.Next() % mapSize.x, ran.Next() % mapSize.y);
            }

            keyBuildings.Add(new KeyValuePair<Vector2Int, int>(currentPos, ssCounter));
            groups.Add(ssCounter);

            for(int i = Mathf.Max(0, currentPos.y - 1); i < Mathf.Min(mapSize.y, currentPos.y + 2); i++)
            {
                for (int j = Mathf.Max(0, currentPos.x - 1); j < Mathf.Min(mapSize.x, currentPos.x + 2); j++)
                {
                    if (i == currentPos.y && j == currentPos.x)
                    {
                        roadMap[i][j] = -2;
                        continue;
                    }

                    roadMap[i][j] = ssCounter;
                }
            }
            ssCounter++;
        }


        void PlaceSchool()
        {

            Vector2Int currentPos = new Vector2Int(ran.Next() % mapSize.x, ran.Next() % mapSize.y);

            while (!PositionFarAway(currentPos))
            {
                currentPos = new Vector2Int(ran.Next() % mapSize.x, ran.Next() % mapSize.y);
            }

            keyBuildings.Add(new KeyValuePair<Vector2Int, int>(currentPos, ssCounter));
            groups.Add(ssCounter);

            for (int i = Mathf.Max(0, currentPos.y - 1); i < Mathf.Min(mapSize.y, currentPos.y + 2); i++)
            {
                for (int j = Mathf.Max(0, currentPos.x - 1); j < Mathf.Min(mapSize.x, currentPos.x + 2); j++)
                {
                    if (i == currentPos.y && j == currentPos.x)
                    {
                        roadMap[i][j] = -2;
                        continue;
                    }

                    roadMap[i][j] = ssCounter;
                }
            }
            ssCounter++;
        }

        int simpleDis(Vector2Int a, Vector2Int b) { return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);  }

        int CountNeighbouringRoads(Vector2Int pos)
        {
            int roadCount = 0;
            int i = pos.y;
            int j = pos.x;

            if (i != 0)
                if (roadMap[i - 1][j] > 0)
                    roadCount++;

            if (i != mapSize.y - 1)
                if (roadMap[i + 1][j] > 0)
                    roadCount++;

            if (j != 0)
                if (roadMap[i][j - 1] > 0)
                    roadCount++;

            if (j != mapSize.x - 1)
                if (roadMap[i][j + 1] > 0)
                    roadCount++;

            return roadCount;
        }

        void JoinBuildingsByRoad(Vector2Int firstBuilding, Vector2Int secondBuilding)
        {
            Vector2Int currentPos = firstBuilding;

            int firstBuildingIndex = 0;
            foreach(var x in keyBuildings)
            {
                if (x.Key == firstBuilding) firstBuildingIndex = x.Value;
            }

            while(currentPos != secondBuilding)
            {
                if (roadMap[currentPos.y][currentPos.x] == 0)
                    roadMap[currentPos.y][currentPos.x] = 1;

                //search for closer paths with roads
                int dis;
                Vector2Int PotentialWay = new Vector2Int(currentPos.x, currentPos.y);
                Vector2Int bestPath = new Vector2Int(-1, -1);

                int toBeat = simpleDis(currentPos, secondBuilding);

                #region Look for way with roads
                PotentialWay.x++;
                if(PotentialWay.x != mapSize.x && roadMap[PotentialWay.y][PotentialWay.x] != 0 && simpleDis(PotentialWay, secondBuilding) < toBeat)
                {
                    bestPath = PotentialWay;
                }

                PotentialWay.x -= 2;
                if (PotentialWay.x != -1 && roadMap[PotentialWay.y][PotentialWay.x] != 0 && simpleDis(PotentialWay, secondBuilding) < toBeat)
                {
                    bestPath = PotentialWay;
                }

                PotentialWay.x++;
                PotentialWay.y++;
                if (PotentialWay.y != mapSize.y && roadMap[PotentialWay.y][PotentialWay.x] != 0 && simpleDis(PotentialWay, secondBuilding) < toBeat)
                {
                    bestPath = PotentialWay;
                }

                PotentialWay.y -= 2;
                if (PotentialWay.y != -1 && roadMap[PotentialWay.y][PotentialWay.x] != 0 && simpleDis(PotentialWay, secondBuilding) < toBeat)
                {
                    bestPath = PotentialWay;
                }

                //we found a path with a road
                if(bestPath.x != -1)
                {
                    if(roadMap[bestPath.y][bestPath.x] == 0) roadMap[bestPath.y][bestPath.x] = roadMap[firstBuilding.y][firstBuilding.x];
                    currentPos = bestPath;
                    continue;
                }

                #endregion

                #region Look for way without roads
                //look for path with no road
                PotentialWay.x = currentPos.x;
                PotentialWay.y = currentPos.y;

                PotentialWay.x++;
                if (PotentialWay.x != mapSize.x && simpleDis(PotentialWay, secondBuilding) < toBeat)
                {
                    bestPath = PotentialWay;
                }

                PotentialWay.x -= 2;
                if (PotentialWay.x != -1 && simpleDis(PotentialWay, secondBuilding) < toBeat)
                {
                    bestPath = PotentialWay;
                }

                PotentialWay.x++;
                PotentialWay.y++;
                if (PotentialWay.y != mapSize.y && simpleDis(PotentialWay, secondBuilding) < toBeat)
                {
                    bestPath = PotentialWay;
                }

                PotentialWay.y -= 2;
                if (PotentialWay.y != -1 && simpleDis(PotentialWay, secondBuilding) < toBeat)
                {
                    bestPath = PotentialWay;
                }

                roadMap[bestPath.y][bestPath.x] = firstBuildingIndex;
                currentPos = bestPath;

                #endregion
            }



        }

        void CleanUpRoads(Vector2Int startingPos, int distance)
        {
            if (startingPos.x == -1 || startingPos.x == mapSize.x) return;
            if (startingPos.y == -1 || startingPos.y == mapSize.y) return;
            if (roadMap[startingPos.y][startingPos.x] <= 0) return;

            if (roadMapPrefabs[startingPos.y][startingPos.x].GetComponent<RoadScript>().RoadModified) return;

            int i = startingPos.y;
            int j = startingPos.x;
            roadMapPrefabs[startingPos.y][startingPos.x].GetComponent<RoadScript>().RoadModified = true;


            if (distance % 4 != 0) roadMapPrefabs[i][j].GetComponent<RoadScript>().DeleteLamp();

            if (distance%7 != 0)
            {
                if (i != 0 && roadMap[i - 1][j] > 0)
                    roadMapPrefabs[i][j].GetComponent<RoadScript>().ClearPath(3);

                if (j != 0 && roadMap[i][j - 1] > 0)
                    roadMapPrefabs[i][j].GetComponent<RoadScript>().ClearPath(0);

                if (i != mapSize.y - 1 && roadMap[i + 1][j] > 0)
                    roadMapPrefabs[i][j].GetComponent<RoadScript>().ClearPath(2);

                if (j != mapSize.x - 1 && roadMap[i][j + 1] > 0)
                    roadMapPrefabs[i][j].GetComponent<RoadScript>().ClearPath(1);
            }

            startingPos.x++;
            CleanUpRoads(startingPos, distance + 1);

            startingPos.x -= 2;
            CleanUpRoads(startingPos, distance + 1);

            startingPos.x++;
            startingPos.y++;
            CleanUpRoads(startingPos, distance + 1);

            startingPos.y -= 2;
            CleanUpRoads(startingPos, distance + 1);
        }

        Transform GetBuildingPrefab(BuildingType bt)
        {
            foreach(var v in Buildings)
            {
                if (v.buildingType == bt) return v.buildingObject;
            }
            return null;
        }

        public void GenerateMap()
        {
            StartCoroutine(GenMapCor());
        }

        IEnumerator GenMapCor()
        {
            //convert seed string to seedValue
            if (seed == null) seedValue = 1285470395;
            else seedValue = StringSeedToInt(seed);

            ran = new System.Random(seedValue);

            // create empty maps

            groups.Add(0);
            groups.Add(0);

            for (int i = 0; i < mapSize.y; i++)
            {
                roadMap.Add(new List<int>());
                visitedMap.Add(new List<bool>());
                roadMapPrefabs.Add(new List<Transform>());
                for (int j = 0; j < mapSize.x; j++)
                {
                    visitedMap[i].Add(false);
                    roadMap[i].Add(0);
                    roadMapPrefabs[i].Add(null);
                }
            }

            // Generate and place key buildings

            int keyBuildingsCount = mapSize.x / MinimumDistanceBetweenKeyBuildings;

            for (int i = 0; i < keyBuildingsCount; i++)
            {
                PlaceBuilding();
            }



            // join two unjoined ss

            for (int i = 0; i < keyBuildingsCount - 1; i++)
            {

                //join the two
                groups[keyBuildings[i].Value] = Find(keyBuildings[i + 1].Value);
                JoinBuildingsByRoad(keyBuildings[i].Key, keyBuildings[i + 1].Key);
            }


            // initialize physical road
            Vector2Int cleanUpStartingPos = new Vector2Int();

            for (int i = 0; i < mapSize.y; i++)
            {
                for (int j = 0; j < mapSize.x; j++)
                {
                    if (roadMap[i][j] > 0)
                    {
                        var x = Instantiate(roadFragmentPrefab);
                        x.position = new Vector3(j * 3, 0, i * 3);
                        roadMapPrefabs[i][j] = x;
                        cleanUpStartingPos.x = j;
                        cleanUpStartingPos.y = i;
                    }
                }
            }

            //clear extra road blocks

            CleanUpRoads(cleanUpStartingPos, 1);

            
            // place schools by roads

            for (int i = 0; i < mapSize.y; i++)
            {
                for (int j = 0; j < mapSize.x; j++)
                {
                    int roadCount = CountNeighbouringRoads(new Vector2Int(j, i));

                    bool placeSchool = (ran.Next() % 100 <= 4);

                    if (roadMap[i][j] == 0 && roadCount != 0 && placeSchool)
                    {
                        schoolCoords.Add(new Vector2Int(j, i));
                        roadMap[i][j] = -1;
                    }
                }
            }


            // place homes by roads

            for (int i = 0; i < mapSize.y; i++)
            {
                for (int j = 0; j < mapSize.x; j++)
                {
                    int roadCount = CountNeighbouringRoads(new Vector2Int(j, i));

                    bool placeHome = (ran.Next() % 100 <= 40);

                    if (roadMap[i][j] == 0 && roadCount != 0 && placeHome)
                    {
                        dwellingBuildings.Add(new Vector2Int(j, i));
                        roadMap[i][j] = -1;
                    }
                }
            }

            // place physical working buildings

            foreach (var bld in keyBuildings)
            {
                var v = Instantiate(GetBuildingPrefab(BuildingType.Skyscraper));
                v.position = new Vector3(bld.Key.x * 3, 0, bld.Key.y * 3);
                var x = Instantiate(buildingBasePrefab);
                x.position = new Vector3(bld.Key.x * 3, 0, bld.Key.y * 3);
                v.GetComponent<WorkingBuilding>().SetRandomName();
            }

            // place physical dwelling buildings

            foreach (var bld in dwellingBuildings)
            {
                var v = Instantiate(GetBuildingPrefab(BuildingType.House));
                v.position = new Vector3(bld.x * 3, 0, bld.y * 3);
                var x = Instantiate(buildingBasePrefab);
                x.position = new Vector3(bld.x * 3, 0, bld.y * 3);

                dwellingBuildingsData.Add(v.GetComponent<DwellingBuilding>());

                if (bld.x != 0 && roadMap[bld.y][bld.x - 1] > 0)
                    v.rotation = Quaternion.Euler(0, 0, 0);

                if (bld.x != mapSize.x - 1 && roadMap[bld.y][bld.x + 1] > 0)
                    v.rotation = Quaternion.Euler(0, 180, 0);

                if (bld.y != 0 && roadMap[bld.y - 1][bld.x] > 0)
                    v.rotation = Quaternion.Euler(0, -90, 0);

                if (bld.y != mapSize.y - 1 && roadMap[bld.y + 1][bld.x] > 0)
                    v.rotation = Quaternion.Euler(0, 90, 0);
            }

            // place physical school building

            foreach (var bld in schoolCoords)
            {
                var v = Instantiate(GetBuildingPrefab(BuildingType.School));
                v.position = new Vector3(bld.x * 3, 0, bld.y * 3);
                var x = Instantiate(buildingBasePrefab);
                x.position = new Vector3(bld.x * 3, 0, bld.y * 3);

                v.GetComponent<SchoolBuilding>().SetName("School #" + GameController.RandomNumberGenerator.Next() % 10);

                if (bld.x != 0 && roadMap[bld.y][bld.x - 1] > 0)
                    v.rotation = Quaternion.Euler(0, 0, 0);

                if (bld.x != mapSize.x - 1 && roadMap[bld.y][bld.x + 1] > 0)
                    v.rotation = Quaternion.Euler(0, 180, 0);

                if (bld.y != 0 && roadMap[bld.y - 1][bld.x] > 0)
                    v.rotation = Quaternion.Euler(0, -90, 0);

                if (bld.y != mapSize.y - 1 && roadMap[bld.y + 1][bld.x] > 0)
                    v.rotation = Quaternion.Euler(0, 90, 0);
            }

            // place grass
            for (int i = 0; i < mapSize.y; i++)
            {
                for (int j = 0; j < mapSize.x; j++)
                {
                    if (roadMap[i][j] == 0)
                    {
                        var x = Instantiate(grassFragmentPrefab);
                        x.position = new Vector3(j * 3, 0, i * 3);
                    }
                }
            }

            yield return null;

            GetComponent<NavMeshSurface>().BuildNavMesh();

            yield return null;

            //Spawn people
            foreach (var b in dwellingBuildingsData)
            {
                b.GetComponent<GeneratePeople>().GenPeople();
            }

        }

        public void SetSeed(string x) { seed = x; }

        private void Start()
        {
            Instance = this;
        }

    }
}
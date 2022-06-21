using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimulationNS;

public class WorkingBuilding : Building
{
    [SerializeField]
    List<Vector2> ShiftTimes;

    public Vector2 GetShift(int index) => ShiftTimes[index];
    public List<Vector2> GetAllShifts(int index) => ShiftTimes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

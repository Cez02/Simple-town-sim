using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimulationNS;

public class GameController : MonoBehaviour
{
    // singleton initialization 

    public static GameController instance;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }

    /*
     * This object handles the most basic functions of the simulation
     * such as the simulation clock (which differs from the unity time)
     * 
     */


    //=================================
    // Variables
    //=================================


    // ==== Time related ====

    //each minute is 1 seconds (like most RTS games)
    public const float secondsInDay = 60f * 24f;
    public const float originalFixedInterval = 0.1f;

    public bool SimulationRunning { get; private set; } = false;

    public float CurrentTime { get; private set; } = 0f;

    private float CurrentTimeScale = 2.0f;

    public delegate void DelEventHandler();
    public static event DelEventHandler NewDay;
    public static event DelEventHandler HandleSecond;
    public static event DelEventHandler PrepareSimulation;

    public static System.Random RandomNumberGenerator = new System.Random();

    // ==== Generation related ====

    [SerializeField] SimulationNS.MapGenerator MG;

    // ===== People related =====

    public Person currentlySelectedPerson;

    public string[] names =
    {
        "Shiv Fields",
        "Sharmin Landry",
        "Layla-Mae Rojas",
        "Cordelia Yu",
        "Mcauley Vu",
        "Lianne Wood",
        "Ami Key",
        "Maddie Flores",
        "Lennie Timms",
        "Emre Flynn",
        "Anwar Walsh",
        "Isla-Rae Knapp",
        "Timur Bowler",
        "Catrina French",
        "Falak Ponce",
        "Danyaal Moon",
        "Juliet Barrett",
        "Abraham Maddox",
        "Isla-Grace Marquez",
        "Jaiden Sears"
    };


    //=================================
    // Simulation loop
    //=================================

    public static float TimeToSeconds(int hour, int minutes)
    {
        return hour * 60f + (float)minutes;
    }

    public static float TimeToSeconds(Vector2Int time)
    {
        return TimeToSeconds(time.x, time.y);
    }


    void SetTimeScale(float newScale)
    {
        CurrentTimeScale = newScale;
        Time.fixedDeltaTime = originalFixedInterval / newScale;
        Time.timeScale = newScale;
    }

    IEnumerator GameLoop()
    {
        NewDay();

        while (true)
        {

            Debug.Log(Time.fixedDeltaTime);

            while (!SimulationRunning)
                yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);

            CurrentTime += 1;
            if(CurrentTime >= secondsInDay)
            {
                NewDay();
                CurrentTime = 0f;
            }

            HandleSecond();


            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime); //move to next frame
        }


        yield return null;
    }


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0f;
        SimulationRunning = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    public void BeginSimulation()
    {
        
        if(PrepareSimulation != null) PrepareSimulation();
        SimulationRunning = true;
        SetTimeScale(CurrentTimeScale);
        StartCoroutine(GameLoop());
    }

    public void SetSimulationTimeScale(float newTimeScale)
    {
        SetTimeScale(newTimeScale);
    }

    public void PauseSimulation()
    {
        Time.timeScale = 0;
        //Time.fixedDeltaTime = 0;
        SimulationRunning = false;
    }

    public void ResumeSimulation()
    {
        SetTimeScale(CurrentTimeScale);
        SimulationRunning = true;
    }


    // people related

    public void DeselectPerson()
    {
        currentlySelectedPerson.DeselectPerson();
        currentlySelectedPerson = null;
    }

}

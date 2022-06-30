using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimulationNS;
using UnityEngine.UI;
using TMPro;

public class UpdateSeed : MonoBehaviour
{
    
    public void SetNewSeed()
    {
        MapGenerator.Instance.SetSeed(GetComponent<TMP_InputField>().text);
    }
}

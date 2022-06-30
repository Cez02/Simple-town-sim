using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SimulationNS
{
    public class InfoWindow : MonoBehaviour
    {
        public static InfoWindow instance;

        [SerializeField] Transform InfoOBJ;
        [SerializeField] TextMeshProUGUI infoTitle, infoText;

        private void Start()
        {
            instance = this;
        }

        public void DisplayInfo(Person ch)
        {
            
            infoTitle.text = ch.GetName();
            infoText.text = "Currently at: " + ch.transform.position.ToString() + "\n" +
                            "Going to    : " + ch.GetDestinationName();
            InfoOBJ.gameObject.SetActive(true);
        }
    }
}
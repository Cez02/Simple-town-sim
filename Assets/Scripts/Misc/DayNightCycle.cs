using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace SimulationNS
{

    public class DayNightCycle : MonoBehaviour
    {
        [SerializeField] Light sun;

        float beginningAngle = -75.131f;
        float endingAngle = 153.813f;


        // Update is called once per frame
        void Update()
        {
            if(180 < GameController.instance.CurrentTime && GameController.instance.CurrentTime < 1380)
            {
                float t = (GameController.instance.CurrentTime - 180f) / (1200f);
                Vector3 currentPos = new Vector3(Mathf.Lerp(beginningAngle, endingAngle, t), 0, 0);

                if (t < 0.5f)
                    sun.intensity = Mathf.SmoothStep(0f, 1f, t * 2);
                else
                    sun.intensity = Mathf.SmoothStep(0f, 1f, 1 - (t - 0.5f) * 2f);

                transform.rotation = Quaternion.Euler(currentPos);
            }


        }
    }

}
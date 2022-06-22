using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Clock : MonoBehaviour
{
    [SerializeField]
    RectTransform rt;
    [SerializeField]
    TextMeshProUGUI clockTimeOfDay;
    Vector3 currentRotation;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {

        currentRotation.z = 2*-1*360f*GameController.instance.CurrentTime / GameController.secondsInDay;
        rt.localRotation = Quaternion.Euler(currentRotation);

        if (GameController.instance.CurrentTime / GameController.secondsInDay > 0.5)
            clockTimeOfDay.text = "PM";
        else
            clockTimeOfDay.text = "AM";
    }
}

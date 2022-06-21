using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    [SerializeField]
    RectTransform rt;
    Vector3 currentRotation;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {

        currentRotation.z = -1*360f*GameController.instance.CurrentTime / GameController.secondsInDay;
        rt.localRotation = Quaternion.Euler(currentRotation);
    }
}

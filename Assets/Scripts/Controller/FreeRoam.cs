using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FreeRoam : MonoBehaviour
{
    //Script for free-roam camera movement with border and height limitations
    //the arrow keys are directional
    //the scroll wheel moves the camrea up and down and will face down as the view rises (can be disabled)
    //depressing the right mouse button and scrolling left/right will change the left/right view
    //the camrea must be a child object of whichever game object this script controls

    //The following values (non-placeholder) can be changed to adjust for the scale and scope of camera movement:

    private float moveFactor = 5.0f;  //movement rate of camrea movement (keys)
    private float moveFactorMax = 150.0f;  //maximum movement rate of camrea movement (keys)
    private float MoveFactorOriginal;  //placeholder for moveFactor
    private float moveFactorIncrement;  //paceholder for increment rate
    private float moveFactorIncrementRate = 0.5f;  //rate (multiply) by which moveFactor will increase (duration * time)

    private float tranformCameraXFactor = 8.0f;  //x axis  factor during change in y axis (scroll) / set value to 1.0f to disable rotation

    private float mouseX;  //placeholder for mouse rotation
    private float mouseScroll;  //placeholder for mouse scroll
    private float mouseScrollFactor = -11.5f;  //rate at which the mouse scroll movement
    private float mouseScrollBackThreshold = 35.0f; //threshold where the view stops moving when the angle moves up (to keep objects in the immideate view visible)
    private float mouseSideFactor = 2.5f;  //rate by which the side panning will turn

    private int maxY = 100;  //maximum camera height
    private int minY = 10;  //minimum camera height
    private float maxX = 100.0f;  //maximum camera position x (left)
    private float minX = -100.0f;  //maximum camera position x (right)
    private float maxZ = 100.0f;  //maximum camera position z (forward)
    private float minZ = -100.0f;  //maximum camera position z (back)

    public string targetCamera = "MainCamera";  //child object target tag
    public GameObject ChildCamera;

    void Start()
    {
        MoveFactorOriginal = moveFactor;
        moveFactorIncrement = moveFactor * moveFactorIncrementRate;
        ChildCamera = GameObject.FindGameObjectWithTag(targetCamera);
    }

    void Update()
    {
        if (moveFactor > moveFactorMax)
        {
            moveFactor = moveFactorMax;
        }

        if (Input.GetKey("up"))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * moveFactor, Space.Self);
            moveFactor = moveFactor + moveFactorIncrement;
        }

        if (Input.GetKey("down"))
        {
            transform.Translate(Vector3.back * Time.deltaTime * moveFactor, Space.Self);
            moveFactor = moveFactor + moveFactorIncrement;
        }

        if (Input.GetKey("left"))
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveFactor, Space.Self);
            moveFactor = moveFactor + moveFactorIncrement;
        }

        if (Input.GetKey("right"))
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveFactor, Space.Self);
            moveFactor = moveFactor + moveFactorIncrement;
        }

        if (Input.GetKeyUp("up"))
        {
            moveFactor = MoveFactorOriginal;
        }

        if (Input.GetKeyUp("down"))
        {
            moveFactor = MoveFactorOriginal;
        }

        if (Input.GetKeyUp("left"))
        {
            moveFactor = MoveFactorOriginal;
        }

        if (Input.GetKeyUp("right"))
        {
            moveFactor = MoveFactorOriginal;
        }

        if (Input.GetMouseButton(1))
        {
            mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(new Vector3(0, (mouseX * mouseSideFactor), 0), Space.World);
        }

        if (ChildCamera.transform.position.y > minY)
        {
            mouseScroll = (Input.GetAxis("Mouse ScrollWheel") * mouseScrollFactor);

            if (ChildCamera.transform.position.y < mouseScrollBackThreshold)
            {
                ChildCamera.transform.position += new Vector3(0, mouseScroll, -1 * mouseScroll);
            }

            if (ChildCamera.transform.position.y > mouseScrollBackThreshold)
            {
                ChildCamera.transform.position += new Vector3(0, mouseScroll, 0);

            }
        }

        if (ChildCamera.transform.position.y < maxY)
        {
            mouseScroll = (Input.GetAxis("Mouse ScrollWheel") * mouseScrollFactor);
            ChildCamera.transform.position += new Vector3(0, mouseScroll, 0);
        }

        if (ChildCamera.transform.position.y < minY)
        {
            ChildCamera.transform.position = new Vector3(ChildCamera.transform.position.x, minY, ChildCamera.transform.position.z);
        }

        if (ChildCamera.transform.position.y > maxY)
        {
            ChildCamera.transform.position = new Vector3(ChildCamera.transform.position.x, maxY, ChildCamera.transform.position.z);
        }

        if (transform.position.x > maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }

        if (transform.position.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }

        if (transform.position.z > maxZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, maxZ);
        }

        if (transform.position.z < minZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, minZ);
        }

        ChildCamera.transform.eulerAngles = new Vector3((ChildCamera.transform.position.y / (minY + 0.01f) * tranformCameraXFactor), ChildCamera.transform.eulerAngles.y, ChildCamera.transform.eulerAngles.z);
    }

}
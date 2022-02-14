using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Instantiate public variables
    public float cameraSpeed;
    public float rotationSensitivity;
    public GameObject cameraFocus;

    //Instantiate private variables
    private Vector3 cameraTranslationVector;
    private Vector3 offset = -Vector3.forward;
    private float mouseX;
    private float mouseY;
    private Rigidbody cameraFocusRb;

    // Start is called before the first frame update
    void Start()
    {
        cameraFocusRb = cameraFocus.GetComponent<Rigidbody>();

        transform.position = cameraFocus.transform.position + offset.normalized * 10;
    }

    void Update()
    {
        
        //Acquire all appropriate input axis.
        AssignTransformAxes();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            cameraSpeed = 2;
        }
        else
        {
            cameraSpeed = 0.5f;
        }

    }

    void FixedUpdate()
    {
        cameraFocus.transform.position += cameraFocus.transform.forward * cameraTranslationVector.z * cameraSpeed;
        cameraFocus.transform.position += cameraFocus.transform.up * cameraTranslationVector.y * cameraSpeed;
        cameraFocus.transform.position += cameraFocus.transform.right * cameraTranslationVector.x * cameraSpeed;
    }

    // LateUpdate is called once per frame at end of frame to establish all physics and graphics before camera shift.
    void LateUpdate()
    {
        if (IsMouseOverGameWindow)
        {
            //We opt to seperate the mouseX and mouseY inputs so that we can rotate around the global Y but the local X
            //Rotate around the global Y using the mouseX input
            cameraFocus.transform.Rotate(new Vector3(0, mouseX, 0), Space.World);
            //Rotate around the local X using the mouseY input (BTW Space.Self is implied but we specify it for clarity)
            //Using negative mouseY to invert camera movement
            cameraFocus.transform.Rotate(new Vector3(-mouseY, 0, 0), Space.Self);
        }

        /* TODO: perform camera rotation only when appropriate */
        transform.LookAt(cameraFocus.transform.position);
    }

    bool IsMouseOverGameWindow 
    { 
        get { return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y); } 
    }
    //Get all necessary input axis from Input Controller and put them in our vectors for cleanliness
    private void AssignTransformAxes()
    {
        mouseX = (Input.GetAxis("Mouse X") * rotationSensitivity);
        mouseY = (Input.GetAxis("Mouse Y") * rotationSensitivity);
        cameraTranslationVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Jump"), Input.GetAxis("Vertical"));
    }
}

using UnityEngine;
using System.Collections;
using NUnit.Framework.Constraints;

public class ShipController : MonoBehaviour
{
    public GameObject Camera;
    public bool RotateWithCamera = true;
    public float RotationSpeed = 1f;
    public Vector3 MovementMultiplier = Vector3.one;
    public Vector3 RotationMultiplier = Vector3.one;
    private Vector3 movementInputVector;
    private Vector3 rotationInputVector;
    private bool isDampening = false;
    private Rigidbody rBody;
    private float drag;
    private float angularDrag;

    void Start() // Use this for initialization
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        rBody = GetComponent<Rigidbody>();
        drag = rBody.drag;
        angularDrag = rBody.angularDrag;
    }

    void Update() // Update is called once per frame
    {
        movementInputVector = new Vector3(Input.GetAxis("ShipXAxis"), Input.GetAxis("ShipYAxis"), Input.GetAxis("ShipThrottle"));
        rotationInputVector = new Vector3(Input.GetAxis("ShipPitch"), Input.GetAxis("ShipYaw"), Input.GetAxis("ShipRoll"));

        if (Input.GetKeyUp(KeyCode.X)) isDampening = !isDampening;
        if (Input.GetMouseButtonUp(2)) RotateWithCamera = !RotateWithCamera;
        
        if (isDampening)
        {
            rBody.angularDrag = 1;
            rBody.drag = 1;
        }
        else
        {
            rBody.drag = drag;
            rBody.angularDrag = angularDrag;
        }
    }

    void FixedUpdate()
    {
        if (RotateWithCamera)
        {
            rBody.MoveRotation(Quaternion.Slerp(transform.rotation, Camera.transform.rotation,
                Time.fixedDeltaTime * RotationSpeed));
        }
        
        rBody.AddRelativeForce(Vector3.Scale(movementInputVector, MovementMultiplier) * Time.fixedDeltaTime);
    }

    void OnGUI()
    {
        if (isDampening)
        {
            GUI.Box(new Rect(10, 10, 160, 20), "Dampening is ON");
        }
    }
}

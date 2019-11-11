using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public Vector3 RotationSpeed = new Vector3(1f, 1f, 1f);
    public Vector3 MovementSpeed = new Vector3(1f, 1f, 1f);
    private Vector3 movementInput, rotationInput;
    private Rigidbody rBody;
    private float drag, angularDrag;

    void Start()
    {
        rBody = GetComponent<Rigidbody>() as Rigidbody;
    }

    void Update()
    {
        movementInput = new Vector3(Input.GetAxis("ShipXAxis"), Input.GetAxis("ShipYAxis"), Input.GetAxis("ShipThrottle"));
        rotationInput = new Vector3(Input.GetAxis("Mouse Y") * -1, Input.GetAxis("Mouse X"), Input.GetAxis("ShipRoll") * -1);

        var applyMovement = new Vector3(movementInput.x * MovementSpeed.x, movementInput.y * MovementSpeed.y, movementInput.z * MovementSpeed.z) * Time.deltaTime;
        var applyRotation = new Vector3(rotationInput.x * RotationSpeed.x, rotationInput.y * RotationSpeed.y, rotationInput.z * RotationSpeed.z) * Time.deltaTime;

        transform.Rotate(applyRotation.x, applyRotation.y, applyRotation.z, Space.Self);
        rBody.AddRelativeForce(applyMovement);


    }
}

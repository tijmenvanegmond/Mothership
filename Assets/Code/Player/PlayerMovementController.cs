using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour {
    private PlayerControls controls;
    public Vector3 rotationSpeed = new Vector3(1f, 1f, 1f);
    public Vector3 movementSpeed = new Vector3(1f, 1f, 1f);
    private Vector3 movementInput, rotationInput;
    private Rigidbody rBody;
    private float drag, angularDrag;

    private Vector2 hMove;
    private float vMove;

    private Vector2 look;
    private float roll;

    void Awake() {
        controls = new PlayerControls();

        controls.Movement.Horizontal.performed += ctx => hMove = ctx.ReadValue<Vector2>();
        controls.Movement.Horizontal.canceled += ctx => hMove = Vector2.zero;

        controls.Movement.Vertical.performed += ctx => vMove = ctx.ReadValue<float>();
        controls.Movement.Vertical.canceled += ctx => vMove = 0f;

        controls.Movement.Look.performed += ctx => look = ctx.ReadValue<Vector2>();
        controls.Movement.Look.canceled += ctx => look = Vector2.zero;

        controls.Movement.Roll.performed += ctx => roll = ctx.ReadValue<float>();
        controls.Movement.Roll.canceled += ctx => roll = 0f;
    }

    void Start() {
        rBody = GetComponent<Rigidbody>() as Rigidbody;

    }

    void Update() {
        movementInput = new Vector3(hMove.x, vMove, hMove.y);
        rotationInput = new Vector3(look.y * -1, look.x, roll * -1);

        var applyMovement = new Vector3(movementInput.x * movementSpeed.x, movementInput.y * movementSpeed.y, movementInput.z * movementSpeed.z) * Time.deltaTime;
        var applyRotation = new Vector3(rotationInput.x * rotationSpeed.x, rotationInput.y * rotationSpeed.y, rotationInput.z * rotationSpeed.z) * Time.deltaTime;

        rBody.AddRelativeTorque(applyRotation, ForceMode.VelocityChange);
        rBody.AddRelativeForce(applyMovement, ForceMode.VelocityChange);

    }

    void OnEnable() {
        if (controls != null)
            controls.Movement.Enable();

    }

    void OnDisable() {
        if (controls != null)
            controls.Movement.Disable();

    }
}
using UnityEngine;
using System.Collections;
using NUnit.Framework.Constraints;

public class SpaceShipController : MonoBehaviour
{
    public GameObject Camera;
    public bool RotateWithCamera = true;
    public float RotationSpeed = 1f;
    public Vector3 MovementMultiplier = Vector3.one;
    public Vector3 RotationMultiplier = Vector3.one;
    private Vector3 _movementInputVector;
    private Vector3 _rotationInputVector;
    private bool _isDampening = false;
    private Rigidbody _rBody;
    private float _drag;
    private float _angularDrag;

    void Start() // Use this for initialization
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        _rBody = GetComponent<Rigidbody>();
        _drag = _rBody.drag;
        _angularDrag = _rBody.angularDrag;
    }

    void Update() // Update is called once per frame
    {
        _movementInputVector = new Vector3(Input.GetAxis("ShipXAxis"), Input.GetAxis("ShipYAxis"), Input.GetAxis("ShipThrottle"));
        _rotationInputVector = new Vector3(Input.GetAxis("ShipPitch"), Input.GetAxis("ShipYaw"), Input.GetAxis("ShipRoll"));

        if (Input.GetKeyUp(KeyCode.X)) _isDampening = !_isDampening;
        if (Input.GetMouseButtonUp(2)) RotateWithCamera = !RotateWithCamera;
        
        if (_isDampening)
        {
            _rBody.angularDrag = 1;
            _rBody.drag = 1;
        }
        else
        {
            _rBody.drag = _drag;
            _rBody.angularDrag = _angularDrag;
        }
    }

    void FixedUpdate()
    {
        if (RotateWithCamera)
        {
            _rBody.MoveRotation(Quaternion.Slerp(transform.rotation, Camera.transform.rotation,
                Time.fixedDeltaTime * RotationSpeed));
        }
        _rBody.AddRelativeForce(Vector3.Scale(_movementInputVector, MovementMultiplier) * Time.fixedDeltaTime);
    }

    void OnGUI()
    {
        if (_isDampening)
        {
            GUI.Box(new Rect(10, 10, 160, 20), "Dampening is ON");
        }
    }
}

using UnityEngine;
using System.Collections;

public class SpaceShipController : MonoBehaviour
{

    public Vector3 MovementMultiplier = Vector3.one;
    public Vector3 RotationMultiplier = Vector3.one;
    private Vector3 movementInputVector;
    private Vector3 rotationInputVector;
    private bool _isDampening = false;
    private Rigidbody _rBody;

    void Start() // Use this for initialization
    {
        _rBody = GetComponent<Rigidbody>();
    }

    void Update() // Update is called once per frame
    {
        movementInputVector = new Vector3(Input.GetAxis("ShipXAxis"), Input.GetAxis("ShipYAxis"), Input.GetAxis("ShipZAxis"));
        rotationInputVector = new Vector3(Input.GetAxis("ShipPitch"), Input.GetAxis("ShipYaw"), Input.GetAxis("ShipRoll"));

        if (Input.GetKeyUp(KeyCode.X)) _isDampening = !_isDampening;
    }

    void FixedUpdate()
    {
        _rBody.AddRelativeForce(Vector3.Scale(movementInputVector, MovementMultiplier) * Time.fixedDeltaTime);
        _rBody.AddRelativeTorque(Vector3.Scale(rotationInputVector, RotationMultiplier) * Time.fixedDeltaTime);

        if (_isDampening)
        {
            _rBody.AddForce(Vector3.Scale(-_rBody.velocity.normalized, MovementMultiplier) * Time.fixedDeltaTime);
            _rBody.AddTorque(Vector3.Scale(-_rBody.angularVelocity.normalized, RotationMultiplier) * Time.fixedDeltaTime);
        }

        //_rBody.drag = _isDampening ? 0.4f : 0;
        //_rBody.angularDrag = _isDampening ? 0.4f : 0;
    }

    void OnGUI()
    {
        if (_isDampening)
        {
            GUI.Box(new Rect(10, 10, 160, 20), "Dampening is ON");
        }
    }
}

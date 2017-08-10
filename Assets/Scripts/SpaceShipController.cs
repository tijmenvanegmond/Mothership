using UnityEngine;
using System.Collections;

public class SpaceShipController : MonoBehaviour
{
    public GameObject Camera;
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
        _rBody = GetComponent<Rigidbody>();
        _drag = _rBody.drag;
        _angularDrag = _rBody.angularDrag;
    }

    void Update() // Update is called once per frame
    {
        _movementInputVector = new Vector3(Input.GetAxis("ShipXAxis"), Input.GetAxis("ShipYAxis"), Input.GetAxis("ShipThrottle"));
        _rotationInputVector = new Vector3(Input.GetAxis("ShipPitch"), Input.GetAxis("ShipYaw"), Input.GetAxis("ShipRoll"));

        if (Input.GetKeyUp(KeyCode.X)) _isDampening = !_isDampening;
        
        if (_isDampening)
        {
            //_rBody.AddForce(Vector3.Scale(-_rBody.velocity.normalized, MovementMultiplier) * Time.fixedDeltaTime);
            //_rBody.AddTorque(Vector3.Scale(-_rBody.angularVelocity.normalized, RotationMultiplier) * Time.fixedDeltaTime);
            _rBody.angularDrag = 1;
            _rBody.drag = 1;
        }
        else
        {
            _rBody.drag = _drag;
            _rBody.angularDrag = _angularDrag;
        }
        
        //find the vector pointing from our position to the target
        //Vector3 direction = (Camera.transform.position - transform.position).normalized;
 
        //create the rotation we need to be in to look at the target
        //Quaternion lookRotation = Quaternion.LookRotation(Camera.transform.rotation*Vector3.forward);
 
        //rotate us over time according to speed until we are in the required rotation
       // transform.rotation = Quaternion.Slerp(transform.rotation, Camera.transform.rotation, Time.deltaTime * RotationSpeed);
        _rBody.MoveRotation( Quaternion.Slerp(transform.rotation, Camera.transform.rotation, Time.deltaTime * RotationSpeed));
        
    }

    void FixedUpdate()
    {
        _rBody.AddRelativeForce(Vector3.Scale(_movementInputVector, MovementMultiplier) * Time.fixedDeltaTime);
        _rBody.AddRelativeTorque(Vector3.Scale(_rotationInputVector, RotationMultiplier) * Time.fixedDeltaTime);
    }

    void OnGUI()
    {
        if (_isDampening)
        {
            GUI.Box(new Rect(10, 10, 160, 20), "Dampening is ON");
        }
    }
}

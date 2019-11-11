using System.Collections;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class ShipController : MonoBehaviour {
    public Ship Ship;
    public GameObject Camera;

    public bool RotateWithCamera = true;
    public float RotationSpeed = 1f;
    public Vector3 RotationMultiplier = Vector3.one;
    public bool IsDampening = false;
    private Vector3 movementInputVector, rotationInputVector;
    private Rigidbody rBody;
    private float drag, angularDrag;
    [SerializeField]
    private float rForce, Kp, Ki, Kd;
    private PIDcontroller pid = new PIDcontroller ();

    void Start () {
        Ship = GetComponent<Ship> () as Ship;
        rBody = GetComponent<Rigidbody> () as Rigidbody;
        //store defaults
        drag = rBody.drag;
        angularDrag = rBody.angularDrag;
    }

    void Update () {
        pid.Kd = Kd;
        pid.Ki = Ki;
        pid.Kp = Kp;

        movementInputVector = new Vector3 (Input.GetAxis ("ShipXAxis"), Input.GetAxis ("ShipYAxis"), Input.GetAxis ("ShipThrottle"));
        rotationInputVector = new Vector3 (Input.GetAxis ("ShipPitch"), Input.GetAxis ("ShipYaw"), Input.GetAxis ("ShipRoll"));

        if (Input.GetButtonUp ("Dampening")) IsDampening = !IsDampening;
        if (Input.GetMouseButtonUp (2)) RotateWithCamera = !RotateWithCamera;
    }

    void FixedUpdate () {
        if (RotateWithCamera) {
            Transform target = Camera.transform;
            //var newRot = Quaternion.RotateTowards(transform.rotation, Camera.transform.rotation, .5f);
            //rBody.MoveRotation(newRot);

            //get the angle between
            float angleDiff = Vector3.Angle (transform.forward, target.forward);

            // get its cross product, which is the axis of rotation to
            // get from one vector to the other
            Vector3 cross = Vector3.Cross (transform.forward, target.forward);
            //get pidcontroller output
            var PIDoutput = Mathf.Clamp (pid.GetOutput (angleDiff, Time.deltaTime), -1f, 1f);
            // apply torque along that axis according to the magnitude of the angle.
            rBody.AddTorque (cross * rForce * PIDoutput);

            angleDiff = Vector3.Angle (transform.up, target.up);
        }

        //Dampening
        //TODO: less escalating / less figthing
        if (IsDampening) {
            var v = rBody.velocity;
            var angularV = rBody.angularVelocity;
            foreach (var truster in Ship.Trusters) {
                //dampen momentum
                var dot = Vector3.Dot (v.normalized, truster.GetForceDirection ().normalized);
                if (dot < 0) {
                    Vector3 force = truster.GetForceVector ();
                    Vector3 postion = truster.GetForcePostion ();
                    truster.FireTuster (Mathf.Min (.5f, -dot));
                }

                //dampen angular momentum
                dot = Vector3.Dot (angularV.normalized, truster.GetExpectedAngularAcceleration ().normalized);
                if (dot < 0) {
                    Vector3 force = truster.GetForceVector ();
                    Vector3 postion = truster.GetForcePostion ();
                    truster.FireTuster (Mathf.Min (.2f, -dot));
                }

            }
        }

        //Movement - curently based on camera axis
        //TODO: stop lowering max speed when goind multiple axis
        var gMoveInputVector = Ship.transform.TransformDirection (movementInputVector);
        foreach (var truster in Ship.Trusters) {
            var dot = Vector3.Dot (gMoveInputVector, truster.GetForceDirection ());
            if (dot > 0) {
                Vector3 force = truster.GetForceVector ();
                Vector3 postion = truster.GetForcePostion ();
                truster.FireTuster (dot);
            }
        }
    }
}

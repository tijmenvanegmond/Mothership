using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class OrbitCamera : MonoBehaviour
{
    public GameObject Target;
    public float MinDistance = 3f;
    public float MaxDistance = 100f;
    public float Distance = 10f;
    public float OrbitSpeed = 160f;
    public float RollSpeed = 80f;
    public Vector3 StartRotation = new Vector3(45f, 0, 0);
    private Quaternion _totalRotation = Quaternion.identity;
    private Vector3 _addedRotation = new Vector3(45f, 0, 0);
    private Quaternion _addedRollRotation = Quaternion.identity;

    private void Start()
    {
        _addedRotation = StartRotation;
    }

    void Update()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 rotationInputVector =
            new Vector3(Input.GetAxis("ShipPitch"), Input.GetAxis("ShipYaw"), Input.GetAxis("ShipRoll"));

        _totalRotation *= Quaternion.Euler(new Vector3(0, 0, rotationInputVector.z * -100f * Time.deltaTime)); //roll
        
        //orbit
        if (Mathf.Abs(mouseDelta.x) > Mathf.Abs(mouseDelta.y))
        {
            _totalRotation *= Quaternion.Euler(new Vector3(0, mouseDelta.x * OrbitSpeed * Time.deltaTime, 0));
        }
        else
        {
            _totalRotation *= Quaternion.Euler(new Vector3(mouseDelta.y * -OrbitSpeed * Time.deltaTime, 0, 0));
        }
        
        _totalRotation *= Quaternion.Euler(new Vector3(0, 0, rotationInputVector.z * -RollSpeed * Time.deltaTime));
        
        Distance -= Input.mouseScrollDelta.y * Time.deltaTime * 50;
        Distance = Mathf.Clamp(Distance, MinDistance, MaxDistance);

        PlaceCameraWithRay();
    }

    void PlaceCameraWithRay()
    {
        Vector3 cameraDirection = -(_totalRotation * Vector3.forward);
        Ray ray = new Ray(Target.transform.position + (cameraDirection * Distance), -cameraDirection);

        try
        {
            ray = GetPoinOfVisibilty(ray);
        }
        catch
        {
            Debug.Log("can't find the ship");
        }

        transform.position = ray.origin + (ray.direction * .5f);

        transform.rotation = _totalRotation;
    }
    /// <summary>
    /// cast an ray an continues to cast it trought object until the target can been seen
    /// </summary>
    /// <param name="ray">a ray aimed at the target</param>
    /// <returns>a ray that will hit the target</returns>
    /// <exception cref="Exception">can't find the bloody target</exception>
    Ray GetPoinOfVisibilty(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Distance))
        {
            if (hit.rigidbody != null && hit.rigidbody.gameObject == Target)
            {
                return ray;
            }
            else
            {
                return GetPoinOfVisibilty(new Ray(hit.point, ray.direction));
            }
        }
        else throw new Exception("ray not aimed at target or target does not match ");
    }
}
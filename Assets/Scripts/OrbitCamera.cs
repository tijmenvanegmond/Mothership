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
    public Quaternion DefaultRotation = Quaternion.Euler(new Vector3(45f, 0, 0));
    private Quaternion _addedRotation = Quaternion.identity;
    private Vector3 _previousMousePos = Vector3.zero;
    private float _lastXdelta = 0f;

    void FixedUpdate()
    {
        Vector3 mouseDelta = (Input.mousePosition - _previousMousePos) * Time.deltaTime;
        mouseDelta = Vector3.ClampMagnitude(mouseDelta, 5f);
        _previousMousePos = Input.mousePosition;
        
        if (Input.GetMouseButton(2))
        {
            if (Mathf.Abs(mouseDelta.x) > Mathf.Abs(mouseDelta.y))
            {
                _addedRotation *= Quaternion.Euler(new Vector3(0,  mouseDelta.x * 200f * Time.deltaTime, 0));

            }
            else
            {
                _addedRotation *= Quaternion.Euler(new Vector3( mouseDelta.y * -200f * Time.deltaTime ,0f,  0f));
            }

            _lastXdelta = mouseDelta.x * 50;
        }
        
        Distance -= Input.mouseScrollDelta.y * Time.deltaTime * 50;
        Distance = Mathf.Clamp(Distance, MinDistance, MaxDistance);
        
        PlaceCameraWithRay();
    }

    void PlaceCameraWithRay()
    {
        Vector3 cameraDirection = -(Target.transform.rotation*_addedRotation*DefaultRotation*Vector3.forward);
        Ray ray = new Ray(Target.transform.position+(cameraDirection * Distance),-cameraDirection);

        try
        {
            ray = GetPoinOfVisibilty(ray);
        }
        catch
        {
            Debug.Log("can't find the ship");
        }
        
        Debug.DrawRay(ray.origin,ray.direction);
        
        transform.position = ray.origin + ray.origin.normalized;
        
        transform.rotation = Target.transform.rotation*_addedRotation* DefaultRotation;
    }

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
        else throw new Exception("ray no aimed at hip or ship does not match ");
    }

}

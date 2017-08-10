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
    public Quaternion StartRotation = Quaternion.Euler(new Vector3(45f, 0, 0));
    private Quaternion _addedRotation =  Quaternion.Euler(new Vector3(45f, 0, 0));
    private bool _cameraControlOn = false;

    private void Start()
    {
        _addedRotation = StartRotation;
    }

    void FixedUpdate()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));

        if (Input.GetMouseButtonDown(2)) _cameraControlOn = !_cameraControlOn;
        Cursor.visible = !_cameraControlOn;
        if(_cameraControlOn){
            
            if (Mathf.Abs(mouseDelta.x) > Mathf.Abs(mouseDelta.y))
                {
                    _addedRotation *= Quaternion.Euler(new Vector3(0,  mouseDelta.x * 200f * Time.deltaTime, 0));
    
                }
                else
                {
                    _addedRotation *= Quaternion.Euler(new Vector3( mouseDelta.y * -200f * Time.deltaTime ,0f,  0f));
                }
        }
        
        Distance -= Input.mouseScrollDelta.y * Time.deltaTime * 50;
        Distance = Mathf.Clamp(Distance, MinDistance, MaxDistance);
        
        PlaceCameraWithRay();
    }

    void PlaceCameraWithRay()
    {
        Quaternion totalRotation = _addedRotation; //*DefaultRotation * Target.transfrom.rotation;
        Vector3 cameraDirection = -(totalRotation*Vector3.forward);
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

        transform.rotation = totalRotation;
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

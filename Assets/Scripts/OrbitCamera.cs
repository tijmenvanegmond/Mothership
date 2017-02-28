using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour {

    public GameObject target;
    bool animate = true;
    Vector3 previousMousePos = Vector3.zero;
    float lastXdelta = 5f;

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseDelta = (Input.mousePosition - previousMousePos) * Time.deltaTime;
        mouseDelta = Vector3.ClampMagnitude(mouseDelta, 5f);
        previousMousePos = Input.mousePosition;

        //if (Input.GetMouseButtonDown(1))
        //{
        //    if (transform.parent == null)
        //    {
        //        animate = false;
        //        transform.SetParent(target.transform);
        //    }
        //    else
        //    {
        //        animate = true;
        //        transform.SetParent(null);
        //    }
        //}


        if (Input.GetMouseButton(2))
        {

            if (Mathf.Abs(mouseDelta.x) > Mathf.Abs(mouseDelta.y))
            {
                transform.RotateAround(target.transform.position, Vector3.up, mouseDelta.x * 200f * Time.deltaTime);

            }
            else
            {
                //transform.RotateAround(target.transform.position, Vector3.left, mouseDelta.y * 200f * Time.deltaTime);
                if (animate)transform.Translate(Vector3.down * mouseDelta.y * Time.deltaTime * 20);
            }

            lastXdelta = mouseDelta.x * 50;
        }
        else
        {
            transform.RotateAround(target.transform.position, Vector3.up, lastXdelta * Time.deltaTime);
        }
        transform.Translate(Vector3.forward * Input.mouseScrollDelta.y * Time.deltaTime * 20);

        if (animate)  transform.LookAt(target.transform.position);
    }
}

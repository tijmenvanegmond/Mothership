using UnityEngine;
using System.Collections;

public class Poke : MonoBehaviour {

    public float power = 1000f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit))
            {
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForceAtPosition(ray.direction.normalized * power, hit.point);
                }
            }
        }

    }
}

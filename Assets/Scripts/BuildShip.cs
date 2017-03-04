using UnityEngine;
using System.Collections;

public class BuildShip : MonoBehaviour
{

    public GameObject placementCursor;
    public enum nodeName
    {
        prisma = 1,
        cube = 2,
        cylinder = 3
    }
    public nodeName selected = nodeName.prisma;
    public float rotation90 = 0f;
    public float rotation120 = 0f;

    void Start()
    {
        selected = nodeName.prisma;
    }

    private void SetCursor(string nameOfNewCursor = null)
    {
        foreach (Transform child in placementCursor.transform)
        {
            // selected = nodeName(nameOfNewCursor);
            if (nameOfNewCursor != null && child.gameObject.name == nameOfNewCursor)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Weapon1")) //then we use a prisma
        {
            selected = nodeName.prisma;
            SetCursor("Prisma");
        }
        else if (Input.GetButtonDown("Weapon2")) //now we use a cube
        {
            selected = nodeName.cube;
            SetCursor("Cube");
        }
        else if (Input.GetButtonDown("Weapon3")) //now we use a cube
        {
            selected = nodeName.cylinder;
            SetCursor("Cylinder");
        }

        if (Input.GetButtonDown("Rotate")) //rotate parameter (720 still equals 360)
        {
            rotation90 += 90f;
            rotation120 += 120f;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green, 1f);
            if (hit.collider != null) // if we hit somethning
            {
                GameObject ship = Utility.FindParentWithTag(hit.collider.gameObject, "Ship");
                GameObject parentNode = Utility.FindParentWithTag(hit.collider.gameObject, "Node");
                GameObject port = Utility.FindParentWithTag(hit.collider.gameObject, "Port");
                if (ship != null && parentNode != null && port != null) //and its part of a node
                {
                    if (Input.GetButtonDown("Fire2")) // if right mouse button was pressed this update delte node that was hit
                    {
                        DestroyObject(parentNode);
                    }
                    else
                    {
                        if (Input.GetButtonDown("Fire1")) // if left mouse button was pressed this update create a node
                        {
                            ship.GetComponent<Ship>().AddNode(hit.collider.gameObject, selected.ToString(), (int)rotation90);
                        }
                        placementCursor.transform.rotation = port.transform.rotation;
                        placementCursor.transform.position = port.transform.position;
                        if (selected != nodeName.prisma || hit.collider.name == "PlateTriangle") //if were not a triangle or if the trinagle is hovering over a prisma shaped port; apply a diffrent rotation
                        {
                            placementCursor.transform.Rotate(Vector3.right * 90f); //no need to rotate since currently all nodes dont change on rotation
                            placementCursor.transform.Translate(Vector3.back * .5f, Space.Self);
                        }
                        else
                        {
                            placementCursor.transform.Rotate(new Vector3(0, rotation90, 0));
                            placementCursor.transform.Translate(Vector3.up * .2887f, Space.Self);

                        }
                        placementCursor.SetActive(true);
                    }
                }
            }
            else
                placementCursor.SetActive(false);
        }
        else
            placementCursor.SetActive(false);
    }
}


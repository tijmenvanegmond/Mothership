using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {

    public Color freeColor = Color.green;
    public Color occupiedColor = Color.red;
    public int selectedInt = 0; 
    public GameObject[] cursorObjects;
    public GameObject selectedObject {
        get { return cursorObjects[selectedInt]; }
    }
    private MeshCollider meshCol{
        get { return selectedObject.GetComponent<MeshCollider>(); }
    }       
    private bool isTriggered = false;
    public bool isOccupied {
        get{ return isTriggered; }
        private set{ isTriggered = value; }
    }
    public bool isFree{
        get { return !isOccupied; }
    }

    // Use this for initialization
    void Start () {
        Debug.Log("sup");
    }
    void FixedUpdate() {
        isOccupied = false;
    }
    void OnTriggerStay(Collider other)
    {
        isOccupied = true;
    }

    // Update is called once per frame
    void Update () {       
        if (isOccupied){
            selectedObject.GetComponent<MeshRenderer>().sharedMaterial.color = occupiedColor;
        } else selectedObject.GetComponent<MeshRenderer>().sharedMaterial.color = freeColor;
    }
}

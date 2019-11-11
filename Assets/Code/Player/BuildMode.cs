using UnityEngine;
public class BuildMode : MonoBehaviour {
    private Node selectedNode;
    private GameObject CursorGO;
    private Ship hitShip;
    private Node hitNode; //selected node to build on / delete
    private ConnectionPoint hitPort; //selected port to build on / delete
    private ConnectionPointType hitPortType; //{ get { return portTypes[hitPort.TypeID]; } }

    private int buildMask;

    public BuildMode (int buildMask) {
        this.buildMask = buildMask;
    }

    void Update () {
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast (ray, out hit, buildMask);

        //make sure there are no nulls
        if (!IsValidShipHit (hit)) //only continue if a viable (with port & ship parent) collider has been hit
        {
            CursorGO.SetActive (false);
            return;
        }

        if (!selectedNode.HasPortOfType (hitPort.TypeID)) {
            Debug.Log ("SelectedNode does not have a port of a matching type");
            CursorGO.SetActive (false);
            return;
        }

        //now assumed there are no nulls
        Debug.DrawLine (ray.origin, hit.point, Color.green, 1f);
        if (Input.GetButtonDown ("Fire2")) // if right mouse button was pressed this update delete node that was hit
            hitShip.RemoveNode (hitNode);
        else if (Input.GetButtonDown ("Fire1")) // if left mouse button was pressed this update create a node
        {
            UpdateCursorPlacement ();
            if (CursorGO.GetComponent<Cursor> ().IsFree) {
                var nodeGO = Instantiate (selectedNode.gameObject, hitShip.transform);
                nodeGO.transform.position = CursorGO.transform.position;
                nodeGO.transform.rotation = CursorGO.transform.rotation;
                nodeGO.transform.localScale = CursorGO.transform.localScale;
                hitShip.AddNode (nodeGO);
            } else
                Debug.Log ("Object:" + CursorGO.GetComponent<Cursor> ().Obstruction + "\n Is obstucting");
        } else {
            UpdateCursorPlacement ();
        }
    }

    private void renderCursor (GameObject CursorGO) {

    }

    private bool IsValidShipHit (RaycastHit hit) {
        if (hit.collider == null)
            return false;
        GameObject shipGameObject = Utility.FindParentWithTag (hit.collider.gameObject, "Ship");
        if (shipGameObject == null)
            return false;
        hitShip = shipGameObject.GetComponent<Ship> ();
        if (hitShip == null)
            return false;
        hitNode = Utility.FindParentWithTag (hit.collider.gameObject, "Node").GetComponent<Node> ();
        if (hitNode == null)
            return false;
        var portGO = Utility.FindParentWithTag (hit.collider.gameObject, "Port");
        if (portGO == null)
            return false;
        if (!hitNode.HasMatchingPort (portGO)) {
            Debug.Log ("Could not match portGO to port");
            return false;
        }
        hitPort = hitNode.GetMatchingPort (portGO);

        return true;
    }

    private void UpdateCursorPlacement () {
        //Calc node placement postion based on portPostions
        var newPort = GetSelectedPort ();
        var pivot = new GameObject ().transform;
        var t = CursorGO.transform;
        //reset cursor transform
        t.rotation = Quaternion.identity;
        t.position = Vector3.zero;
        t.localScale = Vector3.one;

        t.parent = pivot;
        t.localRotation = Quaternion.Inverse (newPort.Transform.localRotation); //rotate so that selectedport rotation is "zero'd"
        t.Translate (-newPort.Transform.localPosition, Space.Self);
        pivot.transform.rotation = hitPort.Transform.rotation;
        pivot.transform.position = hitPort.Transform.position;
        //rotate based on presetvalues
        pivot.transform.Rotate (hitPortType.PlacementRotation.x, hitPortType.PlacementRotation.y, hitPortType.PlacementRotation.z, Space.Self);
        t.transform.parent = null;
        Destroy (pivot.gameObject);

        CursorGO.transform.RotateAround (hitPort.Transform.position, hitPort.Transform.rotation * Vector3.up, Rotation * hitPortType.RotationStep);
        CursorGO.gameObject.SetActive (true);
    }

    /// <summary>
    /// get info of the current selected port of the selectednode (breaks on no matching porttypes)
    /// </summary>
    /// <returns></returns>
    private ConnectionPoint GetSelectedPort () {
        var matchingPorts = selectedNode.GetPortsOfType (hitPortType.ID);
        var amount = matchingPorts.Count ();
        return matchingPorts.ElementAt (PortNumber % amount);
    }

}

using UnityEngine;

public class PlacementCastResult {

}

public class PlacementCast {

    private int buildMask;
    public PlacementCast (int buildMask) {
        this.buildMask = buildMask;
    }

    public void getTarget () {
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast (ray, out hit, buildMask);

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
        //make sure there are no nulls
        if (!IsValidShipHit (hit)) //only continue if a viable (with port & ship parent) collider has been hit

            if (!selectedNode.HasPortOfType (hitPort.TypeID)) {
                Debug.Log ("SelectedNode does not have a port of a matching type");
                CursorGO.SetActive (false);
                return;
            }

        //now assumed there are no nulls
        Debug.DrawLine (ray.origin, hit.point, Color.green, 1f);

    }
}

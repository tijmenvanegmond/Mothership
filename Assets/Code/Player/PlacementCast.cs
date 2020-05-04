using UnityEngine;

public class PlacementCast {
    private int buildMask;
    public PlacementCast() {
        this.buildMask = NodeController.BuildMask;
    }

    public PlacementCastResult getTarget(Ray ray) {
        RaycastHit hit;
        Physics.Raycast(ray, out hit, buildMask);
        Debug.DrawLine(ray.origin, hit.point, Color.green, 1f);

        var result = new PlacementCastResult();
        if (hit.collider == null)
            return null;
        GameObject shipGameObject = Utility.FindParentWithTag(hit.collider.gameObject, "Ship");
        if (shipGameObject == null)
            return null;
        result.ship = shipGameObject.GetComponent<Ship>();
        if (result.ship == null)
            return null;
        result.node = Utility.FindParentWithTag(hit.collider.gameObject, "Node").GetComponent<Node>();
        if (result.node == null)
            return result;
        var portGO = Utility.FindParentWithTag(hit.collider.gameObject, "Port");
        if (portGO == null)
            return null;
        result.port = portGO.GetComponent<ConnectionPoint>();
        return result;
    }
}
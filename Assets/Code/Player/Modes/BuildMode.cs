using UnityEngine;
public class BuildMode : MonoBehaviour {
    private Node selectedNode;
    private GameObject CursorGO;
    private PlacementCast placementCast;
    public BuildMode(PlacementCast placementCast) {
        this.placementCast = placementCast;
    }

    void Update() {

        var result = this.placementCast.getTarget();
        if (result == null) {
            CursorGO.SetActive(false);
            return;
        }

        if (!selectedNode.HasPortOfType(result.port.TypeID)) {
            Debug.Log("SelectedNode does not have a port of a matching type");
            CursorGO.SetActive(false);
            return;
        }

        UpdateCursorPlacement(result);

    }

    private void UpdateCursorPlacement(PlacementCastResult result) {
        //Calc node placement postion based on portPostions
        var newPort = GetSelectedPort();
        var pivot = new GameObject().transform;
        var t = CursorGO.transform;
        //reset cursor transform
        t.rotation = Quaternion.identity;
        t.position = Vector3.zero;
        t.localScale = Vector3.one;

        t.parent = pivot;
        t.localRotation = Quaternion.Inverse(newPort.Transform.localRotation); //rotate so that selectedport rotation is "zero'd"
        t.Translate(-newPort.Transform.localPosition, Space.Self);
        pivot.transform.rotation = result.port.Transform.rotation;
        pivot.transform.position = result.port.Transform.position;
        //rotate based on presetvalues
        pivot.transform.Rotate(hitPortType.PlacementRotation.x, hitPortType.PlacementRotation.y, hitPortType.PlacementRotation.z, Space.Self);
        t.transform.parent = null;
        Destroy(pivot.gameObject);

        CursorGO.transform.RotateAround(hitPort.Transform.position, hitPort.Transform.rotation * Vector3.up, Rotation * hitPortType.RotationStep);
        CursorGO.gameObject.SetActive(true);
    }

    /// <summary>
    /// get info of the current selected port of the selectednode (breaks on no matching porttypes)
    /// </summary>
    /// <returns></returns>
    private ConnectionPoint GetSelectedPort() {
        var matchingPorts = selectedNode.GetPortsOfType(hitPortType.ID);
        var amount = matchingPorts.Count();
        return matchingPorts.ElementAt(PortNumber % amount);
    }

}

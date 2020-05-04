using System.Linq;
using UnityEngine;
public class NodePlacer : MonoBehaviour {
    private GameObject CursorGO;
    private PlacementCast placementCast;
    private int rotationStep = 0;
    private int portNumber = 0;
    private Node selectedNode;
    public Node SelectedNode {
        get => selectedNode;
        private set {
            selectedNode = value;
            UpdateCursorShape();
        }

    }

    public NodePlacer() {
        this.placementCast = new PlacementCast();
    }

    public void SetBuildNode(Node node) {
        SelectedNode = node;
    }

    public bool PlaceNode() {
        Debug.Log("Placing node");
        return true;
    }

    public void UpdateCursor(Ray ray) {
        var result = this.placementCast.getTarget(ray);
        if (result == null) {
            CursorGO.SetActive(false);
            return;
        }

        if (!SelectedNode.HasPortOfType(result.port.TypeID)) {
            Debug.Log("SelectedNode does not have a port of a matching type");
            CursorGO.SetActive(false);
            return;
        }
        UpdateCursorPlacement(result);

    }

    private void UpdateCursorShape() {
        if (CursorGO != null)
            Destroy(CursorGO);

        CursorGO = Instantiate(SelectedNode.BuildPreviewCollider);
        CursorGO.AddComponent<Cursor>();
        CursorGO.SetActive(false);
        CursorGO.name = "CURSOR";
    }

    private void UpdateCursorPlacement(PlacementCastResult result) {
        //Calc node placement postion based on portPostions
        var newPort = GetSelectedPort(result);
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
        pivot.transform.Rotate(result.portType.PlacementRotation.x, result.portType.PlacementRotation.y, result.portType.PlacementRotation.z, Space.Self);
        t.transform.parent = null;
        Destroy(pivot.gameObject);

        CursorGO.transform.RotateAround(result.port.Transform.position, result.port.Transform.rotation * Vector3.up, rotationStep * result.portType.RotationStep);
        CursorGO.gameObject.SetActive(true);
    }

    /// <summary>
    /// get info of the current selected port of the selectednode (breaks on no matching porttypes)
    /// </summary>
    /// <returns></returns>
    private ConnectionPoint GetSelectedPort(PlacementCastResult result) {
        var matchingPorts = SelectedNode.GetPortsOfType(result.port.TypeID);
        var amount = matchingPorts.Count();
        return matchingPorts.ElementAt(portNumber % amount);
    }

}
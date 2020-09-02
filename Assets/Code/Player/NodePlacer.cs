using System.Linq;
using UnityEngine;
public class NodePlacer : MonoBehaviour {
    private GameObject CursorGO;
    private int rotationStep = 0;
    private int portNumber = 0;
    private Node _selectedNode;
    public Node selectedNode {
        get => _selectedNode;
        private set {
            _selectedNode = value;
            UpdateCursorShape();
        }

    }

    public void Rotate() {
        rotationStep++;
    }

    public void SetBuildNode(Node node) {
        selectedNode = node;
    }

    public bool PlaceNode(PlacementCastResult target) {
        Debug.Log(target);
        if (target == null) {
            return false;
        }
        Debug.Log("Placing node");

        UpdateCursor(target);

        if (CursorGO.GetComponent<Cursor>().IsFree) {
            var nodeGO = Instantiate(selectedNode.gameObject, target.ship.transform);
            nodeGO.transform.position = CursorGO.transform.position;
            nodeGO.transform.rotation = CursorGO.transform.rotation;
            nodeGO.transform.localScale = CursorGO.transform.localScale;
            target.ship.AddNode(nodeGO);
            return false;
        } else {
            Debug.Log("Object:" + CursorGO.GetComponent<Cursor>().Obstruction + "\n Is obstucting");
            return true;
        }
    }

    public void UpdateCursor(PlacementCastResult result) {
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

    private void UpdateCursorShape() {
        if (CursorGO != null)
            Destroy(CursorGO);

        CursorGO = Instantiate(selectedNode.BuildPreviewCollider);
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
        var matchingPorts = selectedNode.GetPortsOfType(result.port.TypeID);
        var amount = matchingPorts.Count();
        return matchingPorts.ElementAt(portNumber % amount);
    }

}
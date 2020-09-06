using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildMode : MonoBehaviour {
    private PlayerControls controls;
    private NodePlacer nodePlacer;
    private PlacementCast placementCast;
    private int _selectedNode = 0;
    private int NODE_MAX = 8; //temp for easy loop around
    private int selectedNode {
        get => _selectedNode;
        set {
            if (value < 0) {
                _selectedNode = NODE_MAX;
            } else if (value > NODE_MAX) {
                _selectedNode = 0;
            } else {
                _selectedNode = value;
            }
        }
    }

    public void Awake() {
        controls = new PlayerControls();

        controls.Build.Place.performed += ctx => this.PlaceNode();

        controls.Build.Remove.performed += ctx => this.RemoveNode();

        controls.Build.Rotate.performed += ctx => this.nodePlacer.Rotate();

        controls.Build.ChangePort.performed += ctx => this.nodePlacer.ChangePort();

        controls.Build.CycleNodeSelection.performed += ctx => CycleNodeSelection(ctx.ReadValue<float>());

    }

    private void CycleNodeSelection(float delta) {
        if (delta > 0) {
            selectedNode++;
        } else if (delta < 0) {
            selectedNode--;
        }

        nodePlacer.SetBuildNode(NodeController.GetNode(selectedNode));
    }

    private void RemoveNode() {

        Ray ray = new Ray(transform.position, transform.forward); //Camera.main.ScreenPointToRay(new Vector2(.5f, .5f)); //Input.mousePosition);
        var target = placementCast.getTarget(ray);
        if (target != null) {
            Debug.Log("Removing node");
            nodePlacer.RemoveNode(target);
        } else {
            Debug.Log("No target found");
        }
    }

    private void PlaceNode() {
        Ray ray = new Ray(transform.position, transform.forward); //Camera.main.ScreenPointToRay(new Vector2(.5f, .5f)); //Input.mousePosition);
        var target = placementCast.getTarget(ray);
        if (target != null) {
            Debug.Log("Placing node");
            nodePlacer.PlaceNode(target);
        } else {
            Debug.Log("No target found");
        }
    }

    public void Start() {
        placementCast = new PlacementCast();
        nodePlacer = gameObject.AddComponent<NodePlacer>();
        nodePlacer.SetBuildNode(NodeController.GetNode(selectedNode));
    }

    public void Update() {
        Ray ray = new Ray(transform.position, transform.forward); //Camera.main.ScreenPointToRay(new Vector2(.5f, .5f)); //Input.mousePosition);
        var target = placementCast.getTarget(ray);
        nodePlacer.UpdateCursor(target);
    }

    void OnEnable() {
        if (controls != null)
            controls.Build.Enable();

    }

    void OnDisable() {
        if (controls != null)
            controls.Build.Disable();

    }

}
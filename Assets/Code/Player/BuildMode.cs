using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildMode : MonoBehaviour {
    public GameObject UISelectedNodePlace;
    private PlayerControls controls;
    private NodePlacer nodePlacer;
    private PlacementCast placementCast;
    private int __selectedNodeIndex = 0;
    private int NODE_MAX_INDEX = 8; //temp for easy loop around
    private int _selectedNodeIndex {
        get => __selectedNodeIndex;
        set {
            if (value < 0) {
                __selectedNodeIndex = NODE_MAX_INDEX;
            } else if (value > NODE_MAX_INDEX) {
                __selectedNodeIndex = 0;
            } else {
                __selectedNodeIndex = value;
            }
        }
    }

    private Node _selectedNode;
    public Node selectedNode { 
        get=> _selectedNode;
        private set {
            _selectedNode = value;
            UpdateNodePreview(value);
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

    private void UpdateNodePreview(Node node){
        foreach(Transform child in UISelectedNodePlace.transform)
        {
            Destroy(child.gameObject);
        }

        var nodeGO = Instantiate(selectedNode.gameObject, UISelectedNodePlace.transform);
        nodeGO.layer = UISelectedNodePlace.layer;
        foreach(Transform child in nodeGO.transform)
        {
            child.gameObject.layer = UISelectedNodePlace.layer;
        }
        nodeGO.AddComponent<RotateAnim>();

    }

    private void CycleNodeSelection(float delta) {
        if (delta > 0) {
            _selectedNodeIndex++;
        } else if (delta < 0) {
            _selectedNodeIndex--;
        }

        selectedNode = NodeController.GetNode(_selectedNodeIndex);
        nodePlacer.SetBuildNode(selectedNode);
    }

    public void Start() {
        placementCast = new PlacementCast();
        nodePlacer = gameObject.AddComponent<NodePlacer>();
        CycleNodeSelection(0f);
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

   

}
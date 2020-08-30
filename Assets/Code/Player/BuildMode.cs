using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildMode : MonoBehaviour {
    private PlayerControls controls;

    private NodePlacer nodePlacer;
    private PlacementCast placementCast;

    public void Awake() {
        controls = new PlayerControls();

        controls.Build.Place.performed += ctx => this.PlaceNode();
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
        nodePlacer.SetBuildNode(NodeController.GetNode(0));
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
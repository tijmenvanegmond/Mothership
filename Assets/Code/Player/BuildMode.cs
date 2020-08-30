using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildMode : MonoBehaviour {
    private PlayerControls controls;

    private NodePlacer nodePlacer;

    public void Awake() {
        controls = new PlayerControls();

        controls.Build.Place.performed += ctx => nodePlacer.PlaceNode();
    }

    public void Start() {
        nodePlacer = gameObject.AddComponent<NodePlacer>();
        nodePlacer.SetBuildNode(NodeController.GetNode(0));
    }

    public void Update() {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(.5f, .5f)); //Input.mousePosition);
        nodePlacer.UpdateCursor(ray);
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
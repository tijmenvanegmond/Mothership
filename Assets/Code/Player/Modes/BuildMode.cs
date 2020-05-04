using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildMode : MonoBehaviour {

    private NodePlacer nodePlacer;

    public void OnFire() {
        nodePlacer.PlaceNode();
    }

    public void OnMove(InputValue input) {
        // Debug.Log(input.Get<Vector2>());
    }

    public void OnLook(InputValue input) {
        // Debug.Log(input.Get());
    }

    public void Start() {
        nodePlacer = gameObject.AddComponent<NodePlacer>();
        nodePlacer.SetBuildNode(NodeController.GetNode(0));
    }

    public void Update() {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(.5f, .5f)); //Input.mousePosition);
        nodePlacer.UpdateCursor(ray);
    }

}
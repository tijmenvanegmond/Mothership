using System.Linq;
using UnityEngine;
public class BuildMode : MonoBehaviour {

    private NodePlacer nodePlacer;
    //private Player player;
    //private PlayerNodeSelector
    //private InvetoryManager;

    public BuildMode() {
        nodePlacer = new NodePlacer();
    }

    public void Update() {
        nodePlacer.UpdateCursor();
    }

}
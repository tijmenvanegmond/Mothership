using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node : MonoBehaviour {
    [HideInInspector]
    public int ID;
    public string Name;
    public float Mass = 1;

    [HideInInspector]
    public bool Rendered;
    public GameObject BuildPreviewCollider;
    [SerializeField]
    private ConnectionPoint[] portCollection;

    public Node[] GetConnectedNodes() {
        return portCollection.Where(x => x.Connection != null).Select(x => x.Connection).ToArray();
    }

    public bool HasPortOfType(int typeID) {
        return portCollection.Any(x => x.TypeID == typeID);
    }

    public bool HasMatchingPort(GameObject portGO) {
        return portCollection.Any(x => x.Transform.gameObject == portGO);
    }

    public ConnectionPoint GetMatchingPort(GameObject portGO) {
        return portCollection.First(x => x.Transform.gameObject == portGO);
    }

    public IEnumerable<ConnectionPoint> GetPortsOfType(int id) {
        return portCollection.Where(x => x.TypeID == id);
    }

    public void AddNodeConnection(int localIndex, Node oppositeNode, int oppositeNodeIndex) {
        if (portCollection[localIndex].Connection != null)
            return;
        portCollection[localIndex].Connection = oppositeNode;
        Utility.ChildrenSetActive(portCollection[localIndex].Transform, false);
        oppositeNode.AddNodeConnection(oppositeNodeIndex, this, localIndex);
    }

    public void RemoveNodeConnectionWith(Node node) {
        for (int i = 0; i < portCollection.Length; i++) {
            if (portCollection[i].Connection != node)
                continue;
            portCollection[i].Connection = null;
            Utility.ChildrenSetActive(portCollection[i].Transform, true);
            node.RemoveNodeConnectionWith(this);
            return;
        }
        return;
    }

    public void Awake() {
        gameObject.tag = "Node";

        GameObject defaultPortColliderGO = GetDefaultColliderGO();

        for (int i = 0; i < portCollection.Length; i++) {
            var port = portCollection[i];
            port.Index = i; //assign indexes

            //add default colliders if ports do not have them
            if (port.Transform.childCount != 0)
                continue;
            var portGO = Instantiate(defaultPortColliderGO, port.Transform);
            portGO.transform.localPosition = Vector3.zero;
        }

        Destroy(defaultPortColliderGO);
    }

    public virtual void Update() {
        ///DEBUG
        foreach (var port in portCollection) {
            var c = Color.cyan;
            if (port.Transform.localScale.y < 1f)
                c = Color.magenta;
            Debug.DrawLine(port.Transform.position, port.Transform.TransformPoint(Vector3.up * .5f), c);
        }
    }

    private static GameObject GetDefaultColliderGO() {
        //A default portBuildCollider for each port (of node)
        var defaultPortColliderGO = new GameObject();
        //var defaultPortBuildCollider = defaultPortColliderGO.AddComponent<BoxCollider>() as BoxCollider;
        //defaultPortBuildCollider.center = new Vector3(0, .025f, 0);
        //defaultPortBuildCollider.size = new Vector3(.5f, .05f, .5f);
        var defaultPortBuildCollider = defaultPortColliderGO.AddComponent<SphereCollider>() as SphereCollider;
        defaultPortBuildCollider.radius = .2f;
        defaultPortColliderGO.layer = NodeController.BuildLayer;
        defaultPortColliderGO.name = "portBuildCollider";
        return defaultPortColliderGO;
    }

    /// <summary>
    /// Casts a overlap sphere to see if there is a port of the same type close enough
    /// </summary>
    /// <param name="portInfo"></param>
    /// <param name="oppositePortInfo"></param>
    /// <param name="hitColliders"> optional collider array to save allocation</param>
    /// <returns>return true if its found a match</returns>
    bool GetOppositePort(ConnectionPoint portInfo, out ConnectionPoint oppositePortInfo, Collider[] hitColliders = null) {
        Node oppisiteNode;
        return GetOppositePort(portInfo, out oppositePortInfo, out oppisiteNode, hitColliders);
    }

    /// <summary>
    /// Casts a overlap sphere to see if there is a port of the same type close enough
    /// </summary>
    /// <param name="portInfo"></param>
    /// <param name="oppositePortInfo"></param>
    /// <param name="oppositeNode"></param>
    /// <param name="hitColliders"> optional collider array to save allocation</param>
    /// <returns>return true if its found a match</returns>
    bool GetOppositePort(ConnectionPoint portInfo, out ConnectionPoint oppositePortInfo, out Node oppositeNode, Collider[] hitColliders = null) {
        hitColliders = hitColliders ?? new Collider[8];
        var hitAmount = Physics.OverlapSphereNonAlloc(portInfo.Transform.position, .01f, hitColliders, NodeController.BuildMask, QueryTriggerInteraction.Ignore);
        for (var i = 0; i < hitAmount; i++) {
            var hitCollider = hitColliders[i];
            var hitNodeGO = Utility.FindParentWithTag(hitCollider.gameObject, "Node");
            if (hitNodeGO == null || hitNodeGO == gameObject)
                continue;
            oppositeNode = hitNodeGO.GetComponent<Node>();
            var hitPortGO = hitCollider.transform.parent.gameObject;
            if (!oppositeNode.HasMatchingPort(hitPortGO))
                continue;
            oppositePortInfo = oppositeNode.GetMatchingPort(hitPortGO);
            if (portInfo.TypeID != oppositePortInfo.TypeID)
                continue;
            return true;
        }
        oppositePortInfo = new ConnectionPoint();
        oppositeNode = null;
        return false;
    }

    internal bool HasViableConnections() //TODO: implement required connections
    {
        var hitColliders = new Collider[8];
        foreach (var portInfo in portCollection) {
            ConnectionPoint oppositePortInfo;
            if (!GetOppositePort(portInfo, out oppositePortInfo, hitColliders))
                continue;

            if (oppositePortInfo.Connection == null)
                return true;
        }
        return false;
    }

    internal void ConnectPorts() {
        var hitColliders = new Collider[8];
        foreach (var portInfo in portCollection) {
            ConnectionPoint oppositePortInfo;
            Node oppositeNode;
            if (!GetOppositePort(portInfo, out oppositePortInfo, out oppositeNode, hitColliders))
                continue;

            AddNodeConnection(portInfo.Index, oppositeNode, oppositePortInfo.Index);
        }
    }

    internal void DisconnectPorts() {
        foreach (var portInfo in portCollection) {
            if (portInfo.Connection == null)
                continue;

            RemoveNodeConnectionWith(portInfo.Connection);
        }
    }

    /// <summary>
    /// Checks if neigbours still have a connections path to each other, groups those who are connected.
    /// </summary>
    /// <returns>Returns a list of nodeHashSets that are connected to each other</returns>
    internal List<HashSet<Node>> CheckIfNodeABridge() //TODO: name not descriptive
    {
        var disconnectedNodeSetList = new List<HashSet<Node>>();
        var neigbours = GetConnectedNodes();
        if (neigbours.Length <= 1) //cant be a bridge if it's conected to 1 or less nodes
            return disconnectedNodeSetList;

        //split ship into connected groups
        foreach (var nNode in neigbours) {
            var isRepresented = false;
            foreach (var set in disconnectedNodeSetList) //check if there's already a group with this node
            {
                if (set.Contains(nNode)) {
                    isRepresented = true;
                    break;
                }
            }
            if (!isRepresented)
                disconnectedNodeSetList.Add(GetFloodfillGroup(nNode, this));
        }

        return disconnectedNodeSetList;
    }

    /// <summary>
    /// Makes a group of all nodes that are connected to this node
    /// </summary>
    /// <param name="start"></param>
    /// <param name="ignoreNode">node that the algoritm ignores</param>
    /// <returns></returns>
    private HashSet<Node> GetFloodfillGroup(Node start, Node ignoreNode = null) {
        var set = new HashSet<Node>();
        set.Add(start);
        AddConnectionsToHashSet(start, ref set, ignoreNode);
        return set;
    }

    /// <summary>
    /// Recursion-like method that adds all connected to the set
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="set"></param>
    /// <param name="ignoreNode">node that the algoritm ignores</param>
    private void AddConnectionsToHashSet(Node parent, ref HashSet<Node> set, Node ignoreNode = null) {
        foreach (var child in parent.GetConnectedNodes()) {
            if (child == ignoreNode)
                continue;
            if (set.Contains(child))
                continue;
            set.Add(child);
            AddConnectionsToHashSet(child, ref set, ignoreNode);
        }
    }

    /// <summary>
    /// Uses Breadth First to see if there's a path from start to end
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="useSelf">determines if the algorithm is allowed to traverse the node that calls this function</param>
    /// <returns></returns>
    private bool BreadthFirstNodeSearch(Node start, Node end, out HashSet<Node> passedSet, bool useSelf = true) //TODO: fix possible endless loops
    {
        passedSet = new HashSet<Node>();
        var queue = new Queue<Node>();
        queue.Enqueue(start);

        while (queue.Any()) {
            var subNode = queue.Dequeue();

            if (subNode == end)
                return true;
            foreach (var child in subNode.GetConnectedNodes()) {
                //check if algorithm can navigate the node that called it, if not, check for that node;
                if (!useSelf && child == this)
                    continue;
                if (passedSet.Contains(child))
                    continue;
                if (queue.Contains(child))
                    continue;
                queue.Enqueue(child);
            }

            passedSet.Add(subNode);
        }
        return false;
    }

    public void Remove() {
        DisconnectPorts();
        Destroy(gameObject);
    }
}
public class PlacementCastResult {
    public Ship ship;
    public Node node; //selected node to build on / delete
    public ConnectionPoint port; //selected port to build on / delete
    public ConnectionPointType portType { get { return NodeController.PortTypeDict[port.TypeID]; } }
}
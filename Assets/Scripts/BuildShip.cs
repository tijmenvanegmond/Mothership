using UnityEngine;

namespace Assets.Scripts
{
    public class BuildShip : MonoBehaviour
    {
        public GameObject PlacementCursor;
        public Cursor Cursor;
        public NodeController.NodeName Selected = NodeController.NodeName.prisma;
        public int Rotation = 0;
        private Ship _ship;
        private GameObject _node; //selected node to build on / delete
        private GameObject _port; //selected port to build on / delete

        void Start()
        {
            Selected = NodeController.NodeName.prisma;
            Cursor = PlacementCursor.GetComponent<Cursor>();
        }

        private void UpdateSelectedNode(NodeController.NodeName node)
        {
            Selected = node;
            Cursor.SetCursor(node);
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Weapon1")) //then we use a prisma
            {
                UpdateSelectedNode(NodeController.NodeName.prisma);
            }
            else if (Input.GetButtonDown("Weapon2")) //now we use a cube
            {
                UpdateSelectedNode(NodeController.NodeName.cube);
            }
            else if (Input.GetButtonDown("Weapon3")) //now we use a cube
            {
                UpdateSelectedNode(NodeController.NodeName.cylinder);
            }
            else if (Input.GetButtonDown("Weapon4")) //now we use a cube
            {
                UpdateSelectedNode(NodeController.NodeName.slope);
            }

            if (Input.GetButtonDown("Rotate"))
                Rotation++;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            if (IsValidShipHit(hit))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green, 1f);
                if (Input.GetButtonDown("Fire2")) // if right mouse button was pressed this update delete node that was hit
                    _ship.RemoveNode(_node);
                else if (Input.GetButtonDown("Fire1") && Cursor.IsFree) // if left mouse button was pressed this update create a node
                    _ship.AddNode(hit.collider.gameObject, Selected.ToString(), Rotation);
                else
                    UpdateCursorPlacement(hit);
            }
            else
                PlacementCursor.SetActive(false);
            
        }

        private bool IsValidShipHit(RaycastHit hit)
        {
            if (hit.collider == null)
                return false;

            GameObject shipGameObject = Utility.FindParentWithTag(hit.collider.gameObject, "Ship");
            if (shipGameObject == null)
                return false;

            _ship = shipGameObject.GetComponent<Ship>();
            _node = Utility.FindParentWithTag(hit.collider.gameObject, "Node");
            _port = Utility.FindParentWithTag(hit.collider.gameObject, "Port");

            return _ship != null && _node != null && _port != null; //if nothing is null the hit is valid
        }

        private void UpdateCursorPlacement(RaycastHit hit)
        {
            PlacementCursor.transform.rotation = _port.transform.rotation;
            PlacementCursor.transform.position = _port.transform.position;

            if (Selected == NodeController.NodeName.prisma && hit.collider.name == "PlateTriangle")
            {
                PlacementCursor.transform.Rotate(new Vector3(90f, Rotation * 120f,
                    0));
                PlacementCursor.transform.Translate(Vector3.back * .5f, Space.Self);
            }
            else if (Selected == NodeController.NodeName.prisma)
            {
                PlacementCursor.transform.Rotate(new Vector3(0, Rotation * 90f, 0));
                PlacementCursor.transform.Translate(Vector3.up * .2887f, Space.Self);
            }
            else
            {
                PlacementCursor.transform.Rotate(new Vector3(90f, Rotation * 90f,
                    0));
                PlacementCursor.transform.Translate(Vector3.back * .5f, Space.Self);
            }
            PlacementCursor.SetActive(true);
        }
    }
}


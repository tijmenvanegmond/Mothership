using UnityEngine;

namespace Assets.Scripts
{
    public class Cursor : MonoBehaviour
    {
        public bool IsOccupied { get; private set; }
        public bool IsFree{get { return !IsOccupied; }}
        public Color FreeColor = Color.green;
        public Color OccupiedColor = Color.red;
        public int SelectedInt = 0;
        public GameObject[] CursorObjects;
        public GameObject SelectedObject
        {
            get { return CursorObjects[SelectedInt]; }
        }
        private MeshCollider MeshCol
        {
            get { return SelectedObject.GetComponent<MeshCollider>(); }
        }

        public Cursor()
        {
            IsOccupied = false;
        }

        public void SetCursor(NodeName newNode)
        {
            SelectedInt = (int)newNode;
            string nodeName = newNode.ToString();
            foreach (GameObject child in CursorObjects)
            {
                if (nodeName != "" && child.name == nodeName)
                {
                    child.SetActive(true);
                }
                else
                {
                    child.SetActive(false);
                }
            }
        }

        void FixedUpdate()
        {
            IsOccupied = false;
        }
        void OnTriggerStay(Collider other)
        {
            IsOccupied = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (IsOccupied)
            {
                if (SelectedObject.tag == "Cursor")
                    SelectedObject.GetComponent<MeshRenderer>().sharedMaterial.color = OccupiedColor;
            }
            else
            {
                if (SelectedObject.tag == "Cursor")
                    SelectedObject.GetComponent<MeshRenderer>().sharedMaterial.color = FreeColor;
            }
        }
    }
}

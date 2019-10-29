using UnityEngine;

namespace Assets.Scripts
{
    public class Cursor : MonoBehaviour
    {
        public bool IsOccupied { get; private set; }
        public bool IsFree { get { return !IsOccupied; } }
        public Color FreeColor = Color.green;
        public Color OccupiedColor = Color.red;
        public Collider Obstruction;

        public Cursor()
        {
            IsOccupied = false;
        }

        void FixedUpdate()
        {
            IsOccupied = false;
        }

        private void Start()
        {
            if (gameObject.GetComponent<Rigidbody>() == null)
            {
                var rB = gameObject.AddComponent<Rigidbody>() as Rigidbody; //cursor trigger needs a rigidbody to detect non rigidbodies
                rB.isKinematic = true;
            }
        }

        void OnTriggerStay(Collider other)
        {
            IsOccupied = true;
            Obstruction = other;
        }

        // Update is called once per frame
        void Update()
        {
            gameObject.GetComponent<MeshRenderer>().sharedMaterial.color =
                IsOccupied ? OccupiedColor : FreeColor;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts {
    public class Ship : MonoBehaviour {
        //Max speed in m/s
        //TODO: it's only used for UI V-arrow
        public const float MAX_SPEED = 50f;
        public Rigidbody rBody;
        Dictionary<int, Node> nodes;
        HashSet<Node> myNodes;
        public Truster[] Trusters = new Truster[0];

        public void Start () {
            nodes = NodeController.NodeDict;
            myNodes = new HashSet<Node> ();
            rBody = GetComponent<Rigidbody> ();
            if (rBody == null) {
                rBody = gameObject.AddComponent<Rigidbody> () as Rigidbody;
                rBody.drag = 0;
                rBody.angularDrag = .3f;
                rBody.useGravity = false;
                rBody.interpolation = RigidbodyInterpolation.Interpolate;
                rBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }

            AddChildNodesToMyNodes ();
        }

        private void AddChildNodesToMyNodes () {
            foreach (Transform child in transform)
                AddNode (child.gameObject, false);
        }

        public void UpdateShip () {
            CalculateMass ();
            UpdateTrusters ();
        }

        float CalculateMass () {
            float totalMass = 0;
            Vector3 centerOfMass = Vector3.zero;
            foreach (var node in myNodes) {
                totalMass += node.Mass;
                centerOfMass += node.transform.localPosition * node.Mass;
            }
            rBody.mass = totalMass;
            rBody.centerOfMass = centerOfMass = centerOfMass / totalMass;
            return totalMass;
        }

        void UpdateTrusters () {
            Trusters = myNodes.OfType<Truster> ().ToArray ();
        }

        public bool AddNode (GameObject newNode, bool checkPlacement = true) {
            var nodeComponent = newNode.GetComponent<Node> ();
            if (nodeComponent == null)
                return false;
            return AddNode (nodeComponent, checkPlacement);
        }

        public bool AddNode (Node newNode, bool checkPlacement = true) {
            if (checkPlacement) {
                //Check node placement legit
                var newNodeGO = newNode.gameObject;
                //TODO: check for collisions
                if (newNodeGO.transform.parent != transform || !newNode.HasViableConnections ()) {
                    Debug.Log ("Node placement failed. \n Node not a child of expected ship or no viable connections.");
                    Destroy (newNodeGO);
                    return false;
                }
            }
            newNode.ConnectPorts ();
            myNodes.Add (newNode);
            UpdateShip ();
            return true;
        }

        public void RemoveNode (Node node) {
            //Check node connections
            //TODO: the ship should remain the group where the playerchar is seated
            var nodeSetList = node.CheckIfNodeABridge ().OrderByDescending (g => g.Count ()); //biggest first
            if (nodeSetList.Count () > 1) {
                //bigest group remains the ship
                myNodes = nodeSetList.First ();
                //split up the ship into groups created
                foreach (var set in nodeSetList.Skip (1)) {
                    var newShipGO = new GameObject ();
                    newShipGO.name = name + "-breakoff";
                    var newShip = newShipGO.AddComponent<Ship> ();
                    foreach (var aNode in set)
                        aNode.transform.parent = newShipGO.transform;
                    //TODO: add velocity based on angularVelocity
                    //And better rbody way
                    var newRBody = newShipGO.AddComponent<Rigidbody> () as Rigidbody;
                    newRBody.velocity = rBody.velocity;
                }
            }

            myNodes.Remove (node);
            node.Remove ();
            //TODO: recalculate ship properties
            UpdateShip ();
        }

        ///DEBUG
        void OnDrawGizmosSelected () {
            Gizmos.color = Color.yellow;
            if (rBody != null)
                Gizmos.DrawSphere (transform.TransformPoint (rBody.centerOfMass), rBody.mass * .05f);
        }

    }
}

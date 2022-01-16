using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ship : Construct { //should implement construct
    //Max speed in m/s
    //TODO: it's only used for UI V-arrow
    public const float MAX_SPEED = 50f;
    public Rigidbody rBody;
    public Truster[] Trusters { get; private set; }

    public override void Start() {
        rBody = GetComponent<Rigidbody>();
        if (rBody == null) {
            rBody = gameObject.AddComponent<Rigidbody>() as Rigidbody;
        }
        rBody.drag = 0;
        rBody.angularDrag = .3f;
        rBody.useGravity = false;
        rBody.interpolation = RigidbodyInterpolation.Interpolate;
        rBody.collisionDetectionMode = CollisionDetectionMode.Continuous;

        base.Start();

    }

    protected override IEnumerable<GameObject> HandleBridgeRemoval(IEnumerable<HashSet<Node>> nodeSetList) {
        var breakoffParts = base.HandleBridgeRemoval(nodeSetList);
        foreach (var partGO in breakoffParts) {
            var newRBody = partGO.AddComponent<Rigidbody>() as Rigidbody;
            newRBody.velocity = rBody.velocity;
        }
        return breakoffParts;
    }

    protected override void UpdateConstruct() {
        CalculateMass();
        UpdateTrusters();
    }

    private float CalculateMass() {
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

    private void UpdateTrusters() {
        Trusters = myNodes.OfType<Truster>().ToArray();
    }

    ///DEBUG
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        if (rBody != null)
            Gizmos.DrawSphere(transform.TransformPoint(rBody.centerOfMass), rBody.mass * .05f);
    }

}
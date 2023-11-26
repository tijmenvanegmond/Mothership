using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Rigidbody player_rBody;
    public const float maxSpeed = 50f;
    public GameObject PreviewNode;    
    public Text UINodeName;
    public Text UISpeedometer;
    public Text UIDampeningIndicator;
    public GameObject UIVelocityArrow;
    private ArrowRenderer arrowRenderer;

    void Start()
    {
        if (player_rBody == null)
        {
            Debug.LogError("Player_rBody must be assigned for UI to work");
        }

        arrowRenderer = UIVelocityArrow.GetComponent<ArrowRenderer>();
    }

    void Update()
    {
        //Speed
        var speed = player_rBody.velocity.magnitude;
        UISpeedometer.text = (speed * 3.6f).ToString("0.0") + "km/h";
        //damping
        var isDamping = player_rBody.drag > 0f;
        UIDampeningIndicator.text = isDamping ? "Damping : ON" : "Damping : OFF";
        UIDampeningIndicator.color = isDamping ? Color.cyan : new Color(1f, .2f, 0f);

        //Arrow ui
        if (player_rBody.velocity != Vector3.zero)
            UIVelocityArrow.transform.rotation = Quaternion.LookRotation(player_rBody.velocity);

        var arrowScalar = Mathf.Sqrt(speed / maxSpeed);
        var halfDot = Vector3.Dot(player_rBody.transform.forward, player_rBody.velocity.normalized)/2f;     
        var arrowColor = new Color(.5f  - halfDot, .5f + halfDot, .5f + halfDot);
        arrowRenderer.UpdateArrow(arrowScalar,arrowColor );
        
        //Node Preview
        Node previewNode = PreviewNode.GetComponentInChildren<Node>();
        UINodeName.text = previewNode.Name;

    }
}

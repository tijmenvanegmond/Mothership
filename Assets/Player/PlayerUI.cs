using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Rigidbody player_rBody;
    public const float maxSpeed = 50f;
    public GameObject UISpeedometer;
    public GameObject UIDampeningIndicator;
    public GameObject UIVelocityArrow;
    private ArrowRenderer arrowRenderer;
    private Text speedometerText;
    private Text dampeningIndicatorText;

    void Start()
    {
        if (player_rBody == null)
        {
            Debug.LogError("Player_rBody must be assigned for UI to work");
        }

        speedometerText = UISpeedometer.GetComponent<Text>();
        dampeningIndicatorText = UIDampeningIndicator.GetComponent<Text>();
        arrowRenderer = UIVelocityArrow.GetComponent<ArrowRenderer>();
    }

    void Update()
    {
        var speed = player_rBody.velocity.magnitude;
        speedometerText.text = "Speed : " + (speed * 3.6f).ToString("0.0") + "km/h";
        //Player_rBody.rigidbody.d
        //dampeningIndicatorText.text = isDampening ? "Dampening : ON" : "Dampening : OFF";
        //dampeningIndicatorText.color = isDampening ? Color.cyan : new Color(1f, .2f, 0f);

        //Arrow ui
        if (player_rBody.velocity != Vector3.zero)
            UIVelocityArrow.transform.rotation = Quaternion.LookRotation(player_rBody.velocity);

        var arrowScalar = Mathf.Sqrt(speed / maxSpeed);
        var halfDot = Vector3.Dot(player_rBody.transform.forward, player_rBody.velocity.normalized)/2f;     
        var arrowColor = new Color(.5f  - halfDot, .5f + halfDot, .5f + halfDot);
        arrowRenderer.UpdateArrow(arrowScalar,arrowColor );
    }
}

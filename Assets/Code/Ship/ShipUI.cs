using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipUI : MonoBehaviour {

    public Rigidbody rBody;
    public const float maxSpeed = 50f;
    public GameObject UISpeedometer;
    public GameObject UIDampeningIndicator;
    public GameObject UIVelocityArrow;
    private ArrowRenderer arrowRenderer;
    private Text speedometerText;
    private Text dampeningIndicatorText;

    void Start() {
        if (rBody == null) {
            Debug.LogError("Ship must be assigned for UI to work");
        }

        speedometerText = UISpeedometer.GetComponent<Text>();
        dampeningIndicatorText = UIDampeningIndicator.GetComponent<Text>();
        arrowRenderer = UIVelocityArrow.GetComponent<ArrowRenderer>();
    }

    void Update() {

        var speed = rBody.velocity.magnitude;
        speedometerText.text = "Speed : " + (speed * 3.6f).ToString("0.0") + "km/h";
        //dampeningIndicatorText.text = isDampening ? "Dampening : ON" : "Dampening : OFF";
        //dampeningIndicatorText.color = isDampening ? Color.cyan : new Color(1f, .2f, 0f);

        //Arrow ui
        if (rBody.velocity != Vector3.zero)
            UIVelocityArrow.transform.rotation = Quaternion.LookRotation(rBody.velocity);
        var arrowScalar = Mathf.Sqrt(speed / maxSpeed);
        arrowRenderer.length = Mathf.Min(1f, arrowScalar * 2f);
        arrowRenderer.radius = Mathf.Min(.4f, arrowScalar * 1.2f);
    }
}
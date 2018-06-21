using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShipController : MonoBehaviour
{
	public GameObject Camera;
	public GameObject UISpeedometer;
	public GameObject UIDampeningIndicator;
	public GameObject UIDirectionIndicator;
	private ArrowRenderer arrowRenderer;
	private Text speedometerText;
	private Text dampeningIndicatorText;
	public bool RotateWithCamera = true;
	public float RotationSpeed = 1f;
	public Vector3 MovementMultiplier = Vector3.one;
	public Vector3 RotationMultiplier = Vector3.one;
	private Vector3 movementInputVector;
	private Vector3 rotationInputVector;
	private bool isDampening = false;
	private Rigidbody rBody;
	//Max speed in m/s
	//TODO: it's only used for V-arrow
	private float maxSpeed = 50f;
	private float drag;
	private float angularDrag;

	void Start()
	{
		//Cursor.visible = false;
		//Cursor.lockState = CursorLockMode.Locked;
		rBody = GetComponent<Rigidbody>();
		//store defaults
		drag = rBody.drag;
		angularDrag = rBody.angularDrag;
		//Get UI components TODO: put UI code somewhere better
		speedometerText = UISpeedometer.GetComponent<Text>();
		dampeningIndicatorText = UIDampeningIndicator.GetComponent<Text>();
		arrowRenderer = UIDirectionIndicator.GetComponent<ArrowRenderer>();
	}

	void Update()
	{
		movementInputVector = new Vector3(Input.GetAxis("ShipXAxis"), Input.GetAxis("ShipYAxis"), Input.GetAxis("ShipThrottle"));
		rotationInputVector = new Vector3(Input.GetAxis("ShipPitch"), Input.GetAxis("ShipYaw"), Input.GetAxis("ShipRoll"));

		if (Input.GetButtonUp("Dampening")) isDampening = !isDampening;
		if (Input.GetMouseButtonUp(2)) RotateWithCamera = !RotateWithCamera;

		if (isDampening)
		{
			rBody.angularDrag = .8f;
			rBody.drag = .2f;
		}
		else
		{
			rBody.drag = drag;
			rBody.angularDrag = angularDrag;
		}

		//UI update
			var speed = rBody.velocity.magnitude;
			speedometerText.text = "Speed : " + (speed * 3.6f).ToString("0.0") + "km/h";
			dampeningIndicatorText.text = isDampening ? "Dampening : ON" : "Dampening : OFF";
			dampeningIndicatorText.color = isDampening ? Color.blue : Color.red;
			//arrow ui
			UIDirectionIndicator.transform.rotation = Quaternion.LookRotation(rBody.velocity);
			var arrowScalar = Mathf.Sqrt(speed / maxSpeed);
			arrowRenderer.length = Mathf.Min(1f, arrowScalar*2f);
			arrowRenderer.radius = Mathf.Min(.4f, arrowScalar*1.2f);
	}

	void FixedUpdate()
	{
		if (RotateWithCamera)
		{
			rBody.MoveRotation(Quaternion.Slerp(transform.rotation, Camera.transform.rotation,
				Time.fixedDeltaTime * RotationSpeed));
		}

		rBody.AddRelativeForce(Vector3.Scale(movementInputVector, MovementMultiplier) * Time.fixedDeltaTime);
	}
}

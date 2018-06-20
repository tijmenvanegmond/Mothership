using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShipController : MonoBehaviour
{
	public GameObject Camera;
	public GameObject UISpeedometer;
	public GameObject UIDampeningIndicator;
	public GameObject UIDirectionIndicator;
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
	private float drag;
	private float angularDrag;

	void Start() // Use this for initialization
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
	}

	void Update() // Update is called once per frame
	{
		movementInputVector = new Vector3(Input.GetAxis("ShipXAxis"), Input.GetAxis("ShipYAxis"), Input.GetAxis("ShipThrottle"));
		rotationInputVector = new Vector3(Input.GetAxis("ShipPitch"), Input.GetAxis("ShipYaw"), Input.GetAxis("ShipRoll"));

		if (Input.GetKeyUp(KeyCode.X)) isDampening = !isDampening;
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
		speedometerText.text = "Speed : " + (rBody.velocity.magnitude * 3.6f).ToString("0.0") + "km/h";
		dampeningIndicatorText.text = isDampening ? "Dampening : ON" : "Dampening : OFF";
		dampeningIndicatorText.color = isDampening ? Color.blue : Color.red;
		UIDirectionIndicator.transform.rotation = Quaternion.LookRotation(rBody.velocity);
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

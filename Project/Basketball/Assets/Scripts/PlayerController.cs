using UnityEngine;
using System.Collections;

// This class handles player movement control
public class PlayerController : MonoBehaviour
{

	public enum PlayerIndex
	{
		PlayerNone,
		Player1,
		Player2
	}
	public PlayerIndex currentPlayer;
	public float speedHorizontal;
	public float speedHorizontalMaximum;
	public float jumpPower;
	public float throwPower;

	private int axisHorizontal;
	private bool isLookingRight;
	private bool jumpPressed;
	private bool isGrounded;
	private float wallCollisionDirection;
	private GameObject potentialCarryObject = null;
	private GameObject carriedObject = null;

	void Start()
	{

		axisHorizontal = 0;
		jumpPressed = false;
		potentialCarryObject = null;
		carriedObject = null;
		isLookingRight = false;

	}

	void Update()
	{

		ProcessInput();
		UpdateLookingRightStatus();

	}

	void FixedUpdate()
	{

		UpdateMovement();

	}

	void ProcessInput()
	{

		axisHorizontal = CalculateHorizontalMovement();
		jumpPressed = Input.GetButton("P1Jump");

		ProcessPickupInput();

	}

	void UpdateMovement()
	{

		Rigidbody rb = GetComponent<Rigidbody>();

		ApplyHorizontalMovement(rb);
		ApplyPlayerJump(rb);

	}

	void ApplyPlayerJump(Rigidbody rigidBody)
	{

		if (isGrounded == true)
		{
			if (jumpPressed == true)
			{
				ApplyJump(rigidBody, jumpPower);
			}
		}

	}

	void ApplyJump(Rigidbody rigidBody, float force)
	{

		rigidBody.AddForce(new Vector3(0.0f, force, 0.0f), ForceMode.Impulse);

	}

	void ApplyHorizontalMovement(Rigidbody rigidBody)
	{

		float horizontalForce = CalculateHorizontalForce();

		// permit movement in opposite direction of wall collision
		if (isGrounded == true || (wallCollisionDirection == 0 || Mathf.Sign(wallCollisionDirection) == Mathf.Sign(horizontalForce)))
		{
			Vector3 velocity = rigidBody.velocity;
			velocity.x = horizontalForce;
			rigidBody.velocity = velocity;
		}

	}

	void UpdateLookingRightStatus()
	{

		isLookingRight = false;
		if (axisHorizontal > 0.0f)
		{
			isLookingRight = true;
		}

	}

	int CalculateHorizontalMovement()
	{

		int result = 0;

		float axisHorizontal = Input.GetAxis("Horizontal");
		if (Mathf.Abs(axisHorizontal) > 0.0f)
		{
			result = (int)Mathf.Sign(axisHorizontal);
		}

		return result;

	}

	float CalculateHorizontalForce()
	{

		float result = 0.0f;

		result = speedHorizontal * axisHorizontal;
		result = Mathf.Clamp(result, -speedHorizontalMaximum, speedHorizontalMaximum);

		return result;

	}

	void OnCollisionEnter(Collision collision)
	{

		string collisionTag = collision.gameObject.tag;
		if (collisionTag == "Level Floor")
		{
			isGrounded = true;
		}
		else if (collisionTag == "Level Walls")
		{
			wallCollisionDirection = Mathf.Sign(transform.position.x - collision.gameObject.transform.position.x);
		}

	}

	void OnCollisionExit(Collision collision)
	{

		string collisionTag = collision.gameObject.tag;
		if (collisionTag == "Level Floor")
		{
			isGrounded = false;
		}
		else if (collisionTag == "Level Walls")
		{
			wallCollisionDirection = 0;
		}

	}

	void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.tag == "Jump Pad")
		{
			ProcessJumpPadTrigger(other.gameObject);
		}
		else
		{
			ProcessPotentialCargo(other.gameObject);
		}

	}

	void OnTriggerExit(Collider other)
	{

		if (other.gameObject == potentialCarryObject)
		{
			potentialCarryObject = null;
		}

	}

	void ProcessJumpPadTrigger(GameObject jumpPad)
	{

		Rigidbody rb = GetComponent<Rigidbody>();

		JumpPadScript jpScript = jumpPad.GetComponent<JumpPadScript>();
		if (jpScript != null)
		{
			// terminate any Y velocity
			Vector3 velocity = rb.velocity;
			velocity.y = 0.0f;
			rb.velocity = velocity;

			// add jump impulse
			ApplyJump(rb, jpScript.jumpPadForce);
		}

	}

	void ProcessPotentialCargo(GameObject other)
	{

		if (potentialCarryObject == null && carriedObject == null)
		{
			CarriableObjectScript coScript = other.gameObject.GetComponent<CarriableObjectScript>();
			if (coScript != null)
			{
				potentialCarryObject = other;
			}
		}

	}

	void ProcessPickupInput()
	{

		if (carriedObject == null)
		{
			if (Input.GetButton("P1Pickup") == true)
			{
				PickupPotentialCargo();
			}
		}
		else
		{
			// throwing on button down, otherwise picking up & throwing get into a loop
			if (Input.GetButtonDown("P1Pickup") == true)
			{
				ThrowCargo();
			}
		}

	}

	void PickupPotentialCargo()
	{

		if (potentialCarryObject != null)
		{
			CarriableObjectScript coScript = potentialCarryObject.gameObject.GetComponent<CarriableObjectScript>();
			coScript.Pickup(this, new Vector2(0.0f, 1.25f));
			carriedObject = potentialCarryObject;
		}

		potentialCarryObject = null;

	}

	void ThrowCargo()
	{

		if (carriedObject != null)
		{
			CarriableObjectScript coScript = carriedObject.GetComponent<CarriableObjectScript>();
			coScript.Throw(throwPower, isLookingRight);
		}

		carriedObject = null;
		
	}

}

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
	public float runMultiplier;
	public float jumpPower;
	public float throwPower;
	public float thrownActionDelay = 0.25f; // what time after being thrown the user can start to move

	private int axisHorizontal;
	private bool isLookingRight;
	private bool jumpPressed;
	private bool isGrounded;
	private float wallCollisionDirection;
	private GameObject potentialCarryObject = null;
	private GameObject carriedObject = null;
	private PlayerButtonScript buttonScript = null;
	private float timeSinceLastJumpPad;
	private float thrownActionTimeout = 0.0f;

	void Start()
	{

		axisHorizontal = 0;
		jumpPressed = false;
		potentialCarryObject = null;
		carriedObject = null;
		isLookingRight = false;
		buttonScript = GetComponent<PlayerButtonScript>();
		if (buttonScript != null)
		{
			buttonScript.currentPlayer = currentPlayer;
		}
		else
		{
			Debug.LogError("Player button script not initialized correctly.");
		}
		timeSinceLastJumpPad = 0.0f;

	}

	void Update()
	{

		ProcessInput();
		UpdateLookingRightStatus();
		timeSinceLastJumpPad += Time.deltaTime;

	}

	void FixedUpdate()
	{

		UpdateMovement();

	}

	void ProcessInput()
	{

		thrownActionTimeout -= Time.deltaTime;
		if (thrownActionTimeout <= 0.0f)
		{
			axisHorizontal = CalculateHorizontalMovement();
			jumpPressed = Input.GetButton(GetInputName("Jump"));
		}

		ProcessPickupInput();

	}

	void UpdateMovement()
	{

		Rigidbody rb = GetComponent<Rigidbody>();

		ApplyHorizontalMovement(rb);
		ApplyPlayerJump(rb);

	}

	string GetInputName(string targetInput)
	{

		string result = "";

		if (buttonScript != null)
		{
			result = buttonScript.GetPlayerSpecificInput(targetInput);
		}

		return result;

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

		float horizontalForceAmount = runMultiplier * axisHorizontal;

		bool canMove = CanMove(horizontalForceAmount);
		if (canMove == true)
		{
			Vector3 localPos = transform.localPosition;
			localPos.x += horizontalForceAmount;
			transform.localPosition = localPos;
		}

	}

	bool CanMove(float withForce)
	{

		bool result = false;

		// permit movement in opposite direction of wall collision
		if (isGrounded == true || (wallCollisionDirection == 0 || Mathf.Sign(wallCollisionDirection) == Mathf.Sign(withForce)))
		{
			result = true;
		}

		return result;
	}

	void UpdateLookingRightStatus()
	{

		if (axisHorizontal > 0.0f)
		{
			isLookingRight = true;
		}
		else if (axisHorizontal < 0.0f)
		{
			isLookingRight = false;
		}

	}

	int CalculateHorizontalMovement()
	{

		int result = 0;
		float axisHorizontal = Input.GetAxis(GetInputName("Horizontal"));
		if (Mathf.Abs(axisHorizontal) > 0.0f)
		{
			result = (int)Mathf.Sign(axisHorizontal);
		}

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
		if (jpScript != null && timeSinceLastJumpPad >= jpScript.jumpPadTimeout)
		{
			timeSinceLastJumpPad = 0.0f;

			// prevent users from jumping
			isGrounded = false;

			// coming down from a jump might produce the same (or larger) force in the opposite direction
			// we have to kill the y velocity to allow the pad impulse to work as needed
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

		// picking up & throwing on button down, otherwise possible pickup/throw loop
		if (carriedObject == null)
		{
			if (Input.GetButtonDown(GetInputName("Pickup")) == true)
			{
				PickupPotentialCargo();
			}
		}
		else
		{
			if (Input.GetButtonDown(GetInputName("Pickup")) == true)
			{
				ThrowCargo();
			}
		}

	}

	void PickupPotentialCargo()
	{

		if (potentialCarryObject != null)
		{
			// prevent picking up the player that is carrying us
			CarriableObjectScript coScriptThis = GetComponent<CarriableObjectScript>();
			if (coScriptThis != null && coScriptThis.carrier != potentialCarryObject)
			{
				CarriableObjectScript coScript = potentialCarryObject.GetComponent<CarriableObjectScript>();
				if (coScript != null)
				{
					coScript.Pickup(this, new Vector2(0.0f, 1.25f));
					carriedObject = potentialCarryObject;
					potentialCarryObject = null;
				}
			}
		}

	}

	void ThrowCargo()
	{

		if (carriedObject != null)
		{
			CarriableObjectScript coScript = carriedObject.GetComponent<CarriableObjectScript>();
			coScript.Throw(throwPower, isLookingRight, gameObject);
			carriedObject = null;
		}

	}

	public void ProcessDropping()
	{

		// disable jumping from dropped state
		isGrounded = false;
		thrownActionTimeout = thrownActionDelay;

	}

}

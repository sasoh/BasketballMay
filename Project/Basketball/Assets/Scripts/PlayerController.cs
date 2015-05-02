using UnityEngine;
using System.Collections;

// This class handles player movement control
public class PlayerController : MonoBehaviour
{

	public float speedHorizontal;
	public float speedHorizontalMaximum;
	public float jumpPower;

	private int axisHorizontal;
	private bool jumpPressed;
	private bool isGrounded;
	private float wallCollisionDirection;

	void Start()
	{

		axisHorizontal = 0;
		jumpPressed = false;

	}

	void Update()
	{

		GetInput();

	}

	void FixedUpdate()
	{

		ApplyMovement();

	}

	void GetInput()
	{

		axisHorizontal = CalculateHorizontalMovement();

		jumpPressed = Input.GetButton("Fire1");

	}

	void ApplyMovement()
	{

		Rigidbody rb = GetComponent<Rigidbody>();

		ApplyHorizontalMovement(rb);
		ApplyJump(rb);

	}

	void ApplyJump(Rigidbody rigidBody)
	{

		if (isGrounded == true)
		{
			if (jumpPressed == true)
			{
				rigidBody.AddForce(new Vector3(0.0f, jumpPower, 0.0f), ForceMode.Impulse);
			}
		}

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

}

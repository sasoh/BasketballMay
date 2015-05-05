using UnityEngine;
using System.Collections;

public class CarriableObjectScript : MonoBehaviour
{

	public Collider triggerCollider;
	public float throwForceMultiplier = 1.0f;
	public GameObject carrier {
		get;
		private set;
	}
	private Vector2 positionOffset;

	public PlayerController.PlayerIndex lastHoldingPlayer { get; private set; }

	// Use this for initialization
	void Start()
	{

		if (triggerCollider == null)
		{
			Debug.LogError("Trigger collider for carriable object not set.");
		}

		carrier = null;

	}

	// Update is called once per frame
	void Update()
	{

		UpdateCarriedPosition();

	}

	void UpdateCarriedPosition()
	{

		if (carrier != null)
		{
			Vector3 newPosition = carrier.transform.position;
			newPosition.x += positionOffset.x;
			newPosition.y += positionOffset.y;
			transform.position = newPosition;
		}

	}

	public void ResetLastHolder()
	{

		lastHoldingPlayer = PlayerController.PlayerIndex.PlayerNone;
	
	}

	public void Pickup(PlayerController other, Vector2 carryingPositionOffset)
	{

		carrier = other.gameObject;
		lastHoldingPlayer = other.currentPlayer;
		positionOffset = carryingPositionOffset;
		SetRigidBodyKinematic(true);

	}

	public void Throw(float force, bool rightDirection)
	{

		Drop();

		Rigidbody rb = GetComponent<Rigidbody>();
		
		int multiplier = 1;
		if (rightDirection == false)
		{
			multiplier = -1;
		}

		float throwForce = force * throwForceMultiplier;

		Vector3 forceVector = new Vector3(multiplier * throwForce, throwForce / 2, 0.0f);
		rb.AddForce(forceVector, ForceMode.Impulse);

	}

	public void Drop()
	{

		// notify carried object that it was dropped (players getting dropped require more processing)
		SendMessage("ProcessDropping", null, SendMessageOptions.DontRequireReceiver);
		
		carrier = null;
		SetRigidBodyKinematic(false);

	}

	void SetRigidBodyKinematic(bool status)
	{

		Rigidbody rb = GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.isKinematic = status;
		}

	}

}

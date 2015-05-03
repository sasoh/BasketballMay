using UnityEngine;
using System.Collections;

public class CarriableObjectScript : MonoBehaviour
{

	public Collider triggerCollider;
	private GameObject carrier;
	private Vector2 positionOffset;

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

	public void Pickup(GameObject other, Vector2 carryingPositionOffset)
	{

		carrier = other;
		positionOffset = carryingPositionOffset;
		SetRigidBodyKinematic(true);

	}

	public void Drop()
	{

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

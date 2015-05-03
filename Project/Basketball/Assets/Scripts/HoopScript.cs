using UnityEngine;
using System.Collections;

public class HoopScript : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.tag == "Ball")
		{
			ProcessBallContact(other.gameObject);
		}

	}

	void ProcessBallContact(GameObject ball)
	{

		BallScript bScript = ball.GetComponent<BallScript>();
		if (bScript != null)
		{
			Debug.Log("Last ball holder " + bScript.lastHoldingPlayer);
		}
		else
		{
			Debug.LogError("Ball script null.");
		}

	}

}

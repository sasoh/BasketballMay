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
			// process only if ball comes from above
			if (other.gameObject.transform.position.y > transform.position.y)
			{
				ProcessBallContact(other.gameObject);
			}
		}

	}

	void ProcessBallContact(GameObject ball)
	{

		BallScript bScript = ball.GetComponent<BallScript>();
		if (bScript != null)
		{
			if (bScript.lastHoldingPlayer != PlayerController.PlayerIndex.PlayerNone)
			{
				// count point for player
				print("Point for player " + bScript.lastHoldingPlayer);
				bScript.SpawnNewBall();
				bScript.DestroyBall();
			}

		}
		else
		{
			Debug.LogError("Ball script null.");
		}

	}

}

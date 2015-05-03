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
		CarriableObjectScript coScript = ball.GetComponent<CarriableObjectScript>();
		if (bScript != null && coScript != null)
		{
			if (ball.gameObject.transform.position.y > transform.position.y)
			{
				// ball comes from above
				ProcessPossibleScoringBall(bScript, coScript);
			}
			else
			{
				// ball came from below the hoop, just reset ball owner
				coScript.ResetLastHolder();
			}
		}
		else
		{
			Debug.LogError("Ball script/carriable script is null.");
		}

	}

	void ProcessPossibleScoringBall(BallScript bScript, CarriableObjectScript coScript)
	{

		if (coScript.lastHoldingPlayer != PlayerController.PlayerIndex.PlayerNone)
		{
			AddPointToPlayer(coScript.lastHoldingPlayer);
			coScript.Drop();
			bScript.SpawnNewBall();
			bScript.DestroyBall();
		}

	}

	void AddPointToPlayer(PlayerController.PlayerIndex playerIndex)
	{

		GameManagerScript gManager = GameObject.FindObjectOfType<GameManagerScript>();
		if (gManager != null)
		{
			gManager.AddPointToPlayer(playerIndex);
		}
		else
		{
			Debug.LogError("Failed getting game manager.");
		}

	}

}

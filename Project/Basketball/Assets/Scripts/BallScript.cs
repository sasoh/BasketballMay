using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{

	public PlayerController.PlayerIndex lastHoldingPlayer;
	public GameObject ballSpawnPoint;

	// Use this for initialization
	void Start()
	{

		lastHoldingPlayer = PlayerController.PlayerIndex.PlayerNone;

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SpawnNewBall()
	{

		print (ballSpawnPoint);
		Instantiate(gameObject, ballSpawnPoint.transform.position, transform.rotation);

	}

	public void DestroyBall()
	{

		Destroy(gameObject);

	}

}

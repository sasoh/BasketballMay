using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{

	public GameObject ballSpawnPoint;
	private bool spawnedNewBall = false;

	// Use this for initialization
	void Start()
	{

		spawnedNewBall = false;

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SpawnNewBall()
	{

		if (spawnedNewBall == false)
		{
			spawnedNewBall = true;
			Instantiate(gameObject, ballSpawnPoint.transform.position, transform.rotation);
		}

	}

	public void DestroyBall()
	{

		Destroy(gameObject);

	}

}

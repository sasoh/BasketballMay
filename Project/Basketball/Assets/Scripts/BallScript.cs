using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{

	public GameObject ballSpawnPoint;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SpawnNewBall()
	{
		
		Instantiate(gameObject, ballSpawnPoint.transform.position, transform.rotation);

	}

	public void DestroyBall()
	{

		Destroy(gameObject);

	}

}

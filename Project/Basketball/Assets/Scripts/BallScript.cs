using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{
	
	public PlayerController.PlayerIndex lastHoldingPlayer;

	// Use this for initialization
	void Start()
	{

		lastHoldingPlayer = PlayerController.PlayerIndex.PlayerNone;

	}

	// Update is called once per frame
	void Update()
	{

	}
}

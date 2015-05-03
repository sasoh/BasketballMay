using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour
{

	public ScoreTextScript scoreText;
	int scorePlayer1;
	int scorePlayer2;

	// Use this for initialization
	void Start()
	{

		scorePlayer1 = 0;
		scorePlayer2 = 0;
		scoreText.SetScores(scorePlayer1, scorePlayer2);

	}

	// Update is called once per frame
	void Update()
	{

		CheckForGameExit();

	}

	void CheckForGameExit()
	{

		if (Input.GetKey("escape") == true)
		{
			Application.Quit();
		}

	}

	public void AddPointToPlayer(PlayerController.PlayerIndex playerIndex)
	{

		if (playerIndex == PlayerController.PlayerIndex.Player1)
		{
			++scorePlayer1;
		}
		else
		{
			++scorePlayer2;
		}
		scoreText.SetScores(scorePlayer1, scorePlayer2);

	}

}

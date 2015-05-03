using UnityEngine;
using System.Collections;

public class PlayerButtonScript : MonoBehaviour
{

	public PlayerController.PlayerIndex currentPlayer;

	public string GetPlayerSpecificInput(string action)
	{

		string result = GetPrefixForCurrentPlayer();

		result += action;

		return result;

	}

	string GetPrefixForCurrentPlayer()
	{

		string result = "P";
		
		int number = 0;
		if (currentPlayer == PlayerController.PlayerIndex.Player1)
		{
			number = 1;
		}
		else if (currentPlayer == PlayerController.PlayerIndex.Player2)
		{
			number = 2;
		}
		result += number;

		return result;

	}

}

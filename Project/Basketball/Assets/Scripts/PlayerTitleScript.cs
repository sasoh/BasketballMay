using UnityEngine;
using System.Collections;

public class PlayerTitleScript : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{

		TextMesh textMesh = GetTextMeshInChildren();
		SetTitle(textMesh);

	}

	TextMesh GetTextMeshInChildren()
	{

		TextMesh result = null;

		result = GetComponentInChildren<TextMesh>();

		return result;

	}

	void SetTitle(TextMesh meshText)
	{
		
		if (meshText != null)
		{
			PlayerController controller = GetComponent<PlayerController>();
			if (controller != null)
			{
				meshText.text = GetTitleForPlayerIndex(controller.currentPlayer);
			}
			else
			{
				Debug.LogError("Failed to get player controller component.");
			}
		}
		else 
		{
			Debug.LogError("Failed to get title text component.");
		}

	}

	string GetTitleForPlayerIndex(PlayerController.PlayerIndex playerIndex)
	{

		string result = "";
		
		if (playerIndex == PlayerController.PlayerIndex.Player1)
		{
			result = "P1";
		}
		else
		{
			result = "P2";
		}

		return result;

	}

}

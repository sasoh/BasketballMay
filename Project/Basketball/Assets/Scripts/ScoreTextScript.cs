using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreTextScript : MonoBehaviour
{

	private Text textObject;

	// Use this for initialization
	void Start()
	{

		textObject = GetComponent<Text>();
		if (textObject != null)
		{
			textObject.text = "No score info";
		}
		else
		{
			Debug.LogError("Failed to get score text component.");
		}

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SetScores(int scorePlayer1, int scorePlayer2)
	{

		if (textObject != null)
		{
			string textString = "P1 [" + scorePlayer1 + " : " + scorePlayer2 + "] P2";
			textObject.text = textString;
		}

	}

}

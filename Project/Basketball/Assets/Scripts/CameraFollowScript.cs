using UnityEngine;
using System.Collections;

public class CameraFollowScript : MonoBehaviour
{

	public float maxHorizontal = 2.5f;
	public float maxVertical = 10.0f;
	private GameObject ballToFollow;

	void Update()
	{

		UpdateBallPointer();
		UpdateCameraRotation();

	}

	void UpdateBallPointer()
	{

		BallScript bScript = GameObject.FindObjectOfType<BallScript>();
		ballToFollow = bScript.gameObject;

	}

	void UpdateCameraRotation()
	{

		// rotate cam around x
		float angleX = (maxVertical / 2) - ballToFollow.transform.position.y;
		Quaternion newRotationX = Quaternion.Euler(new Vector3(angleX, 0.0f, 0.0f));
		transform.localRotation = Quaternion.Slerp(transform.localRotation, newRotationX, Time.deltaTime);
		
		// rotate cam around y
		float angleY = (maxHorizontal / 2) - ballToFollow.transform.position.x;
		Quaternion newRotationY = Quaternion.Euler(new Vector3(0.0f, -angleY, 0.0f));
		transform.localRotation = Quaternion.Slerp(transform.localRotation, newRotationY, Time.deltaTime);

	}

}

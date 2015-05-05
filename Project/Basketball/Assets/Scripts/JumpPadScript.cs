using UnityEngine;
using System.Collections;

public class JumpPadScript : MonoBehaviour
{

	[Tooltip("Amount of impulse force to apply to player objects.")]
	public float jumpPadForce = 12.0f;

	[Tooltip("Timeout between consecutive pad jumps from same player. Needed to prevent double triggering when hitting the pad from strange angles.")]
	public float jumpPadTimeout = 0.1f;

	void Start()
	{

		SetupParticleSystemLifetime();	

	}

	void SetupParticleSystemLifetime()
	{

		ParticleSystem ps = gameObject.GetComponentInChildren<ParticleSystem>();
		if (ps != null)
		{
			// set particle lifetime to something related to the jump pad force
			ps.startLifetime *= jumpPadForce / 10.0f;
		}

	}

}

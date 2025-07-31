using System;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
	public EventHandler OnPlayerDeath;

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Enemy"))
			OnPlayerDeath.Invoke(this, EventArgs.Empty);
	}
}

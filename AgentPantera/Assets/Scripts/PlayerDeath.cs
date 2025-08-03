using System;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
	public EventHandler OnPlayerDeath;
	
	private bool _isDead;

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (_isDead)
			return;
		
		if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Enemy"))
		{
			_isDead = true;
			OnPlayerDeath.Invoke(this, EventArgs.Empty);
		}
	}
}

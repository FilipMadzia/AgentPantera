using System;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	[SerializeField] private float speed;
	[SerializeField] private float distance;

	private Vector3 _startPosition;
	private Vector3 _targetPosition;
	private int _direction;

	private void Start()
	{
		_startPosition = transform.position;
		_targetPosition = transform.position + new Vector3(0, distance, 0);
	}

	private void Update()
	{
		if (transform.position.y >= _targetPosition.y)
			_direction = -1;
		else if (transform.position.y <= _startPosition.y)
			_direction = 1;
		
		transform.position = new Vector3(transform.position.x, transform.position.y + speed * _direction * Time.deltaTime, _startPosition.z);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, transform.position + Vector3.up * distance);
	}
}
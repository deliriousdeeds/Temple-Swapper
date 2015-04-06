using UnityEngine;
using System.Collections;

public class BaseScepter{

	private Transform player;

	private Vector3 playerPosition;
	private Vector3 targetPosition;

	public BaseScepter(Transform _player)
	{
		player = _player;
	}

	public void SwapPosition(Transform target)
	{
		//acquire the current positions of both the player and the target
		playerPosition = player.position;
		targetPosition = target.position;

		//Swap position !
		player.position = targetPosition;
		target.position = playerPosition;
	}
}

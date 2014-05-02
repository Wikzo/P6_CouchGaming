using UnityEngine;
using System.Collections;

public class PlayerAnimations : MonoBehaviour {

	private PlayerMove playerMove;
	private PlayerJump playerJump;

	// Use this for initialization
	void Start ()
	{
		playerMove = GetComponent<PlayerMove>();
		playerJump = GetComponent<PlayerJump>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(playerJump.CanJump == false)
			print("Jumping");

		if(playerMove.movingLeft)
			print("Moving Left");

		if(playerMove.movingRight)
			print("Moving Right");
	}
}

using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour {

	public int JumpForce = 350;
	public float GroundDetectLength = 0.5f;
	public float GroundDetectOffset = 0.7f;

	private bool canJump = true;
	private Player playerScript;

	private Transform pTran;

	public bool CanJump
	{
		get{return canJump;}
		set{canJump = value;}	
	}

	// Use this for initialization
	void Start () 
	{
		pTran = transform;

		playerScript = GetComponent<Player>();
	}

	// Update is called once per frame
	public void JumpUpdate () 
	{
		//Rays will be cast on both sides of the player, so edges are also detected
		Vector3 leftPos = pTran.position+Vector3.left*GroundDetectOffset;
		Vector3 rightPos = pTran.position+Vector3.right*GroundDetectOffset;

		if(Physics.Raycast(pTran.position, Vector3.down, GroundDetectLength) || Physics.Raycast(leftPos, Vector3.down, GroundDetectLength) || Physics.Raycast(rightPos, Vector3.down, GroundDetectLength))
		{
			CanJump = true;
		}
		else
		{
			CanJump = false;
		}

		if(CanJump && playerScript.PlayerControllerState.ButtonDownA || CanJump && playerScript.Keyboard && Input.GetKeyDown(KeyCode.Space))
		{
			Jump();
		}

		//Debug.DrawRay(leftPos, Vector3.down);
		//Debug.DrawRay(rightPos, Vector3.down);
	}

	public void Jump()
	{
		//THIS SHOULD PROBABLY BE CALLED FROM FIXED UPDATE:
		rigidbody.AddForce(Vector3.up*JumpForce);

		DataSaver.Instance.highScores[0].timesJumped++;
	}
}

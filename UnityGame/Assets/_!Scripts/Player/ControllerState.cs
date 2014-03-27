using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class ControllerState : MonoBehaviour {

	public GamePadState PreviousState;
	private GamePadState currentState;

	private PlayerIndex playerIndex;
	private int id = 0;

	// Use this for initialization
	void Start () 
	{
		id = GetComponent<Player>().Id;

		//Choose a controller matching the ID
		switch(id)
		{
			case 0:
			playerIndex = PlayerIndex.One;
			break;
			case 2:
			playerIndex = PlayerIndex.Two;
			break;
			case 3:
			playerIndex = PlayerIndex.Three;
			break;
			case 4:
			playerIndex = PlayerIndex.Four;
			break;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		currentState = GamePad.GetState(playerIndex);
	}

	public GamePadState GetCurrentState()
	{
		return currentState;
	}

	public bool ButtonDownA()
	{
		if(GetCurrentState().Buttons.A == ButtonState.Pressed && PreviousState.Buttons.A != ButtonState.Pressed)
		{	
			return true;
		}
		else
		{
			return false;
		}
	}
}
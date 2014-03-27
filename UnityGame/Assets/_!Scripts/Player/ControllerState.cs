using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class ControllerState : MonoBehaviour {

	private GamePadState currentState;
	private GamePadState previousState;
	private PlayerIndex playerIndex;
	private int id = 0;

	// Use this for initialization
	void Start () 
	{
		id = GetComponent<Player>().Id;

		//Choose a controller matching the ID
		if(id == 0)
			playerIndex = PlayerIndex.One;
		else if(id == 1)
			playerIndex = PlayerIndex.Two;
		else if(id == 2)
			playerIndex = PlayerIndex.Three;
		else if(id == 3)
			playerIndex = PlayerIndex.Four;
	}
	
	// Update is called once per frame
	void Update () 
	{
		currentState = GamePad.GetState(playerIndex);

		if(currentState.Buttons.X == ButtonState.Pressed && previousState.Buttons.X != ButtonState.Pressed)
		{
			print("Down");
		}

		previousState = currentState;
	}

	public GamePadState GetCurrentState()
	{
		return currentState;
	}

	//public bool ButtonDown()
	//{

	//}

	//public bool ButtonUp()
	//{
		 //buttonLBDown = (state.Buttons.LeftShoulder == ButtonState.Pressed && prevState.Buttons.LeftShoulder != ButtonState.Pressed);
	//}
}

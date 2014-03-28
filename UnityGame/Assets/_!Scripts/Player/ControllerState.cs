using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class ControllerState : MonoBehaviour {

	public GamePadState PreviousState;
	private GamePadState currentState;

	private PlayerIndex playerIndex;
	private int id = 0;

	private bool buttonDownA = false;
	private bool buttonDownB = false;
	private bool buttonDownX = false;
	private bool buttonDownY = false;
	private bool buttonDownLeftShoulder = false;
	private bool buttonDownRightShoulder = false;
	private bool buttonDownBack = false;
	private bool buttonDownStart = false;

	private bool buttonUpA = false;
	private bool buttonUpB = false;
	private bool buttonUpX = false;
	private bool buttonUpY = false;
	private bool buttonUpLeftShoulder = false;
	private bool buttonUpRightShoulder = false;
	private bool buttonUpBack = false;
	private bool buttonUpStart = false;

	public bool ButtonDownA{get{return buttonDownA;}set{buttonDownA = value;}}
	public bool ButtonDownB{get{return buttonDownB;}set{buttonDownB = value;}}
	public bool ButtonDownX{get{return buttonDownX;}set{buttonDownX = value;}}
	public bool ButtonDownY{get{return buttonDownY;}set{buttonDownY = value;}}
	public bool ButtonDownLeftShoulder{get{return buttonDownLeftShoulder;}set{buttonDownLeftShoulder = value;}}
	public bool ButtonDownRightShoulder{get{return buttonDownRightShoulder;}set{buttonDownRightShoulder = value;}}
	public bool ButtonDownBack{get{return buttonDownBack;}set{buttonDownBack = value;}}
	public bool ButtonDownStart{get{return buttonDownStart;}set{buttonDownStart = value;}}

	public bool ButtonUpA{get{return buttonUpA;}set{buttonUpA = value;}}
	public bool ButtonUpB{get{return buttonUpB;}set{buttonUpB = value;}}
	public bool ButtonUpX{get{return buttonUpX;}set{buttonUpX = value;}}
	public bool ButtonUpY{get{return buttonUpY;}set{buttonUpY = value;}}
	public bool ButtonUpLeftShoulder{get{return buttonUpLeftShoulder;}set{buttonUpLeftShoulder = value;}}
	public bool ButtonUpRightShoulder{get{return buttonUpRightShoulder;}set{buttonUpRightShoulder = value;}}
	public bool ButtonUpBack{get{return buttonUpBack;}set{buttonUpBack = value;}}
	public bool ButtonUpStart{get{return buttonUpStart;}set{buttonUpStart = value;}}

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
			case 1:
			playerIndex = PlayerIndex.Two;
			break;
			case 2:
			playerIndex = PlayerIndex.Three;
			break;
			case 3:
			playerIndex = PlayerIndex.Four;
			break;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		currentState = GamePad.GetState(playerIndex);

		if(GetCurrentState().Buttons.A == ButtonState.Pressed && PreviousState.Buttons.A != ButtonState.Pressed) ButtonDownA = true;else ButtonDownA = false;
		if(GetCurrentState().Buttons.B == ButtonState.Pressed && PreviousState.Buttons.B != ButtonState.Pressed) ButtonDownB = true;else ButtonDownB = false;
		if(GetCurrentState().Buttons.X == ButtonState.Pressed && PreviousState.Buttons.X != ButtonState.Pressed) ButtonDownX = true;else ButtonDownX = false;
		if(GetCurrentState().Buttons.Y == ButtonState.Pressed && PreviousState.Buttons.Y != ButtonState.Pressed) ButtonDownY = true;else ButtonDownY = false;
		if(GetCurrentState().Buttons.LeftShoulder == ButtonState.Pressed && PreviousState.Buttons.LeftShoulder != ButtonState.Pressed) ButtonDownLeftShoulder = true;else ButtonDownLeftShoulder = false;
		if(GetCurrentState().Buttons.RightShoulder == ButtonState.Pressed && PreviousState.Buttons.RightShoulder != ButtonState.Pressed) ButtonDownRightShoulder = true;else ButtonDownRightShoulder = false;
		if(GetCurrentState().Buttons.Back == ButtonState.Pressed && PreviousState.Buttons.Back != ButtonState.Pressed) ButtonDownBack = true;else ButtonDownBack = false;
		if(GetCurrentState().Buttons.Start == ButtonState.Pressed && PreviousState.Buttons.Start != ButtonState.Pressed) ButtonDownStart = true;else ButtonDownStart = false;

		if(GetCurrentState().Buttons.A == ButtonState.Released && PreviousState.Buttons.A != ButtonState.Released)	ButtonUpA = true;else ButtonUpA = false;
		if(GetCurrentState().Buttons.B == ButtonState.Released && PreviousState.Buttons.B != ButtonState.Released)	ButtonUpB = true;else ButtonUpB = false;
		if(GetCurrentState().Buttons.X == ButtonState.Released && PreviousState.Buttons.X != ButtonState.Released)	ButtonUpX = true;else ButtonUpX = false;
		if(GetCurrentState().Buttons.Y == ButtonState.Released && PreviousState.Buttons.Y != ButtonState.Released)	ButtonUpY = true;else ButtonUpY = false;
		if(GetCurrentState().Buttons.LeftShoulder == ButtonState.Released && PreviousState.Buttons.LeftShoulder != ButtonState.Released)	ButtonUpLeftShoulder = true;else ButtonUpLeftShoulder = false;
		if(GetCurrentState().Buttons.RightShoulder == ButtonState.Released && PreviousState.Buttons.RightShoulder != ButtonState.Released)	ButtonUpRightShoulder = true;else ButtonUpRightShoulder = false;
		if(GetCurrentState().Buttons.Back == ButtonState.Released && PreviousState.Buttons.Back != ButtonState.Released)	ButtonUpBack = true;else ButtonUpBack = false;
		if(GetCurrentState().Buttons.Start == ButtonState.Released && PreviousState.Buttons.Start != ButtonState.Released)	ButtonUpStart = true;else ButtonUpStart = false;

		PreviousState = currentState;
	}

	public GamePadState GetCurrentState()
	{
		return currentState;
	}
}
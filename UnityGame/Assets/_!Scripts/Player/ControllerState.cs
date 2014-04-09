using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class ControllerState : MonoBehaviour {

	private GamePadState previousState;
	private GamePadState currentState;

	private bool joinedFromStart = false;

	private PlayerIndex playerController;
	private int id = 0;

	private bool buttonDownA = false;
	private bool buttonDownB = false;
	private bool buttonDownX = false;
	private bool buttonDownY = false;
	private bool buttonDownLeftShoulder = false;
	private bool buttonDownRightShoulder = false;
	private bool buttonDownLeftStick = false;
	private bool buttonDownRightStick = false;
	private bool buttonDownBack = false;
	private bool buttonDownStart = false;

	private bool canRelease = false;

	private bool buttonUpA = false;
	private bool buttonUpB = false;
	private bool buttonUpX = false;
	private bool buttonUpY = false;
	private bool buttonUpLeftShoulder = false;
	private bool buttonUpRightShoulder = false;
	private bool buttonUpLeftStick = false;
	private bool buttonUpRightStick = false;
	private bool buttonUpBack = false;
	private bool buttonUpStart = false;

	public bool ButtonDownA{get{return buttonDownA;}set{buttonDownA = value;}}
	public bool ButtonDownB{get{return buttonDownB;}set{buttonDownB = value;}}
	public bool ButtonDownX{get{return buttonDownX;}set{buttonDownX = value;}}
	public bool ButtonDownY{get{return buttonDownY;}set{buttonDownY = value;}}
	public bool ButtonDownLeftShoulder{get{return buttonDownLeftShoulder;}set{buttonDownLeftShoulder = value;}}
	public bool ButtonDownRightShoulder{get{return buttonDownRightShoulder;}set{buttonDownRightShoulder = value;}}
	public bool ButtonDownLeftStick{get{return buttonDownLeftStick;}set{buttonDownLeftStick = value;}}
	public bool ButtonDownRightStick{get{return buttonDownRightStick;}set{buttonDownRightStick = value;}}
	public bool ButtonDownBack{get{return buttonDownBack;}set{buttonDownBack = value;}}
	public bool ButtonDownStart{get{return buttonDownStart;}set{buttonDownStart = value;}}

	public bool ButtonUpA{get{return buttonUpA;}set{buttonUpA = value;}}
	public bool ButtonUpB{get{return buttonUpB;}set{buttonUpB = value;}}
	public bool ButtonUpX{get{return buttonUpX;}set{buttonUpX = value;}}
	public bool ButtonUpY{get{return buttonUpY;}set{buttonUpY = value;}}
	public bool ButtonUpLeftShoulder{get{return buttonUpLeftShoulder;}set{buttonUpLeftShoulder = value;}}
	public bool ButtonUpRightShoulder{get{return buttonUpRightShoulder;}set{buttonUpRightShoulder = value;}}
	public bool ButtonUpLeftStick{get{return buttonUpLeftStick;}set{buttonUpLeftStick = value;}}
	public bool ButtonUpRightStick{get{return buttonUpRightStick;}set{buttonUpRightStick = value;}}
	public bool ButtonUpBack{get{return buttonUpBack;}set{buttonUpBack = value;}}
	public bool ButtonUpStart{get{return buttonUpStart;}set{buttonUpStart = value;}}

	// Use this for initialization

	void Start () 
	{
		playerController = GetComponent<Player>().PlayerController;
		currentState = GamePad.GetState(playerController);

		GamePad.SetVibration(playerController, 0, 0);

		if(currentState.IsConnected)
			joinedFromStart = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        currentState = GamePad.GetState(playerController);

	    

	    if(joinedFromStart && !currentState.IsConnected)
			print("Player " + playerController + " disconnected!");

		if(GetCurrentState().Buttons.A == ButtonState.Pressed && previousState.Buttons.A != ButtonState.Pressed){ButtonDownA = true; canRelease = true;}else ButtonDownA = false;
		if(GetCurrentState().Buttons.B == ButtonState.Pressed && previousState.Buttons.B != ButtonState.Pressed){ButtonDownB = true; canRelease = true;}else ButtonDownB = false;
		if(GetCurrentState().Buttons.X == ButtonState.Pressed && previousState.Buttons.X != ButtonState.Pressed){ButtonDownX = true; canRelease = true;}else ButtonDownX = false;
		if(GetCurrentState().Buttons.Y == ButtonState.Pressed && previousState.Buttons.Y != ButtonState.Pressed){ButtonDownY = true; canRelease = true;}else ButtonDownY = false;
		if(GetCurrentState().Buttons.LeftShoulder == ButtonState.Pressed && previousState.Buttons.LeftShoulder != ButtonState.Pressed){ButtonDownLeftShoulder = true; canRelease = true;}else ButtonDownLeftShoulder = false;
		if(GetCurrentState().Buttons.RightShoulder == ButtonState.Pressed && previousState.Buttons.RightShoulder != ButtonState.Pressed){ButtonDownRightShoulder = true; canRelease = true;}else ButtonDownRightShoulder = false;
		if(GetCurrentState().Buttons.LeftStick == ButtonState.Pressed && previousState.Buttons.LeftStick != ButtonState.Pressed){ButtonDownLeftStick = true; canRelease = true;}else ButtonDownLeftStick = false;
		if(GetCurrentState().Buttons.RightStick == ButtonState.Pressed && previousState.Buttons.RightStick != ButtonState.Pressed){ButtonDownRightStick = true; canRelease = true;}else ButtonDownRightStick = false;
		if(GetCurrentState().Buttons.Back == ButtonState.Pressed && previousState.Buttons.Back != ButtonState.Pressed){ButtonDownBack = true; canRelease = true;}else ButtonDownBack = false;
		if(GetCurrentState().Buttons.Start == ButtonState.Pressed && previousState.Buttons.Start != ButtonState.Pressed){ButtonDownStart = true; canRelease = true;}else ButtonDownStart = false;

		if(canRelease)
		{
			if(GetCurrentState().Buttons.A == ButtonState.Released && previousState.Buttons.A != ButtonState.Released)	ButtonUpA = true;else ButtonUpA = false;
			if(GetCurrentState().Buttons.B == ButtonState.Released && previousState.Buttons.B != ButtonState.Released)	ButtonUpB = true;else ButtonUpB = false;
			if(GetCurrentState().Buttons.X == ButtonState.Released && previousState.Buttons.X != ButtonState.Released)	ButtonUpX = true;else ButtonUpX = false;
			if(GetCurrentState().Buttons.Y == ButtonState.Released && previousState.Buttons.Y != ButtonState.Released)	ButtonUpY = true;else ButtonUpY = false;
			if(GetCurrentState().Buttons.LeftShoulder == ButtonState.Released && previousState.Buttons.LeftShoulder != ButtonState.Released)	ButtonUpLeftShoulder = true;else ButtonUpLeftShoulder = false;
			if(GetCurrentState().Buttons.RightShoulder == ButtonState.Released && previousState.Buttons.RightShoulder != ButtonState.Released)	ButtonUpRightShoulder = true;else ButtonUpRightShoulder = false;
			if(GetCurrentState().Buttons.LeftStick == ButtonState.Released && previousState.Buttons.LeftStick != ButtonState.Released)	ButtonUpLeftStick = true;else ButtonUpLeftStick = false;
			if(GetCurrentState().Buttons.RightStick == ButtonState.Released && previousState.Buttons.RightStick != ButtonState.Released)	ButtonUpRightStick = true;else ButtonUpRightStick = false;
			if(GetCurrentState().Buttons.Back == ButtonState.Released && previousState.Buttons.Back != ButtonState.Released)	ButtonUpBack = true;else ButtonUpBack = false;
			if(GetCurrentState().Buttons.Start == ButtonState.Released && previousState.Buttons.Start != ButtonState.Released)	ButtonUpStart = true;else ButtonUpStart = false;
		}

		previousState = currentState;
	}

	public GamePadState GetCurrentState()
	{
		return currentState;
	}

	void OnApplicationQuit()
	{
		GamePad.SetVibration(playerController, 0, 0);
	}
}
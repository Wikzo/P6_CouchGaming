using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class RTII_Rumble : MonoBehaviour
{
    private GamePadState state;

    private Vector3 pos;

    // Use this for initialization
    private void Start()
    {
        Application.runInBackground = true;

    }

    // Update is called once per frame
    private void Update()
    {

        //print(GamePad.GetState(PlayerIndex.One).IsConnected);

        this.state = GamePad.GetState(PlayerIndex.One);

        pos = Camera.main.WorldToViewportPoint(transform.position);
        //pos = new Vector2(transform.position.x / Screen.width, transform.position.y / Screen.height);
        print(pos);

        if (pos.x < 0.5f)
            GamePad.SetVibration(PlayerIndex.One, pos.y, 0);
        else
            GamePad.SetVibration(PlayerIndex.One, 0, pos.y);

        if (Input.GetKey(KeyCode.Q))
            GamePad.SetVibration(PlayerIndex.One, 1f, 0);
        else if (Input.GetKey(KeyCode.W))
            GamePad.SetVibration(PlayerIndex.One, 0, 1f);
        else if (Input.GetKey(KeyCode.A))
            GamePad.SetVibration(PlayerIndex.One, 0.1f, 0);
        else if (Input.GetKey(KeyCode.S))
            GamePad.SetVibration(PlayerIndex.One, 0, 0.1f);

        

    }

    private void OnApplicationQuit()
    {
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
        
    }
}

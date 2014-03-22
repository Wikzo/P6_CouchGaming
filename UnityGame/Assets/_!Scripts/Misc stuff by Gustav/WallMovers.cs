using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class WallMovers : MonoBehaviour
{
    private float speed = 5f;
    private GamePadState state;


    void Update()
    {
        state = GamePad.GetState(PlayerIndex.One);

        if (state.Triggers.Right > 0.2f && state.Triggers.Left > 0.2f) // move downwards
            transform.Translate(Vector3.down * speed * state.Triggers.Right * Time.deltaTime);
        else if (state.Triggers.Right > 0.2f) // move right
            transform.Translate(Vector3.right * speed * state.Triggers.Right * Time.deltaTime);
        else if (state.Triggers.Left > 0.2f) // move left
            transform.Translate(Vector3.left * speed * state.Triggers.Left * Time.deltaTime);
        

    }
}

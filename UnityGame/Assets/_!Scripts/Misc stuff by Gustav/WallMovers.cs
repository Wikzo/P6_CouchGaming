using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class WallMovers : MonoBehaviour
{
    Vector3 speed = new Vector3(-1, 0, 0);
    private GamePadState state;


    void Update()
    {
        state = GamePad.GetState(PlayerIndex.One);

        if (state.Triggers.Right > 0.2f)
            transform.Translate(-speed * Time.deltaTime);
        else if (state.Triggers.Left > 0.2f)
            transform.Translate(Vector3.down * 3 * Time.deltaTime);
        else
            transform.Translate(speed * Time.deltaTime);
    }
}

using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class ShootBullets : MonoBehaviour
{
    private GamePadState state;
    private GamePadState prevState;
    public GameObject Bullet;
    
    void Start()
    {
        state = GamePad.GetState(PlayerIndex.One);
        prevState = state;
    }

    // Update is called once per frame
    private void Update()
    {
        state = GamePad.GetState(PlayerIndex.One);


        if (state.Buttons.B == ButtonState.Pressed && prevState.Buttons.B == ButtonState.Released)

        {
            prevState = state;

            GameObject bullet = (GameObject) Instantiate(Bullet, transform.position, transform.rotation);

            bullet.rigidbody.AddForce(bullet.transform.forward*700);
        }

        prevState = state;

    }
}
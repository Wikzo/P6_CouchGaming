using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class Mouse : MonoBehaviour {

float xb;
float yb;
	void Update () {
    var pos = Camera.main.WorldToScreenPoint(transform.position);
    var dir = Input.mousePosition - pos;
    var angle = Mathf.Atan2(dir.x, dir.x) * Mathf.Rad2Deg;
    //transform.rotation = Quaternion.AngleAxis(dir.y, Vector3.right); 
    //transform.rotation = Quaternion.AngleAxis(dir.x, Vector3.up);

    //var x = Input.GetAxis("Horizontal");
    //var y = Input.GetAxis("Vertical");

    GamePadState gState =  GamePad.GetState(PlayerIndex.One);

    var x = gState.ThumbSticks.Left.X;
    var y = gState.ThumbSticks.Left.Y;

    var speed = 200;
    
    // x axis
    if (x > 0 && xb >= -60)
    {
    	xb -= (x * Time.deltaTime * speed);

    	if (xb <= -60)
			xb = -60;
    }
	else if (x < 0 && xb <= 60)
	{
		xb -= (x * Time.deltaTime * speed);

		if (xb >= 60)
			xb = 60;
	}
	else if (xb > 0)
	{
		xb -= (Time.deltaTime * speed*2);

		if (xb <= 0)
			xb = 0;
	}
	else if (xb < 0)
	{
		xb += (Time.deltaTime * speed*2);

		if (xb >= 0)
			xb = 0;
	}
	else
		xb = 0;

	// y axis
	if (y > 0 && yb >= -60)
    {
    	yb -= (y * Time.deltaTime * speed);

    	if (yb <= -60)
			yb = -60;
    }
	else if (y < 0 && yb <= 60)
	{
		yb -= (y * Time.deltaTime * speed);

		if (yb >= 60)
			yb = 60;
	}
	else if (yb > 0)
	{
		yb -= (Time.deltaTime * speed*2);

		if (yb <= 0)
			yb = 0;
	}
	else if (yb < 0)
	{
		yb += (Time.deltaTime * speed*2);

		if (yb >= 0)
			yb = 0;
	}
	else
		yb = 0;

	/*if (y > 0 && yb < 60)
		yb++;
	else if (y < 0 && yb > -60)
		yb--	;
	else if (yb > 0)
		yb--;
	else if (yb < 0)
		yb++;*/



    transform.rotation = Quaternion.Euler(new Vector3(-yb, xb, 0));

    //transform.Rotate(Vector3.right * x * 100 * Time.deltaTime);

}
}

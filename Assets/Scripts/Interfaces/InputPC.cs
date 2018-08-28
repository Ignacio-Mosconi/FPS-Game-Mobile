using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPC : IInput 
{
	public float GetHorizontalAxis()
	{
		return Input.GetAxis("Horizontal");
	}

	public float GetVerticalAxis()
	{
		return Input.GetAxis("Vertical");
	}

	public float GetHorizontalViewAxis()
	{
		return Input.GetAxis("Mouse X");
	}

	public float GetVerticalViewAxis()
	{
		return Input.GetAxis("Mouse Y");
	}

	public bool GetFireButton()
	{
		return Input.GetButton("Fire1");
	}

	public bool GetReloadButton()
	{
		return Input.GetButton("Reload");
	}

	public bool GetJumpButton()
	{
		return Input.GetButton("Jump");
	}

	public bool GetSprintButton()
	{
		return Input.GetButton("Sprint");
	}
}

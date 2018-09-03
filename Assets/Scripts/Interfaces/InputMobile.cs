using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMobile : IInput
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

	public float GetWeaponSwapAxis()
	{
		return Input.GetAxis("Mouse ScrollWheel");
	}

	public bool GetFireButton()
	{
		return Input.GetButton("Fire");
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

	public bool GetSprintButtonModifier()
	{
		return Input.GetButton("Sprint Modifier");
	}

	public bool GetSwapWeaponButton()
	{
		return Input.GetButtonDown("Swap Weapon");
	}

	public bool GetPauseButton()
	{
		return Input.GetButtonDown("Cancel");
	}
}

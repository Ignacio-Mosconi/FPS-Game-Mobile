using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMobile : IInput
{
	Joystick leftStick;

	public void SetSticks(Joystick leftStick)
	{
		this.leftStick = leftStick;
	}

	public float GetHorizontalAxis()
	{
		return leftStick.Horizontal;
	}

	public float GetVerticalAxis()
	{
		return leftStick.Vertical;
	}

	public float GetHorizontalViewAxis()
	{
		return Input.GetAxis("Mouse X");
	}

	public float GetVerticalViewAxis()
	{
		return Input.GetAxis("Mouse Y Controller Mobile");
	}

	public float GetWeaponSwapAxis()
	{
		return Input.GetAxis("Mouse ScrollWheel");
	}

	public bool GetFireButton()
	{
		return false;
	}

	public bool GetReloadButton()
	{
		return Input.GetButtonUp("Reload Controller Mobile");
	}

	public bool GetJumpButton()
	{
		return Input.GetButtonDown("Jump Controller Mobile");
	}

	public bool GetSprintButton()
	{
		return Input.GetButton("Sprint");
	}

	public bool GetSprintButtonModifier()
	{
		return Input.GetButton("Sprint Controller Mobile");
	}

	public bool GetSwapWeaponButton()
	{
		return Input.GetButtonDown("Swap Weapon");
	}

	public bool GetInteractButton()
	{
		return Input.GetButtonDown("Interact");
	}

	public bool GetInteractHoldButton()
	{
		return Input.GetButton("Interact Controller Mobile");
	}

	public bool GetPauseButton()
	{
		return Input.GetButtonDown("Cancel Controller Mobile");
	}
}

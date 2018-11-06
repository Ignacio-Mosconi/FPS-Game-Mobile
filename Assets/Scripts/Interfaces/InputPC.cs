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

	public float GetSwapWeaponAxis()
	{
		return Input.GetAxis("Mouse ScrollWheel");
	}

	public bool GetFireButton()
	{
		return Input.GetButton("Fire");
	}

	public bool GetReloadButton()
	{
		return Input.GetButtonUp("Reload");
	}

	public bool GetJumpButton()
	{
		return Input.GetButtonDown("Jump");
	}

	public bool GetSprintButton()
	{
		return Input.GetButton("Sprint");
	}

	public bool GetSprintButtonModifier()
	{
		return Input.GetButton("Sprint Controller");
	}

	public bool GetInteractButton()
	{
		return Input.GetButtonDown("Interact");
	}

	public bool GetInteractHoldButton()
	{
		return Input.GetButton("Interact Controller");
	}

    public bool GetPauseButton()
    {
        return Input.GetButtonDown("Cancel");
    }
}

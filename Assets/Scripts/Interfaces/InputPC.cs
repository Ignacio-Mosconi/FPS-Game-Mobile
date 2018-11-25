using UnityEngine;

public class InputPC : IInput 
{
	public float GetHorizontalAxis()
	{
		return (InputManager.Instance.ControllerConnected) ? Input.GetAxis("Horizontal Controller") :
															Input.GetAxis("Horizontal");			
	}

	public float GetVerticalAxis()
	{
		return (InputManager.Instance.ControllerConnected) ? Input.GetAxis("Vertical Controller") :
															Input.GetAxis("Vertical");
	}

	public float GetHorizontalViewAxis()
	{
		return (InputManager.Instance.ControllerConnected) ? Input.GetAxis("Horizontal View Controller") :
															Input.GetAxis("Horizontal View");
	}

	public float GetVerticalViewAxis()
	{
		return (InputManager.Instance.ControllerConnected) ? Input.GetAxis("Vertical View Controller") :
															Input.GetAxis("Vertical View");
	}

	public float GetSwapWeaponAxis()
	{
		return (InputManager.Instance.ControllerConnected && Input.GetButtonDown("Swap Weapon")) ?
				1.0f : Input.GetAxis("Swap Weapon");
	}

	public bool GetFireButton()
	{
		return (InputManager.Instance.ControllerConnected) ? Input.GetButton("Fire Controller") :
															Input.GetButton("Fire");
	}

	public bool GetReloadButton()
	{
		return (InputManager.Instance.ControllerConnected) ? Input.GetButtonUp("Reload Controller") :
															Input.GetButtonUp("Reload");
	}

	public bool GetJumpButton()
	{
		return (InputManager.Instance.ControllerConnected) ? Input.GetButtonDown("Jump Controller") :
															Input.GetButtonDown("Jump");
	}

	public bool GetSprintButton()
	{
		return (!InputManager.Instance.ControllerConnected) ? Input.GetButton("Sprint") :
																false;
	}

	public bool GetSprintButtonModifier()
	{
		return (InputManager.Instance.ControllerConnected) ? Input.GetButton("Sprint Controller") :
																false;
	}

	public bool GetInteractButton()
	{
		return (!InputManager.Instance.ControllerConnected) ? Input.GetButtonDown("Interact") :
																false;
	}

	public bool GetInteractHoldButton()
	{
		return (InputManager.Instance.ControllerConnected) ? Input.GetButton("Interact Controller") :
																false;
	}

    public bool GetPauseButton()
    {
        return (InputManager.Instance.ControllerConnected) ? Input.GetButtonDown("Pause Controller") :
																Input.GetButtonDown("Pause");
    }

    public bool GetLeftUIButton()
    {
        return (InputManager.Instance.ControllerConnected) ? Input.GetAxis("Horizontal UI") == -1f :
																false;
    }

    public bool GetRightUIButton()
    {
        return (InputManager.Instance.ControllerConnected) ? Input.GetAxis("Horizontal UI") == 1f :
																false;
    }
}
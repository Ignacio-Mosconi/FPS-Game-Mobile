using UnityStandardAssets.CrossPlatformInput;

public class InputAndroid : IInput
{
	public float GetHorizontalAxis()
	{
		return CrossPlatformInputManager.GetAxis("Horizontal");
	}

	public float GetVerticalAxis()
	{
		return CrossPlatformInputManager.GetAxis("Vertical");
	}

	public float GetHorizontalViewAxis()
	{
		return CrossPlatformInputManager.GetAxis("Horizontal View");
	}

	public float GetVerticalViewAxis()
	{
		return CrossPlatformInputManager.GetAxis("Vertical View");
	}

	public float GetSwapWeaponAxis()
	{
		return (CrossPlatformInputManager.GetButtonDown("Swap Weapon")) ? 1.0f : 0f;
	}

	public bool GetFireButton()
	{
		return CrossPlatformInputManager.GetButton("Fire");
	}

	public bool GetReloadButton()
	{
		return CrossPlatformInputManager.GetButtonDown("Reload");
	}

	public bool GetJumpButton()
	{
		return CrossPlatformInputManager.GetButtonDown("Jump");
	}

	public bool GetSprintButton()
	{
		return false;
	}

	public bool GetSprintButtonModifier()
	{
		return (CrossPlatformInputManager.GetAxis("Vertical") > 0.7f);
	}

	public bool GetInteractButton()
	{
		return CrossPlatformInputManager.GetButtonDown("Interact");
	}

	public bool GetInteractHoldButton()
	{
		return false;
	}

	public bool GetPauseButton()
	{
		return CrossPlatformInputManager.GetButtonDown("Pause");
	}

    public bool GetLeftUIButton()
    {
        return CrossPlatformInputManager.GetAxis("Horizontal UI") == -1f;
    }
	
    public bool GetRightUIButton()
    {
        return CrossPlatformInputManager.GetAxis("Horizontal UI") == 1f;
    }
}
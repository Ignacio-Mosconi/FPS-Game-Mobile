using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	static InputManager instance;
	IInput input;

	void Awake()
	{
		if (Instance == this)
		{
			DontDestroyOnLoad(gameObject);
			
			#if UNITY_STANDALONE
				input = new InputPC();
			#else
				input = new InputMobile();
			#endif
		}
	}

	public float GetHorizontalAxis()
	{
		return input.GetHorizontalAxis();
	}

	public float GetVerticalAxis()
	{
		return input.GetVerticalAxis();
	}

	public float GetHorizontalViewAxis()
	{
		return input.GetHorizontalViewAxis();
	}

	public float GetVerticalViewAxis()
	{
		return input.GetVerticalViewAxis();
	}

	public float GetWeaponSwapAxis()
	{
		return input.GetWeaponSwapAxis();
	}

	public bool GetFireButton()
	{
		return input.GetFireButton();
	}

	public bool GetReloadButton()
	{
		return input.GetReloadButton();
	}

	public bool GetJumpButton()
	{
		return input.GetJumpButton();
	}

	public bool GetSprintButton()
	{
		return input.GetSprintButton();
	}

	public bool GetPauseButton()
	{
		return input.GetPauseButton();
	}

	static public InputManager Instance
	{
		get
		{
			if (!instance)
			{
				instance = FindObjectOfType<InputManager>();
				if (!instance)
				{
					GameObject gameObj = new GameObject("Input Manager");
					instance = gameObj.AddComponent<InputManager>();
				}
			}
			return instance;
		}
	}
}
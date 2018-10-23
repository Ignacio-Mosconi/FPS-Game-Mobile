using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
	static InputManager instance;
	IInput input;
	EventSystem eventSystem;

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

			eventSystem = FindObjectOfType<EventSystem>();

			if (!eventSystem)
        	{
				GameObject gameObj = new GameObject("EventSystem");
				eventSystem = gameObj.AddComponent<EventSystem>();
				StandaloneInputModule inputModule = gameObj.AddComponent<StandaloneInputModule>();
				
				#if UNITY_STANDALONE
					inputModule.verticalAxis = "Vertical UI";
					inputModule.horizontalAxis = "Horizontal UI";
					inputModule.submitButton = "Select";
					inputModule.cancelButton = "Return";
				#else
					inputModule.verticalAxis = "Vertical UI Controller Mobile";
					inputModule.horizontalAxis = "Horizontal UI Controller Mobile";
					inputModule.submitButton = "Select Controller Mobile";
					inputModule.cancelButton = "Return Controller Mobile";
				#endif
        	}
        	
			DontDestroyOnLoad(eventSystem.gameObject);
		}
	}

	public void ChangeFirstMenuItemSelected(GameObject firstMenuElement)
    {
		if (CheckControllerConnection())
		{
			eventSystem.firstSelectedGameObject = firstMenuElement;
			eventSystem.SetSelectedGameObject(firstMenuElement);
		}
    }

    public bool CheckControllerConnection()
    {
        bool controllerConnected = false;
        string[] controllers = Input.GetJoystickNames();
        
        for (int i = 0; i < controllers.Length; i++)
        {
            if (!string.IsNullOrEmpty(controllers[i]))
            {
                controllerConnected = true;
                break;
            }
        }

        return controllerConnected;
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

	public bool GetSprintButtonModifier()
	{
		return input.GetSprintButtonModifier();
	}

	public bool GetSwapWeaponButton()
	{
		return input.GetSwapWeaponButton();
	}

	public bool GetInteractButton()
	{
		return input.GetInteractButton();
	}

	public bool GetInteractHoldButton()
	{
		return input.GetInteractHoldButton();
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
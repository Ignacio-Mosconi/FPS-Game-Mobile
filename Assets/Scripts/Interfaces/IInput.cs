using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInput
{
	float GetHorizontalAxis();
	float GetVerticalAxis();
	float GetHorizontalViewAxis();
	float GetVerticalViewAxis(); 
	bool GetFireButton();
	bool GetReloadButton();
	bool GetJumpButton();
	bool GetSprintButton();
}
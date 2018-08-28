using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInput
{
	float GetHorizontalAxis();
	float GetVerticalAxis();
	float GetHorizontalViewAxis();
	float GetVerticalViewAxis();
	float GetWeaponSwapAxis();
	bool GetFireButton();
	bool GetReloadButton();
	bool GetJumpButton();
	bool GetSprintButton();
}
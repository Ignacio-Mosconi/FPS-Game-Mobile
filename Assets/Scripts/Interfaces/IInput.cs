﻿using System.Collections;
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
	bool GetSprintButtonModifier();
	bool GetSwapWeaponButton();
	bool GetInteractButton();
	bool GetInteractHoldButton();
	bool GetPauseButton();
}
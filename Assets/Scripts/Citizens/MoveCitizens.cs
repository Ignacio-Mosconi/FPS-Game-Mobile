using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCitizens : MonoBehaviour
{
    Transform[] citizens;

    void Start()
    {
        citizens = GetComponentsInChildren<Transform>();
    }

    public void MoveThem()
    {
        foreach (Transform citizen in citizens)
        {
            if(citizen.CompareTag("Citizen"))
            {
                /*CitizenAI c = citizen.GetComponent<CitizenAI>();
                c.enabled = true;
                CitizenAnimation ca = citizen.GetComponentInChildren<CitizenAnimation>();*/

                citizen.gameObject.AddComponent<CitizenAI>();
                citizen.GetChild(0).gameObject.AddComponent<CitizenAnimation>();
            }
        }
    }
}

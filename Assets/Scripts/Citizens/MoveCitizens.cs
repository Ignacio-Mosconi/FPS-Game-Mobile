using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveCitizens : MonoBehaviour
{
    [SerializeField] UnityEvent onAllRescued;

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
                CitizenAI cAI = citizen.gameObject.AddComponent<CitizenAI>();
                citizen.GetChild(0).gameObject.AddComponent<CitizenAnimation>();

                cAI.OnRescued.AddListener(CheckCount);
            }
        }
    }

    void CheckCount()
    {
        // Checked by 1 because this function is called before the last Citizen is destroyed.
        if (transform.childCount <= 1)
        {
            Debug.Log("Citizen group rescued.");

            onAllRescued.Invoke();
        }
    }

    public UnityEvent OnAllRescued
    {
        get { return onAllRescued; }
    }
}

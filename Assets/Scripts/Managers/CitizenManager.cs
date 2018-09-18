using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenManager : MonoBehaviour
{
    GameObject[] citizenGroups;

    void Start()
    {
        citizenGroups = GameObject.FindGameObjectsWithTag("CitizenGroup");

        GameObject[] enemyGroups = GameObject.FindGameObjectsWithTag("ZombieGroup");

        //citizenGroups.
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupsManager : MonoBehaviour
{
    static GroupsManager instance;

    [SerializeField] GameObject[] citizenGroups;
    [SerializeField] GameObject[] enemyGroups;

    void Start()
    {
        enemyGroups = GameObject.FindGameObjectsWithTag("ZombieGroup");

        Life[] lifes = FindObjectsOfType<Life>();

        foreach (Life life in lifes)
        {
            if (life.gameObject.layer == LayerMask.NameToLayer("Zombies"))
                life.OnDeath.AddListener(CheckEnemyGroupState);
        }
    }

    void CheckEnemyGroupState()
    {
        for (int i = 0; i < enemyGroups.Length; i++)
        {
            if (enemyGroups[i].transform.childCount == 0)
            {
                CheckCitizenGrouState(i);
            }
        }
    }

    void CheckCitizenGrouState(int index)
    {
        citizenGroups[index].GetComponent<MoveCitizens>().MoveThem();
    }

    public static GroupsManager Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<GroupsManager>();
            if (!instance)
            {
                GameObject gameObj = new GameObject("Groups Manager");
                instance = gameObj.AddComponent<GroupsManager>();
            }
            return instance;
        }
    }
}

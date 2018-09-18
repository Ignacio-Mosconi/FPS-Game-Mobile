using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] UnityEvent onGroupEliminated;

    GameObject[] enemyGroups;

    void Start()
    {
        enemyGroups = GameObject.FindGameObjectsWithTag("ZombieGroup");

        Life[] lifes = FindObjectsOfType<Life>();

        foreach (Life life in lifes)
        {
            if (life.gameObject.layer == LayerMask.NameToLayer("Zombies"))
                life.OnDeath.AddListener(CheckGroupsState);
        }
    }

    void CheckGroupsState()
    {
        foreach (GameObject enemyGroup in enemyGroups)
        {
            if(enemyGroup.transform.childCount == 0)
            {
                
            }
        }
    }

    public UnityEvent OnGroupEliminated
    {
        get { return onGroupEliminated; }
    }
}

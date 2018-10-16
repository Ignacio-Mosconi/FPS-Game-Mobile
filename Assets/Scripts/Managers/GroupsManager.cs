using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupsManager : MonoBehaviour
{
    static GroupsManager instance;

    [SerializeField] List<Transform> citizenGroups;
    [SerializeField] List<Transform> enemyGroups;

    void Start()
    {
        for (int i = 0; i < enemyGroups.Count; i++)
        {
            for (int j = 0; j < enemyGroups[i].childCount; j++)
            {
                Transform child = enemyGroups[i].GetChild(j);

                if (child.gameObject.layer == LayerMask.NameToLayer("Zombies"))
                {
                    Life life = child.GetComponent<Life>();

                    if (!life) Debug.Log("No life component at " + life.name);

                    life.OnDeath.AddListener(CheckEnemyGroupState);
                }
            }
        }
    }

    void CheckEnemyGroupState()
    {
        for (int i = 0; i < enemyGroups.Count; i++)
        {
            //Debug.Log(enemyGroups[i].childCount);
            if (enemyGroups[i].childCount == 0 || enemyGroups[i].childCount == 1)
            {
                Debug.Log("Zombie group exterminated.");
                CheckCitizenGroupState(i);
                enemyGroups.RemoveAt(i);
                return;
            }
        }
    }

    void CheckCitizenGroupState(int index)
    {
        citizenGroups[index].GetComponent<MoveCitizens>().MoveThem();
        citizenGroups.RemoveAt(index);
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

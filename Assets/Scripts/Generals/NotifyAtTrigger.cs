using UnityEngine;
using UnityEngine.Events;

public class NotifyAtTrigger : MonoBehaviour
{
    bool isTriggering;

    void Start()
    {
        isTriggering = false;
    }

    void OnTriggerEnter()
    {
        isTriggering = true;
    }

    void OnTriggerExit()
    {
        isTriggering = false;
    }

    public bool IsTriggering
    {
        get { return isTriggering;  }
        set { isTriggering = value; }
    }
}

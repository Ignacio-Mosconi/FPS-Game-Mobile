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
        Debug.Log("Enter to " + name);
        isTriggering = true;
    }

    void OnTriggerExit()
    {
        Debug.Log("Exit from " + name);
        isTriggering = false;
    }

    public bool IsTriggering
    {
        get { return isTriggering;  }
        set { isTriggering = value; }
    }
}

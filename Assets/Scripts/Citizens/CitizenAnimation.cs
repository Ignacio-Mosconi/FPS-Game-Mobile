using UnityEngine;

public class CitizenAnimation : MonoBehaviour
{
    CitizenAI ai;
    Animator anim;

    void Awake()
    {
        ai = GetComponentInParent<CitizenAI>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetFloat("Velocity", ai.GetActualSpeed() / ai.getMaxSpeed());
    }

    void OnEnable()
    {
        anim.SetTrigger("Rescued");
    }
}

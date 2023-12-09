using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionObject : MonoBehaviour
{
    [SerializeField] public ActionType actionType;
    [SerializeField] private Animator animator;

    public void Activate()
    {

        animator.SetBool("activated", true);

        if (actionType == ActionType.Bridge) GetComponent<BoxCollider>().enabled = true; ;
    }
}

public enum ActionType
{
    Bridge,
    Door
}

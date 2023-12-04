using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] protected int operationsLimit;
    [SerializeField] protected int operationsLeft;

    public virtual void ShowInfo()
    {

    }

    public virtual void Interact()
    {

    }

    public virtual void DoAction()
    {

    }
}

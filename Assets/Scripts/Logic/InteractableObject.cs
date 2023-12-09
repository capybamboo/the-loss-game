using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public int operationsLimit;
    public int operationsLeft;

    protected GameManager gm;

    public virtual void Interact()
    {

    }

    public virtual void DoAction()
    {

    }

    protected void SomeStart()
    {
        operationsLeft = operationsLimit;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
}

public enum InteractableType
{
    Mif1,
    Mif2,
    Spawner,
    Receiver
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIF : InteractableObject
{
    [SerializeField] protected OperationType currentOperation;
    [SerializeField] protected ConsumType currentTrigger;
    protected MifType mifType;

    protected ConsumableObject currentProduct;

    public virtual bool DoAction(ConsumableObject cob)
    {
        if (operationsLeft == 0) return false;

        KeyValuePair<ConsumType, int> cobInfo = cob.GetConsumInfo();

        if (cobInfo.Key != currentTrigger) return false;

        return true;
    }

    public void ResetProduct()
    {
        currentProduct = null;
    }

    public void SetTrigger(ConsumType trigger)
    {
        currentTrigger = trigger;
    }

    public void SetOperation(OperationType o)
    {
        currentOperation = o;
    }

    public ConsumType GetTrigger()
    {
        return currentTrigger;
    }

    public OperationType GetOperation()
    {
        return currentOperation;
    }

    public MifType GetMifType()
    {
        return mifType;
    }
}

public enum OperationType
{
    Charge,
    Discharge,
    Increase,
    Decrease
}

public enum MifType
{
    Mif1,
    Mif2
}

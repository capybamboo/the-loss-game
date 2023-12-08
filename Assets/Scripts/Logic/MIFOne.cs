using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIFOne : MIF
{
    void Start()
    {
        SomeStart();
        mifType = MifType.Mif1;
    }

    public override bool DoAction(ConsumableObject cob)
    {
        if (!base.DoAction(cob) || cob == currentProduct) return false;

        if (currentOperation == OperationType.Increase && cob.LevelUp()) operationsLeft--;

        else if (currentOperation == OperationType.Decrease && cob.LevelDown()) operationsLeft--;

        else return false;

        currentProduct = cob;

        return true;
    }
}


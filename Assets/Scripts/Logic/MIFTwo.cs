using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIFTwo : MIF
{
    void Start()
    {
        SomeStart();
        mifType = MifType.Mif2;
    }

    public override bool DoAction(ConsumableObject cob)
    {
        if (!base.DoAction(cob) || cob == currentProduct) return false;

        if (currentOperation == OperationType.Charge && cob.ChargeUp()) operationsLeft--;

        else if (currentOperation == OperationType.Discharge && cob.ChargeDown()) operationsLeft--;

        else return false;

        currentProduct = cob;

        return true;
    }
}

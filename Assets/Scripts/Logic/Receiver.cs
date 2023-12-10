using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : InteractableObject
{
    [Header("Reauired Consume info")]
    public ConsumType requiredType;
    [Range(1, 3)] public int requiredLevel;
    public ChargeLevel requiredChargeLevel = ChargeLevel.Neutral;

    [Space]
    [SerializeField] private GameObject particleObject;
    [SerializeField] private Transform itemPlace;
    [SerializeField] private ActionObject actionObject;
    public bool activated = false;

    private void Start()
    {
        SomeStart();
    }

    public void PutRequest(ConsumableObject cob)
    {
        if (activated) return;
        KeyValuePair<ConsumType, int> cobInfo = cob.GetConsumInfo();

        ConsumType type = cobInfo.Key;
        int level = cobInfo.Value;
        ChargeLevel chargeLevel = cob.GetCharge();

        if (type != requiredType || level != requiredLevel || chargeLevel != requiredChargeLevel) return;

        DoAction();

        gm.pb.ResetHandItem();

        cob.SwitchKinematic(false);
        cob.transform.SetParent(itemPlace);
        cob.transform.localPosition = Vector3.zero;
        cob.transform.localRotation = Quaternion.identity;
    }

    public override void DoAction()
    {
        if (actionObject.actionType == ActionType.Bridge) actionObject.gameObject.SetActive(true);

        actionObject.Activate();

        activated = true;
        particleObject.SetActive(true);

        gm.aum.PlayReceiverSound();
    }
}

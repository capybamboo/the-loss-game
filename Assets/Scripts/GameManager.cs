using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static string[] batteryNames = { "Батарейка типа <AAA>", "Батарейка типа <AA>", "Батарейка типа <C>" };
    private static string[] honeyNames = { "Энергосота", "Энергомёд", "Энергоячейка мёда" };

    private UIController uic;
    public PlayerMovement pm;
    public PlayerBehaviour pb;

    public MIF currentMif;

    private void Start()
    {
        uic = GetComponent<UIController>();
    }

    public static string GetConsumeName(KeyValuePair<ConsumType, int> info)
    {
        ConsumType type = info.Key;
        int level = info.Value;

        if (type == ConsumType.Battery) return batteryNames[level - 1];

        else if (type == ConsumType.HoneyCell) return honeyNames[level - 1];

        return "Не определено";
    }

    public static int TriggerToInt(ConsumType type)
    {
        int res = type == ConsumType.Battery ? 0 : 1;

        return res;
    }

    public static int OperationToInt(OperationType type)
    {
        int res;

        if (type == OperationType.Charge || type == OperationType.Increase) res = 0;
        else res = 1;

        return res;
    }

    public static string ChargeLevelToName(ChargeLevel level)
    {
        if (level == ChargeLevel.Negative) return "Отрицательный";
        else if (level == ChargeLevel.Neutral) return "Нейтральный";
        else return "ПолоZHительный";
    }

    public void ChangeMifTrigger(int val)
    {
        if (val == 0) currentMif.SetTrigger(ConsumType.Battery);

        else if (val == 1) currentMif.SetTrigger(ConsumType.HoneyCell);
    }

    public void ChangeMifOperation(int val)
    {
        if (currentMif.GetMifType() == MifType.Mif1)
        {
            if (val == 0) currentMif.SetOperation(OperationType.Increase);

            else if (val == 1) currentMif.SetOperation(OperationType.Decrease);
        }

        else
        {
            if (val == 0) currentMif.SetOperation(OperationType.Charge);

            else if (val == 1) currentMif.SetOperation(OperationType.Discharge);
        }
    }
}

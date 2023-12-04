using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static string[] batteryNames = { "Микробатарейка", "Норм батарейка", "Биг батарейка" };
    private static string[] honeyNames = { "Энергосота", "Энергомёд", "Энергошар" };

    private UIController uic;

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
}

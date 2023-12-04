using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : InteractableObject
{
    [SerializeField] private ConsumType currentType;

    private Transform spawnPoint;
    [SerializeField] private GameObject battery_indicator;
    [SerializeField] private GameObject honey_indicator;

    [Space]
    [SerializeField] private GameObject batteryPrefab;
    [SerializeField] private GameObject honeyPrefab;

    private void Start()
    {
        spawnPoint = transform.Find("spawn point");
        operationsLeft = operationsLimit;
    }

    public override void DoAction()
    {
        if (operationsLeft == 0) return;

        GameObject product;

        if (currentType == ConsumType.Battery) product = Instantiate(batteryPrefab, spawnPoint.position, Quaternion.identity);
        else product = Instantiate(honeyPrefab, spawnPoint.position, Quaternion.identity);

        operationsLeft--;
    }

    public override void Interact()
    {
        if (currentType == ConsumType.Battery)
        {
            currentType = ConsumType.HoneyCell;
            battery_indicator.SetActive(false);
            honey_indicator.SetActive(true);
        }

        else
        {
            currentType = ConsumType.Battery;
            battery_indicator.SetActive(true);
            honey_indicator.SetActive(false);
        }
    }
}

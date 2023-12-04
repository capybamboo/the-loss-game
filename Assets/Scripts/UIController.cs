using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI lookItemName;

    [SerializeField] private Transform hints;
    private TextMeshProUGUI pickupHint;
    private TextMeshProUGUI throwHint;

    private void Start()
    {
        pickupHint = hints.Find("pickupHint").GetComponent<TextMeshProUGUI>();
        throwHint = hints.Find("throwHint").GetComponent<TextMeshProUGUI>();

        SetHandItemInfo("");
    }

    public void SetHandItemInfo(string name)
    {
        itemName.text = name;
    }

    public void SetLookItemInfo(string name)
    {
        lookItemName.text = name;
    }

    public void SetPickupHintVisibility(bool state)
    {
        pickupHint.gameObject.SetActive(state);
    }

    public void SetThrowHintVisibility(bool state)
    {
        throwHint.gameObject.SetActive(state);
    }
}

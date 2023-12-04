using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI lookItemName;

    [SerializeField] private TextMeshProUGUI interactHint;
    [SerializeField] private TextMeshProUGUI throwHint;
    [SerializeField] private TextMeshProUGUI actionHint;

    private void Start()
    {
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

    public void SetInteractHintVisibility(bool state)
    {
        interactHint.gameObject.SetActive(state);
    }

    public void SetThrowHintVisibility(bool state)
    {
        throwHint.gameObject.SetActive(state);
    }

    public void SetActionHintVisibility(bool state)
    {
        actionHint.gameObject.SetActive(state);
    }
}

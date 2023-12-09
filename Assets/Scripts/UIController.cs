using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemChargeLevel;
    [SerializeField] private TextMeshProUGUI lookItemName;

    private GameManager gm;

    [Header("Hints")]
    [SerializeField] private TextMeshProUGUI interactHint;
    [SerializeField] private TextMeshProUGUI throwHint;
    [SerializeField] private TextMeshProUGUI actionHint;
    [SerializeField] private TextMeshProUGUI infoHint;

    [Header("MIF window")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI MifName;
    [SerializeField] private Dropdown dropdownTriggers;
    [SerializeField] private Dropdown dropdownOperationsMif1;
    [SerializeField] private Dropdown dropdownOperationsMif2;

    [Header("Info window")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Text operations;
    [SerializeField] private Text requiredItem;
    [SerializeField] private GameObject mif1Info;
    [SerializeField] private GameObject mif2Info;
    [SerializeField] private GameObject spawnerInfo;
    [SerializeField] private GameObject receiverInfo;

    private void Start()
    {
        gm = GetComponent<GameManager>();
        SetHandItemInfo("", "");
    }

    public void SetHandItemInfo(string name, string level)
    {
        itemName.text = name;
        itemChargeLevel.text = level;
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

    public void SetInfoHintVisibility(bool state)
    {
        infoHint.gameObject.SetActive(state);
    }

    public void ShowMIFWindow(MIF mif)
    {
        Cursor.lockState = CursorLockMode.Confined;
        gm.pm.LockMovement();

        gm.currentMif = mif;

        panel.SetActive(true);

        dropdownTriggers.value = GameManager.TriggerToInt(mif.GetTrigger());

        if (mif.GetMifType() == MifType.Mif1)
        {
            MifName.text = "МИФ-1\n(Материю Изменяющий Фильтр)";
            dropdownOperationsMif1.gameObject.SetActive(true);
            dropdownOperationsMif2.gameObject.SetActive(false);

            dropdownOperationsMif1.value = GameManager.OperationToInt(mif.GetOperation());
        }

        else
        {
            MifName.text = "МИФ-2\n(Материю Изменяющий Фильтр)";
            dropdownOperationsMif2.gameObject.SetActive(true);
            dropdownOperationsMif1.gameObject.SetActive(false);

            dropdownOperationsMif2.value = GameManager.OperationToInt(mif.GetOperation());
        }
    }

    public void HideMIFWindow()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gm.pm.UnlockMovement();

        panel.SetActive(false);

        gm.currentMif = null;
    }

    public void ShowMif1Info(InteractableObject itob)
    {
        infoPanel.SetActive(true);

        spawnerInfo.SetActive(false);
        receiverInfo.SetActive(false);
        mif2Info.SetActive(false);
        mif1Info.SetActive(true);
        requiredItem.gameObject.SetActive(false);

        operations.text = $"Действий: {itob.operationsLeft}/{itob.operationsLimit}";
    }

    public void ShowMif2Info(InteractableObject itob)
    {
        infoPanel.SetActive(true);

        spawnerInfo.SetActive(false);
        receiverInfo.SetActive(false);
        mif2Info.SetActive(true);
        mif1Info.SetActive(false);
        requiredItem.gameObject.SetActive(false);

        operations.text = $"Действий: {itob.operationsLeft}/{itob.operationsLimit}";
    }

    public void ShowSpawnerInfo(InteractableObject itob)
    {
        infoPanel.SetActive(true);

        spawnerInfo.SetActive(true);
        receiverInfo.SetActive(false);
        mif2Info.SetActive(false);
        mif1Info.SetActive(false);
        requiredItem.gameObject.SetActive(false);

        operations.text = $"Действий: {itob.operationsLeft}/{itob.operationsLimit}";
    }

    public void ShowReceiverInfo(InteractableObject itob)
    {
        infoPanel.SetActive(true);

        spawnerInfo.SetActive(false);
        receiverInfo.SetActive(true);
        mif2Info.SetActive(false);
        mif1Info.SetActive(false);

        Receiver receiver = (Receiver)itob;
        ChargeLevel chargeLevel = receiver.requiredChargeLevel;
        KeyValuePair<ConsumType, int> info = new KeyValuePair<ConsumType, int>(receiver.requiredType, receiver.requiredLevel);

        requiredItem.gameObject.SetActive(true);
        requiredItem.text = $"Требуемый объект:\n{GameManager.ChargeLevelToName(chargeLevel)} {GameManager.GetConsumeName(info)}";

        operations.text = "";
    }

    public void HideInfo()
    {
        infoPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        gm.pm.UnlockMovement();
    }
}

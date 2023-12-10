using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private UIController uic;

    [SerializeField] private float lookRange = 100f;
    [SerializeField] private float currentLookRange;
    [SerializeField] private Camera cam;

    [Space]
    private Transform Hand;
    [SerializeField] private ConsumableObject handItem;
    [SerializeField] private GameObject lookObject;
    private bool lookObjectIsConsume;
    private bool lookObjectIsInteractable;
    private InteractableType lookInteractableType;

    [Space]
    private float jumpUpgradeTime;
    private float handUpgradeTime;

    void Start()
    {
        Hand = transform.Find("Hand");
    }

    void Update()
    {
        LookLogic(); // проверка взгляда на объекты
        TryPickUp(); // поднятие расходников
        TryThrow(); // убрать из рук расходник
        TryInteract(); // нажатие F
        TryDoAction(); // нажатие E
        TrySeeInfo(); // нажатие I
        TryEatConsume(); // нажатие R

        if (jumpUpgradeTime > 0) jumpUpgradeTime -= Time.deltaTime;
        else gm.pm.ResetJumpHeight();

        if (handUpgradeTime > 0) handUpgradeTime -= Time.deltaTime;
        else ResetLookRange();
    }
    
    private void LookLogic()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentLookRange) && hit.collider.gameObject.CompareTag("Raycastable"))
        {
            if (hit.collider.gameObject.GetComponentInParent<ConsumableObject>()) // если смотрим на расходник
            {
                ConsumableObject cob = hit.collider.gameObject.GetComponentInParent<ConsumableObject>();

                lookObject = cob.gameObject;
                lookObjectIsConsume = true;

                string cobName = GameManager.GetConsumeName(cob.GetConsumInfo());

                uic.SetLookItemInfo(cobName);
                if (!handItem) uic.SetActionHintVisibility(true);
                else uic.SetActionHintVisibility(false);
            }

            else if (hit.collider.gameObject.GetComponent<InteractableObject>()) // если смотрим на интерактивный объект
            {
                InteractableObject itob = hit.collider.gameObject.GetComponent<InteractableObject>();

                lookObject = itob.gameObject;
                lookObjectIsConsume = false;
                lookObjectIsInteractable = true;

                if (itob.GetComponent<Spawner>())
                {
                    lookInteractableType = InteractableType.Spawner;
                    uic.SetInteractHintVisibility(true);
                    uic.SetLookItemInfo("Спаунер");
                }

                else if (itob.GetComponent<Receiver>())
                {
                    lookInteractableType = InteractableType.Receiver;
                    uic.SetLookItemInfo("Энергоприёмник");
                }

                uic.SetActionHintVisibility(true);
                uic.SetInfoHintVisibility(true);
            }

            else if (hit.collider.gameObject.GetComponentInParent<InteractableObject>()) // если смотрим на интерактивный объект
            {
                InteractableObject itob = hit.collider.gameObject.GetComponentInParent<InteractableObject>();

                lookObject = itob.gameObject;
                lookObjectIsConsume = false;
                lookObjectIsInteractable = true;

                if (((MIF)itob).GetMifType() == MifType.Mif1) lookInteractableType = InteractableType.Mif1;
                else lookInteractableType = InteractableType.Mif2;

                uic.SetLookItemInfo("Миф");
                uic.SetInteractHintVisibility(true);
                uic.SetInfoHintVisibility(true);
            }
        }

        else
        {
            lookObject = null;
            uic.SetLookItemInfo("");
            uic.SetInteractHintVisibility(false);
            uic.SetActionHintVisibility(false);
            uic.SetInfoHintVisibility(false);
        }
    }

    private void TryEatConsume()
    {
        if (handItem && Input.GetKeyDown(KeyCode.R))
        {
            ChargeLevel chargeLevel = handItem.GetCharge();
            ConsumType consumType = handItem.GetConsumInfo().Key;
            int consumLevel = handItem.GetConsumInfo().Value;

            if (chargeLevel != ChargeLevel.Negative) return;

            if (consumType == ConsumType.Battery) UpgradeHand(consumLevel);
            else if (consumType == ConsumType.HoneyCell) UpgradeJump(consumLevel);

            ResetHandItemAndDestroy();
        }
    }

    private void TrySeeInfo()
    {
        if (lookObject && lookObjectIsInteractable && Input.GetKeyDown(KeyCode.I))
        {
            gm.pm.LockMovement();
            Cursor.lockState = CursorLockMode.Confined;

            if (lookInteractableType == InteractableType.Mif1) uic.ShowMif1Info(lookObject.GetComponent<InteractableObject>());
            else if (lookInteractableType == InteractableType.Mif2) uic.ShowMif2Info(lookObject.GetComponent<InteractableObject>());
            else if (lookInteractableType == InteractableType.Spawner) uic.ShowSpawnerInfo(lookObject.GetComponent<InteractableObject>());
            else if (lookInteractableType == InteractableType.Receiver) uic.ShowReceiverInfo(lookObject.GetComponent<InteractableObject>());
        }
    }

    private void TryInteract()
    {
        if (lookObject && !lookObjectIsConsume && Input.GetKeyDown(KeyCode.F))
        {
            InteractableObject itob;

            if (lookObject.GetComponent<Spawner>())
            {
                itob = lookObject.GetComponent<Spawner>();

                itob.Interact();
            }

            else if (lookObject.GetComponent<MIF>())
            {
                itob = lookObject.GetComponent<MIF>();

                uic.ShowMIFWindow((MIF) itob);
            }
        }
    }

    private void TryDoAction()
    {
        if (lookObject && !lookObjectIsConsume && Input.GetKeyDown(KeyCode.E))
        {
            InteractableObject itob;

            if (lookObject.GetComponent<Spawner>())
            {
                itob = lookObject.GetComponent<Spawner>();

                itob.DoAction();
            }

            else if (lookObject.GetComponent<Receiver>() && handItem)
            {
                Receiver receiver = lookObject.GetComponent<Receiver>();

                receiver.PutRequest(handItem);
            }
        }
    }

    private void TryPickUp()
    {
        if (lookObject && lookObjectIsConsume && !handItem && Input.GetKeyDown(KeyCode.E))
        {
            handItem = lookObject.GetComponent<ConsumableObject>();

            handItem.SwitchKinematic(false);
            handItem.transform.SetParent(Hand);
            handItem.transform.localPosition = Vector3.zero;
            handItem.transform.localRotation = Quaternion.identity;
            handItem.transform.Rotate(0, 180, 0);

            string cobName = GameManager.GetConsumeName(handItem.GetConsumInfo());

            uic.SetHandItemInfo(cobName, GameManager.ChargeLevelToName(handItem.GetCharge()));
            uic.SetThrowHintVisibility(true);
        }
    }

    public void UpdateHandItemInfo()
    {
        string name = GameManager.GetConsumeName(handItem.GetConsumInfo());
        string level = GameManager.ChargeLevelToName(handItem.GetCharge());

        uic.SetHandItemInfo(name, level);
    }

    private void TryThrow()
    {
        if (handItem && Input.GetKeyDown(KeyCode.Q)) ResetHandItem();
    }

    public void ChangeLookRange(float val)
    {
        currentLookRange = val;
    }

    public void ResetLookRange()
    {
        currentLookRange = lookRange;
    }

    public void UpgradeJump(int level)
    {
        int multi = 6;

        if (level == 1) jumpUpgradeTime = 10f;
        else if (level == 2) jumpUpgradeTime = 20f;
        else if (level == 3) jumpUpgradeTime = 30f;

        gm.pm.ChangeJumpHeight(multi);
        
    }

    public void UpgradeHand(int level)
    {
        int multi = 12;

        if (level == 1) handUpgradeTime = 10f;
        else if (level == 2) handUpgradeTime = 20f;
        else if (level == 3) handUpgradeTime = 30f;

        ChangeLookRange(multi);
    }

    public void ResetHandItem()
    {
        handItem.transform.SetParent(null);
        handItem.SwitchKinematic(true);
        handItem = null;

        uic.SetHandItemInfo("", "");
        uic.SetThrowHintVisibility(false);
    }

    public void ResetHandItemAndDestroy()
    {
        handItem.transform.SetParent(null);
        handItem.SwitchKinematic(true);
        GameObject o = handItem.gameObject;
        handItem = null;

        Destroy(o);

        uic.SetHandItemInfo("", "");
        uic.SetThrowHintVisibility(false);
    }
}

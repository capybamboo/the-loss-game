using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private UIController uic;

    [SerializeField] private float lookRange = 100f;
    [SerializeField] private Camera cam;

    [Space]
    private Transform Hand;
    [SerializeField] private ConsumableObject handItem;
    [SerializeField] private GameObject lookObject;
    private bool lookObjectIsConsume;

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
    }
    
    private void LookLogic()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, lookRange) && hit.collider.gameObject.CompareTag("Raycastable"))
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

                uic.SetLookItemInfo("Спаунер");
                uic.SetInteractHintVisibility(true);
                uic.SetActionHintVisibility(true);
                uic.SetInfoHintVisibility(true);
            }

            else if (hit.collider.gameObject.GetComponentInParent<InteractableObject>()) // если смотрим на интерактивный объект
            {
                InteractableObject itob = hit.collider.gameObject.GetComponentInParent<InteractableObject>();

                lookObject = itob.gameObject;
                lookObjectIsConsume = false;

                uic.SetLookItemInfo("Мииииф");
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
        if (handItem && Input.GetKeyDown(KeyCode.Q))
        {
            handItem.transform.SetParent(null);
            handItem.SwitchKinematic(true);

            handItem = null;

            uic.SetHandItemInfo("", "");
            uic.SetThrowHintVisibility(false);
        }
    }
}

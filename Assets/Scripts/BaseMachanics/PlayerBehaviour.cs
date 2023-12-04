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
        LookLogic();
        TryPickUp();
        TryThrow();
        TryInteract();
        TryDoAction();
    }

    private void LookLogic()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, lookRange) && hit.collider.gameObject.CompareTag("Raycastable"))
        {
            if (hit.collider.gameObject.GetComponent<ConsumableObject>())
            {
                ConsumableObject cob = hit.collider.gameObject.GetComponent<ConsumableObject>();

                lookObject = cob.gameObject;
                lookObjectIsConsume = true;

                string cobName = GameManager.GetConsumeName(cob.GetConsumInfo());

                uic.SetLookItemInfo(cobName);
                if (!handItem) uic.SetInteractHintVisibility(true);
                else uic.SetInteractHintVisibility(false);
            }

            else if (hit.collider.gameObject.GetComponent<InteractableObject>())
            {
                InteractableObject itob = hit.collider.gameObject.GetComponent<InteractableObject>();

                lookObject = itob.gameObject;
                lookObjectIsConsume = false;

                uic.SetLookItemInfo("Спаунер");
                uic.SetInteractHintVisibility(true);
                uic.SetActionHintVisibility(true);
            }
        }

        else
        {
            lookObject = null;
            uic.SetLookItemInfo("");
            uic.SetInteractHintVisibility(false);
            uic.SetActionHintVisibility(false);
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

            string cobName = GameManager.GetConsumeName(handItem.GetConsumInfo());

            uic.SetHandItemInfo(cobName);
            uic.SetThrowHintVisibility(true);
        }
    }

    private void TryThrow()
    {
        if (handItem && Input.GetKeyDown(KeyCode.Q))
        {
            handItem.transform.SetParent(null);
            handItem.SwitchKinematic(true);

            handItem = null;

            uic.SetHandItemInfo("");
            uic.SetThrowHintVisibility(false);
        }
    }
}

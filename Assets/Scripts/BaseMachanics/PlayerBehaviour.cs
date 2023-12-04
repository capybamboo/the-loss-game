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
    [SerializeField] private ConsumableObject lookItem;

    void Start()
    {
        Hand = transform.Find("Hand");
    }

    void Update()
    {
        LookLogic();
        TryPickUp();
        TryThrow();
    }

    private void LookLogic()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, lookRange) && hit.collider.gameObject.CompareTag("Raycastable"))
        {
            if (hit.collider.gameObject.GetComponent<ConsumableObject>())
            {
                ConsumableObject cob = hit.collider.gameObject.GetComponent<ConsumableObject>();
                lookItem = cob;

                string cobName = GameManager.GetConsumeName(cob.GetConsumInfo());

                uic.SetLookItemInfo(cobName);
                if (!handItem) uic.SetPickupHintVisibility(true);
            }
        }

        else
        {
            lookItem = null;
            uic.SetLookItemInfo("");
            uic.SetPickupHintVisibility(false);
        }
    }

    private void TryPickUp()
    {
        if (lookItem && !handItem && Input.GetKeyDown(KeyCode.E))
        {
            handItem = lookItem;

            handItem.SwitchGravity(false);
            handItem.transform.SetParent(Hand);
            handItem.transform.localPosition = Vector3.zero;

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
            handItem.SwitchGravity(true);

            handItem = null;

            uic.SetHandItemInfo("");
            uic.SetThrowHintVisibility(false);
        }
    }
}

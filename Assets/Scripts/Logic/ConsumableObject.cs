using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableObject : MonoBehaviour
{
    [SerializeField] private ConsumType consumType;
    [SerializeField] [Range(1, 3)] private int consumLevel;
    [SerializeField] private ChargeLevel chargeLevel = ChargeLevel.Neutral;

    [Space]
    [SerializeField] private GameObject lvl1_mesh;
    [SerializeField] private GameObject lvl2_mesh;
    [SerializeField] private GameObject lvl3_mesh;

    private Rigidbody rb;
    private GameManager gm;
    private PlayerBehaviour pb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        pb = gm.pb;
    }

    public KeyValuePair<ConsumType, int> GetConsumInfo()
    {
        return new KeyValuePair<ConsumType, int>(consumType, consumLevel);
    }

    public void SwitchKinematic(bool state)
    {
        rb.useGravity = state;
        rb.isKinematic = !state;
    }

    public bool LevelUp()
    {
        if (consumLevel == 3) return false;

        consumLevel++;

        if (consumLevel == 2)
        {
            lvl2_mesh.SetActive(true);
            lvl1_mesh.SetActive(false);
        }

        else if (consumLevel == 3)
        {
            lvl3_mesh.SetActive(true);
            lvl2_mesh.SetActive(false);
        }

        pb.UpdateHandItemInfo();

        return true;
    }

    public bool LevelDown()
    {
        if (consumLevel == 1) return false;

        consumLevel--;

        if (consumLevel == 2)
        {
            lvl2_mesh.SetActive(true);
            lvl3_mesh.SetActive(false);
        }

        else if (consumLevel == 1)
        {
            lvl1_mesh.SetActive(true);
            lvl2_mesh.SetActive(false);
        }

        pb.UpdateHandItemInfo();

        return true;
    }

    public bool ChargeUp()
    {
        if (chargeLevel == ChargeLevel.Positive) return false;

        chargeLevel = chargeLevel == ChargeLevel.Neutral ? ChargeLevel.Positive : ChargeLevel.Neutral;

        pb.UpdateHandItemInfo();

        return true;
    }

    public bool ChargeDown()
    {
        if (chargeLevel == ChargeLevel.Negative) return false;

        chargeLevel = chargeLevel == ChargeLevel.Neutral ? ChargeLevel.Negative : ChargeLevel.Neutral;

        pb.UpdateHandItemInfo();

        return true;
    }

    public ChargeLevel GetCharge()
    {
        return chargeLevel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<MIF>())
        {
            MIF mif = other.gameObject.GetComponentInParent<MIF>();

            mif.DoAction(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponentInParent<MIF>())
        {
            MIF mif = other.gameObject.GetComponentInParent<MIF>();

            mif.ResetProduct();
        }
    }
}

public enum ConsumType
{
    Battery,
    HoneyCell
}

public enum ChargeLevel
{
    Neutral,
    Positive,
    Negative
}

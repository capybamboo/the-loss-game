using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableObject : MonoBehaviour
{
    [SerializeField] private ConsumType consumType;
    [SerializeField] [Range(1, 3)] private int consumLevel;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
}

public enum ConsumType
{
    Battery,
    HoneyCell
}

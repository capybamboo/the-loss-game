using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private DialogueManager dm;

    private void Start()
    {
        dm = GetComponent<DialogueManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("dialogable"))
        {
            KeyValuePair<string, string>[] ss = other.gameObject.GetComponent<DialogueObject>().GetDialogue();
            dm.StartDialogue(ss);
        }
    }
}

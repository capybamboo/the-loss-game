using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private DialogueManager dm;
    private GameManager gm;

    [SerializeField] private GameObject endGamePanel;

    private void Start()
    {
        dm = GetComponent<DialogueManager>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("dialogable"))
        {
            KeyValuePair<string, string>[] ss = other.gameObject.GetComponent<DialogueObject>().GetDialogue();
            dm.StartDialogue(ss);
        }

        else if (other.gameObject.CompareTag("exit game"))
        {
            Cursor.lockState = CursorLockMode.Confined;
            endGamePanel.SetActive(true);
            gm.pm.LockMovement();
        }
    }
}

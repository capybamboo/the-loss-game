using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Text dialogueText;
    [SerializeField] private Text dialogueSpeaker;

    [SerializeField] private Color playerColor;
    [SerializeField] private Color nonPlayerColor;

    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject DialoguePanel;

    private string[] sentences;
    private string[] speakers;

    [SerializeField] private int index = 0;

    [SerializeField] private float scrollTime;

    private PlayerMovement pm;

    private string playerName = "Антарес (Я)";

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    public void StartDialogue(KeyValuePair<string, string>[] sents)
    {
        Cursor.lockState = CursorLockMode.Confined;
        pm.LockMovement();

        sentences = new string[sents.Length];
        speakers = new string[sents.Length];

        for (int i = 0; i < sents.Length; i++)
        {
            sentences[i] = sents[i].Value;
            speakers[i] = sents[i].Key;
        }

        index = 0;
        dialogueText.text = "";

        NextSentence();
    }

    public void EndDialogue()
    {
        DialoguePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        pm.UnlockMovement();
    }

    public void SetScrollTime(int val)
    {
        scrollTime = val;
    }

    public void NextSentence()
    {

        DialoguePanel.SetActive(true);

        if (index <= sentences.Length - 1)
        {
            dialogueText.text = "";
            nextButton.SetActive(false);
            StartCoroutine(WriteSentence());
        }

        else EndDialogue();
    }

    IEnumerator WriteSentence()
    {
        dialogueSpeaker.text = speakers[index] == "" ? playerName : speakers[index];

        dialogueSpeaker.color = speakers[index] == "" ? playerColor : nonPlayerColor;

        foreach (char el in sentences[index].ToCharArray())
        {
            dialogueText.text += el;
            yield return new WaitForSeconds(scrollTime);
        }

        index++;
        nextButton.SetActive(true);
    }
}

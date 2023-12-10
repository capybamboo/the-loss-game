using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueObject : MonoBehaviour
{
    [Header("Фразы")]
    [SerializeField] private string[] phrases;

    [Header("Говорящие")]
    [SerializeField] private string[] speakers;

    [SerializeField] private KeyValuePair<string, string>[] sentences;

    private void Start()
    {
        sentences = new KeyValuePair<string, string>[phrases.Length];

        for (int i = 0; i < phrases.Length; i++)
        {
            sentences[i] = new KeyValuePair<string, string>(speakers[i], phrases[i]);
        }
    }

    public KeyValuePair<string, string>[] GetDialogue()
    {
        return sentences;
    }
}

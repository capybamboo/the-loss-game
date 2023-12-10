using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [Header("Sounds")]
    [SerializeField] private AudioClip eatSound;
    [SerializeField] private AudioClip mifSound;
    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private AudioClip receiverSound;

    public void PlayEatSound()
    {
        audioSource.clip = eatSound;
        audioSource.Play();
    }

    public void PlayMifSound()
    {
        audioSource.clip = mifSound;
        audioSource.Play();
    }

    public void PlaySpawnSound()
    {
        audioSource.clip = spawnSound;
        audioSource.Play();
    }

    public void PlayReceiverSound()
    {
        audioSource.clip = receiverSound;
        audioSource.Play();
    }
}

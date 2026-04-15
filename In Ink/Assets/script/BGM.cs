using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public static BGM instance;
    private AudioSource audioSource;

    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void Mute()
    {
        audioSource.volume = 0;
    }

    public void UnMute()
    {
        audioSource.volume = 1;
    }
}

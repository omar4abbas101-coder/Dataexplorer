using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("refs")]
    public static AudioManager instance;
    [SerializeField] AudioSource audioSource;

    [Header("sounds")]
    [SerializeField] AudioClip[] audioClips;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySFX(string name, float volume = 0, float pitch = 0)
    {
        AudioClip clip = Array.Find(audioClips, x => x.name == name);

        if (clip == null) Debug.Log("Sound Not Found");
        else
        {
            if (volume != 0) audioSource.volume = volume;
            if (pitch != 0) audioSource.pitch = pitch;
            audioSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Mutes every sound effect in the scene
    /// </summary>
    /// <param name="mute"></param>
    public void MuteSound(bool mute)
    {
        audioSource.mute = mute;
    }
}

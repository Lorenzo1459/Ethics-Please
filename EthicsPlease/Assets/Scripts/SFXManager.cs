using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour {
    public static SFXManager Instance;

    public AudioSource sfxSource;
    public AudioClip[] sfxClips;

    private void Awake() {
        // Singleton pattern to ensure only one instance exists
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(int index) {
        if (index < 0 || index >= sfxClips.Length) {
            Debug.LogWarning("SFX index out of range!");
            return;
        }
        
        sfxSource.PlayOneShot(sfxClips[index]);
    }

    public void PlaySFXLoop(int index) {
        if (index < 0 || index >= sfxClips.Length) {
            Debug.LogWarning("SFX index out of range!");
            return;
        }
        sfxSource.clip = sfxClips[index];
        sfxSource.loop = true;
        sfxSource.Play();
    }

    public void PlaySFX(AudioClip clip) {
        if (clip == null) {
            Debug.LogWarning("SFX clip is null!");
            return;
        }

        sfxSource.PlayOneShot(clip);
    }

    public void StopSFX() {
        sfxSource.Stop();
    }

    public void SetVolume(float volume) {
        sfxSource.volume = Mathf.Clamp(volume, 0f, 1f);
    }

    public void ToggleMute() {
        sfxSource.mute = !sfxSource.mute;
    }
}
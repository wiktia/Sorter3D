using System.Collections.Generic;
using UnityEngine;
using System.Collections; 

public abstract class Level : MonoBehaviour
{
    public LevelManager levelManager;
    public string nextSceneName;

    [Tooltip("Lista dźwięków dialogów dla tego poziomu.")]
    public List<AudioClip> dialogueClips;

    [Tooltip("Ścieżka dźwiękowa dla tego poziomu.")]
    public AudioClip soundtrackClip;

    private AudioSource soundtrackSource; 
    private AudioSource dialogueSource;   
    private float dialogueInterval = 20f;
    private bool soundtrackPlaying = false;

    protected virtual void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        if (levelManager == null)
        {
            Debug.LogError("Nie znaleziono LevelManager na scenie!");
        }

        
        soundtrackSource = gameObject.AddComponent<AudioSource>();
        dialogueSource = gameObject.AddComponent<AudioSource>();

        PlaySoundtrack(0.1f); //glosnosc soundtracku

        if (dialogueClips != null && dialogueClips.Count > 0)
        {
            StartCoroutine(PlayDialogue());
        }
        else
        {
            Debug.LogWarning("Brak przypisanych dźwięków dialogów dla poziomu.");
        }
    }

    public virtual void LevelComplete()
    {
        if (levelManager != null)
        {
            levelManager.LoadNextLevel();
        }
        else
        {
            Debug.LogError("LevelManager is null!");
        }
    }

    
    private IEnumerator PlayDialogue()
    {
        while (true)
        {
            PlayRandomClip();
            yield return new WaitForSeconds(dialogueInterval);
        }
    }

    private void PlayRandomClip()
    {
        if (dialogueClips == null || dialogueClips.Count == 0)
        {
            return;
        }

        AudioClip clip = dialogueClips[Random.Range(0, dialogueClips.Count)];
        dialogueSource.PlayOneShot(clip); 
        Debug.Log($"granie audio clipu: {clip.name}");
    }

    private void PlaySoundtrack(float volume)
    {
        if (soundtrackClip != null && !soundtrackPlaying)
        {
            soundtrackSource.clip = soundtrackClip;
            soundtrackSource.loop = true;
            soundtrackSource.volume = volume; 
            soundtrackSource.Play();
            soundtrackPlaying = true;
            Debug.Log($"granie soundtracku: {soundtrackClip.name}");
        }
        else if (soundtrackClip == null)
        {
            Debug.LogWarning("brak przypisanej ścieżki dźwiękowej dla poziomu.");
        }
    }
}
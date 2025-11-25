using UnityEngine;
using System.Collections;

[System.Serializable]
public class Sound
{
    public AudioClip audioClip;
    public bool loop;
    public float volume = 1f;
}

[System.Serializable]
public class MaterialSoundPair
{
    public Material material;
    public Sound sound;
    public float duration;
}

public class AnimationSequence : MonoBehaviour
{
    public MaterialSoundPair[] materialSoundPairs;

    private Renderer objRenderer;
    private AudioSource audioSource;
    private Coroutine sequenceCoroutine;

    public void Awake()
    {
        objRenderer = GetComponent<Renderer>();
        if (objRenderer == null)
        {
            Debug.LogWarning("No Renderer found on object.");
            return;
        }
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnEnable()
    {
        sequenceCoroutine = StartCoroutine(PlaySequence());
    }

    public void OnDisable()
    {
        if (sequenceCoroutine != null)
        {
            StopCoroutine(sequenceCoroutine);
            sequenceCoroutine = null;
        }
        audioSource.Stop();
    }  

    private IEnumerator PlaySequence()
    {
        while (true)
        {
            foreach (var pair in materialSoundPairs)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                objRenderer.material = pair.material;

                if (pair.sound != null && pair.sound.audioClip != null)
                {
                    
                    audioSource.clip = pair.sound.audioClip;
                    audioSource.loop = pair.sound.loop;
                    audioSource.volume = pair.sound.volume;
                    audioSource.Play();
                }
                yield return new WaitForSeconds(pair.duration);
            }
        }
    }
}
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip clip;
    public float volume = 0.5f;
    public bool loop = true;

    public void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.spatialBlend = 1.0f;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();
    }
}
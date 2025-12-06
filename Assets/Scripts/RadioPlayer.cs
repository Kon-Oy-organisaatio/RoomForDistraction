using UnityEngine;

public class RadioPlayer : MonoBehaviour
{
    public AudioClip[] audioClips;
    private AudioSource audioSource;
    private int currentClipIndex = 0;
    private float timeSincePlayed = 0f;
    private float delayBetweenClips = 2f;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        if (audioClips.Length > 0)
        {
            currentClipIndex = Random.Range(0, audioClips.Length);
            // delays the first clip a bit
            //audioSource.clip = audioClips[currentClipIndex];
            // 3d sound
            audioSource.spatialBlend = 1.0f;
            audioSource.Play();
        }
    }

    void Update()
    {
        if (audioSource.isPlaying) {
            timeSincePlayed = 0f;
            return;
        }
        timeSincePlayed += Time.deltaTime;

        if (timeSincePlayed >= delayBetweenClips)
        {
            currentClipIndex = (currentClipIndex + 1) % audioClips.Length;
            audioSource.clip = audioClips[currentClipIndex];
            audioSource.Play();
            timeSincePlayed = 0f;
        }
    }
}

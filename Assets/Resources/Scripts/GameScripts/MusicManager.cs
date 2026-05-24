using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] songs; // Перетягни 3 пісні сюди в Inspector
    private AudioSource audioSource;
    private int lastPlayedIndex = -1;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayRandomSong();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayRandomSong();
        }
    }

    private void PlayRandomSong()
    {
        if (songs.Length == 0) return;

        int newIndex;
        do
        {
            newIndex = Random.Range(0, songs.Length);
        }
        while (newIndex == lastPlayedIndex && songs.Length > 1); // щоб не повторювалась та сама

        lastPlayedIndex = newIndex;
        audioSource.clip = songs[newIndex];
        audioSource.Play();
    }
}
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource sfxSource;
    private AudioSource ambientSource;

    private AudioClip footstepClip;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.playOnAwake = false;
        ambientSource.loop = true;
        ambientSource.volume = 0.3f;

        footstepClip = GenerateFootstepClip();
    }

    static AudioClip GenerateFootstepClip()
    {
        int sampleRate = 44100;
        int samples = (int)(sampleRate * 0.06f);
        float[] data = new float[samples];
        System.Random rng = new System.Random();
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / sampleRate;
            float envelope = Mathf.Exp(-t * 40f);
            data[i] = (float)(rng.NextDouble() * 2.0 - 1.0) * envelope * 0.25f;
        }
        AudioClip clip = AudioClip.Create("footstep", samples, 1, sampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null) sfxSource.PlayOneShot(clip);
    }

    public void PlayFootstep()
    {
        sfxSource.PlayOneShot(footstepClip);
    }
}

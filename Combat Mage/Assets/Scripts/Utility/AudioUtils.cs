using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioUtils : Singleton<AudioUtils>
{
    public Value<Gunshot> LastGunShot = new Value<Gunshot>(null);

    private Dictionary<AudioSource, Coroutine> _LevelSetters = new Dictionary<AudioSource, Coroutine>();

    [SerializeField]
    private AudioSource _2DAudioSource = null;

    public void Play2D(AudioClip clip, float volume)
    {
        if (_2DAudioSource)
            _2DAudioSource.PlayOneShot(clip, volume);
    }

    public static AudioSource CreateAudioSource(string name, Transform parent, Vector3 localPosition, bool is2D = false, float startVolume = 1f, float minDistance = 1f)
    {
        GameObject audioObject = new GameObject(name, typeof(AudioSource));

        audioObject.transform.parent = parent;
        audioObject.transform.localPosition = localPosition;
        AudioSource audioSource = audioObject.GetComponent<AudioSource>();
        audioSource.volume = startVolume;
        audioSource.spatialBlend = is2D ? 0f : 1f;
        audioSource.minDistance = minDistance;

        return audioSource;
    }

    public void LerpVolumeOverTime(AudioSource audioSource, float targetVolume, float speed)
    {
        if (_LevelSetters.ContainsKey(audioSource))
        {
            if (_LevelSetters[audioSource] != null)
                StopCoroutine(_LevelSetters[audioSource]);

            _LevelSetters[audioSource] = StartCoroutine(C_LerpVolumeOverTime(audioSource, targetVolume, speed));
        }
        else
            _LevelSetters.Add(audioSource, StartCoroutine(C_LerpVolumeOverTime(audioSource, targetVolume, speed)));
    }

    private IEnumerator C_LerpVolumeOverTime(AudioSource audioSource, float volume, float speed)
    {
        while (audioSource != null && Mathf.Abs(audioSource.volume - volume) > 0.001f)
        {
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, volume, Time.deltaTime * speed);
            yield return null;
        }

        if (audioSource.volume == 0f)
            audioSource.Stop();

        _LevelSetters.Remove(audioSource);
    }
}

public class Gunshot
{
    public Vector3 Position { get; private set; }
    public LivingEntity EntityThatShot { get; private set; }

    public Gunshot(Vector3 position, LivingEntity entityThatShot = null)
    {
        Position = position;
        EntityThatShot = entityThatShot;
    }
}

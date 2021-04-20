using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundPlayer : ICloneable
{
    public int ClipCount { get { return _Clips.Count; } }

    [SerializeField]
    private AudioClipList _Clips = null;

    //[SerializeField]
    private Vector2 _VolumeRange = new Vector2(0.5f, 0.75f);

    [SerializeField]
    private Vector2 _PitchRange = new Vector2(0.9f, 1.1f);

    [SerializeField]
    [Range(0f, 1f)]
    private float _VolumeMultiplier = 1f;

    private int _LastClipPlayed = -1;

    public object Clone()
    {
        return MemberwiseClone();
    }

    public void Play(AudioSource audioSource, float volume = 1f)
    {
        Play(ItemSelection.Method.RandomExcludeLast, audioSource, volume);
    }

    public void Play(ItemSelection.Method selectionMethod, AudioSource audioSource, float volume = 1f)
    {
        if (!audioSource || _Clips.Count == 0)
            return;

        if (_LastClipPlayed >= _Clips.Count || _LastClipPlayed <= -1)
            _LastClipPlayed = _Clips.Count - 1;

        AudioClip clipToPlay = _Clips.List.Select(ref _LastClipPlayed, selectionMethod);

        var finalVolume = GetVolume() * volume;
        audioSource.pitch = Random.Range(_PitchRange.x, _PitchRange.y);

        audioSource.PlayOneShot(clipToPlay, finalVolume);
    }

    /// <summary>
    /// Will use the AudioSource.PlayClipAtPoint() method, which doesnt include pitch variation
    /// </summary>
    /// <param name="selectionMethod"></param>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayAtPosition(ItemSelection.Method selectionMethod, Vector3 position, float volume = 1f)
    {
        if (_Clips.Count == 0)
            return;

        AudioClip clipToPlay = _Clips.List.Select(ref _LastClipPlayed, selectionMethod);

        AudioSource.PlayClipAtPoint(clipToPlay, position, GetVolume() * volume);
    }

    public void Play2D(ItemSelection.Method selectionMethod = ItemSelection.Method.RandomExcludeLast, float volume = 1f)
    {
        if (_Clips.Count == 0)
            return;

        AudioClip clipToPlay = _Clips.List.Select(ref _LastClipPlayed, selectionMethod);

        //AudioUtils.Instace.Play2D(clipToPlay, GetVolume() * volume);
    }

    private float GetVolume()
    {
        return Random.Range(_VolumeRange.x, _VolumeRange.y) * _VolumeMultiplier;
    }
}

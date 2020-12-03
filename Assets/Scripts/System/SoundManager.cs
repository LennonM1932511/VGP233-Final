using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour, IGameModule
{
    public GameObject soundResource;
    private Dictionary<Sound, float> soundTimerDictionary;

    private GameObject oneShotGO;
    private AudioSource oneShotAudioSource;
    
    public enum Sound
    {
        Shoot,
        Walk,
        Explosion
    }

    public void Init()
    {
        GameObject soundManagerGO = new GameObject("SoundManager");
        soundManagerGO.transform.SetParent(GameObject.FindWithTag("Services").transform);
        
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.Walk] = 0.0f;
    }

    public void PlayAudioAtPosition(Sound sound, Vector3 position, float delay = 0.0f)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGO = new GameObject("SoundAtPosition_" + sound.ToString());
            soundGO.transform.position = position;
            AudioSource audioSource = soundGO.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);
            audioSource.PlayDelayed(delay);
            Destroy(soundGO, audioSource.clip.length);
        }
    }

    public void PlayAudio(Sound sound)
    {
        if (CanPlaySound(sound))
        {
            if (oneShotGO == null)
            {
                oneShotGO = new GameObject("Sound_" + sound.ToString());
                oneShotAudioSource = oneShotGO.AddComponent<AudioSource>();
                oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
            }
            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }        
    }

    private AudioClip GetAudioClip(Sound sound)
    {
        Sounds resources = soundResource.GetComponent<Sounds>();
        
        foreach(Sounds.SoundAudioClip clip in resources.soundAudioClips)
        {
            if (clip.sound == sound)
            {
                return clip.audioClip;
            }
        }

        Debug.LogError("No sound found");
        return null;
    }

    private bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {         
            case Sound.Walk:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float playerWalkTimerMax = 0.3f;
                    if (lastTimePlayed + playerWalkTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }                
            default:
                return true;
        }
    }

    public IEnumerator LoadModule()
    {
        Init();
        ServiceLocator.Register<SoundManager>(this);
        yield return null;
    }
}

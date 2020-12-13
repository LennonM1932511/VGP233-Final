using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour, IGameModule
{
    public GameObject soundResource;

    private GameObject oneShotGO;
    private AudioSource oneShotAudioSource;

    public enum Sound
    {
        Music_Title,
        Music_Level,
        Player_Jump,
        Player_Land,
        Player_Hurt,
        Weapon_Pistol_Fire,
        Weapon_Shotgun_Fire,
        Weapon_Grenade_Fire,
        Weapon_Grenade_Explode,
        Pickup_Data,
        Pickup_Health,
        Pickup_Key,
        Pickup_Grenade,
        Enemy_Shoot,
        Enemy_Explode,
        Enemy_Boss_Death,
        Menu_Confirm,
        Menu_Highlight,
        End_GameOver,
        End_LevelComplete
    }

    public void Init()
    {
        GameObject soundManagerGO = new GameObject("SoundManager");
        soundManagerGO.transform.SetParent(GameObject.FindWithTag("Services").transform);
    }

    public void PlayAudioAtPosition(Sound sound, Vector3 position, float delay = 0.0f)
    {
        GameObject soundGO = new GameObject("SoundAtPosition_" + sound.ToString());
        soundGO.transform.position = position;
        AudioSource audioSource = soundGO.AddComponent<AudioSource>();
        audioSource.clip = GetAudioClip(sound);
        audioSource.PlayDelayed(delay);
        Destroy(soundGO, audioSource.clip.length);
    }

    public void PlayAudio(Sound sound)
    {
        if (oneShotGO == null)
        {
            oneShotGO = new GameObject("Sound_" + sound.ToString());
            oneShotAudioSource = oneShotGO.AddComponent<AudioSource>();
            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }
        oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
    }

    private AudioClip GetAudioClip(Sound sound)
    {
        Sounds resources = soundResource.GetComponent<Sounds>();

        foreach (Sounds.SoundAudioClip clip in resources.soundAudioClips)
        {
            if (clip.sound == sound)
            {
                return clip.audioClip;
            }
        }

        Debug.LogError("No sound found");
        return null;
    }

    public IEnumerator LoadModule()
    {
        Init();
        ServiceLocator.Register<SoundManager>(this);
        yield return null;
    }
}

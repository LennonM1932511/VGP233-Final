using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour, IGameModule
{
    public GameObject soundResource;
    
    // LENNON: I will delete if we have no use for it
    //
    //private Dictionary<Sound, float> soundTimerDictionary;

    private GameObject oneShotGO;
    private AudioSource oneShotAudioSource;
    
    // LENNON: Added 2 music tracks and 18 sfx
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

        // LENNON: I will delete if we have no use for it
        //
        //soundTimerDictionary = new Dictionary<Sound, float>();
        //soundTimerDictionary[Sound.Walk] = 0.0f;
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

    public void PlayMusic(Sound sound)
    {
        // LENNON:
        // This might need more work for switching tracks properly, but saving for later

        // Play a looping music track at half volumn.         
        if (oneShotGO == null)
        {
            oneShotGO = new GameObject("Music_" + sound.ToString());
            oneShotAudioSource = oneShotGO.AddComponent<AudioSource>();
            oneShotAudioSource.loop = true;
            oneShotAudioSource.volume = 0.5f;
            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
            return;
        }                
        oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
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
            // LENNON: I will remove this if we don't end up needing it
            //         We don't have a walk sound so this may not apply at all
            //
            //case Sound.Walk:
            //    if (soundTimerDictionary.ContainsKey(sound))
            //    {
            //        float lastTimePlayed = soundTimerDictionary[sound];
            //        float playerWalkTimerMax = 0.3f;
            //        if (lastTimePlayed + playerWalkTimerMax < Time.time)
            //        {
            //            soundTimerDictionary[sound] = Time.time;
            //            return true;
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //    }
            //    else
            //    {
            //        return false;
            //    }                
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

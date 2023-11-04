using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioTrack
{
    MainMenu,
    Sprint,
    Crunchtime,
    Deadline,
    DevHell
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer mixer;
    [Header("Audio SFX")]
    public AudioSource soundFXSource;
    public List<SoundEntry> audioList = new List<SoundEntry>();

    [Header("Audio Music")]
    public AudioSource mainMenuMusicSource;
    public AudioSource sprintMusicSource;
    public AudioSource crunchTimeMusicSource;
    public AudioSource deadlineMusicSource;
    public AudioSource devHellMusicSource;

    [Header("Audio Snapshots")]
    public AudioMixerSnapshot mainMenuSnapshot;
    public AudioMixerSnapshot sprintMusicSnapshot;
    public AudioMixerSnapshot crunchtimeSnapshot;
    public AudioMixerSnapshot deadlineSnapshot;
    public AudioMixerSnapshot devHellSnapshot;

    public float MasterVolume { get { return PlayerPrefs.GetFloat("Master Volume"); } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public static void PlaySound(string soundName)
    {
        PlaySound(soundName, 0f);
    }
    public static void PlaySound(string soundName, float pitchVariance)
    {
        instance.soundFXSource.volume = instance.MasterVolume;

        for (int i = 0; i < instance.audioList.Count; i++)
        {
            if (instance.audioList[i].soundName == soundName)
            {
                instance.soundFXSource.pitch = Random.Range(1f, 1f + pitchVariance);
                instance.soundFXSource.PlayOneShot(instance.audioList[i].audioFile);
            }
        }
    }

    public static void UpdateMusicLevels()
    {
        instance.mainMenuMusicSource.volume = instance.MasterVolume;
        instance.sprintMusicSource.volume = instance.MasterVolume;
        instance.crunchTimeMusicSource.volume = instance.MasterVolume;
        instance.deadlineMusicSource.volume = instance.MasterVolume;
        instance.devHellMusicSource.volume = instance.MasterVolume;
    }

    public static void SwapMusic(AudioTrack targetTrack, bool resetMusic = true, float transitionTime = 1f)
    {
        switch (targetTrack)
        {
            case AudioTrack.MainMenu:
                SwapMusic(instance.mainMenuSnapshot, resetMusic, transitionTime);
                break;
            case AudioTrack.Sprint:
                SwapMusic(instance.sprintMusicSnapshot, resetMusic, transitionTime);
                break;
            case AudioTrack.Crunchtime:
                SwapMusic(instance.crunchtimeSnapshot, resetMusic, transitionTime);
                break;
            case AudioTrack.Deadline:
                SwapMusic(instance.deadlineSnapshot, resetMusic, transitionTime);
                break;
            case AudioTrack.DevHell:
                SwapMusic(instance.devHellSnapshot, resetMusic, transitionTime);
                break;
            default:
                break;
        }

        if (resetMusic == true)
        {
            instance.ResetAudioSource(targetTrack);
        }
    }

    public static void SwapMusic(AudioMixerSnapshot targetTrack, bool resetMusic = true, float transitionTime = 1f)
    {
        targetTrack.TransitionTo(transitionTime);
    }

    private void ResetAudioSource(AudioTrack targetTrack)
    {
        switch (targetTrack)
        {
            case AudioTrack.MainMenu:
                mainMenuMusicSource.Stop();
                mainMenuMusicSource.Play();
                break;
            case AudioTrack.Sprint:
                sprintMusicSource.Stop();
                sprintMusicSource.Play();
                break;
            case AudioTrack.Crunchtime:
                crunchTimeMusicSource.Stop();
                crunchTimeMusicSource.Play();
                break;
            case AudioTrack.Deadline:
                deadlineMusicSource.Stop();
                deadlineMusicSource.Play();
                break;
            case AudioTrack.DevHell:
                devHellMusicSource.Stop();
                devHellMusicSource.Play();
                break;
        }
    }

    //public void SetMusicVolume(float value, AudioTrack targetTrack)
    //{
    //    switch (targetTrack)
    //    {
    //        case AudioTrack.MainMenu:
    //            mixer.SetFloat(MIXER_MAIN_MENU, Mathf.Log10(value));
    //            break;
    //        case AudioTrack.Sprint:
    //            mixer.SetFloat(MIXER_SPRINT, Mathf.Log10(value));
    //            break;
    //        case AudioTrack.Crunchtime:
    //            mixer.SetFloat(MIXER_CRUNCHTIME, Mathf.Log10(value));
    //            break;
    //        case AudioTrack.Deadline:
    //            mixer.SetFloat(MIXER_DEADLINE, Mathf.Log10(value));
    //            break;
    //        case AudioTrack.DevHell:
    //            mixer.SetFloat(MIXER_DEVHELL, Mathf.Log10(value));
    //            break;
    //        default:
    //            break;
    //    }
    //}

    [System.Serializable]
    public class SoundEntry
    {
        public string soundName;
        public AudioClip audioFile;
    }


}

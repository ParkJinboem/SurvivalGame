using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;
    #region singletion
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion singletion

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBgm;


    public string[] playerSoundName;

    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    private void Start()
    {
        playerSoundName = new string[audioSourceEffects.Length];
    }
    public void PlaySE(string name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (name == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        playerSoundName[j] = effectSounds[i].name;
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("��� ���� AudioSource�� ������Դϴ�.");
                return;
            }
        }
        Debug.Log($"{name} ���尡 SoundManager�� ��ϵ��� �ʾҽ��ϴ�.");
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    public void StopSE(string name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playerSoundName[i] == name)
            {
                audioSourceEffects[i].Stop();
                return;
            }
        }
        Debug.Log($"������� {name} ���尡 �����ϴ�.");


    }
}
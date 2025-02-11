using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }
    [Header("#BGM")]
    [SerializeField]
    private AudioClip _bgmClip;
    [SerializeField]
    private float _bgmVolume;
    private AudioSource _bgmPlayer;
    private AudioHighPassFilter _bgmHighPassFilter;

    [Header("#SFX")]
    [SerializeField]
    private AudioClip[] _sfxClips;
    [SerializeField]
    private float _sfxVolume;
    [SerializeField]
    private int _channels;

    private AudioSource[] _sfxPlayer;
    private int _channelIndex;

    public enum Sfx { Dead , Hit , LevelUp = 3 , Lose, Melee , Range =7,Select,Win }

    private void Awake()
    {
        _instance = this;
        Init();
    }
    private void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        _bgmPlayer = bgmObject.AddComponent<AudioSource>();
        _bgmPlayer.playOnAwake = false;
        _bgmPlayer.loop = true;
        _bgmPlayer.volume = _bgmVolume;
        _bgmPlayer.clip = _bgmClip;
        _bgmHighPassFilter = Camera.main.GetComponent<AudioHighPassFilter>();

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        _sfxPlayer = new AudioSource[_channels];

        for (int i = 0; i < _sfxPlayer.Length; i++)
        {
            _sfxPlayer[i] = sfxObject.AddComponent<AudioSource>();
            _sfxPlayer[i].playOnAwake = false;
            _sfxPlayer[i].bypassListenerEffects = true;
            _sfxPlayer[i].volume = _sfxVolume;
        }
    }
    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            _bgmPlayer.Play();
        }
        else
        {
            _bgmPlayer.Stop();
        }
    }
    public void EffectBgm(bool isPlay)
    {
        _bgmHighPassFilter.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int i = 0; i < _sfxPlayer.Length; i++)
        {
            int loopIndex = (i + _channelIndex) % _sfxPlayer.Length;

            if (_sfxPlayer[loopIndex].isPlaying)
                continue;

            int randIndex = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                randIndex = UnityEngine.Random.Range(0, 2);
            }
            
            _channelIndex = loopIndex;
            _sfxPlayer[0].clip = _sfxClips[(int)sfx + randIndex];
            _sfxPlayer[0].Play();
            break;
        }

    }
}

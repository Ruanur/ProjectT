using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [Header("# Game Control")]
    [SerializeField]
    private bool _isLive;
    public bool IsLive { get { return _isLive; } }
    [SerializeField]
    private float _gameTime;
    public float GameTime { get { return _gameTime; } set { _gameTime = value; } }
    [SerializeField]
    public float MaxGameTime = 2 * 10f;

    [Header("# Player Info")]
    [SerializeField]
    private float _health;
    public float Health { get { return _health; } set { _health = value; } }
    [SerializeField]
    private int _maxHealth = 100;
    public int MaxHealth { get { return _maxHealth; } }
    [SerializeField]
    private int _level;
    public int Level { get { return _level; } }
    [SerializeField]
    private int _kill;
    public int Kill { get { return _kill; } set { _kill = value; } }
    [SerializeField]
    private int _exp;
    public int Exp { get { return _exp; } }
    [SerializeField]
    private int[] _nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
    public int[] NextExp { get { return _nextExp; } }

    [Header("# Game Object")]
    [SerializeField]
    private int _playerId;
    public int PlayerId { get { return _playerId; } }
    [SerializeField]
    private PoolManager _poolManager;
    public PoolManager PoolManager { get { return _poolManager; } }
    [SerializeField]
    private Player _player;
    public Player Player { get { return _player; } }
    [SerializeField]
    private LevelUp _uiLevelUp;
    public LevelUp UiLevelUp { get { return _uiLevelUp; } }
    [SerializeField]
    private Result _uiResult;
    public Result UiResult { get { return _uiResult; } }
    [SerializeField]
    private GameObject _enemyCleaner;
    public GameObject EnemyCleaner { get { return _enemyCleaner; } }
    [SerializeField]
    private Transform _uiJoy;
    public Transform UiJoy { get { return _uiJoy; } }

    private void Awake()
    {
        _instance = this;
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        GameStart(0);
    }

    public void GameStart(int id)
    {
        _playerId = id;
        _health = _maxHealth;
        _player.gameObject.SetActive(true);
        _uiLevelUp.Select(_playerId % 2);
        Resume();
        AudioManager.Instance.PlayBgm(true);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Select);
    }
    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        _isLive = false;

        yield return new WaitForSeconds(0.5f);
        _uiResult.gameObject.SetActive(true);
        _uiResult.Lose();
        Stop();
        AudioManager.Instance.PlayBgm(false);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Lose);
    }
    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        _isLive = false;
        _enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        _uiResult.gameObject.SetActive(true);
        _uiResult.Win();
        Stop();
        AudioManager.Instance.PlayBgm(false);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Win);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
    public void GameQuit()
    {
        Application.Quit();
    }
    private void Update()
    {
        if (!_isLive) return;

        GameTime += Time.deltaTime;

        if (GameTime > MaxGameTime)
        {
            GameTime = MaxGameTime;
            GameVictory();
        }
    }
    public void GetExp()
    {
        if (!_isLive) return;

        _exp++;

        if (_exp == _nextExp[Mathf.Min(_level , _nextExp.Length - 1)])
        {
            _level++;
            _exp = 0;
            _uiLevelUp.Show();
        }
    }
    public void Stop()
    {
        _isLive = false;
        Time.timeScale = 0;
        _uiJoy.localScale = Vector3.zero;
    }
    public void Resume()
    {
        _isLive = true;
        Time.timeScale = 1;
        _uiJoy.localScale = Vector3.one;
    }
}

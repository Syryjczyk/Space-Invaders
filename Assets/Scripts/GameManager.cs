using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameManager : MonoBehaviour
{
    [SerializeField] private int initLives;
    [SerializeField] private float initTime;
    [SerializeField] private Transform[] livesImages;
    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI readySteadyText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [Header("UI Panels")]
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject movementButton;
    [SerializeField] private GameObject shootButton;
    [SerializeField] private SceneDataSO sceneData;
    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask bunkerLayer;
    [SerializeField] private LayerMask invaderLayer;

    private Player _player;
    private Invaders _invaders;
    private MysteryShip _mysteryShip;
    private Bunker[] _bunkers;
    private int _score;
    private int _bestScore;
    private int _lives;
    private int _level;
    private int _activeLiveCounter;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _invaders = FindObjectOfType<Invaders>();
        _mysteryShip = FindObjectOfType<MysteryShip>();
        _bunkers = FindObjectsOfType<Bunker>();
    }

    private void OnEnable()
    {
        _player.Killed += OnPlayerKilled;
        _mysteryShip.Killed += OnMysteryShipKilled;
        _invaders.Killed += OnInvaderKilled;
        _invaders.BrakeThrough += OnPlayerKilled;

        NewGame();
    }

    //private void Update()
    //{
    //    if (lives < 0)
    //    {
    //        NewGame();
    //    }
    //}

    private void NewRound()
    {
        _invaders.ResetInvaders();
        _invaders.gameObject.SetActive(true);

        for (int i = 0; i < _bunkers.Length; i++)
        {
            _bunkers[i].ResetBunker();
        }

        Respawn();
        SetOffProjectiles();
        StartCoroutine(nameof(StartDelay), initTime);
    }
    private void NewRound2()
    {
        _invaders.ResetInvaders();
        _invaders.gameObject.SetActive(true);

        Respawn();
        SetOffProjectiles();
        StartCoroutine(nameof(StartDelay), initTime);
    }

    private void Respawn()
    {
        Vector3 position = _player.transform.position;
        position.x = 0f;
        _player.transform.position = position;
        _player.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        SetLevel((int)(SceneOrder.SceneOrderType.PRELEVEL));
    }

    private void SetScore(int score)
    {
        this._score = score;
        scoreText.text = score.ToString().PadLeft(5, '0');
    }

    private void SetBestScore()
    {
        if (_score > PlayerPrefs.GetInt("Best Score"))
        {
            PlayerPrefs.SetInt("Best Score", _score);
        }
    }

    private void SetBestScoreText()
    {
        bestScoreText.text = $"Best Score: {PlayerPrefs.GetInt("Best Score").ToString()}";
    }

    private void SetLives(int lives)
    {
        lives = Mathf.Max(lives, 0);
        livesText.text = lives.ToString();
        
        if (lives == initLives) 
        {
            return;
        }

        livesImages[lives].gameObject.SetActive(false);
    }

    private void RestartImages()
    {
        for (int i = 0; i < livesImages.Length; i++)
        {
            livesImages[i].gameObject.SetActive(true);
        }
    }

    private void CheckActiveLives()
    {
        _activeLiveCounter = 0;

        for (int i = 0; i < livesImages.Length; i++)
        {
            if (livesImages[i].gameObject.activeInHierarchy)
            {
                _activeLiveCounter++;
            }
        }
    }

    private void SetOffProjectiles()
    {
        Projectile[] projectiles = FindObjectsOfType<Projectile>();

        for (int i = 0; i < projectiles.Length; i++)
        {
            Debug.Log(projectiles[i]);

            Destroy(projectiles[i].gameObject);
            
        }
    }

    private void SetLayerMask(LayerMask objectLayer, bool isActive)
    {
        if (isActive)
        {
            mainCamera.cullingMask |= objectLayer;
        }
        else
        {
            mainCamera.cullingMask &= ~objectLayer;
        }
    }

    private void SetLivesImages(bool isActive)
    {
        for (int i = 0; i < _activeLiveCounter; i++)
        {
            livesImages[i].gameObject.SetActive(isActive);
        }
    }

    private void SetLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    private void OnPlayerKilled(Transform transform)
    {
        _lives--;
        SetLives(_lives);
        VibrationHandler.DefaultVibration();

        if (_lives < 1)
        {
            GameOver();
        }
    }

    private void OnInvaderKilled(Invader invader)
    {
        VibrationHandler.HeavyVibration();
        SetScore(_score + invader.Score);

        if (_invaders.KilledAmount == _invaders.TotalAmount)
        {
            SetBestScore();
            sceneData.NextLevelAchived = true;
            SetLevel((int)(SceneOrder.SceneOrderType.PRELEVEL));
        }
    }

    private void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        VibrationHandler.DefaultVibration();
        SetScore(_score + mysteryShip.Score);
    }

    private IEnumerator StartDelay()
    {
        Time.timeScale = 0f;
        pauseButton.gameObject.SetActive(false);
        readySteadyText.text = "Ready";
        readySteadyText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(initTime / 4);

        readySteadyText.text = "Steady";
        yield return new WaitForSecondsRealtime(initTime / 4);

        readySteadyText.text = "Go";
        yield return new WaitForSecondsRealtime(initTime / 4);

        readySteadyText.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(initTime / 4);

        pauseButton.gameObject.SetActive(true);
        Time.timeScale = 1f;
    }

    public void NewGame()
    {
        _lives = initLives;
        _level = 1;

        SetScore(0);
        SetLives(initLives);
        RestartImages();
        NewRound();
        StartCoroutine(nameof(StartDelay));

    }

    public void PausePanelUI()
    {
        if (pausePanel.activeInHierarchy)
        {
            // Pause panel turning OFF
            mainCamera.cullingMask = -1;
            livesText.gameObject.SetActive(true);
            scoreText.gameObject.SetActive(true);
            movementButton.SetActive(true);
            shootButton.SetActive(true);
            pauseButton.SetActive(true);
            pausePanel.SetActive(false);

            SetLivesImages(true);

            Time.timeScale = 1f;
            SetLayerMask(playerLayer, true);
        }

        else
        {
            // Pause panel turning ON
            mainCamera.cullingMask = LayerMask.GetMask("UI");
            livesText.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(false);
            movementButton.SetActive(false);
            shootButton.SetActive(false);
            pauseButton.SetActive(false);
            pausePanel.SetActive(true);

            CheckActiveLives();
            SetLivesImages(false);

            Time.timeScale = 0f;
            SetBestScoreText();
            SetLayerMask(playerLayer, false);
        }
    }

    public void OptionPanelUI()
    {
        if (optionPanel.activeInHierarchy)
        {
            optionPanel.SetActive(false);
        }
        else
        {
            optionPanel.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(sceneData.Level);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

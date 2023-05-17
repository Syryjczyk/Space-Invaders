using System;
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
    [Header("UI Panels")]
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private SceneDataSO sceneData;

    private Player player;
    private Invaders invaders;
    private MysteryShip mysteryShip;
    private Bunker[] bunkers;
    private int score;
    private int lives;
    private int level;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        invaders = FindObjectOfType<Invaders>();
        mysteryShip = FindObjectOfType<MysteryShip>();
        bunkers = FindObjectsOfType<Bunker>();
    }

    private void OnEnable()
    {
        player.Killed += OnPlayerKilled;
        mysteryShip.Killed += OnMysteryShipKilled;
        invaders.Killed += OnInvaderKilled;

        NewGame();
    }

    private void Update()
    {
        if (lives < 0)
        {
            NewGame();
        }
    }

    private void NewRound()
    {
        invaders.ResetInvaders();
        invaders.gameObject.SetActive(true);

        for (int i = 0; i < bunkers.Length; i++)
        {
            bunkers[i].ResetBunker();
        }

        Respawn();
        StartCoroutine(nameof(StartDelay), initTime);
    }

    private void Respawn()
    {
        Vector3 position = player.transform.position;
        position.x = 0f;
        player.transform.position = position;
        player.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        //GameOverPanelUI(true);
        //invaders.gameObject.SetActive(false);
        SetLevel((int)(SceneOrder.SceneOrderType.PRELEVEL));
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(5, '0');
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

    private void SetLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    private void OnPlayerKilled(Transform transform)
    {
        lives--;
        SetLives(lives);

        player.gameObject.SetActive(false);

        if (lives > 0)
        {
            Invoke(nameof(NewRound), 1f);
            
        }
        else
        {
            GameOver();
        }
    }

    private void OnInvaderKilled(Invader invader)
    {
        SetScore(score + invader.Score);

        if (invaders.KilledAmount == invaders.TotalAmount)
        {
            sceneData.NextLevelAchived = true;
            SetLevel((int)(SceneOrder.SceneOrderType.PRELEVEL));
        }
    }

    private void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        SetScore(score + mysteryShip.Score);
    }

    private IEnumerator StartDelay()
    {
        Time.timeScale = 0f;
        readySteadyText.text = "Ready";
        readySteadyText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(initTime / 4);

        readySteadyText.text = "Steady";
        yield return new WaitForSecondsRealtime(initTime / 4);

        readySteadyText.text = "Goo";
        yield return new WaitForSecondsRealtime(initTime / 4);

        readySteadyText.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(initTime / 4);

        Time.timeScale = 1f;
    }

    private void GameOverPanelUI(bool isActive)
    {
        if (!isActive)
        {
            gameOverPanel.SetActive(false);
            pauseButton.SetActive(true);
        }
        else
        {
            gameOverPanel.SetActive(true);
            pauseButton.SetActive(false);
        }
    }

    public void NewGame()
    {
        GameOverPanelUI(false);
        lives = initLives;
        level = 1;

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
            pausePanel.SetActive(false);
            pauseButton.SetActive(true);
            Time.timeScale = 1f;
        }
        else
        {
            pausePanel.SetActive(true);
            pauseButton.SetActive(false);
            Time.timeScale = 0f;
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
}

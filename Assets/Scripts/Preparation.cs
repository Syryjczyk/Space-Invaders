using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preparation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private RectTransform tryAgainButton;
    [SerializeField] private RectTransform nextLevelButton;
    [SerializeField] private TextMeshProUGUI nextLevelText;
    [SerializeField] private SceneDataSO sceneData;

    private void OnEnable()
    {
        if (sceneData.IsGameInitialize)
        {
            sceneData.Level = (int)(SceneOrder.SceneOrderType.PRELEVEL);
            sceneData.IsGameInitialize = false;
            sceneData.NextLevelAchived = true;
        }
        Initialization();
    }

    private void Initialization()
    {
        if (sceneData.Level == (int)(SceneOrder.SceneOrderType.PRELEVEL))
        {
            tryAgainButton.gameObject.SetActive(false);
            nextLevelText.text = "Start";
            nextLevelButton.anchoredPosition = new Vector2(0, -100);
            levelText.text = $"level {sceneData.Level}";
        }
        else if (sceneData.NextLevelAchived)
        {
            levelText.text = $"Level {sceneData.Level - 1} complited";
        }
        else if (!sceneData.NextLevelAchived)
        {
            levelText.text = $"Level {sceneData.Level - 1} failed";
            nextLevelButton.gameObject.SetActive(false);
            tryAgainButton.anchoredPosition = new Vector2(0, -100);
        }
        else if (sceneData.Level == 7)
        {

        }
    }

    public void NextLevel()
    {
        sceneData.Level++;
        sceneData.NextLevelAchived = false;

        SceneManager.LoadScene(sceneData.Level);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(sceneData.Level);
    }

    public void Restart()
    {
        SceneManager.LoadScene((int)(SceneOrder.SceneOrderType.START));
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    [SerializeField] private SceneDataSO sceneData;
    [SerializeField] private AudioSource audioSource;

    private void OnEnable()
    {
        sceneData.IsGameInitialize = true;
    }

    public void GoToPreLevelScene()
    {
        audioSource.Play();
        SceneManager.LoadScene(SceneOrder.SceneOrderType.PRELEVEL.ToString());
    }
}

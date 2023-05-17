using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    [SerializeField] private SceneDataSO sceneData;
    [SerializeField] private Intro[] introInvaders;
    [SerializeField] private float timeRate;

    private int invaderAmount;

    private void OnEnable()
    {
        sceneData.IsGameInitialize = true;
        invaderAmount = 0;
        InvokeRepeating(nameof(SpawnInvader), timeRate, timeRate);
    }

    private void SpawnInvader()
    {
        if (invaderAmount < 50)
        {
            int randomIndex = Random.Range(0, introInvaders.Length);
            Intro invader = introInvaders[randomIndex];
            Instantiate(invader);
            invaderAmount++;
        }
    }

    public void GoToPreLevelScene()
    {
        SceneManager.LoadScene(SceneOrder.SceneOrderType.PRELEVEL.ToString());
    }
}

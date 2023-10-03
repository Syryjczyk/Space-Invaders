using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI endText;

    private void OnEnable()
    {
        endText.text = "created\nby\nsyria games";
    }

    public void RepeatAdventure()
    {
        SceneManager.LoadScene(0);
    }
}

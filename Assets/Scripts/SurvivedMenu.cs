using UnityEngine;
using UnityEngine.SceneManagement;

public class SurvivedMenu : MonoBehaviour
{
    [Header("Scene Names")]
    public string gameplaySceneName = "SampleScene";
    public string mainMenuSceneName = "TownScene";

    // Called by Play Again button
    public void PlayAgain()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    // Called by Main Menu button
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}

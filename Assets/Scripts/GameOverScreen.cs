using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public GameObject gameOverUI;
    public TextMeshProUGUI quoteText;

    private static int lastQuoteIndex = -1;

    private string[] quotes =
    {
        "“DO NOT LOOK UP.” — Manananggal, probably",
        "“I heard breathing behind me. It was mine.” — Anonymous survivor",
        "“She only attacks at night. I play at night.” — Bad decision #37",
        "“The scream wasn’t scripted.” — Playtester #4",
        "“I survived by not being brave.” — Local coward",
        "“Running is a valid strategy.” — Ancient Filipino wisdom",
        "“The wings weren’t the worst part.” — Missing person report",
        "“Turn around. Just kidding. Don’t.” — Game tips",
        "“I paused the game. She didn’t.” — Final thought",
        "“Salt was cheaper than courage.” — Village elder",
        "“GET GUD.” — Albert Einstein"
    };

    public void Setup()
    {
        gameOverUI.SetActive(true);

        int index;
        do
        {
            index = Random.Range(0, quotes.Length);
        }
        while (index == lastQuoteIndex && quotes.Length > 1);

        lastQuoteIndex = index;
        quoteText.text = quotes[index];

        Time.timeScale = 0f;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TownScene");
    }
}

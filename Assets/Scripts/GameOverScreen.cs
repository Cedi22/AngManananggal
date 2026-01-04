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
        "\"The ceiling was dripping. It wasn't rain.\" — Last journal entry",
        "\"I felt her breath on my neck... she has no lungs.\" — Survivor testimony",
        "\"She smiled at me. Her teeth went all the way back.\" — Medical report",
        "\"The sound of her wings never stops in my head.\" — Psychiatric patient",
        "\"I saw her torso crawling. The legs were still walking.\" — Police report",
        "\"She whispered my name. I never told her my name.\" — Missing person",
        "\"Her tongue reached the floor from the ceiling.\" — Crime scene notes",
        "\"I looked up. She was already looking down.\" — Final mistake",
        "\"The proboscis pierced the roof like butter.\" — Witness statement",
        "\"She left. Her shadow stayed and watched me.\" — Asylum patient #4",
        "\"I heard her eating in the attic. I live alone.\" — 911 call transcript",
        "\"Her eyes reflected no light. They absorbed it.\" — Forensic notes",
        "\"The vinegar didn't work. Nothing works.\" — Village elder's warning",
        "\"She peeled apart at the waist. It made no sound.\" — Survivor's account",
        "\"I pretended to sleep. She knew I was pretending.\" — Anonymous",
        "\"Her innards dragged across my roof all night.\" — Last testimony",
        "\"She sang my mother's lullaby. My mother died screaming.\" — Orphan's diary",
        "\"The blood dripped upward into her mouth.\" — Coroner's observation",
        "\"I counted her ribs through the window. She had too many.\" — Case file #217",
        "\"She asked to come inside. I didn't answer. She came anyway.\" — Final words"
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
        
        // Lock cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        SceneManager.LoadScene("SampleScene");
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        
        // Keep cursor visible for main menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        SceneManager.LoadScene("TownScene");
    }
}
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.SceneManagement;

public class LowerBodyBuffController : MonoBehaviour
{
    [Header("References")]
    public ManananggalAI manananggalAI;
    public NavMeshAgent manananggalAgent;
    public TMP_Text timerText;

    [Header("Timer")]
    public float duration = 180f;

    [Header("Aggression Scaling")]
    public float speedBonus = 1.5f;
    public float detectionBonus = 6f;

    private float timer;
    private bool active;

    private float baseSpeed;
    private float baseDetection;

    [Header("Scene Transition")]
    public string bossFightSceneName = "BossFight";
    private void Start()
    {
        baseSpeed = manananggalAgent.speed;
        baseDetection = manananggalAI.detectionRange;

        timerText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!active) return;

        timer -= Time.deltaTime;
        UpdateTimerUI();

        float t = 1f - (timer / duration);
        manananggalAgent.speed = Mathf.Lerp(baseSpeed, baseSpeed + speedBonus, t);
        manananggalAI.detectionRange = Mathf.Lerp(baseDetection, baseDetection + detectionBonus, t);

        if (timer <= 0f)
            EndCurse();
    }

    public void StartCurse()
    {
        active = true;
        timer = duration;
        timerText.gameObject.SetActive(true);

        Debug.Log("⏳ Curse started");
    }

    private void UpdateTimerUI()
    {
        int m = Mathf.FloorToInt(timer / 60f);
        int s = Mathf.FloorToInt(timer % 60f);
        timerText.text = $"{m}:{s:00}";
    }

private void EndCurse()
{
    active = false;

    manananggalAgent.speed = baseSpeed;
    manananggalAI.detectionRange = baseDetection;
    timerText.gameObject.SetActive(false);

    Debug.Log("☠ Curse ended — loading ending scene");

    SceneManager.LoadScene("EndCutscene");
}
}

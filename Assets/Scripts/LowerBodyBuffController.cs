using UnityEngine;
using UnityEngine.AI;

public class LowerBodyBuffController : MonoBehaviour
{
    [Header("References")]
    public ManananggalAI manananggalAI;
    public NavMeshAgent manananggalAgent;

    [Header("Timer")]
    public float duration = 180f;

    [Header("Aggression Scaling")]
    public float speedBonus = 1.5f;
    public float detectionBonus = 6f;

    private float timer;
    private bool active;

    private float baseSpeed;
    private float baseDetection;

    private void Start()
    {
        baseSpeed = manananggalAgent.speed;
        baseDetection = manananggalAI.detectionRange;
    }

    private void Update()
    {
        if (!active) return;

        timer -= Time.deltaTime;
        float t = 1f - (timer / duration);

        // Stronger ramp
        manananggalAgent.speed = Mathf.Lerp(baseSpeed, baseSpeed + speedBonus, t);
        manananggalAI.detectionRange = Mathf.Lerp(baseDetection, baseDetection + detectionBonus, t);

        if (timer <= 0f)
        {
            EndCurse();
        }
    }

    public void StartCurse()
    {
        active = true;
        timer = duration;

        Debug.Log("⏳ Curse countdown started");
    }

    private void EndCurse()
    {
        active = false;

        manananggalAgent.speed = baseSpeed;
        manananggalAI.detectionRange = baseDetection;

        Debug.Log("☠ Curse ended");

        // TODO: TELEPORT LOWER BODY / LOAD SCENE
        // SceneManager.LoadScene("LowerBodyScene");
    }
}

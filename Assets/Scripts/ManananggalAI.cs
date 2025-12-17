using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class ManananggalAI : MonoBehaviour
{
    [Header("AI Settings")]
    public NavMeshAgent agent;
    public Transform player;
    public float detectionRange = 15f;
    public float stopChaseRange = 25f;
    public float jumpscareDistance = 2f;

    [Header("Disappear After Chase")]
    public float disappearDelay = 3f;
    public GameObject model;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] idleVoices;
    public AudioClip[] chaseVoices;
    public AudioClip[] disappearVoices;
    public AudioClip jumpScareSFX;

    [Header("Jumpscare Effects")]
    public Camera cutsceneCamera;
    public Camera playerCamera;
    public CanvasGroup fadePanel;
    public float fadeSpeed = 2f;

    [Header("Game Over")]
    public GameOverScreen gameOverScreen;
    public float gameOverDelay = 2.5f;

    [Header("Player Control")]
    public MonoBehaviour playerInputScript; // drag your movement/look script here

    private bool isChasing;
    private bool hasDisappeared;
    private bool hasJumpscared;

    private void Start()
    {
        cutsceneCamera.gameObject.SetActive(false);
        fadePanel.alpha = 0f;
    }

    private void Update()
    {
        if (hasDisappeared || hasJumpscared) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (!isChasing && distance <= detectionRange)
        {
            isChasing = true;
            agent.SetDestination(player.position);
            PlayRandom(chaseVoices);
        }

        if (isChasing && distance >= stopChaseRange)
        {
            isChasing = false;
            StartCoroutine(Disappear());
        }

        if (isChasing)
        {
            agent.SetDestination(player.position);
        }

        if (isChasing && distance <= jumpscareDistance)
        {
            StartCoroutine(JumpScare());
        }
    }

    private void PlayRandom(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;
        audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }

    private IEnumerator Disappear()
    {
        if (hasDisappeared) yield break;
        hasDisappeared = true;

        yield return new WaitForSeconds(disappearDelay);
        PlayRandom(disappearVoices);
        model.SetActive(false);
    }

    private IEnumerator JumpScare()
    {
        if (hasJumpscared) yield break;
        hasJumpscared = true;

        // STOP EVERYTHING
        agent.isStopped = true;
        if (playerInputScript != null)
            playerInputScript.enabled = false;

        // Camera switch
        playerCamera.gameObject.SetActive(false);
        cutsceneCamera.gameObject.SetActive(true);

        // Audio
        if (jumpScareSFX != null)
            audioSource.PlayOneShot(jumpScareSFX);

        yield return StartCoroutine(CameraShake(0.4f, 0.3f));

        // Fade to black
        while (fadePanel.alpha < 1f)
        {
            fadePanel.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // Freeze cameras permanently
        cutsceneCamera.enabled = false;

        yield return new WaitForSeconds(gameOverDelay);

        gameOverScreen.Setup();
    }

    private IEnumerator CameraShake(float duration, float amount)
    {
        Vector3 originalPos = cutsceneCamera.transform.localPosition;
        float timer = 0f;

        while (timer < duration)
        {
            cutsceneCamera.transform.localPosition =
                originalPos + Random.insideUnitSphere * amount;

            timer += Time.deltaTime;
            yield return null;
        }

        cutsceneCamera.transform.localPosition = originalPos;
    }
}

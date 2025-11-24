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
    public AudioClip[] idleVoices;       // <--- MULTIPLE
    public AudioClip[] chaseVoices;      // <--- MULTIPLE
    public AudioClip[] disappearVoices;  // <--- MULTIPLE
    public AudioClip jumpScareSFX;

    [Header("Jumpscare Effects")]
    public Camera cutsceneCamera;
    public Camera playerCamera;
    public CanvasGroup fadePanel;
    public float fadeSpeed = 2f;

    private bool isChasing = false;
    private bool hasDisappeared = false;
    private bool hasJumpscared = false;

    private void Start()
    {
        cutsceneCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (hasDisappeared || hasJumpscared) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Start chasing
        if (!isChasing && distance <= detectionRange)
        {
            isChasing = true;
            agent.SetDestination(player.position);

            PlayRandom(chaseVoices);
        }

        // Stop chase
        if (isChasing && distance >= stopChaseRange)
        {
            isChasing = false;
            StartCoroutine(Disappear());
        }

        // Keep following player
        if (isChasing)
        {
            agent.SetDestination(player.position);
        }

        // Jumpscare trigger
        if (isChasing && distance <= jumpscareDistance)
        {
            StartCoroutine(JumpScare());
        }
    }

    private void PlayRandom(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;
        int index = Random.Range(0, clips.Length);
        audioSource.PlayOneShot(clips[index]);
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

        // Switch to jumpscare camera
        playerCamera.gameObject.SetActive(false);
        cutsceneCamera.gameObject.SetActive(true);

        // Scream
        if (jumpScareSFX != null)
            audioSource.PlayOneShot(jumpScareSFX);

        // Camera shake
        yield return StartCoroutine(CameraShake(0.4f, 0.3f));

        // Fade to black
        while (fadePanel.alpha < 1f)
        {
            fadePanel.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    private IEnumerator CameraShake(float duration, float amount)
    {
        Vector3 originalPos = cutsceneCamera.transform.localPosition;
        float timer = 0;

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

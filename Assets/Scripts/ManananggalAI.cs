using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using StarterAssets;
using UnityEngine.InputSystem;

public class ManananggalAI : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform playerCapsule;   // drag PlayerCapsule here
    public GameObject model;

    [Header("Detection")]
    public float detectionRange = 15f;
    public float jumpscareDistance = 2f;

    [Header("Movement Speed")]
    public float patrolSpeed = 2.5f;
    public float chaseSpeed = 4.8f;

    [Header("Patrol Settings")]
    public float patrolRadius = 20f;
    private Vector3 patrolPoint;
    private bool hasPatrolPoint;

    [Header("Spawn Settings")]
    public float minSpawnDistanceFromPlayer = 80f; // closer minimum
    public float minX = -151f;
    public float maxX = 348f;
    public float minZ = -184f;
    public float maxZ = 270f;
    public float initialSpawnFavorDistance = 80f; // first spawn closer

    [Header("Respawn Timing")]
    public float disappearDelay = 3f;
    public float minRespawnDelay = 10f;
    public float maxRespawnDelay = 20f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] chaseVoices;
    public AudioClip[] disappearVoices;
    public AudioClip jumpScareSFX;

    [Header("Jumpscare Effects")]
    public Camera playerCamera;
    public Camera cutsceneCamera;
    public CanvasGroup fadePanel;
    public float fadeSpeed = 2f;

    [Header("Game Over")]
    public GameOverScreen gameOverScreen;
    public float gameOverDelay = 3f; // longer delay

    // Private refs
    private FirstPersonController fpsController;
    private PlayerInput playerInput;

    private bool hasJumpscared;
    private bool isRespawning;
    private bool firstSpawn = true;

    private void Start()
    {
        cutsceneCamera.gameObject.SetActive(false);
        fadePanel.alpha = 0f;

        fpsController = playerCapsule.GetComponent<FirstPersonController>();
        playerInput = playerCapsule.GetComponent<PlayerInput>();

        SpawnAtRandomLocation(firstSpawn);
        firstSpawn = false;
    }

    private void Update()
    {
        if (hasJumpscared || isRespawning) return;

        float distance = Vector3.Distance(transform.position, playerCapsule.position);

        if (distance > detectionRange)
            Patrol();
        else if (distance <= detectionRange && distance > jumpscareDistance)
            Chase();
        else if (distance <= jumpscareDistance)
            StartCoroutine(JumpScare());
    }

    private void Patrol()
    {
        agent.speed = patrolSpeed;
        if (!hasPatrolPoint) FindPatrolPoint();
        agent.SetDestination(patrolPoint);
        if (Vector3.Distance(transform.position, patrolPoint) < 1.5f) hasPatrolPoint = false;
    }

    private void FindPatrolPoint()
    {
        float randomX = Random.Range(-patrolRadius, patrolRadius);
        float randomZ = Random.Range(-patrolRadius, patrolRadius);
        patrolPoint = new Vector3(transform.position.x + randomX, 0f, transform.position.z + randomZ);
        hasPatrolPoint = true;
    }

    private void Chase()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(playerCapsule.position);
    }

    private void SpawnAtRandomLocation(bool favorCloseToPlayer = false)
    {
        Vector3 spawnPos = Vector3.zero;
        int attempts = 0;

        do
        {
            float x = Random.Range(minX, maxX);
            float z = Random.Range(minZ, maxZ);
            spawnPos = new Vector3(x, 0f, z);
            attempts++;

            float distanceToPlayer = Vector3.Distance(spawnPos, playerCapsule.position);
            if (favorCloseToPlayer)
            {
                if (distanceToPlayer <= initialSpawnFavorDistance && distanceToPlayer >= 50f) break;
            }
            else
            {
                if (distanceToPlayer >= minSpawnDistanceFromPlayer) break;
            }

            if (attempts > 50) break;

        } while (true);

        agent.enabled = false;
        transform.position = spawnPos;
        agent.enabled = true;

        model.SetActive(true);
        hasJumpscared = false;
        isRespawning = false;
    }

    private IEnumerator Disappear()
    {
        isRespawning = true;
        agent.isStopped = true;

        yield return new WaitForSeconds(disappearDelay);

        PlayRandom(disappearVoices);
        model.SetActive(false);

        yield return new WaitForSeconds(Random.Range(minRespawnDelay, maxRespawnDelay));

        agent.isStopped = false;
        SpawnAtRandomLocation();
    }

    private IEnumerator JumpScare()
    {
        if (hasJumpscared) yield break;
        hasJumpscared = true;

        agent.isStopped = true;

        // Disable player smoothly
        if (fpsController != null) fpsController.enabled = false;
        if (playerInput != null) playerInput.enabled = false;

        playerCamera.gameObject.SetActive(false);
        cutsceneCamera.gameObject.SetActive(true);

        if (jumpScareSFX != null) audioSource.PlayOneShot(jumpScareSFX);

        // wait for audio length to finish before fade
        float waitTime = jumpScareSFX != null ? jumpScareSFX.length : 0.5f;
        yield return new WaitForSeconds(waitTime);

        // Fade to black
        while (fadePanel.alpha < 1f)
        {
            fadePanel.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // Disable cutscene camera to make fully black
        cutsceneCamera.gameObject.SetActive(false);

        yield return new WaitForSeconds(gameOverDelay);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        gameOverScreen.Setup();
    }

    private IEnumerator CameraShake(float duration, float amount)
    {
        Vector3 originalPos = cutsceneCamera.transform.localPosition;
        float timer = 0f;
        while (timer < duration)
        {
            cutsceneCamera.transform.localPosition = originalPos + Random.insideUnitSphere * amount;
            timer += Time.deltaTime;
            yield return null;
        }
        cutsceneCamera.transform.localPosition = originalPos;
    }

    private void PlayRandom(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;
        audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
}

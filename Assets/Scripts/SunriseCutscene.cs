using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;

public class SunriseCutscene : MonoBehaviour
{
    [Header("Sun")]
    public Light sun;
    public float startSunX = -30.1f;
    public float endSunX = 13.8f;
    public float sunRiseDuration = 12f; // fits whole cutscene
    public float finalSunIntensity = 1.2f;

    [Header("Characters")]
    public Transform manananggal;
    public Transform bodyTarget;
    public NavMeshAgent agent;

    [Header("Cameras")]
    public Camera sunriseCamera;
    public Camera reactionCamera;
    public Camera cutsceneCamera;

    [Header("UI")]
    public TMP_Text cutsceneText;
    public GameObject survivedPanel;

    [Header("Movement")]
    public float panicSpeed = 6f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sunriseClip;
    public AudioClip worriedClip;
    public AudioClip flyingClip;
    public AudioClip screamClip;

    void Start()
    {
        // Disable everything at start
        sunriseCamera.gameObject.SetActive(false);
        reactionCamera.gameObject.SetActive(false);
        cutsceneCamera.gameObject.SetActive(false);

        survivedPanel.SetActive(false);
        cutsceneText.gameObject.SetActive(false);

        StartCoroutine(CutsceneSequence());
    }

    IEnumerator CutsceneSequence()
    {
        // Start sun rising in background
        StartCoroutine(SunriseRoutine());

        // 1️⃣ SUNRISE CAMERA
        sunriseCamera.gameObject.SetActive(true);
        PlaySound(sunriseClip);

        yield return new WaitForSeconds(3.5f);

        // 2️⃣ REACTION CAMERA
        sunriseCamera.gameObject.SetActive(false);
        reactionCamera.gameObject.SetActive(true);

        cutsceneText.gameObject.SetActive(true);
        cutsceneText.text = "The sun... it's rising.";
        PlaySound(worriedClip);

        yield return new WaitForSeconds(2.5f);

        // 3️⃣ CUTSCENE CAMERA (PANIC)
        reactionCamera.gameObject.SetActive(false);
        cutsceneCamera.gameObject.SetActive(true);

        cutsceneText.text = "I need my body!";
        PlaySound(flyingClip);

        agent.speed = panicSpeed;
        agent.isStopped = false;
        agent.SetDestination(bodyTarget.position);

        yield return new WaitForSeconds(3f);

        // 4️⃣ SCREAM
        cutsceneText.text = "AAAAAAH!";
        PlaySound(screamClip);

        yield return new WaitForSeconds(1.2f);

        // 5️⃣ DEATH
        KillManananggal();
    }

    IEnumerator SunriseRoutine()
    {
        float t = 0f;
        float startIntensity = sun.intensity;
        Vector3 sunEuler = sun.transform.eulerAngles;

        while (t < sunRiseDuration)
        {
            t += Time.deltaTime;
            float lerp = t / sunRiseDuration;

            float xRot = Mathf.Lerp(startSunX, endSunX, lerp);
            sun.transform.rotation = Quaternion.Euler(xRot, sunEuler.y, sunEuler.z);
            sun.intensity = Mathf.Lerp(startIntensity, finalSunIntensity, lerp);

            yield return null;
        }
    }

    void KillManananggal()
    {
        agent.isStopped = true;
        manananggal.gameObject.SetActive(false);

        cutsceneText.text = "The Manananggal was consumed by the sun.";

        StartCoroutine(ShowSurvivedUI());
    }

    IEnumerator ShowSurvivedUI()
    {
        yield return new WaitForSeconds(2.5f);

        cutsceneText.gameObject.SetActive(false);
        cutsceneCamera.gameObject.SetActive(false);

        survivedPanel.SetActive(true);
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(clip);
        }
    }
}

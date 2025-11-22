using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransformationCutscene : MonoBehaviour
{
    [Header("Models")]
    public GameObject baseModel;
    public GameObject transformedModel;

    [Header("Cameras")]
    public Camera cutsceneCamera;
    public Camera playerCamera;

    [Header("Effects")]
    public float cutsceneDuration = 3f;
    public AudioSource screamSFX;
    public Animator fadePanelAnimator; // Animator of the FadePanel
    public float fadeDuration = 0.75f; // Matches 45 frames at 60fps
    public float shakeAmount = 0.25f;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return; // Make sure PlayerCapsule tag matches

        triggered = true;
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        // Switch to cutscene camera
        playerCamera.gameObject.SetActive(false);
        cutsceneCamera.gameObject.SetActive(true);

        // Play scream SFX
        if (screamSFX != null)
            screamSFX.Play();

        // Swap models
        baseModel.SetActive(false);
        transformedModel.SetActive(true);

        // Start camera shake
        StartCoroutine(CameraShake(0.3f, shakeAmount));

        // Let cutscene play for a few seconds
        yield return new WaitForSeconds(cutsceneDuration);

        // Play fade animation
        if (fadePanelAnimator != null)
            fadePanelAnimator.SetTrigger("Fade"); // Make sure the animation has a "Fade" trigger

        yield return new WaitForSeconds(fadeDuration);

        // Load next scene
        SceneManager.LoadScene("SampleScene");
    }

    private IEnumerator CameraShake(float duration, float amount)
    {
        Vector3 originalPos = cutsceneCamera.transform.localPosition;
        float timer = 0;

        while (timer < duration)
        {
            cutsceneCamera.transform.localPosition = originalPos + Random.insideUnitSphere * amount;
            timer += Time.deltaTime;
            yield return null;
        }

        cutsceneCamera.transform.localPosition = originalPos;
    }
}

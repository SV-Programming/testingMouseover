using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class HoverSceneChanger : MonoBehaviour, IPointerEnterHandler
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private float transitionDelay = 0.5f; // Time in seconds

    private AudioSource audioSource;
    private bool isTransitioning = false;

    [Header("Scene Settings")]
    [SerializeField] private Object sceneToLoad;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Prevent starting the coroutine multiple times if the mouse jitters
        if (!isTransitioning)
        {
            StartCoroutine(PlaySoundAndLoad());
        }
    }

    private IEnumerator PlaySoundAndLoad()
    {
        isTransitioning = true;

        // 1. Play the sound
        if (hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }

        // 2. Wait for the specified delay
        yield return new WaitForSeconds(transitionDelay);

        // 3. Load the scene
        if (sceneToLoad != null)
        {
            SceneManager.LoadScene(sceneToLoad.name);
        }
        else
        {
            Debug.LogWarning("No scene assigned to " + gameObject.name);
            isTransitioning = false; // Reset if failed so user can try again
        }
    }
}
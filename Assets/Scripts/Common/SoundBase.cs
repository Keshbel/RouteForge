using UnityEngine;
using UnityEngine.UI;

public class SoundBase : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private Button[] clickButtons;

    private void Awake()
    {
        if (audioSource == null)
        {
            Debug.LogError("SoundBase requires an explicit AudioSource reference.", this);
            enabled = false;
        }
    }

    private void OnEnable()
    {
        SubscribeClickSound();
    }

    private void OnDisable()
    {
        UnsubscribeClickSound();
    }

    private void SoundClick()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    private void SubscribeClickSound()
    {
        if (clickButtons == null)
        {
            return;
        }

        for (int i = 0; i < clickButtons.Length; i++)
        {
            if (clickButtons[i] != null)
            {
                clickButtons[i].onClick.AddListener(SoundClick);
            }
        }
    }

    private void UnsubscribeClickSound()
    {
        if (clickButtons == null)
        {
            return;
        }

        for (int i = 0; i < clickButtons.Length; i++)
        {
            if (clickButtons[i] != null)
            {
                clickButtons[i].onClick.RemoveListener(SoundClick);
            }
        }
    }
}

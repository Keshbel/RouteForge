using Lean.Localization;
using TMPro;
using UnityEngine;

/// <summary>
/// Обновляет TMP-текст значением фразы из Lean Localization.
/// </summary>
[DisallowMultipleComponent]
public sealed class LeanTmpTextLocalizer : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private string phraseName;
    [SerializeField, TextArea(1, 4)] private string fallbackText;

    private void OnEnable()
    {
        if (!ValidateReferences())
        {
            enabled = false;
            return;
        }

        LeanLocalization.OnLocalizationChanged += ApplyText;
        ApplyText();
    }

    private void OnDisable()
    {
        LeanLocalization.OnLocalizationChanged -= ApplyText;
    }

    private void ApplyText()
    {
        tmpText.text = LeanLocalization.GetTranslationText(phraseName, fallbackText);
    }

    private bool ValidateReferences()
    {
        if (tmpText != null)
        {
            return true;
        }

        Debug.LogError("TMP text localizer is missing a required TMP_Text reference.", this);
        return false;
    }
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Binds one UI text component to English and Russian string variants.
/// </summary>
[Serializable]
public sealed class LocalizedTextBinding
{
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private Text legacyText;
    [SerializeField, TextArea(1, 6)] private string english;
    [SerializeField, TextArea(1, 6)] private string russian;

    /// <summary>
    /// Applies the string variant for the selected language.
    /// </summary>
    /// <param name="language">Language used to choose the text value.</param>
    public void Apply(EGameLanguage language)
    {
        string value = language == EGameLanguage.Russian ? russian : english;

        if (tmpText != null)
        {
            tmpText.text = value;
        }

        if (legacyText != null)
        {
            legacyText.text = value;
        }
    }
}

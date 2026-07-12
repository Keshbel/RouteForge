using System;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Applies localized static UI text and formats dynamic gameplay messages.
/// </summary>
public sealed class GameTextLocalizer : MonoBehaviour
{
    private const string EnglishLanguageToggleLabel = "EN";
    private const string RussianLanguageToggleLabel = "RU";
    private const string DescriptionTextName = "Description_Text";
    private const string TitleTextName = "Title_Text";
    private const string CubePanelName = "Panel_CubePanel";
    private const string MapPanelName = "Panel_Map";
    private const string TryAgainKey = "Try again...";
    private const string AlmostWonKey = "You almost won...";
    private const string WonKey = "You won!";

    [Header("Language")]
    [SerializeField] private bool useSystemLanguage = true;
    [SerializeField] private EGameLanguage language = EGameLanguage.Russian;

    [Header("Static UI")]
    [SerializeField] private LocalizedTextBinding[] staticTexts = Array.Empty<LocalizedTextBinding>();
    [SerializeField] private Transform staticTextRoot;

    [Header("Language Toggle")]
    [SerializeField] private Button languageToggleButton;
    [SerializeField] private TMP_Text languageToggleTmpText;
    [SerializeField] private Text languageToggleLegacyText;
    [SerializeField] private bool createLanguageToggleIfMissing = true;
    [SerializeField] private Vector2 languageToggleAnchoredPosition = new Vector2(-56f, -40f);
    [SerializeField] private Vector2 languageToggleSize = new Vector2(72f, 44f);

    [Header("Score")]
    [SerializeField] private string englishScoreFormat = "Score: {0}";
    [SerializeField] private string russianScoreFormat = "Очки: {0}";

    /// <summary>
    /// Current UI language after system-language resolution.
    /// </summary>
    public EGameLanguage Language => language;

    private void Awake()
    {
        ResolveInitialLanguage();
        EnsureLanguageToggle();
        SubscribeLanguageToggle();
        ApplyStaticTexts();
    }

    private void OnDestroy()
    {
        UnsubscribeLanguageToggle();
    }

    private void OnValidate()
    {
        ApplyStaticTexts();
    }

    /// <summary>
    /// Formats the score label for the current language.
    /// </summary>
    /// <param name="score">Score value to show.</param>
    /// <returns>Localized score label.</returns>
    public string FormatScore(int score)
    {
        string format = language == EGameLanguage.Russian ? russianScoreFormat : englishScoreFormat;
        return string.Format(format, score);
    }

    /// <summary>
    /// Converts a domain result key into localized UI text.
    /// </summary>
    /// <param name="resultText">Domain result key or fallback text.</param>
    /// <returns>Localized result text.</returns>
    public string LocalizeResult(string resultText)
    {
        switch (resultText)
        {
            case TryAgainKey:
                return language == EGameLanguage.Russian ? "Попробуйте ещё раз" : "Try again";
            case AlmostWonKey:
                return language == EGameLanguage.Russian ? "Почти получилось" : "Almost there";
            case WonKey:
                return language == EGameLanguage.Russian ? "Победа!" : "You won!";
            default:
                return string.IsNullOrEmpty(resultText)
                    ? (language == EGameLanguage.Russian ? "Результат неизвестен" : "Unknown result")
                    : resultText;
        }
    }

    /// <summary>
    /// Applies localization to every assigned static text.
    /// </summary>
    public void ApplyStaticTexts()
    {
        SyncLeanLocalizationLanguage();

        if (staticTexts == null)
        {
            ApplyKnownTextsUnderRoot();
            UpdateLanguageToggleLabel();
            return;
        }

        for (int i = 0; i < staticTexts.Length; i++)
        {
            staticTexts[i]?.Apply(language);
        }

        ApplyKnownTextsUnderRoot();
        UpdateLanguageToggleLabel();
    }

    /// <summary>
    /// Changes the active language and refreshes static UI text.
    /// </summary>
    /// <param name="nextLanguage">Language to apply.</param>
    public void SetLanguage(EGameLanguage nextLanguage)
    {
        useSystemLanguage = false;
        language = nextLanguage;
        ApplyStaticTexts();
    }

    /// <summary>
    /// Switches the UI to English text.
    /// </summary>
    public void SetEnglish()
    {
        SetLanguage(EGameLanguage.English);
    }

    /// <summary>
    /// Switches the UI to Russian text.
    /// </summary>
    public void SetRussian()
    {
        SetLanguage(EGameLanguage.Russian);
    }

    /// <summary>
    /// Переключает интерфейс между русским и английским языками.
    /// </summary>
    public void ToggleLanguage()
    {
        SetLanguage(language == EGameLanguage.Russian ? EGameLanguage.English : EGameLanguage.Russian);
    }

    private void EnsureLanguageToggle()
    {
        if (languageToggleButton != null || !createLanguageToggleIfMissing || staticTextRoot == null)
        {
            return;
        }

        RectTransform parent = staticTextRoot as RectTransform;
        if (parent == null)
        {
            return;
        }

        GameObject buttonObject = new GameObject("Button_Language", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        buttonObject.layer = staticTextRoot.gameObject.layer;
        buttonObject.transform.SetParent(staticTextRoot, false);

        RectTransform buttonTransform = (RectTransform)buttonObject.transform;
        buttonTransform.anchorMin = new Vector2(1f, 1f);
        buttonTransform.anchorMax = new Vector2(1f, 1f);
        buttonTransform.pivot = new Vector2(1f, 1f);
        buttonTransform.anchoredPosition = languageToggleAnchoredPosition;
        buttonTransform.sizeDelta = languageToggleSize;

        Image image = buttonObject.GetComponent<Image>();
        image.color = new Color(0.08f, 0.1f, 0.13f, 0.92f);

        languageToggleButton = buttonObject.GetComponent<Button>();
        languageToggleButton.targetGraphic = image;

        GameObject textObject = new GameObject("Text_Language", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        textObject.layer = buttonObject.layer;
        textObject.transform.SetParent(buttonTransform, false);

        RectTransform textTransform = (RectTransform)textObject.transform;
        textTransform.anchorMin = Vector2.zero;
        textTransform.anchorMax = Vector2.one;
        textTransform.offsetMin = Vector2.zero;
        textTransform.offsetMax = Vector2.zero;

        languageToggleLegacyText = textObject.GetComponent<Text>();
        languageToggleLegacyText.alignment = TextAnchor.MiddleCenter;
        languageToggleLegacyText.color = Color.white;
        languageToggleLegacyText.fontSize = 22;
        languageToggleLegacyText.raycastTarget = false;
        languageToggleLegacyText.font = GetBuiltinUIFont();
    }

    private void SubscribeLanguageToggle()
    {
        if (languageToggleButton != null)
        {
            languageToggleButton.onClick.AddListener(ToggleLanguage);
        }
    }

    private void UnsubscribeLanguageToggle()
    {
        if (languageToggleButton != null)
        {
            languageToggleButton.onClick.RemoveListener(ToggleLanguage);
        }
    }

    private void UpdateLanguageToggleLabel()
    {
        string label = language == EGameLanguage.Russian ? EnglishLanguageToggleLabel : RussianLanguageToggleLabel;

        if (languageToggleTmpText != null)
        {
            languageToggleTmpText.text = label;
        }

        if (languageToggleLegacyText != null)
        {
            languageToggleLegacyText.text = label;
        }
    }

    private void ApplyKnownTextsUnderRoot()
    {
        if (staticTextRoot == null)
        {
            return;
        }

        // Prefab-instance child text IDs are brittle in YAML; this one-time initialization pass
        // localizes only known static labels and keeps interaction paths free from hierarchy scans.
        TMP_Text[] tmpTexts = staticTextRoot.GetComponentsInChildren<TMP_Text>(true);
        for (int i = 0; i < tmpTexts.Length; i++)
        {
            TMP_Text text = tmpTexts[i];
            if (text != null)
            {
                text.text = LocalizeStaticText(text, text.text);
            }
        }

        Text[] legacyTexts = staticTextRoot.GetComponentsInChildren<Text>(true);
        for (int i = 0; i < legacyTexts.Length; i++)
        {
            Text text = legacyTexts[i];
            if (text != null)
            {
                text.text = LocalizeStaticText(text, text.text);
            }
        }
    }

    private string LocalizeStaticText(Component textComponent, string value)
    {
        GameObject textObject = textComponent.gameObject;
        if (textObject.name == DescriptionTextName && IsIntroductionDescription(value))
        {
            return GetIntroductionDescription();
        }

        if (textObject.name == TitleTextName)
        {
            Transform parent = textComponent.transform.parent;
            if (parent != null)
            {
                switch (parent.name)
                {
                    case CubePanelName:
                        return language == EGameLanguage.Russian ? "Кубы" : "Cubes";
                    case MapPanelName:
                        return language == EGameLanguage.Russian ? "Карта" : "Map";
                }
            }
        }

        return LocalizeStaticText(value);
    }

    private string LocalizeStaticText(string value)
    {
        switch (value)
        {
            case "Cube Controller":
            case "Кубы":
                return language == EGameLanguage.Russian ? "Кубы" : "Cubes";
            case "Map Controller":
            case "Карта":
                return language == EGameLanguage.Russian ? "Карта" : "Map";
            case "Interactive Map":
            case "Interactive Mapo":
            case "Интерактивная карта":
                return language == EGameLanguage.Russian ? "Интерактивная карта" : "Interactive Map";
            case "Game Over!":
            case "Итоги":
                return language == EGameLanguage.Russian ? "Итоги" : "Results";
            case "Cube 1":
            case "Куб 1":
                return language == EGameLanguage.Russian ? "Куб 1" : "Cube 1";
            case "Cube 2":
            case "Куб 2":
                return language == EGameLanguage.Russian ? "Куб 2" : "Cube 2";
            case "Score = 50":
            case "Очки: 50":
            case "Score: 50":
                return FormatScore(50);
            case "You Win!\n\n":
            case "Победа!":
                return language == EGameLanguage.Russian ? "Победа!" : "You won!";
            default:
                return LocalizeDescription(value);
        }
    }

    private string LocalizeDescription(string value)
    {
        if (!IsIntroductionDescription(value))
        {
            return value;
        }

        return GetIntroductionDescription();
    }

    private string GetIntroductionDescription()
    {
        if (language == EGameLanguage.Russian)
        {
            return "Проведите оба куба к зелёным клеткам в центре карты.\n\n"
                + "Левой кнопкой мыши рисуйте маршрут, правой удаляйте лишние клетки. "
                + "Переключайтесь между кубами и запускайте движение, когда оба пути готовы.";
        }

        return "Guide both cubes to the green goal cells in the center of the map.\n\n"
            + "Use the left mouse button to draw a route and the right mouse button to erase cells. "
            + "Switch between cubes, then start movement when both paths are ready.";
    }

    private static bool IsIntroductionDescription(string value)
    {
        return !string.IsNullOrEmpty(value)
            && (value.StartsWith("Приветствую!", StringComparison.Ordinal)
                || value.StartsWith("Проведите оба куба", StringComparison.Ordinal)
                || value.StartsWith("Guide both cubes", StringComparison.Ordinal));
    }

    private void ResolveInitialLanguage()
    {
        if (!useSystemLanguage)
        {
            SyncLeanLocalizationLanguage();
            return;
        }

        language = Application.systemLanguage == SystemLanguage.Russian
            ? EGameLanguage.Russian
            : EGameLanguage.English;
        SyncLeanLocalizationLanguage();
    }

    private void SyncLeanLocalizationLanguage()
    {
        LeanLocalization.SetCurrentLanguageAll(language == EGameLanguage.Russian ? "Russian" : "English");
    }

    private static Font GetBuiltinUIFont()
    {
        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        return font != null ? font : Resources.GetBuiltinResource<Font>("Arial.ttf");
    }
}

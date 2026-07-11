using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace RouteForge
{
    /// <summary>
    /// Описание проблемы, найденной при проверке уровня.
    /// </summary>
    public readonly struct LevelValidationIssue
    {
        /// <summary>
        /// Путь asset, в котором найдена проблема.
        /// </summary>
        public string AssetPath { get; }

        /// <summary>
        /// Понятное описание проблемы.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Признак ошибки, которая должна блокировать build.
        /// </summary>
        public bool IsCritical { get; }

        private LevelValidationIssue(string assetPath, string message, bool isCritical)
        {
            AssetPath = string.IsNullOrEmpty(assetPath) ? "<unknown asset>" : assetPath;
            Message = message;
            IsCritical = isCritical;
        }

        /// <summary>
        /// Создает критическую ошибку проверки.
        /// </summary>
        /// <param name="assetPath">Путь asset.</param>
        /// <param name="message">Описание проблемы.</param>
        /// <returns>Критическая ошибка проверки.</returns>
        public static LevelValidationIssue Critical(string assetPath, string message)
        {
            return new LevelValidationIssue(assetPath, message, true);
        }

        /// <summary>
        /// Формирует текст ошибки с путем asset.
        /// </summary>
        /// <returns>Строка для логов и build errors.</returns>
        public override string ToString()
        {
            return $"{AssetPath}: {Message}";
        }
    }

    /// <summary>
    /// Логирует найденные ошибки проверки уровня.
    /// </summary>
    public static class LevelValidationReporter
    {
        /// <summary>
        /// Выводит ошибки проверки в Unity Console.
        /// </summary>
        /// <param name="issues">Ошибки проверки.</param>
        public static void LogIssues(LevelValidationIssue[] issues)
        {
            for (int i = 0; i < issues.Length; i++)
            {
                if (issues[i].IsCritical)
                {
                    Debug.LogError(issues[i].ToString());
                }
                else
                {
                    Debug.LogWarning(issues[i].ToString());
                }
            }
        }
    }

    /// <summary>
    /// Проверяет все LevelDefinition assets проекта.
    /// </summary>
    public static class LevelProjectValidator
    {
        /// <summary>
        /// Запускает проверку проекта из меню Unity Editor.
        /// </summary>
        [MenuItem("Tools/RouteForge/Validate Project")]
        public static void ValidateProjectMenu()
        {
            LevelValidationIssue[] issues = ValidateProject();
            LevelValidationReporter.LogIssues(issues);
            if (issues.Length == 0)
            {
                Debug.Log("RouteForge project validation passed.");
            }
        }

        /// <summary>
        /// Проверяет все assets уровней в проекте.
        /// </summary>
        /// <returns>Список найденных проблем.</returns>
        public static LevelValidationIssue[] ValidateProject()
        {
            var issues = new List<LevelValidationIssue>();
            string[] guids = AssetDatabase.FindAssets("t:LevelDefinition");
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var levelDefinition = AssetDatabase.LoadAssetAtPath<LevelDefinition>(path);
                LevelValidationIssue[] assetIssues = LevelDefinitionValidator.Validate(levelDefinition, path);
                for (int j = 0; j < assetIssues.Length; j++)
                {
                    issues.Add(assetIssues[j]);
                }
            }

            return issues.ToArray();
        }

        /// <summary>
        /// Проверяет наличие критических ошибок.
        /// </summary>
        /// <param name="issues">Ошибки проверки.</param>
        /// <returns>Возвращает true, если есть хотя бы одна критическая ошибка.</returns>
        public static bool HasCriticalIssues(LevelValidationIssue[] issues)
        {
            for (int i = 0; i < issues.Length; i++)
            {
                if (issues[i].IsCritical)
                {
                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// Блокирует build при критически невалидных конфигурациях уровней.
    /// </summary>
    public sealed class LevelBuildValidator : IPreprocessBuildWithReport
    {
        /// <summary>
        /// Порядок выполнения build preprocessor.
        /// </summary>
        public int callbackOrder => 0;

        /// <summary>
        /// Проверяет уровни перед сборкой проекта.
        /// </summary>
        /// <param name="report">Отчет сборки Unity.</param>
        public void OnPreprocessBuild(BuildReport report)
        {
            LevelValidationIssue[] issues = LevelProjectValidator.ValidateProject();
            if (!LevelProjectValidator.HasCriticalIssues(issues))
            {
                return;
            }

            LevelValidationReporter.LogIssues(issues);
            throw new BuildFailedException("RouteForge level validation failed. Fix critical level configuration errors before building.");
        }
    }
}

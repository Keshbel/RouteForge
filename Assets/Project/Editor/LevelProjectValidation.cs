using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public readonly struct LevelValidationIssue
    {
        /// <summary>
        /// Describes this API member.
        /// </summary>
        public string AssetPath { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public bool IsCritical { get; }

        private LevelValidationIssue(string assetPath, string message, bool isCritical)
        {
            AssetPath = string.IsNullOrEmpty(assetPath) ? "<unknown asset>" : assetPath;
            Message = message;
            IsCritical = isCritical;
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="assetPath">The assetPath value.</param>
        /// <param name="message">The message value.</param>
        /// <returns>The operation result.</returns>
        public static LevelValidationIssue Critical(string assetPath, string message)
        {
            return new LevelValidationIssue(assetPath, message, true);
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <returns>The operation result.</returns>
        public override string ToString()
        {
            return $"{AssetPath}: {Message}";
        }
    }

    /// <summary>
    /// Describes this API member.
    /// </summary>
    public static class LevelValidationReporter
    {
        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="issues">The issues value.</param>
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
    /// Describes this API member.
    /// </summary>
    public static class LevelProjectValidator
    {
        /// <summary>
        /// Describes this API member.
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
        /// Describes this API member.
        /// </summary>
        /// <returns>The operation result.</returns>
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
        /// Describes this API member.
        /// </summary>
        /// <param name="issues">The issues value.</param>
        /// <returns>The operation result.</returns>
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
    /// Describes this API member.
    /// </summary>
    public sealed class LevelBuildValidator : IPreprocessBuildWithReport
    {
        /// <summary>
        /// Describes this API member.
        /// </summary>
        public int callbackOrder => 0;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="report">The report value.</param>
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

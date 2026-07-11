using UnityEditor;
using UnityEngine;

namespace RouteForge
{
    /// <summary>
    /// Inspector для ручной проверки конфигурации уровня.
    /// </summary>
    [CustomEditor(typeof(LevelDefinition))]
    public sealed class LevelDefinitionEditor : Editor
    {
        /// <summary>
        /// Рисует стандартный inspector и кнопку проверки.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Validate"))
            {
                var levelDefinition = (LevelDefinition)target;
                string path = AssetDatabase.GetAssetPath(levelDefinition);
                LevelValidationIssue[] issues = LevelDefinitionValidator.Validate(levelDefinition, path);
                LevelValidationReporter.LogIssues(issues);
                if (issues.Length == 0)
                {
                    Debug.Log($"Level validation passed: {path}", levelDefinition);
                }
            }
        }
    }
}

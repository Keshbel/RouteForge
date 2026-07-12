using UnityEditor;
using UnityEngine;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    [CustomEditor(typeof(LevelDefinition))]
    public sealed class LevelDefinitionEditor : Editor
    {
        /// <summary>
        /// Describes this API member.
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

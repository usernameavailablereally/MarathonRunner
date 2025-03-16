using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace Game.Configs
{
    [CustomEditor(typeof(MatchConfig))]
    public class MatchConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var matchConfig = (MatchConfig)target;
           
            var titleStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 18,
                fontStyle = FontStyle.Bold
            };
            GUILayout.Label("Hint: Changes work also in Runtime", titleStyle);
            
            SerializedProperty property = serializedObject.GetIterator();
            property.NextVisible(true);

            while (property.NextVisible(false))
            {
                FieldInfo fieldInfo = matchConfig.GetType().GetField(property.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                {
                    var tooltip = (TooltipAttribute)fieldInfo.GetCustomAttribute(typeof(TooltipAttribute), false);
                    if (tooltip != null)
                    {
                        EditorGUILayout.HelpBox(tooltip.tooltip, MessageType.Info);
                    }
                }
                EditorGUILayout.PropertyField(property, true);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
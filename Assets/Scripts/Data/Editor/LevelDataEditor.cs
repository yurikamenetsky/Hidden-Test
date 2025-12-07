using UnityEditor;
using UnityEngine;

namespace Data.Editor
{
    [CustomEditor(typeof(LevelData))]
    public class LevelDataEditor : UnityEditor.Editor
    {
        private SerializedProperty _itemsProperty;
        private SerializedProperty _backgroundSprite;
        private SerializedProperty _levelId;
        private bool _showActiveOnly = false;
    
        private void OnEnable()
        {
            _itemsProperty = serializedObject.FindProperty("items");
            _backgroundSprite = serializedObject.FindProperty("backgroundSprite");
            _levelId = serializedObject.FindProperty("levelId");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            DrawPropertiesExcluding(serializedObject, "items", "backgroundSprite");
            
            EditorGUILayout.LabelField("Background");
            _backgroundSprite.objectReferenceValue = EditorGUILayout.ObjectField(_backgroundSprite.objectReferenceValue, typeof(Sprite), false, 
                GUILayout.Width(64), GUILayout.Height(64));

            EditorGUILayout.Space(10);

            _showActiveOnly = EditorGUILayout.Toggle("Show Active Only", _showActiveOnly);

            EditorGUILayout.Space(5);
            
            EditorGUILayout.LabelField($"Items ({(_showActiveOnly ? "Active Only" : "All")})", EditorStyles.boldLabel);
            
            // Draw items
            if (_itemsProperty != null)
            {
                for (int i = 0; i < _itemsProperty.arraySize; i++)
                {
                    var item = _itemsProperty.GetArrayElementAtIndex(i);
                    var isEnable = item.FindPropertyRelative("isEnable").boolValue;
                
                    if (_showActiveOnly && !isEnable)
                        continue;
                    
                    // Draw each item in one line
                    EditorGUILayout.PropertyField(item, new GUIContent($"Item {i}"), false);
                }
            }
        
            // Buttons for adding/removing items
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
        
            if (GUILayout.Button("Add Item"))
            {
                _itemsProperty.arraySize++;
            }
            
            if (GUILayout.Button("Remove Last Item") && _itemsProperty.arraySize > 0)
            {
                _itemsProperty.arraySize--;
            }
        
            EditorGUILayout.EndHorizontal();
        
            serializedObject.ApplyModifiedProperties();
        }
    }
}

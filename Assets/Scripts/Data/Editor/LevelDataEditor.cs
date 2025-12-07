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
            LevelData levelData = (LevelData)target;
            
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
                    
                    // Item sorting
                    if (i > 0 && GUILayout.Button("↑", GUILayout.Width(25)))
                    {
                        var it0 = levelData.items[i - 1];
                        var tmp = it0;
                        var it1 = levelData.items[i];
                        levelData.items[i - 1] = it1;
                        levelData.items[i] = tmp;
                    }
                
                    if (i < levelData.items.Count - 1 && GUILayout.Button("↓", GUILayout.Width(25)))
                    {
                        var it0 = levelData.items[i];
                        var it1 = levelData.items[i + 1];
                        var tmp = it1;
                        levelData.items[i + 1] = it0;
                        levelData.items[i] = tmp;
                    }
                    
                    EditorGUILayout.Space(30);
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

            // Сохранение изменений
            if (GUI.changed)
            {
                EditorUtility.SetDirty(levelData);
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}

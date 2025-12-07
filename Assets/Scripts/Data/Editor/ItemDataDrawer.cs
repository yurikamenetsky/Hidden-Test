using UnityEditor;
using UnityEngine;
using Views;

namespace Data.Editor
{
    [CustomPropertyDrawer(typeof(ItemData))]
    public class ItemDataDrawer : PropertyDrawer
    {
        private const float EnableWidth = 20f;
        private const float SpriteWidth = 64f;
        private const float UiIconWidth = 64f;
        private const float NameWidth = 120f;
        private const float PrefabWidth = 120f;
        private const float PositionWidth = 200f;
        
        private const float LineHeight = 20f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return Mathf.Max(SpriteWidth, UiIconWidth) * 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var isEnable = property.FindPropertyRelative("isEnable");
            var nameProp = property.FindPropertyRelative("name");
            var itemSprite = property.FindPropertyRelative("itemSprite");
            var uiIcon = property.FindPropertyRelative("uiIcon");
            var prefab = property.FindPropertyRelative("prefab");
            var itemPos = property.FindPropertyRelative("position");

            position.height = EditorGUIUtility.singleLineHeight;

            // Calculate rects
            var x = position.x;
            var y = position.y;
            
            var isEnableRect = new Rect(x, y, EnableWidth, LineHeight);
            x += EnableWidth + 2;

            var spriteRect = new Rect(x, y, SpriteWidth, SpriteWidth);
            x += SpriteWidth + 2;

            var uiIconRect = new Rect(x, y, UiIconWidth, UiIconWidth);
            x += UiIconWidth + 2;

            var nameRect = new Rect(x, y, NameWidth, LineHeight);
            x += NameWidth + 2;
            var prefabRect = new Rect(x, y, PrefabWidth, LineHeight);


            // Draw fields
            EditorGUI.PropertyField(isEnableRect, isEnable, GUIContent.none);
            EditorGUI.ObjectField(spriteRect, itemSprite, typeof(Sprite), GUIContent.none);
            EditorGUI.ObjectField(uiIconRect, uiIcon, typeof(Sprite), GUIContent.none);
            EditorGUI.PropertyField(nameRect, nameProp, GUIContent.none);
            EditorGUI.ObjectField(prefabRect, prefab, typeof(HiddenItemView), GUIContent.none);
            
            y += Mathf.Max(SpriteWidth, UiIconWidth);
            var positionLabelRect = new Rect(position.x, y, 60, LineHeight);
            var positionFieldRect = new Rect(position.x + 65, y, PositionWidth, LineHeight);
            
            EditorGUI.LabelField(positionLabelRect, "Position");
            EditorGUI.PropertyField(positionFieldRect, itemPos, GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.U2D;
using UnityEditor.U2D.PSD;
using UnityEngine;
using UnityEngine.U2D;
using Views;
using Object = UnityEngine.Object;

namespace Utils.Editor
{
    public class PhotoshopLevelImporter : EditorWindow
    {
        private Object _photoshopFile;

        private string _levelId;
        
        private string _backgroundId = "Background";
        private string _hiddenItemsLayerId = "Find Objects";
        private string _uiLayerId = "UI";
        
        public SpriteAtlas targetAtlas;

        [MenuItem("Tools/Hidden Object/Import Photoshop Level")]
        public static void ShowWindow() => GetWindow<PhotoshopLevelImporter>("Photoshop Importer");

        private void OnGUI()
        {
            _photoshopFile = EditorGUILayout.ObjectField("Photoshop File", _photoshopFile, typeof(Object), false);
            _levelId = EditorGUILayout.TextField("Level ID", _photoshopFile != null ? _photoshopFile.name : String.Empty);
            _backgroundId = EditorGUILayout.TextField("Background ID", _backgroundId);
            _hiddenItemsLayerId = EditorGUILayout.TextField("Hidden Items Layer Id", _hiddenItemsLayerId);
            _uiLayerId = EditorGUILayout.TextField("UI Items Layer Id", _uiLayerId);

            if (GUILayout.Button("Import Level"))
            {
                if (_photoshopFile == null)
                {
                    EditorUtility.DisplayDialog("Error", $"Select imported photoshop file", "OK");
                    return;
                }

                if (string.IsNullOrEmpty(_levelId))
                {
                    EditorUtility.DisplayDialog("Error", $"Enter Level ID", "OK");
                    return;
                }
                
                ImportPhotoshopLevel(AssetDatabase.GetAssetPath(_photoshopFile));
            }
        }

        private void ImportPhotoshopLevel(string filePath)
        {
            // 1. Level folder creation
            string levelPath = $"Assets/Levels/{_levelId}";
            if (!Directory.Exists(levelPath))
            {
                Directory.CreateDirectory(levelPath);
            }
            
            // 2. LevelData creation
            var levelData = CreateInstance<LevelData>();
            levelData.levelId = _levelId;
            levelData.items = new List<ItemData>();
            
            // 3. Items analysis
            Object[] allAssets = AssetDatabase.LoadAllAssetsAtPath(filePath);
            var allGameObjects = allAssets.OfType<GameObject>().ToArray();
            foreach (var go in allGameObjects)
            {
                var sprite = go.GetComponent<SpriteRenderer>();
                if (!sprite)
                {
                    continue;
                }

                if (sprite.name == _backgroundId)
                {
                    if (levelData.backgroundSprite)
                    {
                        Debug.LogWarning($"Multiple background sprite for level: {_levelId}");
                    }
                    levelData.backgroundSprite = sprite.sprite;
                    continue;
                }

                if (!go.transform.parent)
                {
                    continue;
                }

                var item = levelData.items.Find(x => go.name.Contains(x.name));
                if (item == null)
                {
                    var parts = go.name.Split('_');
                    var itemName = parts.Length > 0 ? parts[0] : go.name;
                    item = new ItemData { name = itemName };
                    levelData.items.Add(item);
                }
                    
                if (go.transform.parent.name == _hiddenItemsLayerId)
                {
                    item.itemSprite = sprite.sprite;
                    item.position = go.transform.position;
                }
                else if (go.transform.parent.name == _uiLayerId)
                {
                    item.uiIcon = sprite.sprite;
                }
            }

            var toDelete = new List<ItemData>();
            // 4. Prefabs creation
            foreach (var item in levelData.items)
            {
                if (!item.itemSprite || !item.uiIcon)
                {
                    Debug.LogWarning($"Item not consistent: {item.name}");
                    toDelete.Add(item);
                    continue;
                }
                item.prefab = CreateObjectPrefab(item.itemSprite, levelPath);
            }

            levelData.items.RemoveAll(x => toDelete.Contains(x));
            
            AssetDatabase.CreateAsset(levelData, $"{levelPath}/{_levelId}_Data.asset");
            AssetDatabase.SaveAssets();
            
            EditorUtility.DisplayDialog("Level import complete", $"Created {levelData.items.Count} items", "OK");
        }

        private HiddenItemView CreateObjectPrefab(Sprite sprite, string path)
        {
            var go = new GameObject(sprite.name);
            go.AddComponent<SpriteRenderer>().sprite = sprite;
            go.AddComponent<PolygonCollider2D>();
            go.AddComponent<HiddenItemView>();

            // Prefab creation
            var result = PrefabUtility.SaveAsPrefabAsset(go, $"{path}/{sprite.name}.prefab");
            DestroyImmediate(go);
            return result.GetComponent<HiddenItemView>();
        }
    }
}

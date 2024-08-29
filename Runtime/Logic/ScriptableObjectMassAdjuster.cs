using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace com.amuzak.SOMA.Logic
{
    public static class ScriptableObjectMassAdjuster
    {
        public static void ApplyChanges(Type selectedType, string regexPattern, Dictionary<FieldInfo, object> fieldValues)
        {
            if (selectedType == null || string.IsNullOrEmpty(regexPattern))
            {
                Debug.LogError("Invalid type or regex pattern.");
                return;
            }

            string[] guids = AssetDatabase.FindAssets($"t:{selectedType.Name}");

            if (guids.Length == 0)
            {
                Debug.LogWarning("No ScriptableObject assets found.");
                return;
            }

            Regex regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
            string modifiedAssetsLog = "";
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                string assetName = Path.GetFileNameWithoutExtension(assetPath);

                if (!regex.IsMatch(assetName)) continue;
                ScriptableObject asset = AssetDatabase.LoadAssetAtPath(assetPath, selectedType) as ScriptableObject;
                if (asset == null) continue;
                ApplyFieldValuesToAsset(asset, selectedType, fieldValues);
                EditorUtility.SetDirty(asset);
                modifiedAssetsLog += $"Modified: {assetName}\n";
            }
            if(modifiedAssetsLog.Length > 0)
            {
                modifiedAssetsLog = "ScriptableObject assets updated successfully:\n" + modifiedAssetsLog;
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log(modifiedAssetsLog);
            }
            else
            {
                Debug.Log("No valid assets found based on Regex logic.");
            }
        }

        private static void ApplyFieldValuesToAsset(ScriptableObject asset, Type selectedType, Dictionary<FieldInfo, object> fieldValues)
        {
            // sanity check
            Type assetType = asset.GetType();
            Debug.Assert(assetType == selectedType, $"Asset Match Failed! Selected asset of type: {assetType}, Target Type: {selectedType}");

            foreach ((FieldInfo field, object value) in fieldValues)
            {
                if (value != null)
                {
                    field.SetValue(asset, value);
                }
            }
        }
    }
}


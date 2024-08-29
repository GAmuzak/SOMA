using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Editor
{
    public class ScriptableObjectMassAdjusterEditorWindow : EditorWindow
    {
        private Type selectedType;
        private readonly Dictionary<FieldInfo, object> fieldValues = new Dictionary<FieldInfo, object>();
        private string regexPattern = "";
        private bool findMySOsOnly = true;
        private string searchQuery = "";

        [MenuItem("Tools/(SOMA) Scriptable Object Mass Adjuster")]
        public static void ShowWindow()
        {
            GetWindow<ScriptableObjectMassAdjusterEditorWindow>("SO Mass Adjuster");
        }

        private void OnGUI()
        {
            findMySOsOnly = EditorGUILayout.Toggle("Find My SOs Only", findMySOsOnly);

            searchQuery = EditorGUILayout.TextField("Search", searchQuery);

            DrawScriptableObjectTypeSelector();

            regexPattern = EditorGUILayout.TextField("Regex Pattern", regexPattern);

            if (selectedType != null)
            {
                DrawFieldsForSelectedType();
            }

            if (GUILayout.Button("Apply Changes"))
            {
                Debug.Log("Apply Changes functionality not implemented yet.");
            }
        }

        private void DrawScriptableObjectTypeSelector()
        {
            List<Type> scriptableObjectTypes = GetAllScriptableObjectTypes();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                scriptableObjectTypes = scriptableObjectTypes
                    .Where(t => t.Name.IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            string[] typeNames = scriptableObjectTypes.Select(t => t.Name).ToArray();

            int selectedIndex = selectedType != null ? scriptableObjectTypes.IndexOf(selectedType) : 0;
            int newIndex = EditorGUILayout.Popup("ScriptableObject Type", selectedIndex, typeNames);

            if (newIndex != selectedIndex)
            {
                selectedType = scriptableObjectTypes[newIndex];
                ResetFieldValues();
            }

            if (scriptableObjectTypes.Count == 0 && !string.IsNullOrWhiteSpace(searchQuery))
            {
                EditorGUILayout.HelpBox("No Scriptable Objects match the search criteria!", MessageType.Warning);
            }
        }

        private List<Type> GetAllScriptableObjectTypes()
        {
            IEnumerable<Type> allTypes = TypeCache.GetTypesDerivedFrom<ScriptableObject>().Where(t => !t.IsAbstract);

            if (findMySOsOnly)
            {
                string userAssemblyName = "Assembly-CSharp";
                allTypes = allTypes.Where(t => t.Assembly.GetName().Name == userAssemblyName);
            }

            List<Type> filteredTypes = allTypes.ToList();

            if (findMySOsOnly && filteredTypes.Count == 0)
            {
                EditorGUILayout.HelpBox("No user-defined Scriptable Objects found!", MessageType.Warning);
            }

            return filteredTypes;
        }

        private void DrawFieldsForSelectedType()
        {
            if (selectedType == null) return;

            EditorGUILayout.LabelField("Editable Fields:", EditorStyles.boldLabel);

            FieldInfo[] fields = selectedType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                try
                {
                    if (field.FieldType == typeof(int))
                    {
                        string currentValue = fieldValues.ContainsKey(field)
                            ? (fieldValues[field] as int?).ToString()
                            : string.Empty;
                        string newValue = EditorGUILayout.TextField($"(int) {field.Name}", currentValue);
                        fieldValues[field] = string.IsNullOrEmpty(newValue) ? null : int.Parse(newValue);
                    }
                    else if (field.FieldType == typeof(float))
                    {
                        string currentValue = fieldValues.ContainsKey(field)
                            ? (fieldValues[field] as float?).ToString()
                            : string.Empty;
                        string newValue = EditorGUILayout.TextField($"(float) {field.Name}", currentValue);
                        fieldValues[field] = string.IsNullOrEmpty(newValue) ? null : float.Parse(newValue);
                    }
                    else if (field.FieldType == typeof(string))
                    {
                        string currentValue =
                            fieldValues.ContainsKey(field) ? (string)fieldValues[field] : string.Empty;
                        string newValue = EditorGUILayout.TextField($"(string) {field.Name}", currentValue);
                        fieldValues[field] = newValue;
                    }
                    else if (field.FieldType == typeof(bool))
                    {
                        string currentValue = fieldValues.ContainsKey(field)
                            ? (fieldValues[field] as bool?).ToString()
                            : string.Empty;
                        string newValue = EditorGUILayout.TextField($"(bool) {field.Name}", currentValue);
                        fieldValues[field] = ParseBool(newValue);
                    }
                }
                catch
                {
                    Debug.LogError($"{field.Name} has been assigned an incorrect data type");
                    fieldValues[field] = string.Empty;
                }
            }
        }

        private void ResetFieldValues()
        {
            fieldValues.Clear();
            if (selectedType == null) return;

            FieldInfo[] fields = selectedType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(int)) fieldValues[field] = null;
                else if (field.FieldType == typeof(float)) fieldValues[field] = null;
                else if (field.FieldType == typeof(string)) fieldValues[field] = string.Empty;
                else if (field.FieldType == typeof(bool)) fieldValues[field] = false;
            }
        }

        private bool? ParseBool(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            value = value.Trim().ToLower();
            return value switch
            {
                "true" or "t" or "1" => true,
                "false" or "f" or "0" => false,
            };
        }
    }
}
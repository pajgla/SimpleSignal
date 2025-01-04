using System.IO;
using UnityEditor;
using UnityEngine;

namespace Signal.Editor
{
    public class SignalGeneratorWindow : EditorWindow
    {
        static const string S_PATH = "Assets/Signals/Generated/";
        
        string m_SignalName = "NewSignal";
        string m_SimpleDataType = "bool";
        string m_DataClassName = "SignalData";
        int m_PropertyCount = 0;

        string[] m_PropertyNames;
        string[] m_PropertyTypes;

        [MenuItem("Tools/Signal Generator")]
        public static void ShowWindow()
        {
            GetWindow<SignalGeneratorWindow>("Signal Generator");
        }

        void OnGUI()
        {
            GUILayout.Label("Signal Generator", EditorStyles.boldLabel);

            m_SignalName = EditorGUILayout.TextField("Signal Name", m_SignalName);
            m_SimpleDataType = EditorGUILayout.TextField("Simple Data Type", m_SimpleDataType);
            m_DataClassName = EditorGUILayout.TextField("Data Class Name (optional)", m_DataClassName);
            
            m_PropertyCount = Mathf.Max(0, EditorGUILayout.IntField("Number of Properties", m_PropertyCount));
            if (m_PropertyNames == null || m_PropertyNames.Length != m_PropertyCount)
            {
                m_PropertyNames = new string[m_PropertyCount];
                m_PropertyTypes = new string[m_PropertyCount];
            }

            for (int i = 0; i < m_PropertyCount; i++)
            {
                GUILayout.BeginHorizontal();
                int propertyID = i + 1;
                m_PropertyNames[i] = EditorGUILayout.TextField($"Property {propertyID} Name", m_PropertyNames[i]);
                m_PropertyTypes[i] = EditorGUILayout.TextField($"Property {propertyID} Type", m_PropertyTypes[i]);
                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Generate Signal"))
            {
                GenerateSignal();
            }
        }

        void GenerateSignal()
        {
            if (string.IsNullOrEmpty(m_SignalName))
            {
                Debug.LogError("Signal name cannot be empty!");
                return;
            }

            if (!string.IsNullOrEmpty(m_DataClassName) && !string.IsNullOrEmpty(m_SimpleDataType))
            {
                Debug.LogError("Signal cannot have both simple data type and data class.");
                return;
            }
            
            string signalPath = $"{S_PATH}{m_SignalName}.cs";
            string dataClassPath = string.IsNullOrEmpty(m_DataClassName) ? "" : $"{S_PATH}{m_DataClassName}.cs";
            
            //Generate Signal
            string signalScript = GenerateSignalScript();
            File.WriteAllText(signalPath, signalScript);
            bool hasDataClass = !string.IsNullOrEmpty(m_DataClassName);
            if (hasDataClass)
            {
                string dataClassScript = GenerateDataClassScript();
                File.WriteAllText(dataClassPath, dataClassScript);
            }
            
            AssetDatabase.Refresh();
            string outputString = hasDataClass ? $"Signal '{m_SignalName}' and data '{m_DataClassName}' are created successfully." : $"Signal '{m_SignalName}' created successfully.";
            Debug.Log(outputString);
        }

        string GenerateSignalScript()
        {
            string dataClassReference = string.IsNullOrEmpty(m_DataClassName) ? m_SimpleDataType : m_DataClassName;

            return $@"
using System;

namespace Core.Signals
{{
    public class {m_SignalName} : Signal<{dataClassReference}>
    {{
    }}
}}
";
        }

        string GenerateDataClassScript()
        {
            string properties = "";

            for (int i = 0; i < m_PropertyCount; i++)
            {
                properties += $"    public {m_PropertyTypes[i]} {m_PropertyNames[i]} {{ get; set; }}\n";
            }

            return $@"
namespace Core.Signals
{{
    public class {m_DataClassName}
    {{
    {properties}
    }}
}}
";
        }
    }
}


using System;
using UnityEditor;
using UnityEngine;

namespace Laputa.Localization.Editor
{
    [CustomEditor(typeof(LocalizationConfig))]
    public class LocalizationConfigEditor : UnityEditor.Editor
    {
        private LocalizationConfig _localizationConfig;

        void OnEnable()
        {
            _localizationConfig = (LocalizationConfig) target;
        }

        public override async void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(GUILayout.Button("Update Language Data", GUILayout.Height(40)))
            {
                _localizationConfig.UpdateLanguageData();
                serializedObject.SetIsDifferentCacheDirty();
                serializedObject.ApplyModifiedProperties();
            }
            
            if(GUILayout.Button("Translate All Predata", GUILayout.Height(40)))
            {
                await _localizationConfig.TranslateAllPredata();
                serializedObject.SetIsDifferentCacheDirty();
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}

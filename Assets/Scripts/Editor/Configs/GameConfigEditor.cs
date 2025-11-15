#if UNITY_EDITOR
using System.Collections.Generic;
using Configs;
using Gameplay.Data;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Configs
{
    [CustomEditor(typeof(GameConfig))]
    public class GameConfigEditor : Editor
    {
        private PowerUpData[] m_allPowerUps;

        private void OnEnable()
        {
            m_allPowerUps = Resources.LoadAll<PowerUpData>("PowerUps");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script", "m_BoosterModePowerUpConfigByLevel");

            EditorGUILayout.Space();
            DrawLevelConfigs((GameConfig)target);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawLevelConfigs(GameConfig config)
        {
            config.m_BoosterModePowerUpConfigByLevel ??= new List<LevelPowerUpConfig>();

            List<LevelPowerUpConfig> levels = config.m_BoosterModePowerUpConfigByLevel;

            EditorGUILayout.LabelField("BoosterMode PowerUps Per Level", EditorStyles.boldLabel);

            if (m_allPowerUps.Length == 0)
            {
                EditorGUILayout.HelpBox(
                    "No PowerUpData assets found in Resources/PowerUps.",
                    MessageType.Warning
                );
                return;
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Level"))
                AddLevel(config, levels);

            if (levels.Count > 0 && GUILayout.Button("Remove Last Level"))
                RemoveLastLevel(config, levels);

            if (levels.Count > 0 && GUILayout.Button("Clear All Levels"))
                ClearLevels(config, levels);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if (levels.Count == 0)
            {
                EditorGUILayout.HelpBox(
                    "No booster mode levels defined yet. Click 'Add Level' to create one.",
                    MessageType.Info
                );
                return;
            }

            for (int i = 0; i < levels.Count; i++)
            {
                LevelPowerUpConfig levelConfig = levels[i];
                levelConfig.m_EnabledPowerUps ??= new List<PowerUpData>();

                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Level " + i, EditorStyles.boldLabel);

                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    RemoveLevelAt(config, levels, i);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    break;
                }
                EditorGUILayout.EndHorizontal();

                DrawPowerUpToggles(config, levelConfig);

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
        }

        private void DrawPowerUpToggles(GameConfig config, LevelPowerUpConfig levelConfig)
        {
            EditorGUI.indentLevel++;

            List<PowerUpData> enabled = levelConfig.m_EnabledPowerUps;

            for (int i = 0; i < m_allPowerUps.Length; i++)
            {
                PowerUpData powerUp = m_allPowerUps[i];
                bool isEnabled = enabled.Contains(powerUp);
                bool newEnabled = EditorGUILayout.ToggleLeft(powerUp.name, isEnabled);

                if (newEnabled != isEnabled)
                {
                    Undo.RecordObject(config, "Toggle PowerUp");
                    if (newEnabled)
                        enabled.Add(powerUp);
                    else
                        enabled.Remove(powerUp);

                    EditorUtility.SetDirty(config);
                }
            }

            EditorGUI.indentLevel--;
        }

        private void AddLevel(GameConfig config, List<LevelPowerUpConfig> levels)
        {
            Undo.RecordObject(config, "Add Level");

            LevelPowerUpConfig newLevel = new LevelPowerUpConfig
            {
                m_EnabledPowerUps = new List<PowerUpData>(m_allPowerUps)
            };

            levels.Add(newLevel);
            EditorUtility.SetDirty(config);
        }

        private void RemoveLastLevel(GameConfig config, List<LevelPowerUpConfig> levels)
        {
            Undo.RecordObject(config, "Remove Last Level");
            levels.RemoveAt(levels.Count - 1);
            EditorUtility.SetDirty(config);
        }

        private void ClearLevels(GameConfig config, List<LevelPowerUpConfig> levels)
        {
            Undo.RecordObject(config, "Clear All Levels");
            levels.Clear();
            EditorUtility.SetDirty(config);
        }

        private void RemoveLevelAt(GameConfig config, List<LevelPowerUpConfig> levels, int index)
        {
            Undo.RecordObject(config, "Remove Level");
            levels.RemoveAt(index);
            EditorUtility.SetDirty(config);
        }
    }
}
#endif


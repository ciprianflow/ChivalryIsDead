using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(QuestParameters))]
public class QuestParametersEditor : Editor {

    #region Editor variables
    private const string dataFolderPath = @"Assets/Resources/data/";
    private const string assetFileName = "QuestParameters.asset";

    #region Target references
    private QuestParameters qParamTarget { get { return (target as QuestParameters); } }
    private int enemyThreshCount { get { return qParamTarget.enemyThresholds.Count; } }
    #endregion

    private static bool diffThreshIsToggled;

    private static bool diffDefsIsToggled;
    private static bool[] diffDefIsToggled = new bool[3];

    private static bool enemyThreshIsToggled;

    private static bool repOverrideIsToggled;
    #endregion

    void OnEnable()
    {
        try {
            AssetDatabase.LoadAssetAtPath<QuestParameters>(dataFolderPath + assetFileName);
        } catch (System.Exception ex) {
            Debug.LogWarning(string.Format("Exception '{0}' caught. Make sure asset exists at '{1}{2}'",
                ex.ToString(),
                dataFolderPath, assetFileName));
        }
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        Undo.RecordObject(this, "Changed QuestParameters");

        if (qParamTarget.difficultyThresholds == null)
            qParamTarget.difficultyThresholds = new int[2];

        if (qParamTarget.difficultyDefinitions == null)
            qParamTarget.difficultyDefinitions = new DifficultyDefinition[3];

        if (qParamTarget.enemyThresholds == null)
            qParamTarget.enemyThresholds = new List<EnemyThreshold>();

        /// Setting up Difficulty Thresholds
        diffThreshIsToggled = EditorGUILayout.Foldout(diffThreshIsToggled, "Difficulty Thresholds");
        if (diffThreshIsToggled) {
            EditorGUI.indentLevel++;
            DrawDifficultyThreshold("Easy Threshold:", 0);
            DrawDifficultyThreshold("Medium Threshold:", 1);
            EditorGUI.indentLevel--;
        }

        /// Setting up Difficulty Definitions
        diffDefsIsToggled = EditorGUILayout.Foldout(diffDefsIsToggled, "Difficulty Definitions");
        if (diffDefsIsToggled) {
            EditorGUI.indentLevel++;
            EditorGUILayout.HelpBox("All min/max values are inclusive.", MessageType.Info);
            DrawDifficultyDefinition(0);
            DrawDifficultyDefinition(1);
            DrawDifficultyDefinition(2);
            EditorGUI.indentLevel--;
        }

        /// Setting up Enemy Thresholds
        enemyThreshIsToggled = EditorGUILayout.Foldout(enemyThreshIsToggled, "Enemy Thresholds");
        if (enemyThreshIsToggled) {
            EditorGUI.indentLevel++;
            for (int i = 0; i < enemyThreshCount; i++) { // Draw all enemy thresholds
                DrawEnemyThreshold(i);
                EditorGUILayout.Space();
            }

            if (GUILayout.Button("Add Threshold"))
                qParamTarget.enemyThresholds.Add(new EnemyThreshold());
            EditorGUI.indentLevel--;
        }

        /// Setting up Reputation Overrides
        repOverrideIsToggled = EditorGUILayout.Foldout(repOverrideIsToggled, "Reputation Overrides");
        if (repOverrideIsToggled) {
            EditorGUI.indentLevel++;
            DrawReputationOverride(0, "Melee (Rep)");
            DrawReputationOverride(1, "Ranged (Rep)");
            DrawReputationOverride(2, "Suicide (Rep)");
            EditorGUI.indentLevel--;
        }

        EditorUtility.SetDirty(target);

#if UNITY_EDITOR
        EditorGUILayout.Separator();
        if (GUILayout.Button("Save Data")) {
            AssetDatabase.SaveAssets();
        }
#endif
    }

    #region Draw Functions
    private void DrawDifficultyThreshold(string label, int idx)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(label);
        qParamTarget.difficultyThresholds[idx] = EditorGUILayout.IntField(qParamTarget.difficultyThresholds[idx]);
        EditorGUILayout.EndHorizontal();
    }

    private void DrawDifficultyDefinition(int idx)
    {
        if (qParamTarget.difficultyDefinitions[idx] == null)
            qParamTarget.difficultyDefinitions[idx] = new DifficultyDefinition();

        var newDefinition = qParamTarget.difficultyDefinitions[idx];

        diffDefIsToggled[idx] = EditorGUILayout.Foldout(
            diffDefIsToggled[idx],
            string.Format("Difficulty Definition ({0})", DifficultyFromInt(idx))
            );

        if (diffDefIsToggled[idx]) {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Min/Max Non-suicide:");
            newDefinition.minNonSuicide = EditorGUILayout.IntField(newDefinition.minNonSuicide);
            newDefinition.maxNonSuicide = EditorGUILayout.IntField(newDefinition.maxNonSuicide);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Min/Max Suicide:");
            newDefinition.minSuicide = EditorGUILayout.IntField(newDefinition.minSuicide);
            newDefinition.maxSuicide = EditorGUILayout.IntField(newDefinition.maxSuicide);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Min/Max Friendlies (Obsolete):");
            newDefinition.minFriendlies = EditorGUILayout.IntField(newDefinition.minFriendlies);
            newDefinition.maxFriendlies = EditorGUILayout.IntField(newDefinition.maxFriendlies);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("House Status:");
            newDefinition.houseStatus = (HouseStatus)EditorGUILayout.EnumMaskPopup(new GUIContent(), newDefinition.houseStatus);
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
        }
    }

    private void DrawEnemyThreshold(int idx)
    {
        //EditorGUILayout.BeginVertical();
        // Activation Day
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Activation Day:");
        qParamTarget.enemyThresholds[idx].DayMarker = EditorGUILayout.IntField(qParamTarget.enemyThresholds[idx].DayMarker);
        EditorGUILayout.EndHorizontal();

        //// Activation Rep
        //EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.PrefixLabel("Activation Rep:");
        //qParamTarget.enemyThresholds[idx].RepMarker = EditorGUILayout.IntField(qParamTarget.enemyThresholds[idx].RepMarker);
        //EditorGUILayout.EndHorizontal();

        // Type and button
        qParamTarget.enemyThresholds[idx].AvailableEnemies = (EnemyTypes)EditorGUILayout.EnumMaskPopup(new GUIContent(), qParamTarget.enemyThresholds[idx].AvailableEnemies);
        EditorGUILayout.EndVertical();
        if (GUILayout.Button("-")) qParamTarget.enemyThresholds.RemoveAt(idx);
        EditorGUILayout.EndHorizontal();

        //EditorGUILayout.EndVertical();
    }

    private void DrawReputationOverride(int idx, string label)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(label);
        qParamTarget.reputationOverrides[idx].RepMarker = EditorGUILayout.IntField(qParamTarget.reputationOverrides[idx].RepMarker);
        EditorGUILayout.EndHorizontal();

        switch (idx) {
            case 0:
                qParamTarget.reputationOverrides[idx].enemyType = EnemyTypes.HasMelee; break;
            case 1:
                qParamTarget.reputationOverrides[idx].enemyType = EnemyTypes.HasRanged; break;
            case 2:
                qParamTarget.reputationOverrides[idx].enemyType = EnemyTypes.HasSuicide; break;
        }
        //qParamTarget.reputationOverrides[idx].enemyType = (EnemyTypes)EditorGUILayout.EnumPopup(qParamTarget.reputationOverrides[idx].enemyType);
    }
    #endregion

    private string DifficultyFromInt(int idx)
    {
        return System.Enum.GetName(typeof(Difficulty), (Difficulty)idx);
    }
}

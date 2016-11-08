using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text;

[CustomEditor(typeof(TextGeneration))]
public class TextGenerationEditor : Editor
{
    TextGeneration textGenTarget;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("Choose amount of sentenses and number of text files, above", MessageType.Info);
        EditorGUILayout.HelpBox("Choose in which text files each sentence belongs, below", MessageType.Info);
        textGenTarget = (TextGeneration)target;

        GenerateText();

        if (GUILayout.Button("Re-Generate Text"))
        {
            textGenTarget.ClearText();
            GenerateText();
            textGenTarget.initTextBags(textGenTarget.NewBagInitializer);
            //ReGenText();
        }
    }

    private void GenerateText()
    {
        if (textGenTarget.NewBagInitializer == null || textGenTarget.NewBagInitializer.Length != (textGenTarget.textSize * textGenTarget.numFiles))
            textGenTarget.NewBagInitializer = new bool[textGenTarget.textSize * textGenTarget.numFiles];

        // Setup Editor layout.
        EditorGUILayout.LabelField("Choose txts to generate text:");

        // labels on top with names of the files (appear on runtime)
        GUILayout.BeginHorizontal();

        //for (int i = 0; i < textGenTarget.filesNames.Count; i++)
        //{ 
        GUILayout.Label("Greetings", GUILayout.Width(45));
        GUILayout.Label("Pal", GUILayout.Width(45));
        GUILayout.Label("Killing", GUILayout.Width(45));
        GUILayout.Label("Number", GUILayout.Width(45));
        GUILayout.Label("Creature", GUILayout.Width(45));
        GUILayout.Label("State", GUILayout.Width(45));
        GUILayout.Label("Story", GUILayout.Width(45));


        //}
        GUILayout.EndHorizontal();

        for (int i = 0; i < textGenTarget.textSize; i++)
        {
           
            // Create row of toggle controls.
            EditorGUILayout.BeginHorizontal(GUILayout.MaxHeight(10), GUILayout.MaxWidth(7 * 12));
            GUILayout.BeginHorizontal();
                GUILayout.Label(i.ToString());
            GUILayout.EndHorizontal();

            for (int j = 0; j < textGenTarget.numFiles; j++)
            {
                EditorGUIUtility.fieldWidth = 64;
                textGenTarget.NewBagInitializer[TranslateVector(i, j, textGenTarget.textSize)] = EditorGUILayout.Toggle(textGenTarget.NewBagInitializer[TranslateVector(i, j, textGenTarget.textSize)]);
                GUILayout.Space(32);

            }
            EditorGUILayout.EndHorizontal();

        }
     
    }

    public void ReGenText()
    {
        textGenTarget.ClearText();
        GenerateText();
        textGenTarget.initTextBags(textGenTarget.NewBagInitializer);
    }

    private int TranslateVector(int x, int y, int sideLength)
    {
        return x + (y * sideLength);
    }
}
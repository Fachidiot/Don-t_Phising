// DialogueParser.cs (Editor ���� CSV �� SO ��ȯ��)
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;

/// <summary>
/// CSV ������ �о� DialogueEvent ScriptableObject�� ��ȯ�ϴ� ������ ��ƿ��Ƽ
/// </summary>
public class DialogueParser : MonoBehaviour
{
    [MenuItem("Tools/Import Dialogue To CSV")]
    public static void ImportCSV()
    {
        string path = EditorUtility.OpenFilePanel("CSV Import", "", "csv");
        if (string.IsNullOrEmpty(path)) return;

        string[] data = File.ReadAllLines(path);
        DialogueEvent dialogueEvent = ScriptableObject.CreateInstance<DialogueEvent>();
        dialogueEvent.lines = new List<Dialogue>();

        for (int i = 1; i < data.Length; i++)
        {
            var row = data[i].Split(',');

            Dialogue line = new Dialogue
            {
                id = int.Parse(row[0]),
                speaker = row[1],
                text = row[2],
                choices = row[3],
                nextId = int.TryParse(row[4], out var n) ? n : 0,
                tag = row[5]
            };

            dialogueEvent.lines.Add(line);
        }

        string assetPath = "Assets/Resources/DialogueEvents/NewEvent.asset";
        AssetDatabase.CreateAsset(dialogueEvent, assetPath);
        AssetDatabase.SaveAssets();

        Debug.Log("DialogueEvent ���� �Ϸ�: " + assetPath);
    }
}
#endif
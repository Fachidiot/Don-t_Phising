using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;

/// <summary>
/// CSV 파일을 읽어 DialogueEvent ScriptableObject로 변환하는 에디터 유틸리티
/// </summary>
public class DialogueParser : MonoBehaviour
{
    [MenuItem("Tools/Import Dialogue To CSV")]
    public static void ImportCSV()
    {
        string path = EditorUtility.OpenFilePanel("CSV Import", "", "csv");
        if (string.IsNullOrEmpty(path)) return;

        string[] data = File.ReadAllLines(path);
        if (data.Length <= 1)
        {
            Debug.LogError("CSV 파일이 비어 있거나 헤더만 있습니다.");
            return;
        }

        DialogueEvent dialogueEvent = ScriptableObject.CreateInstance<DialogueEvent>();
        dialogueEvent.lines = new List<Dialogue>();

        for (int i = 1; i < data.Length; i++) // 첫 줄은 헤더
        {
            if (string.IsNullOrWhiteSpace(data[i])) continue;

            var row = SplitCSVLine(data[i]);

            if (row.Length < 6)
            {
                Debug.LogWarning($" 줄 무시됨 (열 부족): {data[i]}");
                continue;
            }

            if (!int.TryParse(row[0].Trim(), out int id))
            {
                Debug.LogWarning($"잘못된 ID: '{row[0]}' → 건너뜀");
                continue;
            }

            int nextId = int.TryParse(row[4].Trim(), out int parsedNextId) ? parsedNextId : 0;

            Dialogue line = new Dialogue
            {
                id = id,
                speaker = row[1].Trim(),
                text = row[2].Trim(),
                choices = row[3].Trim(),
                nextId = nextId,
                tag = row[5].Trim()
            };

            dialogueEvent.lines.Add(line);
        }

        string fileName = Path.GetFileNameWithoutExtension(path);
        string assetPath = $"Assets/Resources/DialogueEvents/{fileName}_event.asset";

        Directory.CreateDirectory("Assets/Resources/DialogueEvents");

        AssetDatabase.CreateAsset(dialogueEvent, assetPath);
        AssetDatabase.SaveAssets();

        Debug.Log($" DialogueEvent 생성 완료: {assetPath}");
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = dialogueEvent;
    }

    /// <summary>
    /// 콤마 포함 문자열을 안전하게 파싱하는 유틸리티 (ex: "선택1:2,선택2:3")
    /// </summary>
    private static string[] SplitCSVLine(string line)
    {
        List<string> result = new();
        bool inQuotes = false;
        string current = "";

        foreach (char c in line)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(current);
                current = "";
            }
            else
            {
                current += c;
            }
        }

        result.Add(current);
        return result.ToArray();
    }
}
#endif

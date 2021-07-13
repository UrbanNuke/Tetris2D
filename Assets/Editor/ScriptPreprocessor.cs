//Assets/Editor/ScriptPreprocessor.cs
using UnityEditor;
using System.IO;

public class ScriptPreprocessor : UnityEditor.AssetModificationProcessor
{

    public static void OnWillCreateAsset(string metaFilePath)
    {
        string fileName = Path.GetFileNameWithoutExtension(metaFilePath);

        if (!fileName.EndsWith(".cs"))
            return;

        string actualFilePath = $"{Path.GetDirectoryName(metaFilePath)}{Path.DirectorySeparatorChar}{fileName}";

        string content = File.ReadAllText(actualFilePath);
        string newContent = content.Replace("#PROJECTNAME#", PlayerSettings.productName);
        newContent = newContent.Replace("#CREATIONDATE#", System.DateTime.Now + "");
        newContent = newContent.Replace("#COMPANYNAME#", PlayerSettings.companyName);
        if (content == newContent) 
            return;
        
        File.WriteAllText(actualFilePath, newContent);
        AssetDatabase.Refresh();
    }
}
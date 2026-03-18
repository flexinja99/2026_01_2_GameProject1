#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor.UIElements;



public class JsonToScriptableConverter : EditorWindow
{

    private string jsonFilePaht = "";
    private string outputFolder = "Assests/ScriptableObjects/Items";
    private bool createDataBase = true;

    [MenuItem("Tools/JSON to Scriptable Objects")]
    public static void ShowWindow()
    {
        GetWindow<JsonToScriptableConverter>("JSON to Scriptable Objects");
    }

    private void OnGUI()
    {
        GUILayout.Label("JSON to Scriptable object Converter", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if(GUILayout.Button("Select JSON File"))
        {
            jsonFilePaht = EditorUtility.OpenFilePanel("Select JSON File", "", "json");
        }

        EditorGUILayout.LabelField("Selected File : ", jsonFilePaht);
        EditorGUILayout.Space();
        outputFolder = EditorGUILayout.TextField("Output Folder : ", outputFolder);
        createDataBase = EditorGUILayout.Toggle("Create Database Asset", createDataBase);
        EditorGUILayout.Space();

        if(GUILayout.Button("Convert to Scriptable Objects"))
        {
            if (string.IsNullOrEmpty(jsonFilePaht))
            {
                EditorUtility.DisplayDialog("Error", "Pease Select a JSON file first", "OK");
                return;
            }
            ConvertJsonToScriptableObject();
        }
    }

    private void ConvertJsonToScriptableObject()
    {
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        string jasonText = File.ReadAllText(jsonFilePaht);

        try
        {
            List<ItemData> itemDataList = JsonConvert.DeserializeObject<List<ItemData>>(jasonText);

            List<ItemSo> createdItems = new List<ItemSo>();

            foreach (ItemData itemData in itemDataList)
            {
                ItemSo itemSO = ScriptableObject.CreateInstance<ItemSo>();

                itemSO.id = itemData. id;
                itemSO.itemName = itemData.itemName;
                itemSO.nameEng = itemData.nameEng;
                itemSO.description = itemData.description;


                if (System.Enum.TryParse(itemData.itemTypeString, out ItemType parsedType))
                {
                    itemSO.itemType = parsedType;
                }
                else
                {
                    Debug.LogWarning($"아이템{itemData.itemName}의 유효하지 않는 타입 : {itemData.itemTypeString}");
                }

                itemSO.price = itemData.price;
                itemSO.power = itemData.power;
                itemSO.level = itemData.level;
                itemSO.isStackable = itemData.isStackable;

                //데이터 배이스
                if(createDataBase && createdItems.Count > 0)
                {
                    ItemDataBaseSO dataBase = ScriptableObject.CreateInstance<ItemDataBaseSO>();   // 생성
                    dataBase.items = createdItems;

                    AssetDatabase.CreateAsset(dataBase, $"{outputFolder}/ItemDatabase.asset");
                    EditorUtility.SetDirty(dataBase);
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                EditorUtility.DisplayDialog("Sucess", $"Created{createdItems.Count} scriptable object!", "OK");

            }
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Error", $"Failed to Convert.JSON : {e .Message}", "OK");
        }
    }

   
}

#endif





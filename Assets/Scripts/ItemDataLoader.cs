using UnityEngine;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;


public class ItemDataLoader : MonoBehaviour
{

    [SerializeField]
    private string jasonFileName = "items"; // 리소스 폴더에서 가져온 제이슨 파일

    private List<ItemData> itemList;

    private void Start()
    {
        LoadItemData();
    }


    // 한글 인코딩을 위한 핼퍼 함수
    private string EncodeKorean(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";
        byte[] bytes = Encoding.Default.GetBytes(text);
        return Encoding.UTF8.GetString(bytes);
    }

   void LoadItemData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jasonFileName);

        if(jsonFile != null)
        {
            byte[] bytes = Encoding.Default.GetBytes(jsonFile.text);
            string currnetText = Encoding.UTF8.GetString(bytes);

            itemList = JsonConvert.DeserializeObject<List<ItemData>>(currnetText);

            Debug.Log($"로드된 아이템 수 : {itemList.Count}");

            foreach(var item in itemList)
            {
                Debug.Log($"아이템 : {EncodeKorean(item.itemName)},설명:{EncodeKorean(item.description)}");
            }
            


        }
        else
        {
            Debug.LogError($"JSON 파일을 찾을 수 없습니다. : {jasonFileName}");
        }
            
        
    }
}

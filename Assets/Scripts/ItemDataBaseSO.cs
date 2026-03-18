using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBaseSO", menuName = "Inventory/DataBase")]
public class ItemDataBaseSO : ScriptableObject
{
    public List<ItemSo> items = new List<ItemSo>();          // itemso 를 리스트로 관리한다

    // 캐싱을 위한 딕셔어리
    private Dictionary<int, ItemSo> itemsByld;                                //id로 아이템 찾기위한 캐싱
    private Dictionary<string, ItemSo> itemsByName;                           //이름으로 아이템 찾기

    public void Initialze()
    {
        itemsByld = new Dictionary<int, ItemSo>();
        itemsByName = new Dictionary<string, ItemSo>();

        foreach(var item in items)
        {
            itemsByld[item.id] = item;
            itemsByName[item.itemName]  = item;

        }
    }

    //id로 아이템 찾을때

    public ItemSo GetItemByld(int id)
    {
        if (itemsByld == null)
        {
            Initialze();
        }
        if(itemsByld.TryGetValue(id, out var item))           //id 값을 찾아서 itemso 를 리턴한다
            return item;

        return null;                                            // 없을 경유 null

        //이름으로 아이템 찾기

        
    }

    public ItemSo GetItemByName(string name) 
    {
        if (itemsByName == null)
        {
            Initialze();                                            //캐싱이 되어있는 확인하고 아니면 초기화한다 
        }                                              
        if (itemsByName.TryGetValue(name, out var item))                   // null값을 찾아서 itemso를 리턴한다          

            return item;

        return null;

    }

    public List<ItemSo>GetItemsType(ItemType type)
    {
        return items.FindAll(item => item.itemType == type);
    }
}

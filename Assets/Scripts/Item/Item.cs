using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Item {

    //===============================================================================
    //ITEM
    public string name;
    public string icon;
    public ItemType type;
    //[SerializeField]int _count;
    public int count;
    public int maxCount;

    /*public int count
    {
        get { return _count; }
        set { _count = Mathf.Clamp(value, 0, maxCount); }
    }*/




    public Item() { }

    public Item(string name, string icon, ItemType type = ItemType.Other, int count=1, int maxCount=1)
    {
        this.name = name;
        this.icon = icon;
        this.type = type;
        this.maxCount = maxCount;
        this.count = count;
    }


    public enum ItemType
    {
        Other = 0,
        Weapon = 1,
        Clothes = 2,
    }


    //===============================================================================
    //CLOTHES
    public string spriteAtlasPath;
    public ClothesType clothesType;
    public Color color;

    public static Item Clothes(string name, string icon, ClothesType clothesType, Color color, string spriteAtlasPath = "")
    {
        Item i = new Item(name, icon, ItemType.Clothes);
        i.clothesType = clothesType;
        i.color = color;
        i.spriteAtlasPath = spriteAtlasPath;
        return i;
    }

    public enum ClothesType
    {
        Head,
        Torso,
        Pants
    }



    //===============================================================================
    //WEAPON
    public float fireRate;


    public static Item Weapon(string name, string icon, float fireRate=0.2f)
    {
        Item i = new Item(name, icon, ItemType.Weapon);
        i.fireRate = fireRate;
        return i;
    }





    //===============================================================================

    public static Item Copy(Item item){
        if (item == null) return null;
        Item copy = new Item(item.name, item.icon, item.type, item.count, item.maxCount);

        copy.spriteAtlasPath = item.spriteAtlasPath;
        copy.clothesType = item.clothesType;
        copy.color = item.color;

        copy.fireRate = item.fireRate;
        return copy;
    }
}




public class SyncListItem : SyncList<Item>
{
    protected override Item DeserializeItem(NetworkReader reader)
    {
        if (reader.Position >= reader.Length) return null;

        Item i = new Item();
        i.name = reader.ReadString();
        i.icon = reader.ReadString();
        i.type = (Item.ItemType)reader.ReadInt32();
        i.count = reader.ReadInt32();
        i.maxCount = reader.ReadInt32();

        i.spriteAtlasPath = reader.ReadString();
        i.clothesType = (Item.ClothesType)reader.ReadInt32();
        i.color = reader.ReadColor();

        i.fireRate = (float)reader.ReadDouble();
        return i;
    }

    protected override void SerializeItem(NetworkWriter writer, Item item)
    {
        if (item == null) {
            writer.FinishMessage();
            return;
        }

        //Debug.Log(item.name + "   write " + item.count);

        writer.Write(item.name);
        writer.Write(item.icon);
        writer.Write((int)item.type);
        writer.Write(item.count);
        writer.Write(item.maxCount);

        writer.Write(item.spriteAtlasPath);
        writer.Write((int)item.clothesType);
        writer.Write(item.color);

        writer.Write((double)item.fireRate);
        writer.FinishMessage();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsLib : MonoBehaviour {

    public static ItemsLib i;

    public Item[] items;

    public Sprite[] icons;
	
	void Awake () {
        i = this;
	}
	
	
	public Sprite GetIcon(string name) {
        for(int i=0; i< icons.Length; i++)
        {
            if (icons[i].name == name) return icons[i];
        }
        return null;
	}


    public static Item GetItem(string name){
        if(i != null)
            foreach(Item item in i.items){
                if (item.name == name)
                {
                    return item;
                }
            }
        return null;
    }

    public static Item GetItemCopy(string name, int count=-1){
        Item i = Item.Copy(GetItem(name));
        if (i == null) return null;
        if(count>=0)
            i.count = count;
        return i;
    }
}

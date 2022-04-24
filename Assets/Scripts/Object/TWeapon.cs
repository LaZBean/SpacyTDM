using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TWeapon : TPickable {

    public Item item;


	
	void Start () {
		
	}
	
	
	void Update () {
        renderer.transform.eulerAngles = new Vector3(0, 0, (Time.time * 180f) % 360);

    }



    public override bool OnEntityPicked(Human h)
    {
        for(int i=0; i<h.items.Count; i++){
            if(h.items[i].name == item.name){

                if (h.items[i].count >= h.items[i].maxCount) return false;

                Item w = Item.Copy(h.items[i]);
                w.count += item.count;
                h.items[i] = w;

                h.CmdChangeItem(i);
                break;
            }
        }
        
        return true;
    }
}

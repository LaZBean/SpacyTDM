using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TPickable : TObject {

    public new SpriteRenderer renderer;

    void Start () {
		
	}
	
	
	void Update () {
		
	}




    void OnTriggerEnter2D(Collider2D col)
    {
        if (!isServer) return;

        Human h = col.GetComponent<Human>();
        if (h)
        {

            if(OnEntityPicked(h))
                Destroy(gameObject);
        }
    }

    public virtual bool OnEntityPicked(Human h)
    {
        return true;
    }
}

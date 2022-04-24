using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMedkit : TPickable {

    public int restore = 50;



	void Start () {
		
	}

    void Update()
    {
        //renderer.transform.eulerAngles = new Vector3(0, 0, (Time.time * 180f) % 360);
        renderer.transform.localScale = Vector3.one * (Mathf.Sin(Time.time * 10) * 0.2f + 0.8f);
    }


    public override bool OnEntityPicked(Human h)
    {
        if (h.health >= h.maxHealth)
            return false;
        h.health += restore;
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TShield : TPickable {


    public int restore = 50;



    void Start()
    {

    }

    void Update()
    {
        Vector3 p = new Vector3(Mathf.Sin(Time.time*10)*0.01f, Mathf.Cos(Time.time * 10) * 0.01f, 1);
        renderer.transform.localPosition = p;
    }


    public override bool OnEntityPicked(Human h)
    {
        if (h.shield >= h.maxShield)
            return false;
        h.shield += restore;
        return true;
    }
}

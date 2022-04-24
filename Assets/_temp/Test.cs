using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public int curEntity;
    public Entity[] entities;

	void Start () {
		
	}
	
	
	void Update () {
		
	}

    public void NextEntity()
    {
        curEntity = ++curEntity % entities.Length;
        ControlManager.i.entity = entities[curEntity];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SimpleBillboard : MonoBehaviour {

    public bool faceToCamera = false;

    public bool freezeX = true, freezeY, freezeZ = true;

    void Start () {
		
	}
	
	
	void Update () {
        if (Camera.main == null) return;

        transform.forward = Camera.main.transform.forward * ((faceToCamera)? -1 : 1);
        if(freezeX || freezeY || freezeZ)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x * ((freezeX) ? 0 : 1), transform.eulerAngles.y * ((freezeY) ? 0 : 1), transform.eulerAngles.z * ((freezeZ)? 0 : 1));
	}
}

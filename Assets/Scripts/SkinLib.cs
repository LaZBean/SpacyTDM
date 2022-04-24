using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinLib : MonoBehaviour {

	public static SkinLib i;

    public Sprite[] skinAvatars;
    public Sprite[] skinIcons;
    public string[] skinColors;

	void Awake () {
        i = this;
	}
	
	
	void Update () {
		
	}
}

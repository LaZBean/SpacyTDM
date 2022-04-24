using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscLib : MonoBehaviour {

    public static MiscLib i;

    public CursorPreset[] cursorPresets;
	
	void Awake () {
        i = this;
	}
	
	
	public CursorPreset GetCursorPreset(int id) {
        return cursorPresets[id];
    }
}


[System.Serializable]
public class CursorPreset
{
    public Sprite normal;
    public Sprite hover;
    public Sprite active;

    public CursorPreset() { }
}

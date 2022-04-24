#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShaderTest : MonoBehaviour {

	private SpriteRenderer _sr;


	void Start () {
		if(_sr==null) _sr = GetComponent<SpriteRenderer> ();


		for (int i = 0; i < ShaderUtil.GetPropertyCount(_sr.material.shader); i++) {
			string s = ShaderUtil.GetPropertyName (_sr.material.shader, i);
			print (s);
		}
			



	}
	

	void Update () {
		
	}
}

#endif

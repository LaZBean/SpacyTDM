using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualizer : MonoBehaviour {

	public AudioSource source;

	public GameObject prefab;
	private GameObject[] columns;

	public int number = 32;
	public float radius = 2f;
	public float height = 40f;

	public Gradient color = new Gradient(){ colorKeys = new GradientColorKey[2] {new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.white, 1f)} };
	public Vector3 pivot = new Vector3(0, 0.5f, 0);


	void Awake(){
		if(source == null)
			source = GetComponent<AudioSource> ();
	}

	void Start () {
		columns = new GameObject[number];

		for (int i = 0; i < number; i++) {
			float angle = ((i * Mathf.PI * 2)/ number);
			Vector3 pos = new Vector3 (Mathf.Cos (angle - Mathf.Deg2Rad * transform.eulerAngles.y), 0, Mathf.Sin (angle - Mathf.Deg2Rad * transform.eulerAngles.y)) * radius;
			GameObject g = (GameObject)Instantiate (prefab, pos + transform.position, Quaternion.identity);

			g.transform.parent = this.transform;

			columns [i] = g;
		}

		ApplyPivot ();
		//ApplyColor ();
	}


	/*void ApplyColor(){
		for (int i = 0; i < number; i++) {
			MeshRenderer mr = columns[i].GetComponent<MeshRenderer> ();
			mr.material.SetColor ("_Color", color);
			mr.material.SetColor ("_EmissionColor", color);
		}
	}*/



	void ApplyPivot(){
		for (int i = 0; i < number; i++) {
			MeshFilter mf = columns[i].GetComponent<MeshFilter> ();

			Mesh mesh = new Mesh ();
			mesh = mf.mesh;
			Vector3[] vs = new Vector3[mesh.vertices.Length];

			for (int n = 0; n < mesh.vertices.Length; n++) {
				vs[n] = pivot + mesh.vertices [n];
			}
			mesh.vertices = vs;
			mf.mesh = mesh;
		}
	}
	

	void Update () {
		float[] spectrum = source.GetSpectrumData (1024, 0, FFTWindow.Triangle);	//AudioListener.GetSpectrumData (1024, 0, FFTWindow.Hamming);

		for (int i = 0; i < number; i++) {
			Vector3 prevScale = columns [i].transform.localScale;
			prevScale.y = Mathf.Lerp (prevScale.y, spectrum [i] * height, Time.deltaTime * 30);

			SetColor (columns [i], i);

			columns [i].transform.localScale = prevScale;
		}
	}


	void SetColor(GameObject obj, float s){

		s = s * (1f / number);
		MeshRenderer mr = obj.GetComponent<MeshRenderer> ();

		Color c = color.Evaluate (s);

		MaterialPropertyBlock mpb = new MaterialPropertyBlock ();
		mpb.SetColor ("_Color", c);
		mpb.SetColor ("_EmissionColor", c);
		mpb.SetFloat ("_Emission",10);

		mr.SetPropertyBlock (mpb);
	}
}

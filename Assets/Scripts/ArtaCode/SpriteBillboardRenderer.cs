using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[System.Serializable]
public class SpriteBillboardRenderer : MonoBehaviour {

	public bool show = true;


	[SerializeField]private Sprite _sprite;
	[SerializeField]private Color _color = Color.white;
	[SerializeField]private bool _lookAtCamera;
	[SerializeField]private Material _material;
	[SerializeField]private float _pixelPerUnit = 32f;


	public float angle;
	[SerializeField]public bool doNotRotate = true;
	[SerializeField]public bool castShadow = false;

	[SerializeField]public float zOffset = 0;
	[SerializeField]public Vector2 xyOffset;

	[SerializeField]private Mesh _mesh;

	private MaterialPropertyBlock block;

	void Start () {
		CreateMesh();
	}
	

	void Update () {
		if(_lookAtCamera)
			
		AngleToCamera ();

		if(_mesh == null || block == null || _material == null)   CreateMesh ();
		else   DrawBillboard ();
	}



	 
		

	void AngleToCamera(){
		if (Camera.main != null) {

			Transform t = (transform.parent!=null)? transform.parent : transform;

			Vector3 direction = (Camera.main.transform.position - t.transform.position).normalized;
			angle = Mathf.Atan2 (direction.x, direction.z) * Mathf.Rad2Deg;

			angle -= (t.transform.eulerAngles.y);

			if (angle < 0)
				angle += 360;	
		}
	}





	void CreateMesh(){
		if (_sprite == null)
			return;

		float w = ((1/_pixelPerUnit) * _sprite.rect.width);
		float h = ((1/_pixelPerUnit) * _sprite.rect.height);

		Vector2 pv = new Vector2((1/_sprite.rect.width) * _sprite.pivot.x, (1/_sprite.rect.height) * _sprite.pivot.y);

		float wp = (1/_pixelPerUnit) * (_sprite.rect.width * pv.x);
		float hp = (1/_pixelPerUnit) * (_sprite.rect.height * pv.y);
		
		_mesh = new Mesh ();
		_mesh.vertices = new Vector3[] {
			new Vector3 (0 -wp, 0 -hp, 0),
			new Vector3 (w -wp, 0 -hp, 0),
			new Vector3 (w -wp, h -hp, 0),
			new Vector3 (0 -wp, h -hp, 0)
		};
		_mesh.uv = new Vector2[] {
			new Vector2(0,0),
			new Vector2(1,0),
			new Vector2(1,1),
			new Vector2(0,1)
		};
		_mesh.triangles = new int[] {
			3, 1, 0,
			1, 3, 2
        };

		//_mesh.RecalculateBounds ();
		//_mesh.RecalculateNormals ();


		Vector2 offs = new Vector2 (1f / _sprite.texture.width * _sprite.rect.x, 1f / _sprite.texture.height * _sprite.rect.y);
		Vector2 scale = new Vector2 (1f/_sprite.texture.width * _sprite.rect.width, 1f/_sprite.texture.height * _sprite.rect.height);

		block = new MaterialPropertyBlock();
		block.SetTexture("_MainTex", _sprite.texture);
		block.SetVector("_MainTex_ST", new Vector4(scale.x, scale.y, offs.x, offs.y));
		block.SetColor("_Color", _color);
	}



	void DrawBillboard(){
		if (!show)	return;

		Camera cam = Camera.main;
		if (cam != null) {



			

			if (lookAtCamera) {
				Quaternion r = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (cam.transform.position - transform.position), 1);
				rot = Quaternion.Euler (0, r.eulerAngles.y + 180f, 0);
				if(!doNotRotate)
					rot = Quaternion.Euler (rot.x + transform.eulerAngles.x, rot.y + transform.eulerAngles.y, rot.z + transform.eulerAngles.z);
			} else {
				rot = transform.rotation;
				rot.y += 180f;
			}

			Vector3 dir = (transform.position - cam.transform.position).normalized;

			pos = transform.position + (dir * zOffset);

            

			Graphics.DrawMesh (_mesh, pos, rot, _material, 0, null, 0, block, (castShadow)? UnityEngine.Rendering.ShadowCastingMode.TwoSided : UnityEngine.Rendering.ShadowCastingMode.Off);

		}
	}


	Quaternion rot;
	Vector3 pos;


	public Sprite sprite{
		get{return _sprite;}
		set{
			if (_sprite != value) {
				_sprite = value; 
				CreateMesh ();
			}
		}
	}

	public Color color{
		get{return _color;}
		set{
			if (_color != value) {
				_color = value; 
				CreateMesh ();
			}
		}
	}

	public bool lookAtCamera{
		get{return _lookAtCamera;}
		set{
			if (_lookAtCamera != value) {
				_lookAtCamera = value; 
			}
		}
	}

	public Material material{
		get{return _material;}
		set{
			_material = value;
			CreateMesh ();
		}
	}

	public float pixelPerUnit{
		get{return _pixelPerUnit;}
		set{
			if (_pixelPerUnit != value) {
				_pixelPerUnit = value; 
				CreateMesh ();
			}
		}
	}




	void OnDrawGizmos(){
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position, 0.025f);
	}

	void OnDrawGizmosSelected(){
		
		//Gizmos.color = Color.yellow;
		//Gizmos.DrawSphere (transform.position, 0.1f);

		Gizmos.DrawWireMesh (_mesh);
	}
}

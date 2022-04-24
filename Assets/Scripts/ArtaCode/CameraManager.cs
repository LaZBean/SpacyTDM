using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	//============================(VARIABLES)====================================//
	public static CameraManager i;
	public Transform camera;
	public Mode mode;


	public bool freeze = false;
	public float flySpeed = 2f;
    public int roundPosToN = 0;

	//ENUM MODE
	public enum Mode{
		None = 0,
		FlyFree = 1,
		FlyAround = 2,
		Fly2D = 3,
		FlyIsometric = 4,
		Follow = 5,
	}

	[Header("[Fly Around]")]
	public Transform target; 
	public bool flyAroundMouseControl = true;
	public float distanceToAroundPoint = 5f;

	[Header("[Fly 2D]")]
	public bool local = false;
	public Axis axis = Axis.XY;

	[Header("[Follow]")]
	[Range(0,1)]public float followTime = 0.2f;

    [Header("[ZOOM]")]
    public float zoomSpeed = 10f;


    public enum Axis{
		XY,
		XZ
	}



	private GameObject _parent;
	private float rotationY = 0;
	private float rotationX = 0;

	public GameObject parent{
		get{return _parent;}
		set{_parent = value; UpdateParent();}
	}
	//===========================================================================//

	//UPDATE PARENT
	public void UpdateParent(){
		if(_parent!=null){
			camera.transform.parent = _parent.transform;
			camera.transform.localPosition = Vector3.zero;
			camera.transform.rotation = _parent.transform.rotation;
		}else{
			camera.transform.parent = null;
		}
	}








	//AWAKE
	void Awake () {
		if(i==null) i = this;
		if(i!=this) Destroy(gameObject);

		if(camera==null)
			camera = GetComponentInChildren<Camera>().transform;
	}

	//START
	void Start(){
		
	}

	//UPDATE
	void Update () {
		if(i==null) i=this;

		if (freeze) return;


		switch(mode){

		case Mode.FlyFree : FreeFly(); break;
		case Mode.FlyAround : FlyAround(); break;
		case Mode.Fly2D : Fly2D(); break;
		case Mode.FlyIsometric : FlyIsometric(); break;
		case Mode.Follow : Follow(); break;
			
		default : break;
		
		}

        if(roundPosToN > 0)
        {
            camera.transform.localPosition = GameUtility.RoundToXDVector3(camera.transform.localPosition, 2);
        }
	}








	//1. CameraFreeFly
	void FreeFly(){
		if(parent==null){

			//if(UI_Manager.i!=null) if(UI_Manager.i.ShowAnyUI) return; 

			Vector3 dir = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
			dir = camera.transform.TransformDirection(dir);
			camera.transform.position += dir * flySpeed * Time.deltaTime;

			float rotCameraX = 0;
			float rotCameraY = 0;


			rotCameraX = Input.GetAxisRaw("Mouse X") * 6f;
			camera.transform.Rotate(0, rotCameraX, 0);

			rotCameraY = Input.GetAxisRaw("Mouse Y") * 6f;
			rotationY += rotCameraY;
			rotationY = Mathf.Clamp (rotationY, -85f, 85f);
			camera.transform.localEulerAngles = new Vector3(-rotationY, camera.transform.localEulerAngles.y, 0);


		}
	}


	//2. FlyAround
	void FlyAround(){
		if(target==null) return;

		distanceToAroundPoint += Input.GetAxisRaw ("Mouse ScrollWheel") * -zoomSpeed;

		float mouseX = (Input.GetMouseButton(1))?  Input.GetAxis("Mouse X") : 0;
		float mouseY = (Input.GetMouseButton(1))? -Input.GetAxis("Mouse Y") : 0;

		rotationX += mouseX * 6f;
		rotationY += mouseY * 6f;

		rotationY = Mathf.Clamp (rotationY, -85, 85);

		Vector3 dir = new Vector3 (0, 0, -distanceToAroundPoint);
		Quaternion rot = Quaternion.Euler (rotationY, rotationX, 0);
		Vector3 offset = target.up * 1.0f;
		camera.transform.position = target.position + rot * dir;
		camera.transform.LookAt (target.position + offset);
	}

	//3. Fly2D
	void Fly2D(){
		Vector2 mDir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		mDir = GameUtility.DirNormalize (mDir);

		Vector3 dir = Vector3.zero;

		switch (axis) {
			case Axis.XY:	dir = new Vector3 (1, 1, 0);	break;
			case Axis.XZ:	dir = new Vector3 (1, 0, 1);	break;
			
			default :	break;
		}

		dir = new Vector3 (dir.x * mDir.x, dir.y * mDir.y, dir.z * mDir.y);

		if (local) {
			camera.transform.localPosition += dir * flySpeed * Time.deltaTime;
		} else {
			camera.transform.position += dir * flySpeed * Time.deltaTime;
		}


		camera.transform.position = GameUtility.RoundToXDVector3 (camera.transform.position, 2);
	}


	//4. FlyIsometric
	void FlyIsometric(){
		Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0,  Input.GetAxis("Vertical"));
		dir = GameUtility.DirNormalize (dir);

		dir = camera.transform.parent.TransformDirection (dir);
		dir.y = 0;

		camera.transform.position += dir * flySpeed * Time.deltaTime;


		float zoom = Input.GetAxis ("Mouse ScrollWheel");
		camera.transform.localPosition = new Vector3(0,0,zoom * zoomSpeed) + camera.transform.localPosition;
	}




	//5. Follow
	void Follow(){
		if(target==null) return;

		camera.transform.position = Vector3.Lerp (camera.transform.position, new Vector3(target.position.x, target.position.y, camera.transform.position.z), followTime);
	}









	public void FollowByTime(float t){
		if(target==null) return;
		camera.transform.position = Vector3.Lerp(camera.transform.position, target.position, t);
		camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, target.transform.rotation, t);
	}

	//RAYCAST OPTIONAL
	public RaycastHit GetHitFromCenter(float distance){
		RaycastHit hit;
		Ray ray = new Ray(i.camera.transform.position, i.camera.transform.forward);
		if(Physics.Raycast(ray,out hit,distance)){
		}
		return hit;
	}
	public RaycastHit GetHitFromCenter(float distance, LayerMask mask){
		RaycastHit hit;
		Ray ray = new Ray(i.camera.transform.position, i.camera.transform.forward);
		if(Physics.Raycast(ray,out hit,distance,mask)){
		}
		return hit;
	}

	public RaycastHit GetHitFromMouse(float distance){
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray,out hit,distance)){
		}
		return hit;
	}
}

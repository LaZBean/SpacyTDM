using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour {

    public static ControlManager i;

    public Entity entity;

    public bool smoothInput = true;


    void Awake()
    {
        i = this;
    }

	void Start () {
		
	}
	
	
	void Update () {
        if (entity == null) return;

        entity.inputDir = (!smoothInput)? new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0) : new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        entity.aimPos = GameUtility.MouseWorldPosOnPlane(Camera.main, Vector3.forward, Input.mousePosition);

        Human h = entity as Human;
        if(h != null)
        {
            h.isRunning = Input.GetKey(KeyCode.LeftShift);

            if (Input.GetMouseButton(0))
            {
                h.Attack();
            }
        }



        CameraManager.i.target = entity.transform;
    }






    public void SetSmoothInput(){
        smoothInput = !smoothInput;
    }
}

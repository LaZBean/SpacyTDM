using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Starship : MonoBehaviour {

    [Header("[Starship]")]
    public int health = 1000;

    [Header("* Animation")]
    public Transform body;
    public AnimationCurve bodyRotate;
    public float maxAngle = 10f;
    public float rotateScale = 1f;

    float rotateTimer;

	void Start () {
		
	}
	
	
	void Update () {
        body.localEulerAngles = new Vector3(body.localEulerAngles.x, bodyRotate.Evaluate(rotateTimer * rotateScale) * maxAngle, body.localEulerAngles.z);

        rotateTimer = (rotateTimer + Time.deltaTime) % (1f / rotateScale);

    }
}

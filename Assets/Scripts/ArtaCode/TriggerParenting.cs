using UnityEngine;
using System.Collections;

public class TriggerParenting : MonoBehaviour {

	public Transform parent;

	void OnTriggerEnter(Collider coll){
		Rigidbody rb = coll.GetComponent<Rigidbody>();
		if(rb!=null){
			rb.transform.parent = (parent==null)? this.transform : parent;
		}
	}

	void OnTriggerExit(Collider coll){
		Rigidbody rb = coll.GetComponent<Rigidbody>();
		if(rb!=null){
			rb.transform.parent = null;
		}
	}

	void OnDestroy(){
		Rigidbody[] rbs = gameObject.GetComponents<Rigidbody>();
		for(int i=0; i<rbs.Length; i++){
			rbs[i].transform.parent = null;
		}
	}
}

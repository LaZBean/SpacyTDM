using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Explosion : NetworkBehaviour {

    public GameObject sender;

    public int minDamage = 90;
    public int maxDamage = 100;

    public float radius = 0.16f;
	
	void Start () {
        Destroy(gameObject, 1f);

        if (!isServer) return;
        Explode();
	}
	
	
	void Explode () {
        RaycastHit2D[] hits;
        hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.up);
        
        for (int i=0; i<hits.Length; i++)
        {
            //if (sender != null && sender.GetComponent<Collider2D>() == hits[i].collider) continue;
            
            Human h = hits[i].collider.GetComponent<Human>();
            if (h != null)
            {
                
                float dist = (h.transform.position - transform.position).magnitude;
                int totalDmg = (int)(Random.Range(minDamage, maxDamage) * (1f-(dist/(radius))));
                
                h.Damage(Mathf.Clamp(totalDmg, 0, maxDamage), sender);
            }
        }

        
	}
}

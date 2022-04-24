using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TTrap : TObject {

    public new SpriteRenderer renderer;
    public new Collider2D collider;
    public SpriteGridAnimator animator;

    public float timeToActivate = 1f;
    public float timeInActiveMode = 0.5f;
    
    public int minDamage = 50;
    public int maxDamage = 80;

    [SyncVar]public bool isActivate = false;
    float activateTimer;




    void Start () {
        activateTimer = (isActivate) ? timeInActiveMode : timeToActivate;
    }
	
	
	void Update () {
        activateTimer -= Time.deltaTime;
		if(activateTimer <= 0)
        {
            isActivate = !isActivate;
            activateTimer = (isActivate) ? timeInActiveMode : timeToActivate;
        }


        collider.enabled = isActivate;

        animator.SetCurrentAnimation((isActivate) ? "active" : "default");
    }




    void OnTriggerEnter2D(Collider2D col)
    {
        if (!isServer) return;

        Human h = col.GetComponent<Human>();
        if (h)
        {
            h.Damage(Random.Range(minDamage, maxDamage));
        }
    }
}

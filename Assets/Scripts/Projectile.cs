using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour {

    [SyncVar]public GameObject sender;

    public int minDamage = 5;
    public int maxDamage = 15;

    public float speed = 3f;

    public float visibleIn = 0.05f;
    public float lifeTime = 5f;

    public bool destroyControl = false;

    public new Rigidbody2D rigidbody;
    public SpriteRenderer[] renderers;
    public new BoxCollider2D collider;
    public new AudioSource audio;

    float visibleTimer;
    bool isVisible = false;

    float lifeTimer;

    public LayerMask mask;
    Vector2 lastPos;

    [Header("{Hit Options}")]
    public GameObject[] hitEffects;


    void Awake()
    {
        SetVisibility(false);
    }

    void Start () {

        lifeTimer = lifeTime;

        rigidbody.velocity = (transform.up) * speed;

        lastPos = transform.position;
    }


    void SetVisibility(bool v){
        foreach(SpriteRenderer renderer in renderers)
        {
            renderer.enabled = v;
        }
    }


    public void SetDestroyTime(Vector2 pos)
    {
        if (!destroyControl) return;
        float d = (pos - new Vector2(transform.position.x, transform.position.y)).magnitude;
        float t = d / speed;
        lifeTime = t;
    }

	
	
	void Update () {
        visibleTimer += Time.deltaTime;
        if(visibleTimer >= visibleIn && !isVisible)
        {
            SetVisibility(true);
            isVisible = true;
        }


        if (rigidbody.simulated)
        {
            RaycastHit2D hit = Physics2D.Linecast(lastPos, transform.position, mask);

            if (hit)
            {
                if (hit.collider && (sender != null && hit.collider != sender.GetComponent<Collider2D>()))
                {

                    Destruct(hit.collider, hit.point, hit.normal);
                }
            }
            lastPos = transform.position;
        }


        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            Destruct(null, transform.position, transform.up);
        }
    }




    void Destruct(Collider2D col, Vector2 point, Vector2 normal)
    {
        rigidbody.simulated = false;
        SetVisibility(false);

        if (!isServer) return;

        if (col)
        {
            Entity e = col.GetComponent<Entity>();
            if (e != null && isServer)
            {
                e.Damage(Random.Range(minDamage, maxDamage), sender);
            }
        }


        for (int i = 0; i < hitEffects.Length; i++)
        {
            GameObject g = Instantiate(hitEffects[i], transform.position, transform.rotation);
            NetworkServer.Spawn(g);

            Explosion ex = g.GetComponent<Explosion>();
            if (ex != null)
                ex.sender = sender;
        }

        Destroy(gameObject);
    }


    /*[System.Serializable]
    public class HitEffect
    {
        public string name;
        public PhysicMaterial material;
        [Space(10)]
        public GameObject effect;
        public AudioClip sound;
    }*/
}

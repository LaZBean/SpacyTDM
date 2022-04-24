using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

[RequireComponent(typeof(EntityMotor))]
public class Entity : NetworkBehaviour {

    [Header("[ENTITY]")]
    [SyncVar(hook = "OnTeamChanged")]public int team;

    [SyncVar]float _health = 100;
    public int maxHealth = 100;

    [SyncVar] bool _isDead = true;


    public EntityMotor motor;

    public Vector3 inputDir;
    public Vector3 aimPos;


    public GameObject spritesHolder;



    public new Collider2D collider;
    public SortingGroup sorting;


    public GameObject lastDamager;


    



    public virtual void Damage(int dmg, GameObject sender=null)
    {
        OnDamaged();
        health -= dmg;
        lastDamager = sender;
    }

    public virtual void OnDamaged()
    {
         
    }

    public virtual void OnDeath()
    {
        isDead = true;
    }

    

    public bool isDead{
        get { return _isDead; }
        set {
            _isDead = value;
            //spritesHolder.SetActive(_isDead);
            collider.enabled = !_isDead;

            sorting.sortingOrder = (_isDead) ? -10 : 0;
        }
    }


    public float health
    {
        get { return _health; }
        set
        {
            _health = Mathf.Clamp(value, 0, maxHealth);
            if (health <= 0)
                OnDeath();
        }
    }





    public virtual void OnTeamChanged(int team)
    {
        this.team = team;
    }

    
    public virtual void Respawn()
    {

    }




    [Client]
    public void ClientSetTeam(int team)
    {
        CmdServerSetTeam(team);     //ANetworkManager.i.client.connection.connectionId
    }

    [Command]
    public void CmdServerSetTeam(int team)
    {
        this.team = team;
        PlayerList.i.SetPlayerTeam(connectionToClient.connectionId, team);
    }
}

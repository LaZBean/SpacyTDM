using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Human : Entity {

    public static Human my;

    [Header("[HUMAN]")]
    [SyncVar]float _shield = 50;
    public int maxShield = 50;

    [SyncVar]public float energy = 100;
    public int  maxEnergy = 100;

    public int energyRestore = 50;
    public int energyRunSub = 25;

    public ActionType curAction = ActionType.idle;


    [SyncVar]public int curItem = 0;
    [SyncVar]public Item curWeapon;

    public SyncListItem items;

    [SyncVar(hook = "UpdateTorso")] public Item curTorso;
    [SyncVar(hook = "UpdatePants")] public Item curPants;


    [Header("- movement")]
    [SyncVar]public bool isRunning = false;

    public float walkSpeed = 1f;
    public float runSpeed = 2f;

    float speed;





    [Header("- render")]
    public SpriteRenderer headRenderer;
    public SpriteRenderer torsoRenderer;
    public SpriteRenderer legsRenderer;



    [Header("- animations")]
    public SpriteGridAnimator headAnimator;
    public SpriteGridAnimator torsoAnimator;
    public SpriteGridAnimator legsAnimator;

    public SpriteGridAnimator shirtAnimator;
    public SpriteGridAnimator pantsAnimator;
    public SpriteGridAnimator weaponAnimator;



    [Header("- actions")]
    float attackTimer;
    [SyncVar] float attackShowTimer;


   

    



    public enum ActionType
    {
        idle,
        walk,
        run,
        dead
    }

  












    void Awake()
    {
        motor = GetComponent<EntityMotor>();
    }


    void Start()
    {
        if (!isServer) return;

        //Players autoteam
        int red = 0;
        int blue = 0;
        Human[] players = FindObjectsOfType<Human>();
        for(int i=0; i<players.Length; i++){
            if (players[i].team == 0) red++;
            else blue++;
        }
        CmdServerSetTeam((red < blue) ? 0 : 1);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }






    public override void OnStartLocalPlayer(){
        base.OnStartLocalPlayer();
        my = this;
    }

    




    

    



    void Update()
    {
        attackTimer -= Time.deltaTime;

        
        energy = Mathf.Clamp(energy, 0, maxEnergy);
        if (!isRunning)
        {
            energy += (Time.deltaTime * energyRestore);
        }


        ///////////////////////////////////
        if(items!=null)
        for (int i = 0; i < items.Count; i++)
        {
            print(items[i].name + ",   " + items[i].count + "/"+items[i].maxCount + ",       " + items[i].fireRate);
        }


        Move();
        Animate();

        if (!isLocalPlayer) return;
        float wheel = Input.GetAxisRaw("Mouse ScrollWheel") * 10;
        int itemChange = (int)wheel;
        if (itemChange != 0) {
            
            CmdChangeItem(curItem + itemChange);
        }
    }



    void Move()
    {
        if (isDead)
        {
            curAction = ActionType.dead;
        }
        else if (motor.velocity == Vector3.zero) {
            curAction = ActionType.idle;
            speed = walkSpeed;
        }
        else {
            if (isRunning && energy > 0){
                speed = runSpeed;
                energy -= (Time.deltaTime * energyRunSub); 
                curAction = ActionType.run;
            }
            else{
                speed = walkSpeed;
                curAction = ActionType.walk;
            }

        }



        if (isDead)
        {
            motor.moveDir = Vector3.zero;
            return;
        }


        motor.speed = speed;
        motor.moveDir = GameUtility.DirNormalize(inputDir);



        Vector3 mouseDir = (aimPos - transform.position).normalized;

        if (curAction != ActionType.idle && inputDir != Vector3.zero)
        {
            torsoRenderer.transform.eulerAngles = new Vector3(0, 0, -GameUtility.DirAngle(inputDir, Vector3.up));
            legsRenderer.transform.eulerAngles = new Vector3(0, 0, -GameUtility.DirAngle(inputDir, Vector3.up));
        }

        if (attackShowTimer > 0){
            torsoRenderer.transform.eulerAngles = new Vector3(0, 0, -GameUtility.DirAngle(mouseDir, Vector3.up));
        }
    }


    void Animate()
    {
        if (isDead){
            torsoAnimator.SetCurrentAnimation("dead");
            shirtAnimator.SetCurrentAnimation("dead");
            legsAnimator.SetCurrentAnimation("dead");
            headAnimator.SetCurrentAnimation("dead");
            weaponAnimator.SetCurrentAnimation("dead");
            return;
        }

        if (curAction == ActionType.idle){


            switch (curWeapon.name)
            {
                case ("Laser Rifle"):       torsoAnimator.SetCurrentAnimation("idle_laser");    shirtAnimator.SetCurrentAnimation("idle_laser"); break;
                case ("Rocket Launcher"):   torsoAnimator.SetCurrentAnimation("idle_rocket");   shirtAnimator.SetCurrentAnimation("idle_rocket"); break;
                case ("Pistol"):            torsoAnimator.SetCurrentAnimation("idle_pistol");   shirtAnimator.SetCurrentAnimation("idle_pistol"); break;
                case ("Sniper Rifle"):      torsoAnimator.SetCurrentAnimation("idle_sniper");   shirtAnimator.SetCurrentAnimation("idle_sniper"); break;
            }
            legsAnimator.SetCurrentAnimation("idle");
            weaponAnimator.SetCurrentAnimation("idle");
        }
        else if (curAction == ActionType.walk){

            switch (curWeapon.name)
            {
                case ("Laser Rifle"):       torsoAnimator.SetCurrentAnimation("walk_laser");    shirtAnimator.SetCurrentAnimation("walk_laser"); break;
                case ("Rocket Launcher"):   torsoAnimator.SetCurrentAnimation("walk_rocket");   shirtAnimator.SetCurrentAnimation("walk_rocket"); break;
                case ("Pistol"):            torsoAnimator.SetCurrentAnimation("walk_pistol");   shirtAnimator.SetCurrentAnimation("walk_pistol"); break;
                case ("Sniper Rifle"):      torsoAnimator.SetCurrentAnimation("walk_sniper");   shirtAnimator.SetCurrentAnimation("walk_sniper"); break;
            }
            legsAnimator.SetCurrentAnimation("walk");
            weaponAnimator.SetCurrentAnimation("walk");
        }
        else if (curAction == ActionType.run)
        {
            switch (curWeapon.name)
            {
                case ("Laser Rifle"):       torsoAnimator.SetCurrentAnimation("run_laser");  shirtAnimator.SetCurrentAnimation("run_laser"); break;
                case ("Rocket Launcher"):   torsoAnimator.SetCurrentAnimation("run_rocket"); shirtAnimator.SetCurrentAnimation("run_rocket"); break;
                case ("Pistol"):            torsoAnimator.SetCurrentAnimation("run_pistol"); shirtAnimator.SetCurrentAnimation("run_pistol"); break;
                case ("Sniper Rifle"):      torsoAnimator.SetCurrentAnimation("run_sniper"); shirtAnimator.SetCurrentAnimation("run_sniper"); break;
            }
            legsAnimator.SetCurrentAnimation("run");
            weaponAnimator.SetCurrentAnimation("run");
        }

        if(attackShowTimer > 0)
        {
            weaponAnimator.SetCurrentAnimation("shoot");
            
            switch (curWeapon.name)
            {
                case ("Laser Rifle") :      torsoAnimator.SetCurrentAnimation("shoot_laser");  shirtAnimator.SetCurrentAnimation("shoot_laser");  break;
                case ("Rocket Launcher"):   torsoAnimator.SetCurrentAnimation("shoot_rocket"); shirtAnimator.SetCurrentAnimation("shoot_rocket"); break;
                case ("Pistol"):            torsoAnimator.SetCurrentAnimation("shoot_pistol"); shirtAnimator.SetCurrentAnimation("shoot_pistol"); break;
                case ("Sniper Rifle"):      torsoAnimator.SetCurrentAnimation("shoot_sniper"); shirtAnimator.SetCurrentAnimation("shoot_sniper"); break;
            }
        }

        headAnimator.SetCurrentAnimation("idle");

        attackShowTimer -= Time.deltaTime;






        switch (curWeapon.name)
        {
            case ("Laser Rifle"):       weaponAnimator.LoadAtlas("sprites/human/weapon_laserrifle"); break;
            case ("Rocket Launcher"):   weaponAnimator.LoadAtlas("sprites/human/weapon_rocketlauncher"); break;
            case ("Pistol"):            weaponAnimator.LoadAtlas("sprites/human/weapon_pistol"); break;
            case ("Sniper Rifle"):      weaponAnimator.LoadAtlas("sprites/human/weapon_sniperrifle"); break;
        }

    }











    public void Attack()
    {
        if(!isDead && attackTimer <= 0)
        {
            CmdFire(this.gameObject);

            attackShowTimer = 0.5f;
            attackTimer = curWeapon.fireRate;



            
        }
    }



    [ClientRpc]
    public void RpcOnAttack(bool wasShooting)
    {
        attackShowTimer = 0.5f;

        if (wasShooting) return;


        string muzzlePath = "prefabs/effects.muzzle.";
        switch (curWeapon.name)
        {
            case ("Laser Rifle"): muzzlePath += "laser"; break;
            case ("Rocket Launcher"): muzzlePath += "rocket"; break;
            case ("Pistol"): muzzlePath += "pistol"; break;
            case ("Sniper Rifle"): muzzlePath += "sniper"; break;
        }
        GameObject muzzleObj = (GameObject)Instantiate(Resources.Load(muzzlePath), weaponAnimator.transform);
        Destroy(muzzleObj, muzzleObj.GetComponentInChildren<SpriteAnimator>().lifetime);
    }

    
  

    [Command]
    public void CmdFire(GameObject sender)
    {

        RpcOnAttack(curWeapon.count <= 0);

        attackShowTimer = 0.5f;

        if (items[curItem].count <= 0) return;

        GameObject projectile;
        if(curWeapon.name == "Rocket Launcher"){
            projectile = (GameObject)Resources.Load("prefabs/projectile.rocket");
        }
        else if(curWeapon.name == "Laser Rifle"){
            projectile = (GameObject)Resources.Load("prefabs/projectile.laser");
        }
        else if (curWeapon.name == "Sniper Rifle"){
            projectile = (GameObject)Resources.Load("prefabs/projectile.sniperbullet");
        }
        else{
            projectile = (GameObject)Resources.Load("prefabs/projectile.bullet");
        }



        Item w = Item.Copy(items[curItem]);
        w.count -= 1;
        items[curItem] = w;

        ///BULLSIHT

        CmdChangeItem(curItem);






        GameObject bulletObj = (GameObject)Instantiate(projectile, transform.position, transform.rotation);

        Projectile bullet = bulletObj.GetComponent<Projectile>();
        bullet.sender = gameObject;
        bullet.SetDestroyTime(aimPos);

        Vector3 mouseDir = (aimPos - transform.position).normalized;
        bulletObj.transform.eulerAngles = new Vector3(0, 0, -GameUtility.DirAngle(mouseDir, Vector3.up));

        NetworkServer.Spawn(bulletObj);
    }






    public void AskForRespawn()
    {
        CmdRespawn();
    }

    [Command]
    public void CmdRespawn()
    {
        Respawn();
    }

    public override void Respawn()
    {
        health = maxHealth;
        shield = maxShield;
        energy = maxEnergy;

        items.Clear();
        items.Add(ItemsLib.GetItemCopy("Pistol", 999));
        items.Add(ItemsLib.GetItemCopy("Laser Rifle", 0));
        items.Add(ItemsLib.GetItemCopy("Sniper Rifle", 0));
        items.Add(ItemsLib.GetItemCopy("Rocket Launcher", 0));

        CmdChangeItem(0);

        curTorso = Item.Clothes("Team Shirt", "icon.clothes.teamshirt", Item.ClothesType.Torso, (team == 0) ? Color.red : Color.blue, "sprites/human/shirt_team");
        curPants = Item.Clothes("Team Pants", "icon.clothes.teampants", Item.ClothesType.Pants, (team == 0) ? Color.red : Color.blue, "sprites/human/pants_team");

        Transform startPos = ANetworkManager.i.GetStartPosition();
        GetComponent<NetEntitySync>().CmdTeleportTo(startPos.position);

        isDead = false;
    }


    [Command]
    public void CmdChangeItem(int i){
        curItem = (i<0)? items.Count-1 : (i % items.Count);

        curWeapon = items[curItem];
    }






    public float shield
    {
        get { return _shield; }
        set{
            _shield = Mathf.Clamp(value, 0, maxShield);
        }
    }


    public override void Damage(int dmg, GameObject sender=null)
    {
        int rest = Mathf.CeilToInt(dmg - shield);

        OnShieldDamaged();
        shield -= dmg;
        
        if(rest>0)
            base.Damage(rest, sender);
    }

    public virtual void OnShieldDamaged()
    {

    }




    public override void OnTeamChanged(int team)
    {
        print("changed");
        base.OnTeamChanged(team);
    }



    public void UpdatePants(Item pants)
    {
        curPants = pants;
        legsAnimator.LoadAtlas(curPants.spriteAtlasPath);
        legsAnimator.renderer.color = curPants.color;
    }

    public void UpdateTorso(Item torso)
    {
        curTorso = torso;
        shirtAnimator.LoadAtlas(curTorso.spriteAtlasPath);
        shirtAnimator.renderer.color = curTorso.color;
    }



    public override void OnDeath()
    {
        base.OnDeath();
        CmdDead();
    }
    
    [Command]
    public void CmdDead()
    {
        
        if(lastDamager != null) { 
            Human killer = lastDamager.GetComponent<Human>();

            bool isAlly = PlayerList.i.GetPlayerFromConnectionID(killer.connectionToClient.connectionId).team == PlayerList.i.GetPlayerFromConnectionID(connectionToClient.connectionId).team;

            PlayerList.i.SetKillPoints(killer.connectionToClient.connectionId, (isAlly)? -1 : 1);
        }


        PlayerList.i.SetDeathPoints(connectionToClient.connectionId);

        if (team == 0)
            QuestManager.i.score1--;
        if (team == 1)
            QuestManager.i.score2--;
    }

}

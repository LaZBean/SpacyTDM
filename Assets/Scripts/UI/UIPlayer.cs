using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour {

    Entity _entity;

    public Image entityIcon;
    public Text entityName;

    public Image healthImg;

    public Image energyImg;
    public Image shieldImg;


    public Image curItemIcon;
    public Text curItemCount;
    public Text curItemName;


    void Awake()
    {

    }

	void Start () {
		
	}
	
	
	void Update () {
        entity = ControlManager.i.entity;

        if (entity == null)
        {
            entityIcon.sprite = null;
        }
        else
        {
            healthImg.fillAmount = entity.health * (1f / entity.maxHealth);

            Human h = (Human)entity;
            if (h != null)
            {
                energyImg.fillAmount = h.energy * (1f / h.maxEnergy);
                shieldImg.fillAmount = h.shield * (1f / h.maxShield);


                bool showCurWeapon = (h.curWeapon != null && h.curWeapon.name != "");
                curItemIcon.gameObject.SetActive(showCurWeapon);
                curItemName.gameObject.SetActive(showCurWeapon);
                curItemCount.gameObject.SetActive(showCurWeapon);
                

                curItemIcon.sprite = ItemsLib.i.GetIcon(h.curWeapon.icon);
                curItemIcon.SetNativeSize();
                curItemCount.text = h.curWeapon.count + "/" + h.curWeapon.maxCount;
                curItemName.text = h.curWeapon.name;


                entityIcon.color = (h.isDead) ? new Color(0.2f, 0.2f, 0.2f) : Color.white;
            }

            
        }
    }




    public Entity entity
    {
        get { return _entity; }
        set {
            if (_entity == value) return;
            _entity = value;
            Refresh();
        }
    }




    public void Refresh()
    {
        entityName.text = GameManager.i.playerData.name;
        entityIcon.sprite = SkinLib.i.skinIcons[GameManager.i.playerData.skin];
    }
}

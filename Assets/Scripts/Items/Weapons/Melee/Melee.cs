using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon 
{

    private void Start() {
        
    }

    public virtual void Update() {
        if (fireCoolDown > 0)
            fireCoolDown -= Time.deltaTime;
    }

    public override sealed bool Use() {
        return Swing();
    }

    public virtual bool Swing() {
        if (ammoLoaded > 0 || ammoRequired == null) {
            if (fireCoolDown <= 0) {
                if (AmmoRequired != null) ammoLoaded--;
                FireEffect();
                fireCoolDown = fireRate;
            }
            return true;
        }
        return false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon 
{
    [Header("Gun Settings")]
    [SerializeField] protected Bullet bulletToShoot = null;
    [SerializeField] protected float accuracy = 90.0f;
    [SerializeField] protected float recoil = 1.0f;

    public Transform barrel;
    protected float reloadCooldown = 0;
    protected float inAccuracy;
    private bool reloadInterrupted = false;
    protected float recoiling = 0.0f;

    protected virtual void Start() {
        inAccuracy = 100.0f - accuracy;
        Mathf.Clamp(inAccuracy, 0.01f, 99.0f);
    }

    public virtual void Update() {
        if (fireCoolDown > 0) {
            fireCoolDown -= Time.deltaTime;
        }

        if (recoiling > 0) {
            recoiling -= Time.deltaTime;
        }

        if (reloadCooldown > 0) {
            reloadCooldown -= Time.deltaTime;

            if (reloadCooldown <= 0 && !reloadInterrupted) {
                ReloadSystem();
            }

            if (reloadInterrupted) {
                reloadInterrupted = false;
            }
        } 
    }

    public override sealed bool Use() {
        return Shoot();
    }

    public virtual bool Shoot() {
        if (ammoLoaded > 0 || ammoRequired == null) {
            if (fireCoolDown <= 0) {
                if (reloadCooldown > 0) {
                    reloadInterrupted = true;
                    reloadCooldown = 0;
                }

                if (AmmoRequired != null) ammoLoaded--;
                recoiling += recoil;

                Mathf.Clamp(recoiling, 0.0f, 85.0f);

                FireEffect();
                fireCoolDown = fireRate;
            }
            return true;
        }
        return false;
    }

    public virtual void ReloadSystem() {
        if (user.infiniteAmmo) {
            ammoLoaded = maxAmmoLoaded;
        }
        else {
            ammoLoaded += user.Inventory.GetAmmo(maxAmmoLoaded - ammoLoaded);
        }
    }

    public void Reload() {
        if (reloadCooldown > 0) return;
        if (ammoRequired == null) return;
        if (ammoLoaded == maxAmmoLoaded) return;
        if (reloadCooldown > 0) return;

        if (user.infiniteAmmo || user.Inventory.activeAmmoStack.itemAmount > 0) {
            reloadCooldown = reloadTime;
        }   

    }

    public float GetReloadPercent() {
        return reloadCooldown / reloadTime;
    }

    public bool IsReloading() {
        return reloadCooldown > 0;
    }
}

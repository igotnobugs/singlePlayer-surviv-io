using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun 
{
    [Header("Shotgun")]
    [SerializeField] public int pellets;
    public AnimationCurve spread;

    public override void FireEffect() {

        for (int i = 0; i < pellets; i++) {
            Bullet bulletFired = bulletToShoot.Fire(damage, barrel, user);

            float inaccuracy = Random.Range(-inAccuracy, inAccuracy);
            inaccuracy *= recoiling / 100.0f;

            float spreadDistribution = spread.Evaluate(Random.value) * 2.0f;
            bulletFired.transform.eulerAngles += new Vector3(0, 0, inaccuracy * spreadDistribution);
        }
    }

    public override void ReloadSystem() {
        if (user.infiniteAmmo) {
            ammoLoaded += 1;
        }
        else {
            ammoLoaded += user.Inventory.GetAmmo(1);
        }
        
        if (ammoLoaded < maxAmmoLoaded) {
            Reload();
        }

    }
}

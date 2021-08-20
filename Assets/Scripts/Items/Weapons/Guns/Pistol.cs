using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun 
{
    public override void FireEffect() {
        Bullet bulletFired = bulletToShoot.Fire(damage, barrel, user);

        float inaccuracy = Random.Range(-inAccuracy, inAccuracy);
        inaccuracy *= recoiling / 100.0f;

        bulletFired.transform.eulerAngles += new Vector3(0, 0, inaccuracy);
    }
}

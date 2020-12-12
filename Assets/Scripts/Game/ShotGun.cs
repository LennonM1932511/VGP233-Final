using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.VFX;

public class ShotGun : Gun
{
    public float maxBulletOffet = 0.2f;
    public int cannonTotalBullets = 10;

    protected override void Shoot()
    {
        // play visual effect
        muzzleFlash.Play();

        //play sfx for pistol
        ServiceLocator.Get<SoundManager>().PlayAudio(SoundManager.Sound.Weapon_Shotgun_Fire);


        for (int i = 0; i < cannonTotalBullets; i++)
        {
            // cast ray to see what the shot hits
            RaycastHit hit;

            var direction = fpsCam.transform.forward;
            direction.x += Random.Range(-maxBulletOffet, maxBulletOffet);
            direction.y += Random.Range(-maxBulletOffet, maxBulletOffet);

            if (Physics.Raycast(fpsCam.transform.position, direction, out hit, cannonRange))
            {
                //Debug.Log(hit.transform.name);  
                GameObject hitBlast = Instantiate(cannonHitPrefab, hit.point, Quaternion.identity);
                cannonHit.Play();
            }

            // only damagable objects are targets
            IDamagable target = hit.transform?.GetComponent<IDamagable>();

            // deal damage to target
            if (target != null)
            {
                float random = Random.Range(cannonDamageMin, cannonDamageMax);
                target.TakeDamage(cannonDamageBase + random);
            }

            // apply force if rigidbody is shot
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * cannonForce);
            }
        }

        // fixed fire rate
        nextTimeToFire = Time.realtimeSinceStartup + fireRate;
    }

}

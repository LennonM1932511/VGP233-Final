using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.VFX;

public class Gun : MonoBehaviour
{
    public float cannonDamageBase = 10.0f;
    public float cannonDamageMin = 1.0f;
    public float cannonDamageMax = 15.0f;
    public float cannonRange = 100.0f;
    public float cannonForce = 100.0f;
    public float bombVelocity = 1000.0f;
    public float fireRate = 0.5f;

    public Camera fpsCam;
    public VisualEffect muzzleFlash;
    public VisualEffect cannonHit;
    public Transform muzzleTransform;
    public GameObject bombPrefab;
    public GameObject cannonHitPrefab;

    protected float nextTimeToFire = 0.0f;

    private void Awake()
    {
        muzzleFlash.Stop();
        cannonHit.Stop();
    }


    // Update is called once per frame
    void Update()
    {
        if (!PauseControl.gameIsPaused && !GameManager._isGameOver)
        {
            // main cannon
            if (Input.GetButton("Fire1") && nextTimeToFire <= Time.realtimeSinceStartup)
            {
                Shoot();
            }

            // launch bomb
            if (Input.GetButton("Fire2")
                && ServiceLocator.Get<GameManager>().CurrentBombs > 0
                && nextTimeToFire <= Time.realtimeSinceStartup)
            {
                LaunchBomb();
            }
        }
    }

    protected virtual void Shoot()
    {
        // play visual effect
        muzzleFlash.Play();

        //play sfx for pistol
        ServiceLocator.Get<SoundManager>().PlayAudio(SoundManager.Sound.Weapon_Pistol_Fire);

        // cast ray to see what the shot hits
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, cannonRange))
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

        // fixed fire rate
        nextTimeToFire = Time.realtimeSinceStartup + fireRate;
    }

    private void LaunchBomb()
    {
        ObjectPool_Manager poolManager = ServiceLocator.Get<ObjectPool_Manager>();
        GameObject bomb = poolManager.GetObjectFromPool("Bombs");
        bomb.transform.position = muzzleTransform.position;
        bomb.transform.rotation = Quaternion.identity;        
        bomb.SetActive(true);

        //GameObject bomb = Instantiate(bombPrefab, muzzleTransform.position, Quaternion.identity);

        

        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        Vector3 reset = new Vector3(0f, 0f, 0f);
        rb.velocity = reset;
        rb.AddForce(muzzleTransform.forward * bombVelocity, ForceMode.Force);
        nextTimeToFire = Time.realtimeSinceStartup + fireRate;

        ServiceLocator.Get<SoundManager>().PlayAudio(SoundManager.Sound.Weapon_Grenade_Fire);

        ServiceLocator.Get<GameManager>().UpdateBombs(-1);
    }
}

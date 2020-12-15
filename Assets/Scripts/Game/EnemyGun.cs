using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public Transform muzzleTransform;
    public GameObject bulletPrefab;
    public float bulletVelocity = 1000.0f;

    public void Shoot()
    {
        ObjectPool_Manager poolManager = ServiceLocator.Get<ObjectPool_Manager>();
        GameObject bullet = poolManager.GetObjectFromPool("Bullets");
        bullet.transform.position = muzzleTransform.position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.SetActive(true);

        //GameObject bullet = Instantiate(bulletPrefab, muzzleTransform.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        Vector3 reset = new Vector3(0f, 0f, 0f);
        rb.velocity = reset;
        rb.AddForce(muzzleTransform.up * bulletVelocity, ForceMode.Force);

        // LENNON: play enemy shooting sfx at muzzle
        ServiceLocator.Get<SoundManager>().PlayAudioAtPosition(SoundManager.Sound.Enemy_Shoot, muzzleTransform.position);
    }
}

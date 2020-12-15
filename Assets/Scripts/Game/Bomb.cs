using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float bombDamage = 50.0f;
    [SerializeField] private float bombRadius = 10.0f;

    public GameObject bombExplodePrefab;
    public VisualEffect explodeVFX;

    private void Awake()
    {
        explodeVFX.Stop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // get location of first contact of bomb
        Vector3 location = collision.GetContact(0).point;

        _ = Instantiate(bombExplodePrefab, location, Quaternion.identity);
        explodeVFX.Play();

        // LENNON:
        // Play grenade explosion sfx
        ServiceLocator.Get<SoundManager>().PlayAudioAtPosition(SoundManager.Sound.Weapon_Grenade_Explode, location);

        // for debugging/reporting
        int hitCount = 0;

        // deal damage to first enemy hit
        EnemyNPC directhit = collision.transform.GetComponent<EnemyNPC>();
        if (directhit != null)
        {
            directhit.GetComponent<IDamagable>().TakeDamage(bombDamage);
            ++hitCount;
        }

        // create a list of objects within the blast radius
        Collider[] objectsInRange = Physics.OverlapSphere(location, bombRadius);

        foreach (var col in objectsInRange)
        {
            // check if the object is an enemy
            //EnemyNPC enemy = col.GetComponent<EnemyNPC>();
            IDamagable enemy = col.GetComponent<IDamagable>();
            if (enemy != null && col.name != "Player")
            {
                // test if enemy is exposed to blast, or behind cover
                RaycastHit hit;
                var exposed = false;
                if (Physics.Raycast(location, (col.transform.position - location), out hit))
                {
                    exposed = (hit.collider == col.GetComponent<Collider>());
                }

                if (exposed)
                {
                    // damage enemy with a linear falloff
                    float proximity = (location - col.transform.position).magnitude;
                    float effect = 1 - (proximity / bombRadius);
                    float damage = bombDamage * effect;

                    enemy.TakeDamage(damage);

                    ++hitCount;
                    Debug.Log(col.transform.name + " was " + Mathf.RoundToInt(proximity).ToString() + " from the blast.");
                }
            }
        }
        // destroy bomb on collision
        //gameObject.SetActive(false);
        ServiceLocator.Get<ObjectPool_Manager>().RecycleObject(gameObject);

        Debug.Log("Bomb exploded on " + collision.transform.name + ", hitting " + hitCount.ToString() + " enemies.");
    }
}

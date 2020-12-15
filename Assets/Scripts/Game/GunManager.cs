using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    private List<GameObject> guns = new List<GameObject>();

    public GameObject currentGun;
    private int currentGunIndex;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in GetComponentsInChildren<Gun>())
        {
            GameObject go = item.gameObject;
            guns.Add(go);
            if (currentGun != go)
            {
                go.SetActive(false);
            }
        }
        currentGunIndex = guns.FindIndex((g) => g == currentGun);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && ServiceLocator.Get<GameManager>().CurrentLevel - 2 >= (currentGunIndex + 1) % guns.Count)
        {
            currentGun.SetActive(false);
            currentGunIndex = (currentGunIndex + 1) % guns.Count;

            currentGun = guns[currentGunIndex];
            currentGun.SetActive(true);
        }
    }
}

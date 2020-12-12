using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private int _value;
    [SerializeField] private float _health;
    [SerializeField] private int _bombs;
    [SerializeField] private int _keys;
    [SerializeField] private int _dataShards;

    public int Value { get { return _value; } private set { _value = value; } }
    public float Health { get { return _health; } private set { _health = value; } }
    public int Bombs { get { return _bombs; } private set { _bombs = value; } }
    public int Keys { get { return _keys; } private set { _keys = value; } }
    public int DataShards { get { return _dataShards; } private set { _dataShards = value; } }

    public void Collect()
    {
        if (_value > 0)
        {
            ServiceLocator.Get<GameManager>().UpdateScore(_value);
            Debug.Log(_value + " points collected!");
        }

        if (_health > 0)
        {
            ServiceLocator.Get<GameManager>().UpdateHealth(_health);
            ServiceLocator.Get<SoundManager>().PlayAudio(SoundManager.Sound.Pickup_Health);
            Debug.Log("Player gained " + _health + " health!");
        }

        if (_bombs > 0)
        {
            ServiceLocator.Get<GameManager>().UpdateBombs(_bombs);
            ServiceLocator.Get<SoundManager>().PlayAudio(SoundManager.Sound.Pickup_Grenade);
            Debug.Log("Player gained " + _bombs + " bombs!");
        }

        if (_keys > 0)
        {
            ServiceLocator.Get<GameManager>().UpdateKeys(_keys);
            ServiceLocator.Get<SoundManager>().PlayAudio(SoundManager.Sound.Pickup_Key);
            Debug.Log("Player gained " + _keys + " keys!");
        }

        if (_dataShards > 0)
        {
            ServiceLocator.Get<GameManager>().UpdateDataShards(_dataShards);
            ServiceLocator.Get<SoundManager>().PlayAudio(SoundManager.Sound.Pickup_Data);
            Debug.Log("Player gained " + _dataShards + " Data Shards!");
        }


        gameObject.SetActive(false);
    }
}

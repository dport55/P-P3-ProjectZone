using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu]
public class Gunstats : ScriptableObject
{
    
    public bool RedFlash;
    public GameObject model;
    public int shootDamage;
    public int shootDist;
    public float shootRate;
    public float ReloadTimer;
    public int AmmoCur, AmmoMax;
    public Transform muzzlePos;
    public float freezeTime;
    //public Vector3 position;
    //public Quaternion rotation;
    public ParticleSystem HitEffect;
    [SerializeField] public GameObject ShootEffect;
    public AudioClip shootSound, gunClick;
    public float shootVol;
}
    
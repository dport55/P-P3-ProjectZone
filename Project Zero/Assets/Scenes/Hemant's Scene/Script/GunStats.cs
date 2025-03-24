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
    
    public ParticleSystem HitEffect;
    [SerializeField] public GameObject ShootEffect;
    public AudioClip shootSound, gunClick,gunReload;
    public float shootVol;
}
    
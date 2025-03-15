using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu]
public class Gunstats : ScriptableObject
{
    public GameObject model;
    public int shootDamage;
    public int shootDist;
    public float shootRate;
    public int AmmoCur, AmmoMax;
    public Vector3 muzzlePos;
    public ParticleSystem HitEffect;
    public AudioClip[] shootSound;
    public float shootVol;
}

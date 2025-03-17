using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu]
public class Gunstats : ScriptableObject
{
    public string Name;
    public GameObject model;
    public int shootDamage;
    public int shootDist;
    public float shootRate;
    public int AmmoCur, AmmoMax;
    public Transform muzzlePos;
    //public Vector3 position;
    //public Quaternion rotation;
    public ParticleSystem HitEffect;
    [SerializeField] public GameObject ShootEffect;
    public AudioClip[] shootSound;
    public float shootVol;
}
    
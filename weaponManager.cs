using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponManager : MonoBehaviour
{

    public float shotgunAmmo = 20f;

    public Material[] bulletDecals;
    public GameObject decalTemplate;

    public int ignoreDecal = 6;

    public bool ownShotgun = true;
    public bool ownRifle = true;
    public bool ownAssaultRifle = true;
    public bool ownRevolver = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public GameObject generateDecal()
    {
        Material mat = bulletDecals[Random.Range(0, bulletDecals.Length)];
        decalTemplate.GetComponentInChildren<MeshRenderer>().sharedMaterial = mat;
        return decalTemplate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

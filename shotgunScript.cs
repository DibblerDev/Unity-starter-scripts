using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class shotgunScript : MonoBehaviour
{
    public const int magSize = 2;
    public Animator anim;

    public GameObject player;

    Rigidbody rb;
    weaponManager manager;

    public ParticleSystem particles;

    public Transform guntipL;
    public Transform guntipR;

    /// <summary>
    /// How much force will be applied to the character
    /// </summary>
    public float recoil;
    /// <summary>
    /// damage per pellet
    /// </summary>
    public float damage;
    /// <summary>
    /// The amount of pellets per shell launched from individual shotgun barrels
    /// </summary>
    public float pellets = 50f;
    /// <summary>
    /// The spread of the pellets
    /// </summary>
    public float spread = 0.3f;
    /// <summary>
    /// The range of the pellets
    /// </summary>
    public float range = 20f;
    public float cooldown = 1f;

    float tick;

    public bool canShoot;
    public bool reload;

    float ammo;
    GameObject decal;
    int ignoreDecal;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(guntipL.position, 0.05f);
        Gizmos.DrawWireSphere(guntipR.position, 0.05f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(guntipL.position, guntipL.forward * range);
        Gizmos.DrawLine(guntipR.position, guntipR.forward * range);
    }

    void Awake()
    {
        rb = player.GetComponent<Rigidbody>();
        manager = player.GetComponent<weaponManager>();
        ignoreDecal = manager.ignoreDecal;
        ammo = manager.shotgunAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        tick += Time.deltaTime;
        canShoot = tick > cooldown && !reload && !anim.GetCurrentAnimatorStateInfo(0).IsName("Reload");
        if (Input.GetMouseButtonDown(0))
        {            
            if(reload && manager.shotgunAmmo - magSize >= 0)
            {
                anim.SetTrigger("Reload");
                manager.shotgunAmmo -= magSize;
                reload = false;
            }
            if (canShoot)
            {
                tick = 0;
                shoot(guntipR);
                shoot(guntipL);
                anim.SetTrigger("Shoot");
                reload = true;
            }

        }
    }

    void shoot(Transform pos)
    {
        RaycastHit hit;
        List<RaycastHit> hits = new List<RaycastHit>();
        GameObject particleSystem;
        Vector3 dir = Vector3.zero;
        rb.AddForce(-transform.forward * recoil, ForceMode.Impulse);
        for (int i = 0; i <= pellets ; i++)
        {
            dir = pos.forward + (pos.right * Random.Range(-spread, spread)) + (pos.up * Random.Range(-spread, spread));
            if (Physics.Raycast(pos.position, dir, out hit, range)){
                hits.Add(hit);
            }
        }
        foreach (RaycastHit point in hits)
        {
            particleSystem = Instantiate(particles.gameObject, point.point, Quaternion.identity);
            Debug.Log(ignoreDecal);
            
            if(point.collider.gameObject.layer != ignoreDecal)
            {
                decal = Instantiate(manager.generateDecal(), point.point + point.normal * 0.001f, Quaternion.identity);
                decal.transform.LookAt(point.point + point.normal);
            }
            

            particleSystem.transform.LookAt(point.point + point.normal);
            
        }
    }
}

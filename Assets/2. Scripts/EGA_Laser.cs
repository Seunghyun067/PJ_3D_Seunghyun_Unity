using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System;
using UnityEngine;

public class EGA_Laser : MonoBehaviour
{
    private Transform owner = null;
    public GameObject HitEffect;
    public float HitOffset = 0;

    public float MaxLength;
    private LineRenderer Laser;

    public Vector3 dstPosition;

    public float MainTextureLength = 1f;
    public float NoiseTextureLength = 1f;
    private Vector4 Length = new Vector4(1,1,1,1);
    //private Vector4 LaserSpeed = new Vector4(0, 0, 0, 0); {DISABLED AFTER UPDATE}
    //private Vector4 LaserStartSpeed; {DISABLED AFTER UPDATE}
    //One activation per shoot
    private bool LaserSaver = false;
    private bool UpdateSaver = false;

    private ParticleSystem[] Effects;
    private ParticleSystem[] Hit;
    Vector3 dir;
    public LayerMask layermask;
    private float curLength = 0f;
    public void SetLayerMask()
    {
        layermask = 1 << LayerMask.NameToLayer("Monster");
    }
    public void LaserShoot(Vector3 _dstPosition, Transform owner = null)
    {
        this.owner = owner;
        gameObject.SetActive(true);
        dstPosition = _dstPosition;
        Laser.enabled = true;
        UpdateSaver = false;
        dir = (dstPosition - transform.position).normalized;

        Laser.SetPosition(0, transform.position);
        
        StartCoroutine(ShootTime());
    }

    IEnumerator ShootTime()
    {
        float shootTime = 0.3f;
        float time = 0f;

        Laser.SetPosition(0, transform.position);
        Laser.SetPosition(1, dir * curLength);
        foreach (var AllPs in Effects)
        {
            if (AllPs.isPlaying) AllPs.Play();
        }

        while (shootTime >= time)
        {
            if (curLength < MaxLength && !isHit)
                curLength += Time.deltaTime * 50f;

            Debug.Log(curLength);
            LaserRaycast();
            Laser.SetPosition(0, transform.position);
            Laser.SetPosition(1, transform.position + dir * curLength);

            time += Time.deltaTime;
            yield return null;
        }
        DisablePrepare();
        curLength = 0f;

    }

    void Awake ()
    {
        //Get LineRender and ParticleSystem components from current prefab;  
        Laser = GetComponent<LineRenderer>();
        Effects = GetComponentsInChildren<ParticleSystem>();
        Hit = HitEffect.GetComponentsInChildren<ParticleSystem>();
        //if (Laser.material.HasProperty("_SpeedMainTexUVNoiseZW")) LaserStartSpeed = Laser.material.GetVector("_SpeedMainTexUVNoiseZW");
        //Save [1] and [3] textures speed
        //{ DISABLED AFTER UPDATE}
        //LaserSpeed = LaserStartSpeed;
        // 
        DisablePrepare();
    }

    bool isHit = false;
    void LaserRaycast()
    {
        //Laser.material.SetTextureScale("_MainTex", new Vector2(Length[0], Length[1]));
        //Laser.material.SetTextureScale("_Noise", new Vector2(Length[2], Length[3]));

        if (Laser.enabled && UpdateSaver == false)
        {
            RaycastHit hit;

            Debug.DrawRay(transform.position, transform.position + dir * curLength, Color.red);
            if (Physics.Raycast(transform.position, dir, out hit, curLength, layermask))//CHANGE THIS IF YOU WANT TO USE LASERRS IN 2D: if (hit.collider != null)
            {
                if (!isHit && hit.transform.gameObject.layer == LayerMask.NameToLayer("Sword"))
                {
                    isHit = true;
                    // GameObject obj = Instantiate(gameObject, hit.point, Quaternion.identity);
                    GameObject obj = ObjectPooling.Instance.PopObject("BlueLaser", hit.point);
                    GameManager.Instance.TimeSleep(0.05f, 0.3f);
                    obj.GetComponent<EGA_Laser>().SetLayerMask();
                    obj.GetComponent<EGA_Laser>().LaserShoot(owner.position);
                }
                if (!isHit && hit.transform.gameObject.layer == LayerMask.NameToLayer("Monster"))
                {
                    isHit = true;
                    IDamable damagable = hit.transform.GetComponent<IDamable>();
                    damagable.TakeDamage(10);

                }
                if (!isHit && hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    isHit = true;
                    hit.transform.gameObject.GetComponent<IDamable>().TakeDamage(5);
                }

                //End laser position if collides with object
                Laser.SetPosition(1, hit.point);
                HitEffect.transform.position = hit.point + hit.normal * HitOffset;
                //Hit effect zero rotation
                HitEffect.transform.rotation = Quaternion.identity;
                foreach (var AllPs in Effects)
                {
                    if (!AllPs.isPlaying) AllPs.Play();
                }

                Length[0] = MainTextureLength * (Vector3.Distance(transform.position, hit.point));
                Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, hit.point));
            }
            else if(curLength > MaxLength)
            {
                HitEffect.transform.position = transform.position + dir * MaxLength;
                //Hit effect zero rotation
                HitEffect.transform.rotation = Quaternion.identity;
                foreach (var AllPs in Effects)
                {
                    if (!AllPs.isPlaying) AllPs.Play();
                }

                Length[0] = MainTextureLength * (Vector3.Distance(transform.position, transform.position + dir * curLength));
                Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, transform.position + dir * curLength));
            }
            //Insurance against the appearance of a laser in the center of coordinates!
            if (Laser.enabled == false && LaserSaver == false)
            {
                LaserSaver = true;
                Laser.enabled = true;
            }
        }
    }
    private void Update()
    {
                
    }
    public void DisablePrepare()
    {
        isHit = false;
        if (Laser != null)
        {
            Laser.enabled = false;
        }
        UpdateSaver = true;
        //Effects can = null in multiply shooting
        if (Effects != null)
        {
            foreach (var AllPs in Effects)
            {
                if (AllPs.isPlaying) AllPs.Stop();
            }
        }

        Invoke("Disable", 0.2f);
        
    }
    public void Disable()
    {
        ObjectPooling.Instance.PushObject(gameObject);
    }
}

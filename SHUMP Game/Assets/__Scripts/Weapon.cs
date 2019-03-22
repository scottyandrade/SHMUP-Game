﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum WeaponType
{
    none,
    blaster,
    simple
}

[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;
    public Color color = Color.white;
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;
    public float delayBetweenShots = 0;
    public float velocity = 20;
}

public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;

    [Header("Set Dynamically")]
    [SerializeField]
    private WeaponType _type = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime;
    private Renderer collarRend;


    // Start is called before the first frame update
    void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();

        SetType(_type);

        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        GameObject rootGO = transform.root.gameObject;
        if (rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }

    }

    public WeaponType type
    {
        get { return (_type); }
        set
        { SetType(value); }
    }

    public void SetType(WeaponType weaponType)
    {
        _type = weaponType;
        if (_type == WeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
            this.gameObject.SetActive(true);

        def = Main.GetWeaponDefinition(_type);
        collarRend.material.color = def.color;
        lastShotTime = 0;
    }

    public void Fire()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        if (Time.time - lastShotTime < def.delayBetweenShots)
        {
            return;
        }
        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;
        if (transform.up.y < 0)
        {
            vel.y = -vel.y;
        }

        switch (type)
        {
            case WeaponType.simple:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;

            case WeaponType.blaster:
                // middle projectile
                p = MakeProjectile();
                p.rigid.velocity = vel;
                // right projectile
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(30, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                // left projectile
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(-30, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;

            // idk about this one
            case WeaponType.none:
                break;
        }
    }

    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        if (transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "Projectile Enemy";
            go.layer = LayerMask.NameToLayer("Projectile Enemy");
        }
        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
        p.type = _type;
        lastShotTime = Time.time;
        return p;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (type == WeaponType.simple)
            {
                SetType(WeaponType.blaster);
            }
            else
            {
                SetType(WeaponType.simple);
            }
        }
    }
}

using System.Collections.Generic;
using Game.Utils;
using Unity.VisualScripting;
using UnityEngine;

public class Gun
{
    private bool isReloaded;
    private int currLevel;
    private int currAmmo;
    private int maxLevel;

    private Transform fireEffect;
    public GunType gunType { get; private set; }
    public Sprite imageUp { get; private set; }
    public Sprite imageRight { get; private set; }
    public Sprite imageDown { get; private set; }
    public Sprite imageLeft { get; private set; }
    private int particlesPerShot;
    private int maxAmmo;
    private int damage;
    private int accuracy;
    private float deviation;
    private float reloadSpeed;
    private float range;
    public float weight { get; private set; }

    private int init_pps;
    private int init_amo;
    private int init_dmg;
    private int init_acr;
    private float init_dev;
    private float init_rspd;
    private float init_rng;
    private float init_wgt;

    private int final_pps;
    private int final_amo;
    private int final_dmg;
    private int final_acr;
    private float final_dev;
    private float final_rspd;
    private float final_rng;
    private float final_wgt;

    private int pow_pps; // for particles per shot
    private int pow_amo; // for final ammo
    private int pow_dmg; // for damage
    private int pow_acr; // for accuracy
    private int pow_dev; // for deviation
    private int pow_rspd; // for reload speed
    private int pow_rng; // for range
    private int pow_wgt; // for weight

    public static Gun Create(GunSO gunSO)
    {
        if (gunSO == null)
            return null;

        return new Gun
        {
            fireEffect = gunSO.fireEffect,
            gunType = gunSO.gunType,
            imageUp = gunSO.imageUp,
            imageRight = gunSO.imageRight,
            imageDown = gunSO.imageDown,
            imageLeft = gunSO.imageLeft,

            particlesPerShot = gunSO.init_pps,
            maxAmmo = gunSO.init_amo,
            damage = gunSO.init_dmg,
            accuracy = gunSO.init_acr,
            deviation = gunSO.init_dev,
            reloadSpeed = gunSO.init_rspd,
            range = gunSO.init_rng,
            weight = gunSO.init_wgt,

            init_pps = gunSO.init_pps,
            init_amo = gunSO.init_amo,
            init_dmg = gunSO.init_dmg,
            init_acr = gunSO.init_acr,
            init_dev = gunSO.init_dev,
            init_rspd = gunSO.init_rspd,
            init_wgt = gunSO.init_wgt,

            final_pps = gunSO.final_pps,
            final_amo = gunSO.final_amo,
            final_dmg = gunSO.final_dmg,
            final_acr = gunSO.final_acr,
            final_dev = gunSO.final_dev,
            final_rspd = gunSO.final_rspd,
            final_rng = gunSO.final_rng,
            final_wgt = gunSO.final_wgt,

            pow_pps = gunSO.pow_pps,
            pow_amo = gunSO.pow_amo,
            pow_dmg = gunSO.pow_dmg,
            pow_acr = gunSO.pow_acr,
            pow_dev = gunSO.pow_dev,
            pow_rspd = gunSO.pow_rspd,
            pow_rng = gunSO.pow_rng,
            pow_wgt = gunSO.pow_wgt,

            currAmmo = gunSO.init_amo,
            currLevel = 1,
            maxLevel = 50,
            isReloaded = true,
        };
    }

    public void LevelUp()
    {
        if (currLevel < maxLevel)
        {
            currLevel += currLevel < maxLevel ? 1 : 0;
            UpdateProperties();
            // ShowProperties();
            currAmmo = maxAmmo;
        }
    }
    private void UpdateProperties()
    {
        float t = currLevel * 1.0f / maxLevel;
        particlesPerShot = (int)(init_pps + (final_pps - init_pps) * Mathf.Pow(t, pow_pps));
        maxAmmo = (int)(init_amo + (final_amo - init_amo) * Mathf.Pow(t, pow_amo));
        damage = (int)(init_dmg + (final_dmg - init_dmg) * Mathf.Pow(t, pow_dmg));
        accuracy = (int)(init_acr + (final_acr - init_acr) * Mathf.Pow(t, pow_acr));
        deviation = init_dev + (final_dev - init_dev) * Mathf.Pow(t, pow_dev);
        reloadSpeed = init_rspd + (final_rspd - init_rspd) * Mathf.Pow(t, pow_rspd);
        range = init_rng + (final_rng - init_rng) * Mathf.Pow(t, pow_rng);
        weight = init_wgt + (final_wgt - init_wgt) * Mathf.Pow(t, pow_wgt);
    }

    private void ShowProperties()
    {
        Debug.Log(currLevel + " / " + maxLevel);
        Debug.Log(particlesPerShot);
        Debug.Log(maxAmmo);
        Debug.Log(damage);
        Debug.Log(accuracy);
        Debug.Log(deviation);
        Debug.Log(reloadSpeed);
        Debug.Log(range);
    }

    public void Shoot(Transform firePoint, Vector3 playerDir)
    {
        if (isReloaded)
        {
            int n = particlesPerShot;
            if (maxAmmo > 0) n = currAmmo > particlesPerShot ? particlesPerShot : currAmmo;

            for (int i = 0; i < n; i++)
            {
                Vector3 dir = VectorHandler.GenerateRandomDir(playerDir, deviation);
                RaycastBullet(firePoint, dir);
                currAmmo--;
            }
            if (maxAmmo > 0 && currAmmo < 0) currAmmo = 0;
            isReloaded = false;
            FunctionTimer.CreateSceneTimer(() => isReloaded = true, reloadSpeed, "GunReloadTimer");
        }
    }

    public void RaycastBullet(Transform firePoint, Vector3 dir)
    {
        Vector3 start = firePoint.position + dir * 0.1f;
        int layerMask = LayerMask.GetMask("BlockBullet", "Enemy");

        RaycastHit2D hit = Physics2D.Raycast(start, dir, range, layerMask);
        Collider2D collider = hit.collider;

        if (collider != null)
        {
            Vector3 end = hit.point;
            MeshHandler.DrawLineMesh(start, end, 0.1f);

            Stats targetStats = collider.GetComponent<Stats>();
            if (targetStats != null)
            {
                float[] factors = { 1, 1, 1, 1 };
                targetStats.TakeDamage(6, accuracy);
                targetStats.HP();
            }

        }
        else
        {
            Vector3 end = start + dir * range;
            MeshHandler.DrawLineMesh(start, end, 0.1f);
        }
    }
}
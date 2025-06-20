using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunSO", menuName = "Scriptable Objects/GunSO")]
public class GunSO : ScriptableObject {
    public Transform fireEffect;
    public GunType gunType;
    public Sprite imageUp;
    public Sprite imageRight;
    public Sprite imageDown;
    public Sprite imageLeft;

    public int init_pps;
    public int init_amo;
    public int init_dmg;
    public int init_acr;
    public float init_dev;
    public float init_rspd;
    public float init_rng;
    public float init_wgt;

    public int final_pps;
    public int final_amo;
    public int final_dmg;
    public int final_acr;
    public float final_dev;
    public float final_rspd;
    public float final_rng;
    public float final_wgt;

    public int pow_pps; // for particles per shot
    public int pow_amo; // for final ammo
    public int pow_dmg; // for damage
    public int pow_acr; // for accuracy
    public int pow_dev; // for deviation
    public int pow_rspd; // for reload speed
    public int pow_rng; // for range
    public int pow_wgt; // for weight
}

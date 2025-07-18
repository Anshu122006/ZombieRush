using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunListSO", menuName = "Scriptable Objects/GunListSO")]
public class GunListSO : ScriptableObject {
    public GunSO Pistol;
    public GunSO Smg;
    public GunSO Shotgun;
    public GunSO MiniGun;
    public GunSO Bazuca;
    public GunSO LaserGun;
}

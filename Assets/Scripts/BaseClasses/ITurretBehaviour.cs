using UnityEngine;

public abstract class ITurretBehaviour : MonoBehaviour {
    [SerializeField] protected TurretDefinition data;
    [SerializeField] protected Transform visual;
    protected int curLevel;
    protected int maxLevel;
    protected int curHp;
    public int CurLevel => curLevel;
    public int RepairCost => (int)((MHP - HP) * 0.7f);
    public int UpgradeCost => data.upgradeCost.EvaluateStat(curLevel, maxLevel);
    public string Name => data.turretName;
    public int HP { get => curHp; set => curHp = value; }
    public int MHP => data.mhp.EvaluateStat(curLevel, maxLevel);
    public int DEF => data.def.EvaluateStat(curLevel, maxLevel);
    public int Damage => data.damage.EvaluateStat(curLevel, maxLevel);
    public int Accuracy => data.accuracy.EvaluateStat(curLevel, maxLevel);
    public float Range => data.range.EvaluateStat(curLevel, maxLevel);
    public int ExpDrop => data.expDrop.EvaluateStat(curLevel, maxLevel);

    public virtual void LevelUp() { }
}

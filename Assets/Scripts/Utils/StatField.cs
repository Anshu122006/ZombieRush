using UnityEngine;
using UnityEngine.PlayerLoop;

[System.Serializable]
public class StatField<Tval, Tpow> {
    public Tval init;
    public Tval final;
    public Tpow pow;

    public StatField(Tval init, Tval final, Tpow pow) {
        this.init = init;
        this.final = final;
        this.pow = pow;
    }

    public Tval EvaluateStat(int curLevel, int maxLevel) {
        curLevel = Mathf.Clamp(curLevel, 0, maxLevel);
        float t = Mathf.Pow((float)curLevel / maxLevel, System.Convert.ToSingle(this.pow));

        float init = System.Convert.ToSingle(this.init);
        float final = System.Convert.ToSingle(this.final);

        float result = init + (final - init) * t;
        return (Tval)System.Convert.ChangeType(result, typeof(Tval));
    }
}

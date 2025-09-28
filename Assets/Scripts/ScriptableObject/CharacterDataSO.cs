using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataSO", menuName = "Scriptable Objects/CharacterDataSO")]
public class CharacterDataSO : ScriptableObject {
    public int id;
    public string cahrName;
    public string charClass;
    [TextArea] public string charDesc;
    public int startLevel;
    public int maxLevel;
    public StatField<int, int> mhp;
    public StatField<int, int> def;
    public StatField<int, int> speed;
}

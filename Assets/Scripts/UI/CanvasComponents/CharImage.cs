using UnityEngine;

public class CharImage : MonoBehaviour {
    [SerializeField] private CharacterDefinition data;

    public string Name => data.charName;
    public Sprite bust => data.bust;
    public string Desc => data.charDesc;
}

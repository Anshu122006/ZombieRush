using UnityEngine;
using UnityEngine.UI;
public class MapImage : MonoBehaviour {
    [SerializeField] private SceneName sceneName;
    [TextArea] public string desc;

    public string Name => sceneName.GetString();
}

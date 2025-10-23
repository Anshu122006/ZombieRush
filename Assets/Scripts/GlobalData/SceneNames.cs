using System;
using System.Reflection;

public enum SceneName {
    [SceneString("MainMenu")]
    MainMenu = 0,

    [SceneString("LoadInput")]
    LoadInput = 1,

    [SceneString("LoadNoInput")]
    LoadNoInput = 2,

    [SceneString("LoadSilent")]
    LoadSilent = 3,

    [SceneString("GameOver")]
    GameOver = 4,

    [SceneString("CityMap")]
    CityMap = 5,

    [SceneString("ForestMap")]
    ForestMap = 6,

    [SceneString("TrainMap")]
    TrainMap = 7,
}

[AttributeUsage(AttributeTargets.Field)]
public class SceneStringAttribute : Attribute {
    public string Value { get; }
    public SceneStringAttribute(string value) => Value = value;
}

public static class SceneNameExtensions {
    public static string GetString(this SceneName scene) {
        var type = scene.GetType();
        var member = type.GetMember(scene.ToString());
        var attr = member[0].GetCustomAttribute<SceneStringAttribute>();
        return attr?.Value ?? scene.ToString();
    }
}

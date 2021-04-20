using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SceneField
{
    public Object SceneAsset { get { return _SceneAsset; } }

    [SerializeField]
    private Object _SceneAsset = null;

    [SerializeField]
    private string _SceneName = "";

    public string SceneName { get { return _SceneName; } }

    // Makes it work with existing unity methods such as LoadLevel/LoadScene
    public static implicit operator string(SceneField sceneField)
    {
        return sceneField.SceneName;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SceneField))]
public class SceneFieldPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        EditorGUI.BeginProperty(_position, GUIContent.none, _property);
        SerializedProperty sceneAsset = _property.FindPropertyRelative("_SceneAsset");
        SerializedProperty sceneName = _property.FindPropertyRelative("_SceneName");
        _position = EditorGUI.PrefixLabel(_position, GUIUtility.GetControlID(FocusType.Passive), _label);
        if (sceneAsset != null)
        {
            sceneAsset.objectReferenceValue = EditorGUI.ObjectField(_position, sceneAsset.objectReferenceValue, typeof(SceneAsset), false);
            if (sceneAsset.objectReferenceValue != null)
                sceneName.stringValue = (sceneAsset.objectReferenceValue as SceneAsset).name;
        }
        EditorGUI.EndProperty();
    }
}
#endif

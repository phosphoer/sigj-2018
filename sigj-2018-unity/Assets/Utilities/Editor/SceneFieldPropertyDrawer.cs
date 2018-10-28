using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SceneField))]
public class SceneFieldPropertyDrawer : PropertyDrawer
{
  public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
  {
    EditorGUI.BeginProperty(position, label, property);

    position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
    SerializedProperty sceneAssetProp = property.FindPropertyRelative("sceneAsset");
    EditorGUI.PropertyField(position, sceneAssetProp, GUIContent.none);

    EditorGUI.EndProperty();
  }
}
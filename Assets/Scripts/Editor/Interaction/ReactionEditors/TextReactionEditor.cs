using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextReaction))]
public class TextReactionEditor : ReactionEditor
{
    private SerializedProperty messageProperty;
	//private SerializedProperty textColorProperty;
	//private SerializedProperty delayProperty;
	private SerializedProperty soundProperty;
	private SerializedProperty volumeProperty;


    private const float messageGUILines = 3f;
    private const float areaWidthOffset = 19f;
    private const string textReactionPropMessageName = "message";
	//private const string textReactionPropTextColorName = "textColor";
	//private const string textReactionPropDelayName = "delay";
	private const string textReactionPropSoundName = "sound";
	private const string textReactionPropVolumeName = "volume";

    protected override void Init ()
    {
        messageProperty = serializedObject.FindProperty (textReactionPropMessageName);
		//textColorProperty = serializedObject.FindProperty (textReactionPropTextColorName);
		//delayProperty = serializedObject.FindProperty (textReactionPropDelayName);
		soundProperty = serializedObject.FindProperty (textReactionPropSoundName);
		volumeProperty = serializedObject.FindProperty (textReactionPropVolumeName);
    }


    protected override void DrawReaction ()
    {
        EditorGUILayout.BeginHorizontal ();
        EditorGUILayout.LabelField ("Message", GUILayout.Width (EditorGUIUtility.labelWidth - areaWidthOffset));
        messageProperty.stringValue = EditorGUILayout.TextArea (messageProperty.stringValue, GUILayout.Height (EditorGUIUtility.singleLineHeight * messageGUILines));
        EditorGUILayout.EndHorizontal ();

		//EditorGUILayout.PropertyField (textColorProperty);
		//EditorGUILayout.PropertyField (delayProperty);
		EditorGUILayout.PropertyField (soundProperty);
		EditorGUILayout.PropertyField (volumeProperty);
    }


    protected override string GetFoldoutLabel ()
    {
        return "Text Reaction";
    }
}

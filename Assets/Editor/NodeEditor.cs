using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

//[CustomEditor(typeof(Node))]
public class NodeEditor : Editor {
    private ReorderableList list;

    private void OnEnable () {
        list = new ReorderableList (serializedObject,
            serializedObject.FindProperty ("ConnectionInfo"),
            true, true, true, true);

        list.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = list.serializedProperty.GetArrayElementAtIndex (index);
                rect.y += 2;
                EditorGUI.PropertyField (
                    new Rect (rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative ("Transform"), GUIContent.none);
                EditorGUI.PropertyField (
                    new Rect (rect.x + 60, rect.y, rect.width - 60 - 30, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative ("ConnectionType"), GUIContent.none);
                EditorGUI.PropertyField (
                    new Rect (rect.x + rect.width - 30, rect.y, 30, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative ("IsConnected"), GUIContent.none);
            };

    }

    public override void OnInspectorGUI () {
        serializedObject.Update ();
        list.DoLayoutList ();
        serializedObject.ApplyModifiedProperties ();
    }
}

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(tk2dUIUpDownDisableButton))]
public class tk2dUIUpDownDisableButtonEditor : tk2dUIBaseItemControlEditor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        tk2dUIUpDownDisableButton upDownButton = (tk2dUIUpDownDisableButton)target;

        upDownButton.upStateGO = tk2dUICustomEditorGUILayout.SceneObjectField("Up State GameObject", upDownButton.upStateGO,target);
        upDownButton.downStateGO = tk2dUICustomEditorGUILayout.SceneObjectField("Down State GameObject", upDownButton.downStateGO,target);
        upDownButton.disabledStateGO = tk2dUICustomEditorGUILayout.SceneObjectField("Disabled State GameObject", upDownButton.disabledStateGO,target);

        EditorGUIUtility.LookLikeControls(200);

        bool newUseOnReleaseInsteadOfOnUp = EditorGUILayout.Toggle("Use OnRelease Instead of OnUp", upDownButton.UseOnReleaseInsteadOfOnUp);
        if (newUseOnReleaseInsteadOfOnUp != upDownButton.UseOnReleaseInsteadOfOnUp)
        {
            upDownButton.InternalSetUseOnReleaseInsteadOfOnUp(newUseOnReleaseInsteadOfOnUp);
            GUI.changed = true;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(upDownButton);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

[CustomEditor(typeof(ActionCamera))]
public class ActionCameraEditor : Editor
{

    private BoxBoundsHandle m_BoundsHandle = new BoxBoundsHandle();

    protected virtual void OnSceneGUI()
    {
        ActionCamera boundsExample = (ActionCamera)target;

        // copy the target object's data to the handle
        m_BoundsHandle.center = boundsExample.cameraBounds.center;
        m_BoundsHandle.size = boundsExample.cameraBounds.size;

        // draw the handle
        EditorGUI.BeginChangeCheck();
        m_BoundsHandle.DrawHandle();
        if (EditorGUI.EndChangeCheck())
        {
            // record the target object before setting new values so changes can be undone/redone
            Undo.RecordObject(boundsExample, "Change Bounds");

            // copy the handle's updated data back to the target object
            Bounds newBounds = new Bounds();
            newBounds.center = m_BoundsHandle.center;
            newBounds.size = m_BoundsHandle.size;
            boundsExample.cameraBounds = newBounds;
        }
    }
}

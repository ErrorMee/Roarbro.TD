//reference https://gist.github.com/mob-sakai/0a31db3582c5c2bda587916e03028a3e

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// Component converter for editor.
/// </summary>
public static class ComponentConverter
{
    /// <summary>
    /// Verify whether it can be converted to the specified component.
    /// </summary>
    public static bool CanConvertTo<T>(Object context) where T : MonoBehaviour
    {
        return context && context.GetType() != typeof(T);
    }

    /// <summary>
    /// Convert to the specified component.
    /// </summary>
    public static Object ConvertTo<T>(Object context) where T : MonoBehaviour
    {
        var target = context as MonoBehaviour;
        var so = new SerializedObject(target);
        so.Update();

        bool oldEnable = target.enabled;
        target.enabled = false;

        // Find MonoScript of the specified component.
        foreach (var script in Resources.FindObjectsOfTypeAll<MonoScript>())
        {
            if (script.GetClass() != typeof(T))
                continue;

            // Set 'm_Script' to convert.
            so.FindProperty("m_Script").objectReferenceValue = script;
            so.ApplyModifiedProperties();
            break;
        }

        (so.targetObject as MonoBehaviour).enabled = oldEnable;
        return so.targetObject;
    }

    public static void CopyComponentProperties(Component sourceComponent, Component targetComponent)
    {
        // 获取源组件的所有属性
        var sourceProps = sourceComponent.GetType().GetProperties();

        foreach (var prop in sourceProps)
        {
            // 检查属性是否可写
            if (prop.CanWrite)
            {
                // 获取源属性的值
                var value = prop.GetValue(sourceComponent, null);

                // 将值应用到目标属性
                prop.SetValue(targetComponent, value, null);
            }
        }
    }
}
#endif
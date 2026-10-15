// =============================================================================
// 文件：SerializableDictionaryDrawer.cs
// 作用：在检视面板中绘制 SerializableDictionary 的键值对。
// 约束：本文件只依赖 UnityEditor，必须放在 Editor 目录中。
//       玩家构建不会编译 Editor 目录，因此不会再把编辑器类型打进玩家程序集。
// =============================================================================

using UnityEditor;
using UnityEngine;

/// <summary>
/// 为 <see cref="SerializableDictionary{TKey, TValue}"/> 提供检视面板绘制。
/// </summary>
[CustomPropertyDrawer(typeof(SerializableDictionary<,>))]
public class SerializableDictionaryDrawer : PropertyDrawer
{
    private const float lineHeight = 18f;

    /// <summary>
    /// 绘制字典标题、每一对键值，以及用于追加空项的按钮。
    /// </summary>
    /// <param name="position">当前属性在检视面板中的矩形区域。</param>
    /// <param name="property">字典对应的序列化属性，包含 keys 与 values。</param>
    /// <param name="label">检视面板上显示的属性名。</param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // 获取 keys 和 values 的 SerializedProperty
        SerializedProperty keysProperty = property.FindPropertyRelative("keys");
        SerializedProperty valuesProperty = property.FindPropertyRelative("values");

        // 计算总行数
        int numElements = Mathf.Max(keysProperty.arraySize, valuesProperty.arraySize);

        // 显示标题
        position.height = lineHeight;
        EditorGUI.LabelField(position, label.text);

        // 遍历每个键值对
        for (int i = 0; i < numElements; i++)
        {
            position.y += lineHeight;

            // 获取当前键和值的 SerializedProperty
            SerializedProperty keyProperty = keysProperty.GetArrayElementAtIndex(i);
            SerializedProperty valueProperty = valuesProperty.GetArrayElementAtIndex(i);

            // 显示键值对
            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 40f; // 设置键的标签宽度
            Rect keyRect = new Rect(position.x, position.y, position.width * 0.4f, lineHeight);
            Rect valueRect = new Rect(position.x + position.width * 0.4f, position.y, position.width * 0.6f, lineHeight);

            EditorGUI.PropertyField(keyRect, keyProperty, new GUIContent("Key"));
            EditorGUI.PropertyField(valueRect, valueProperty, new GUIContent("Value"));

            EditorGUIUtility.labelWidth = labelWidth; // 恢复默认标签宽度
        }

        // 添加新的键值对
        position.y += lineHeight;
        if (GUI.Button(new Rect(position.x, position.y, position.width, lineHeight), "Add New Pair"))
        {
            // 向 keys 和 values 列表中添加新的空元素
            keysProperty.InsertArrayElementAtIndex(keysProperty.arraySize);
            valuesProperty.InsertArrayElementAtIndex(valuesProperty.arraySize);
        }

        EditorGUI.EndProperty();
    }

    /// <summary>
    /// 按键值对数量计算属性占用高度，包含标题行和添加按钮行。
    /// </summary>
    /// <param name="property">字典对应的序列化属性，包含 keys 与 values。</param>
    /// <param name="label">检视面板上显示的属性名。</param>
    /// <returns>该属性在检视面板中应占的像素高度。</returns>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty keysProperty = property.FindPropertyRelative("keys");
        SerializedProperty valuesProperty = property.FindPropertyRelative("values");

        int numElements = Mathf.Max(keysProperty.arraySize, valuesProperty.arraySize);
        return lineHeight * (numElements + 2); // 每个键值对一行，加上标题和添加按钮
    }
}

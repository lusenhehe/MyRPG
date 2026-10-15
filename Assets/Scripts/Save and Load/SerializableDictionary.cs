using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
        {
            GameManager.instance.consule_UI.AddDebug("<系统>:错误Error! \nNumber of keys is not equal to number of values!");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            TKey key = keys[i];
            TValue value = values[i];

            // 检查键是否已经存在
            if (!this.ContainsKey(key))
            {
                this.Add(key, value);
            }
            else
            {
                // 如果键已经存在，记录错误信息
                //GameManager.instance.consule_UI.AddDebug($"<系统>:警告Warning! \nDuplicate key found: {key}. Skipping this key-value pair.");
            }
        }
    }

}

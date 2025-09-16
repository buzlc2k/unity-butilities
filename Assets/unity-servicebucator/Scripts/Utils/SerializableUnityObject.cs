using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class SerializableUnityObject : ISerializationCallbackReceiver
{
    [SerializeField]
    private string m_guid;
    [SerializeField]
    private string m_type;
    private UnityEngine.Object uObject;

    public SerializableUnityObject() { }

    public SerializableUnityObject(UnityEngine.Object uObject)
    {
        m_guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(uObject));
        m_type = uObject.GetType().FullName;
        this.uObject = uObject;
    }

    private void SetObject()
    {
        if (string.IsNullOrEmpty(m_guid))
            return;

        try
        {
            string uObjPath = AssetDatabase.GUIDToAssetPath(m_guid);
            uObject = AssetDatabase.LoadAssetAtPath(uObjPath, Type.GetType(m_type));
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Could not deserialize object '{m_guid}': {ex.Message}");
            return;
        }
    }

    public void OnBeforeSerialize(){ }

    public void OnAfterDeserialize()
        => SetObject();

    public static implicit operator string(SerializableUnityObject serializableUnityObject)
        => serializableUnityObject.m_guid;

    public static implicit operator UnityEngine.Object(SerializableUnityObject serializableUnityObject)
        => serializableUnityObject.uObject;
}
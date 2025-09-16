using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableType : ISerializationCallbackReceiver
{
    [SerializeField]
    private string m_type;
    private Type type;
    private static Dictionary<Type, SerializableType> typesMap;

    public static SerializableType Of(Type type)
    {
        typesMap ??= new();

        if (type == null)
            throw new ArgumentNullException(nameof(type), "type cannot be null.");

        if (!typesMap.ContainsKey(type))
            typesMap[type] = new SerializableType(type);

        return typesMap[type];
    }

    public static SerializableType Of<T>()
        where T : class
    {
        var type = typeof(T);

        typesMap ??= new();

        if (type == null)
            throw new ArgumentNullException(nameof(type), "type cannot be null.");

        if (!typesMap.ContainsKey(type))
            typesMap[type] = new SerializableType(type);

        return typesMap[type];
    }

    public SerializableType() { }

    private SerializableType(Type type)
    {
        m_type = type.AssemblyQualifiedName;
        this.type = type;
    }

    public void SetType()
    {
        if (string.IsNullOrEmpty(m_type))
            return;

        try
        {
            type = Type.GetType(m_type);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Could not deserialize type '{m_type}': {ex.Message}");
            return;
        }
    }

    public void OnBeforeSerialize(){ }

    public void OnAfterDeserialize()
    {
        if (string.IsNullOrEmpty(m_type))
            return;

        SetType();

        typesMap ??= new();
        typesMap[type] = this;
    }

    public static implicit operator string(SerializableType serializableType)
        => serializableType.m_type;

    public static implicit operator Type(SerializableType serializableType)
        => serializableType.type;
}
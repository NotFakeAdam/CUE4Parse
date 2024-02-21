using System.Runtime.InteropServices;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Writers;

namespace CUE4Parse.UE4.Objects.Core.Serialization;

/// <summary>
/// Structure to hold unique custom key with its version.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FCustomVersion(FGuid key, int version) : ISerializable
{
    /** Unique custom key. */
    public FGuid Key = key;

    /** Custom version */
    public int Version = version;

    public override bool Equals(object? obj) => obj is FCustomVersion other && Equals(other);
    public bool Equals(FCustomVersion other) => Key == other.Key && Version == other.Version;
    public static bool operator ==(FCustomVersion one, FCustomVersion two) => one.Key == two.Key && one.Version == two.Version;
    public static bool operator !=(FCustomVersion one, FCustomVersion two) => one.Key != two.Key || one.Version != two.Version;

    public override string ToString() => $"{nameof(Key)}: {Key}, {nameof(Version)}: {Version}";
    public override int GetHashCode() => HashCode.Combine(Key, Version);
    
    public void Serialize(FArchiveWriter Ar)
    {
        Ar.Serialize(Key);
        Ar.Write(Version);
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FEnumCustomVersion_DEPRECATED
{
    public uint Tag;
    public int Version;

    public FCustomVersion ToCustomVersion() => new(new FGuid(0, 0, 0, Tag), Version);
}

public struct FGuidCustomVersion_DEPRECATED
{
    public FGuid Tag;
    public int Version;
    public string FriendlyName;

    public FGuidCustomVersion_DEPRECATED(FArchive Ar)
    {
        Tag = Ar.Read<FGuid>();
        Version = Ar.Read<int>();
        FriendlyName = Ar.ReadFString();
    }

    public FCustomVersion ToCustomVersion() => new(Tag, Version);
}

public enum ECustomVersionSerializationFormat : byte
{
    Unknown,
    Guids,
    Enums,
    Optimized,

    // Add new versions above this comment
    CustomVersion_Automatic_Plus_One,
    Latest = CustomVersion_Automatic_Plus_One - 1
}

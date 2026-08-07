using Content.Server.Maps;
using Content.Shared.Maps;
using Content.Shared.Whitelist;
using Robust.Shared.Map;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server._Sunrise.StationCentComm;

[RegisterComponent]
public sealed partial class StationCentCommComponent : Component
{
    [DataField(customTypeSerializer:typeof(PrototypeIdSerializer<GameMapPrototype>), required: true)]
    public string Station = default!;

    [DataField]
    public EntityUid Entity = EntityUid.Invalid;

    [DataField]
    public EntityWhitelist? ShuttleWhitelist;

    [DataField]
    public LocId? GridName;

    public MapId MapId = MapId.Nullspace;
}

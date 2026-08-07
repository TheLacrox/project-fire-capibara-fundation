using Content.Server._Scp.Localization.Components;
using Robust.Shared.GameObjects;

namespace Content.Server._Scp.Localization.Systems;

public sealed class LocalizedGridNameSystem : EntitySystem
{
    [Dependency] private readonly MetaDataSystem _metaData = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<LocalizedGridNameComponent, ComponentStartup>(OnStartup);
    }

    private void OnStartup(Entity<LocalizedGridNameComponent> ent, ref ComponentStartup args)
    {
        _metaData.SetEntityName(ent, Loc.GetString(ent.Comp.Name));
    }
}

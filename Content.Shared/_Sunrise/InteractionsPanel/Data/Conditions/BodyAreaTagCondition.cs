using Robust.Shared.Containers;
using Robust.Shared.Serialization;
using Content.Shared.Tag;

namespace Content.Shared._Sunrise.InteractionsPanel.Data.Conditions;

[Serializable, NetSerializable, DataDefinition]
public sealed partial class BodyAreaTagCondition : IAppearCondition
{
    [DataField]
    public bool CheckInitiator { get; private set; }

    [DataField]
    public bool CheckTarget { get; private set; } = true;

    [DataField]
    public bool RequireExposed { get; private set; } = true;

    [DataField(required: true)]
    public HashSet<string> Categories { get; private set; } = new();

    public bool IsMet(EntityUid initiator, EntityUid target, EntityManager entityManager)
    {
        if (CheckInitiator && !CheckEntity(initiator, entityManager))
            return false;

        if (CheckTarget && !CheckEntity(target, entityManager))
            return false;

        return true;
    }

    private bool CheckEntity(EntityUid entity, EntityManager entMan)
    {
        if (!entMan.TryGetComponent<ContainerManagerComponent>(entity, out var inventory))
            return RequireExposed;

        var restricted = GetCoveredCategories(entMan, inventory);
        foreach (var category in Categories)
        {
            var isCovered = restricted.Contains(category);

            if (RequireExposed && isCovered)
                return false;

            if (!RequireExposed && !isCovered)
                return false;
        }

        return true;
    }

    private HashSet<string> GetCoveredCategories(EntityManager entMan, ContainerManagerComponent inventory)
    {
        var result = new HashSet<string>();

        foreach (var (slot, container) in inventory.Containers)
        {
            if (container.ContainedEntities.Count == 0)
                continue;

            var ent = container.ContainedEntities[0];

            if (!entMan.TryGetComponent<TagComponent>(ent, out var tags))
                continue;

            result.UnionWith(GetCategoriesBySlotAndTags(slot, tags));
        }

        return result;
    }

    private HashSet<string> GetCategoriesBySlotAndTags(string slot, TagComponent tags)
    {
        var set = new HashSet<string>();

        switch (slot)
        {
            case "jumpsuit":
                set.UnionWith(new[] { "chest", "thighs", "buttocks" });
                if (tags.Tags.Contains("NudeBottom")) set = new() { "chest" };
                if (tags.Tags.Contains("NudeTop")) set = new() { "thighs", "buttocks" };
                if (tags.Tags.Contains("CommandSuit")) set = new() { "chest", "thighs", "buttocks" };
                break;

            case "outerClothing":
                set.UnionWith(new[] { "chest", "thighs", "buttocks" });
                if (tags.Tags.Contains("NudeBottom")) set = new() { "chest" };
                if (tags.Tags.Contains("NudeFull")) set.Clear();
                if (tags.Tags.Contains("FullCovered")) set = new() {
                    "cheeks", "lips", "neck", "ears", "hair",
                    "mouth", "chest", "feet", "thighs", "buttocks", "face", "tail", "palms", "smooth-gloves"
                };
                if (tags.Tags.Contains("FullBodyOuter")) set = new() {
                    "chest", "feet", "thighs", "buttocks", "neck", "palms", "smooth-gloves"
                };
                break;

            case "head":
                set.UnionWith(new[] { "hair" });
                if (tags.Tags.Contains("TopCovered")) set = new() { "ears", "hair" };
                if (tags.Tags.Contains("FullCovered")) set = new() { "ears", "hair", "mouth", "face", "lips", "cheeks" };
                break;

            case "gloves":
                set.UnionWith(new[] { "palms", "smooth-gloves" });
                if (tags.Tags.Contains("SmoothGloves")) set = new() { "palms" };
                if (tags.Tags.Contains("Ring")) set.Clear();
                break;

            case "neck":
                set.UnionWith(new[] { "neck" });
                if (tags.Tags.Contains("OpenNeck")) set.Clear();
                break;

            case "mask":
                set.UnionWith(new[] { "mouth" });
                if (tags.Tags.Contains("FaceCovered")) set = new() { "mouth", "cheeks", "face" };
                break;

            case "bra":
                set.UnionWith(new[] { "chest" });
                break;

            case "socks":
                set.UnionWith(new[] { "feet" });
                break;

            case "shoes":
                set.UnionWith(new[] { "socks", "feet" });
                break;
        }

        return set;
    }
}

using Robust.Shared.Localization;

namespace Content.Shared.Starlight.Medical.Surgery;
// Based on the RMC14.
// https://github.com/RMC-14/RMC-14
public interface ISurgeryToolComponent
{
    public LocId ToolName { get; } // Fire edit - локализуемое название инструмента
}

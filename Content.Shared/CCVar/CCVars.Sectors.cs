using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    /// <summary>
    /// Radius around the world origin that resolves to the center sector.
    /// </summary>
    public static readonly CVarDef<float> SectorCenterRadius =
        CVarDef.Create("sector.center_radius", 1250f, CVar.ARCHIVE | CVar.REPLICATED | CVar.SERVER);

    /// <summary>
    /// Reserved maximum radius for future sector-based systems.
    /// </summary>
    public static readonly CVarDef<float> SectorMaxRadius =
        CVarDef.Create("sector.max_radius", 50000f, CVar.ARCHIVE | CVar.REPLICATED | CVar.SERVER);
}
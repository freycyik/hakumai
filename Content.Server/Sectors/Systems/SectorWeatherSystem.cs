using Content.Server.Sectors.Events;
using Content.Shared.Sectors;
using Content.Shared.Sectors.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.Server.Sectors.Systems;

/// <summary>
/// Tracks active sector weather events and broadcasts changes for UI systems.
/// </summary>
public sealed class SectorWeatherSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototypes = default!;

    private readonly Dictionary<SpaceSector, string> _activeWeather = new();

    public Dictionary<SpaceSector, string> GetWeatherSnapshot()
    {
        return new Dictionary<SpaceSector, string>(_activeWeather);
    }

    public bool TrySetWeather(SpaceSector sector, string weatherId)
    {
        if (!_prototypes.HasIndex<SectorWeatherPrototype>(weatherId))
            return false;

        _activeWeather[sector] = weatherId;
        RaiseLocalEvent(new SectorWeatherChangedEvent(sector, weatherId));
        return true;
    }

    public bool ClearWeather(SpaceSector sector)
    {
        if (!_activeWeather.Remove(sector))
            return false;

        RaiseLocalEvent(new SectorWeatherChangedEvent(sector, null));
        return true;
    }
}

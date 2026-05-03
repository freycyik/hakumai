using Robust.Shared.Maths;
using Robust.Shared.Prototypes;

namespace Content.Shared.Sectors.Prototypes;

[Prototype("sectorWeather")]
public sealed partial class SectorWeatherPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField]
    public string Name { get; private set; } = string.Empty;

    [DataField(required: true)]
    public Color BorderColor { get; private set; } = Color.White;
}

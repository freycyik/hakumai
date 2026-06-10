using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared._Persistence14.RandomTable;

public sealed partial class RandomTableContext
{
    public required IRobustRandom Random { get; init; }
    public required IPrototypeManager PrototypeManager { get; init; }
    public required RandomTableSystem RandomTableSystem { get; init; }
    public required IEntityManager EntityManager { get; init; }

    public EntityUid? User { get; init; }
    public EntityUid? Source { get; init; }
    public EntityUid? Target { get; init; }
}
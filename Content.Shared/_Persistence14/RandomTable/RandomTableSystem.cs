using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared._Persistence14.RandomTable;

public sealed partial class RandomTableSystem : EntitySystem
{
    [Dependency] private IRobustRandom _random = default!;
    [Dependency] private IPrototypeManager _protoMan = default!;
    [Dependency] private IEntityManager _entMan = default!;

    private static RandomTableContext? _ctx = null;

    public override void Initialize()
    {

    }

    # region Run
    /// <summary>
    /// Runs the random table, fetching all rolled and valid items from the table in the provided prototype.
    /// </summary>
    public IEnumerable<RandomTableValue> Run(ProtoId<RandomTablePrototype> tableProtoId) => Run(_protoMan.Index(tableProtoId).Table);

    public IEnumerable<RandomTableValue> Run(RandomTableSelector table, RandomTableContext? ctx = null)
    {
        ctx ??= AssembleContext();
        foreach (var item in table.Run(ctx))
            yield return item;
    }

    /// <summary>
    /// Runs the random table, fetching all rolled and valid integer items (including converted floats) from the table in the provided prototype.
    /// </summary>
    public IEnumerable<int> RunInt(ProtoId<RandomTablePrototype> tableProtoId) => RunInt(_protoMan.Index(tableProtoId).Table);

    /// <summary>
    /// Runs the random table, fetching all rolled and valid integer items (including converted floats) from the table in the provided prototype.
    /// </summary>
    public IEnumerable<int> RunInt(RandomTableSelector table)
    {
        foreach (var item in Run(table))
            if (item.TryGetInt(out var val))
                yield return val.Value;
    }

    /// <summary>
    /// Runs the random table, fetching all rolled and valid float items (including converted integers) from the table in the provided prototype.
    /// </summary>
    public IEnumerable<float> RunFloat(ProtoId<RandomTablePrototype> tableProtoId) => RunFloat(_protoMan.Index(tableProtoId).Table);

    /// <summary>
    /// Runs the random table, fetching all rolled and valid float items (including converted integers) from the table in the provided prototype.
    /// </summary>
    public IEnumerable<float> RunFloat(RandomTableSelector table)
    {
        foreach (var item in Run(table))
            if (item.TryGetFloat(out var val))
                yield return val.Value;
    }

    /// <summary>
    /// Runs the random table, fetching all rolled and valid integer string from the table in the provided prototype.
    /// </summary>
    public IEnumerable<string> RunString(ProtoId<RandomTablePrototype> tableProtoId) => RunString(_protoMan.Index(tableProtoId).Table);

    /// <summary>
    /// Runs the random table, fetching all rolled and valid integer string from the table in the provided prototype.
    /// </summary>
    public IEnumerable<string> RunString(RandomTableSelector table)
    {
        foreach (var item in Run(table))
            if (item.TryGetString(out var val))
                yield return val;
    }

    /// <summary>
    /// Runs the random table, fetching all rolled and valid prototype items from the table in the provided prototype.
    /// </summary>
    public IEnumerable<T> RunPrototype<T>(ProtoId<RandomTablePrototype> tableProtoId) where T : class, IPrototype => RunPrototype<T>(_protoMan.Index(tableProtoId).Table);

    /// <summary>
    /// Runs the random table, fetching all rolled and valid prototype items from the table in the provided prototype.
    /// </summary>
    public IEnumerable<T> RunPrototype<T>(RandomTableSelector table) where T : class, IPrototype
    {
        foreach (var item in Run(table))
            if (item.TryGetPrototype<T>(_protoMan, out var proto))
                yield return proto;
    }
    # endregion

    # region List
    public IEnumerable<(RandomTableValue value, float prob)> List(ProtoId<RandomTablePrototype> tableProtoId, RandomTableContext? ctx = null) => List(_protoMan.Index(tableProtoId).Table, ctx);
    public IEnumerable<(RandomTableValue value, float prob)> List(RandomTableSelector table, RandomTableContext? ctx = null)
    {
        ctx ??= AssembleContext();
        foreach (var (value, prob) in table.List(ctx))
            yield return (value, prob);
    }

    public IEnumerable<(int value, float prob)> ListInt(ProtoId<RandomTablePrototype> tableProtoId, RandomTableContext? ctx = null) => ListInt(_protoMan.Index(tableProtoId).Table, ctx);
    public IEnumerable<(int value, float prob)> ListInt(RandomTableSelector table, RandomTableContext? ctx = null)
    {
        foreach (var (value, prob) in List(table, ctx))
            if (value.TryGetInt(out var val))
                yield return (val.Value, prob);
    }

    public IEnumerable<(float value, float prob)> ListFloat(ProtoId<RandomTablePrototype> tableProtoId, RandomTableContext? ctx = null) => ListFloat(_protoMan.Index(tableProtoId).Table, ctx);
    public IEnumerable<(float value, float prob)> ListFloat(RandomTableSelector table, RandomTableContext? ctx = null)
    {
        foreach (var (value, prob) in List(table, ctx))
            if (value.TryGetFloat(out var val))
                yield return (val.Value, prob);
    }

    public IEnumerable<(string value, float prob)> ListString(ProtoId<RandomTablePrototype> tableProtoId, RandomTableContext? ctx = null) => ListString(_protoMan.Index(tableProtoId).Table, ctx);
    public IEnumerable<(string value, float prob)> ListString(RandomTableSelector table, RandomTableContext? ctx = null)
    {
        foreach (var (value, prob) in List(table, ctx))
            if (value.TryGetString(out var val))
                yield return (val, prob);
    }

    public IEnumerable<(T prototype, float prob)> ListPrototype<T>(ProtoId<RandomTablePrototype> tableProtoId, RandomTableContext ctx) where T : class, IPrototype => ListPrototype<T>(_protoMan.Index(tableProtoId).Table, ctx);
    public IEnumerable<(T prototype, float prob)> ListPrototype<T>(RandomTableSelector table, RandomTableContext? ctx = null) where T : class, IPrototype
    {
        foreach (var (value, prob) in List(table, ctx))
            if (value.TryGetPrototype<T>(_protoMan, out var proto))
                yield return (proto, prob);

    }
    #endregion

    private RandomTableContext AssembleContext()
    {
        if (_ctx is not null)
            return _ctx;

        _ctx = new RandomTableContext()
        {
            Random = _random,
            PrototypeManager = _protoMan,
            RandomTableSystem = this,
            EntityManager = _entMan
        };

        return _ctx;
    }
}
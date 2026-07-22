using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Shared.Map;

namespace Content.Client._Art.Tiles;

[UsedImplicitly]
public sealed class TileSmoothSystem : EntitySystem
{
    [Dependency] private readonly IOverlayManager _overlayMan = default!;

    private TileSmoothOverlay _overlay = default!;

    public override void Initialize()
    {
        base.Initialize();
        _overlay = new TileSmoothOverlay();
        _overlayMan.AddOverlay(_overlay);
        SubscribeLocalEvent<TileChangedEvent>(OnTileChanged);
    }

    public override void Shutdown()
    {
        base.Shutdown();
        _overlayMan.RemoveOverlay<TileSmoothOverlay>();
    }

    private void OnTileChanged(ref TileChangedEvent ev)
    {
        foreach (var change in ev.Changes)
        {
            var pos = change.GridIndices;
            for (var dx = -1; dx <= 1; dx++)
            for (var dy = -1; dy <= 1; dy++)
                _overlay.InvalidateCache(ev.Entity.Owner, pos + new Vector2i(dx, dy));
        }
    }
}
using Content.Server.Cargo.Components;
using Content.Shared.Cargo;
using Content.Shared.Cargo.Components;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Robust.Shared.Map;

namespace Content.Server.Cargo.Systems;

public sealed partial class CargoSystem
{
    private void InitializeDispenser()
    {
        SubscribeLocalEvent<CargoDispenserComponent, InteractUsingEvent>(OnDispenserInteractUsing);
    }

    private void OnDispenserInteractUsing(EntityUid uid, CargoDispenserComponent component, ref InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        if (!TryComp<CargoOrderTicketComponent>(args.Used, out var ticket))
            return;

        args.Handled = true;

        var gridUid = Transform(uid).GridUid;
        if (gridUid == null || !TryComp<TradeStationComponent>(gridUid.Value, out var tradeStation))
        {
            _popup.PopupEntity(Loc.GetString("cargo-ticket-not-trade-station"), uid, args.User);
            _audio.PlayPvs(component.DenySound, uid);
            return;
        }

        if (tradeStation.UID != ticket.TradeStation)
        {
            _popup.PopupEntity(
                Loc.GetString("cargo-ticket-wrong-station", ("station", ticket.TradeStationName)),
                uid, args.User);
            _audio.PlayPvs(component.DenySound, uid);
            return;
        }

        var tradePads = GetCargoPallets(gridUid.Value, BuySellType.Buy);
        _random.Shuffle(tradePads);
        var freePads = GetFreeCargoPallets(gridUid.Value, tradePads);

        if (freePads.Count < ticket.OrderQuantity)
        {
            _popup.PopupEntity(Loc.GetString("cargo-console-unfulfilled"), uid, args.User);
            _audio.PlayPvs(component.DenySound, uid);
            return;
        }

        var order = new CargoOrderData(
            ticket.OrderId,
            ticket.Product,
            ticket.OrderQuantity,
            ticket.Requester,
            ticket.Reason,
            ticket.Account,
            ticket.TradeStation
        );
        order.SetApproverData(ticket.Approver);
        order.Approved = true;

        int dispatched = 0;
        foreach (var pad in freePads)
        {
            var coords = new EntityCoordinates(gridUid.Value, pad.Transform.LocalPosition);
            if (FulfillOrder(order, order.Account, coords, component.PrinterOutput, ticket.PersonalAccount))
            {
                dispatched++;
                if (dispatched >= order.OrderQuantity)
                    break;
            }
        }

        _audio.PlayPvs(component.RedeemSound, uid);
        _popup.PopupEntity(
            Loc.GetString("cargo-ticket-redeemed", ("station", ticket.TradeStationName)),
            uid, args.User);

        QueueDel(args.Used);
    }
}
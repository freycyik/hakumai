using Content.Shared.Cargo.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.Shared.Cargo.Components;

[RegisterComponent]
public sealed partial class CargoOrderTicketComponent : Component
{
    [DataField] public int OrderId;
    [DataField] public ProtoId<CargoProductPrototype> Product;
    [DataField] public int OrderQuantity;
    [DataField] public string Requester = string.Empty;
    [DataField] public string Reason = string.Empty;
    [DataField] public string? Approver;
    [DataField] public ProtoId<CargoAccountPrototype> Account;
    [DataField] public int TradeStation;
    [DataField] public string TradeStationName = string.Empty;
    [DataField] public string? PersonalAccount;
}
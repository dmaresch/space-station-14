using Content.Shared.Chemistry.Components;
using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared._Impstation.Body.Eui;

[Serializable, NetSerializable]
public sealed class BloodstreamReferenceEuiState(Solution bloodstreamReference, NetEntity target) : EuiStateBase
{
    public Solution BloodstreamReference { get; } = bloodstreamReference;
    public NetEntity Target { get; } = target;
}

[Serializable, NetSerializable]
public sealed class BloodstreamReferenceSaveMessage(Solution bloodstreamReference, NetEntity target) : EuiMessageBase
{
    public Solution BloodstreamReference { get; } = bloodstreamReference;
    public NetEntity Target { get; } = target;
}

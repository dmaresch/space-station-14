using System.Diagnostics.CodeAnalysis;
using Content.Server.Administration.Managers;
using Content.Server.Body.Systems;
using Content.Server.EUI;
using Content.Shared._Impstation.Body.Eui;
using Content.Shared.Administration;
using Content.Shared.Body.Components;
using Content.Shared.Eui;

namespace Content.Server._Impstation.Body.Eui;
public sealed class BloodstreamReferenceEui(
    BloodstreamSystem bloodstream,
    EntityManager entity,
    IAdminManager admin) : BaseEui
{
    private readonly ISawmill _sawmill = Logger.GetSawmill("bloodstream-eui");

    private EntityUid _target;
    private BloodstreamComponent? _bloodstream;

    public override EuiStateBase GetNewState()
    {
        var bloodstreamReference = _bloodstream?.BloodReferenceSolution!;
        return new BloodstreamReferenceEuiState(bloodstreamReference, entity.GetNetEntity(_target));
    }

    private bool IsAllowed()
    {
        var adminData = admin.GetAdminData(Player);

        if (adminData != null && adminData.HasFlag(AdminFlags.Moderator))
            return true;

        _sawmill.Warning($"Player {Player.UserId} tried to open / use strange moods UI without permission.");
        return false;
    }

    public void UpdateBloodstream(EntityUid uid, BloodstreamComponent comp)
    {
        if (!IsAllowed())
            return;

        _target = uid;
        _bloodstream = comp;
        StateDirty();
    }

    public void UpdateBloodstream(Entity<BloodstreamComponent> ent)
    {
        UpdateBloodstream(ent, ent.Comp);
    }

    public override void HandleMessage(EuiMessageBase msg)
    {
        base.HandleMessage(msg);

        if (!IsAllowed())
            return;

        var saveData = (BloodstreamReferenceSaveMessage)msg;
        var uid = entity.GetEntity(saveData.Target);

        if (!HasBloodstream(saveData.Target, out uid, out var comp))
            return;

        bloodstream.ChangeBloodReagents((uid, comp), saveData.BloodstreamReference);
    }

    public bool HasBloodstream(NetEntity ent, out EntityUid uid, [NotNullWhen(true)] out BloodstreamComponent? comp)
    {
        uid = entity.GetEntity(ent);
        if (entity.TryGetComponent(uid, out comp))
            return true;

        _sawmill.Warning($"Entity {entity.ToPrettyString(uid)} does not have BloodstreamComponent!");
        return false;
    }
}

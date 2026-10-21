using Content.Client.Eui;
using Content.Shared._Impstation.Body.Eui;
using Content.Shared._Impstation.StrangeMoods;
using Content.Shared._Impstation.StrangeMoods.Eui;
using Content.Shared.Eui;
using Robust.Client.UserInterface.Controls;

namespace Content.Client._Impstation.Body.Eui;

public sealed class BloodstreamReferenceEui : BaseEui
{
    private readonly BloodstreamReferenceUi _bloodstreamReferenceUi;
    private NetEntity _target;

    public BloodstreamReferenceEui()
    {
        _bloodstreamReferenceUi = new BloodstreamReferenceUi();
        _bloodstreamReferenceUi.OnSave += SaveBloodstreamReference;
    }
    private void SaveBloodstreamReference()
    {
        var newBlood = _bloodstreamReferenceUi.GetBloodstreamReference();
        if (newBlood == null)
            return;

        SendMessage(new BloodstreamReferenceSaveMessage(newBlood, _target));
    }
    public override void Opened()
    {
        base.Opened();
        _bloodstreamReferenceUi.OpenCentered();
    }
    public override void Closed()
    {
        base.Closed();
        _bloodstreamReferenceUi.Close();
    }

    public override void HandleState(EuiStateBase state)
    {
        if (state is not BloodstreamReferenceEuiState s)
            return;

        _target = s.Target;
        _bloodstreamReferenceUi.SetBloodstreamReferenceSolution(s.BloodstreamReference);
    }
}

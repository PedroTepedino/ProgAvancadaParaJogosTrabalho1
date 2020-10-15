public class PausePanel : AbstractMenuPanel
{
    protected override void HandleGameStateChanged(IState state)
    {
        _panel.SetActive(state is PauseState);
    }
}
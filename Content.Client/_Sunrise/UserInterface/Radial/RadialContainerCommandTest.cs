using Robust.Shared.Console;
using Robust.Shared.Localization;
using Robust.Shared.Random;

namespace Content.Client._Sunrise.UserInterface.Radial;

public sealed partial class RadialContainerCommandTest : LocalizedCommands
{
    [Dependency] private readonly IRobustRandom _robustRandom = default!;

    public override string Command => "radialtest";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        string[] tips =
        {
            Loc.GetString("fire-radial-test-tooltip-info"),
            Loc.GetString("fire-radial-test-tooltip-joke"),
        };
        var radial = new RadialContainer();
        for (int i = 0; i < 8; i++)
        {
            var testButton = radial.AddButton(
                Loc.GetString("fire-radial-test-action", ("index", i)),
                "/Textures/Interface/emotions.svg.192dpi.png");
            testButton.Tooltip = tips[_robustRandom.Next(0, 2)];
            testButton.Controller.OnPressed += (_) => { Logger.Debug("Press gay"); };
        }

        radial.CloseButton.Controller.OnPressed += (_) =>
        {
            Logger.Debug("Close event for your own logic");
        };
        radial.OpenAttachedLocalPlayer();
    }
}

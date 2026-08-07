using Content.Client.Pinpointer.UI;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.SurveillanceCamera.UI;

public sealed partial class SurveillanceCameraNavMapControl : NavMapControl
{
    public NetEntity? Focus;
    public Dictionary<NetEntity, string> LocalizedNames = new();

    private Label _trackedEntityLabel;
    private PanelContainer _trackedEntityPanel;

    public SurveillanceCameraNavMapControl()
    {
        WallColor = new Color(192, 122, 196);
        TileColor = new(71, 42, 72);
        BackgroundColor = Color.FromSrgb(TileColor.WithAlpha(BackgroundOpacity));

        _trackedEntityLabel = new Label
        {
            Margin = new Thickness(10f, 8f),
            HorizontalAlignment = HAlignment.Center,
            VerticalAlignment = VAlignment.Center,
            Modulate = Color.White,
        };

        _trackedEntityPanel = new PanelContainer
        {
            PanelOverride = new StyleBoxFlat
            {
                BackgroundColor = BackgroundColor,
            },

            Margin = new Thickness(5f, 45f),
            HorizontalAlignment = HAlignment.Left,
            VerticalAlignment = VAlignment.Bottom,
            Visible = false,
        };

        _trackedEntityPanel.AddChild(_trackedEntityLabel);
        AddChild(_trackedEntityPanel);
        VerticalExpand = true;
        VerticalAlignment = VAlignment.Stretch;

    }

    protected override void Draw(DrawingHandleScreen handle)
    {
        base.Draw(handle);

        if (Focus == null)
        {
            _trackedEntityLabel.Text = string.Empty;
            _trackedEntityPanel.Visible = false;

            return;
        }

        foreach ((var netEntity, var blip) in TrackedEntities)
        {
            if (netEntity != Focus)
                continue;

            // Fire edit start - локализация сведений об отслеживаемом объекте
            if (!LocalizedNames.TryGetValue(netEntity, out var name))
                name = Loc.GetString("surveillance-camera-monitor-ui-unknown");

            var message = Loc.GetString("surveillance-camera-monitor-ui-tracked-entity",
                ("name", name),
                ("x", MathF.Round(blip.Coordinates.X)),
                ("y", MathF.Round(blip.Coordinates.Y)));
            // Fire edit end

            _trackedEntityLabel.Text = message;
            _trackedEntityPanel.Visible = true;

            return;
        }

        _trackedEntityLabel.Text = string.Empty;
        _trackedEntityPanel.Visible = false;
    }
}

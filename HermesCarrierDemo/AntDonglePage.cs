using HermesLibrary.Devices;
using HermesLibrary.Devices.Ant.EventArgs;
using Microsoft.Maui.Controls.Shapes;

namespace Demo;

public class AntDonglePage : ContentPage
{
    private readonly DeviceService mDeviceService;
    private readonly Label mDevicesCountLabel;
    private readonly Span mStatusSpan;

    public AntDonglePage()
    {
        mDeviceService = new DeviceService();

        mStatusSpan = new Span { Text = "Disconnected", TextColor = Color.FromArgb("#FF0000") };
        mDevicesCountLabel = new Label
        {
            TextColor = Colors.White,
            FontSize = 24,
            FontAttributes = FontAttributes.Bold,
            FormattedText = new FormattedString()
            {
                Spans =
                {
                    new Span { Text = "Devices status: ", TextColor = Color.FromArgb("#FFEAD0") },
                    mStatusSpan
                }
            }
        };

        Content = new StackLayout
        {
            HorizontalOptions = LayoutOptions.Fill,
            Padding = 16,
            BackgroundColor = Color.FromArgb("#37505C"),
            Children =
            {
                mDevicesCountLabel
            }
        };

        mDeviceService.AntService.TransmitterStatusChanged += OnTransmitterStatusChanged;
    }

    private void OnTransmitterStatusChanged(object sender, AntTransmitterStatusChangedEventArgs e)
    {
        mStatusSpan.Text = e.Transmitter.IsConnected ? "Connected" : "Disconnected";
        mStatusSpan.TextColor = e.Transmitter.IsConnected ? Color.FromArgb("#00FF00") : Color.FromArgb("#FF0000");
    }
}
using HermesCarrierDemo;
using HermesCarrierLibrary.Devices;
using HermesCarrierLibrary.Devices.Ant.Channel;
using HermesCarrierLibrary.Devices.Ant.Enum;
using HermesCarrierLibrary.Devices.Ant.EventArgs;

namespace Demo;

public class AntDonglePage : ContentPage
{
    private readonly Label mDevicesCountLabel;
    private readonly DeviceService mDeviceService;
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
            FormattedText = new FormattedString
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

        if (e.Transmitter.IsConnected)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                var transmitter = e.Transmitter;
                var channel = new Channel(0, 1, ChannelType.TransmitChannel, ExtendedAssignmentType.UNKNOWN, 0x2000,
                    0x03);

                var testDevice = new TestDevice();
                testDevice.Open(transmitter, channel).ConfigureAwait(false);
            });
        }
    }
}
using CommunityToolkit.Maui.Markup;
using HermesCarrierDemo;

namespace Demo;

public class AntDonglePage : ContentPage
{
    private readonly Label mDevicesCountLabel;
    private readonly Span mStatusSpan;
    private readonly VerticalStackLayout mValuesListView;
    private readonly AntDongleViewModel mViewModel = new();

    public AntDonglePage()
    {
        BindingContext = mViewModel;

        mStatusSpan =
            new Span { TextColor = Color.FromArgb("#FF0000") }
                .Bind(Span.TextProperty, nameof(mViewModel.Status))
                .Bind(Span.TextColorProperty, nameof(mViewModel.StatusColor));
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
                mDevicesCountLabel,
                new Entry { Placeholder = "Device number", TextColor = Colors.White }.Bind(Entry.TextProperty,
                    nameof(mViewModel.DeviceNumber)),
                new Entry { Placeholder = "Device type", TextColor = Colors.White }.Bind(Entry.TextProperty,
                    nameof(mViewModel.DeviceType)),
                new Entry { Placeholder = "Transmission type", TextColor = Colors.White }.Bind(Entry.TextProperty,
                    nameof(mViewModel.TransmissionType)),
                new Button
                {
                    Text = "Connect", TextColor = Colors.White, BackgroundColor = Color.FromArgb("#FF0000"),
                    Command = mViewModel.ConnectCommand
                },
                new ScrollView
                {
                    Content = new VerticalStackLayout().Assign(out mValuesListView)
                }
            }
        };

        mViewModel.ValueReceived += OnValueReceived;
    }

    private void OnValueReceived(object sender, TestDevice.ValueReceivedEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() => { mValuesListView.Add(new ValueView(e.Value, e.Unit)); });
    }

    private class ValueView : ContentView
    {
        public ValueView(float value, byte unit)
        {
            Content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Fill,
                Padding = 16,
                BackgroundColor = Color.FromArgb("#37505C"),
                Children =
                {
                    new Label
                    {
                        TextColor = Colors.White,
                        FontSize = 24,
                        FontAttributes = FontAttributes.Bold,
                        FormattedText = new FormattedString
                        {
                            Spans =
                            {
                                new Span { Text = "Value: ", TextColor = Color.FromArgb("#FFEAD0") },
                                new Span { Text = value.ToString(), TextColor = Color.FromArgb("#FF0000") }
                            }
                        }
                    },
                    new Label
                    {
                        TextColor = Colors.White,
                        FontSize = 24,
                        FontAttributes = FontAttributes.Bold,
                        FormattedText = new FormattedString
                        {
                            Spans =
                            {
                                new Span { Text = "Unit: ", TextColor = Color.FromArgb("#FFEAD0") },
                                new Span { Text = unit.ToString(), TextColor = Color.FromArgb("#FF0000") }
                            }
                        }
                    }
                }
            };
        }
    }
}
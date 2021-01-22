using System.Linq;
using System.Windows.Input;
using ExpandableFloatingActionButton.Extensions;
using ExpandableFloatingActionButton.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpandableFloatingActionButton
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpandableFloatingActionButton : Frame
    {
        private const int ButtonSize = 52;

        public static readonly BindableProperty ExpandedWidthProperty = BindablePropertyUtils.Create<int, ExpandableFloatingActionButton>(nameof(ExpandedWidth), defaultValue: ButtonSize);

        public static readonly BindableProperty IsExpandedProperty = BindablePropertyUtils.Create<bool, ExpandableFloatingActionButton>(nameof(IsExpanded), button => button.OnExpandedPropertyChanged);

        public static readonly BindableProperty TapCommandProperty =
            BindablePropertyUtils.Create<ICommand, ExpandableFloatingActionButton>(nameof(TapCommand), button => button.OnTapCommandPropertyChanged);

        public static readonly BindableProperty TextProperty = BindablePropertyUtils.Create<string, ExpandableFloatingActionButton>(nameof(Text));

        public static readonly BindableProperty ImageProperty = BindablePropertyUtils.Create<ImageSource, ExpandableFloatingActionButton>(nameof(Image));

        public ExpandableFloatingActionButton()
        {
            InitializeComponent();

            WidthRequest = ButtonSize;
            HeightRequest = ButtonSize;
            HorizontalOptions = LayoutOptions.EndAndExpand;
            VerticalOptions = LayoutOptions.EndAndExpand;
            Margin = 16;

            ImageGrid.WidthRequest = ButtonSize;
            ImageGrid.HeightRequest = ButtonSize;
            ImageGrid.MinimumWidthRequest = ButtonSize;
            ImageGrid.MinimumHeightRequest = ButtonSize;
        }

        public int ExpandedWidth
        {
            get => this.GetProperty<int>();
            set => this.SetProperty(value);
        }

        public bool IsExpanded
        {
            get => this.GetProperty<bool>();
            set => this.SetProperty(value);
        }

        public ICommand TapCommand
        {
            get => this.GetProperty<ICommand>();
            set => this.SetProperty(value);
        }

        public string Text
        {
            get => this.GetProperty<string>();
            set => this.SetProperty(value);
        }

        public ImageSource Image
        {
            get => this.GetProperty<ImageSource>();
            set => this.SetProperty(value);
        }

        private void OnExpandedPropertyChanged(bool oldValue, bool newValue)
        {
            var isExpanded = newValue;

            if (isExpanded)
            {
                _ = this.AnimatePropertyAsync((value, view) => view.WidthRequest = value, Width, ExpandedWidth, length: 100);
                return;
            }

            _ = this.AnimatePropertyAsync((value, view) => view.WidthRequest = value, Width, ButtonSize, length: 100);
        }

        private void OnTapCommandPropertyChanged(ICommand oldValue, ICommand newValue)
        {
            if (newValue is null)
            {
                return;
            }

            if (GestureRecognizers.Any())
            {
                return;
            }

            GestureRecognizers.Add(new TapGestureRecognizer {Command = newValue});
        }
    }
}
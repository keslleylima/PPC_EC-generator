using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PpcEcGenerator.Views
{
    public class MainWindow : Window
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private ContentControl contentControl;


        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            contentControl = this.FindControl<ContentControl>("contentControl");
            contentControl.Content = new HomeView(this);
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void NavigateToHomeView()
        {
            contentControl.Content = new HomeView(this);
        }

        public void NavigateToEndView(string output)
        {
            contentControl.Content = new EndView(this, output);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

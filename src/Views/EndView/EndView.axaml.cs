using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PpcEcGenerator.Style.Color;
using System;

namespace PpcEcGenerator.Views
{
    public class EndView : UserControl
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private MainWindow window;


        //---------------------------------------------------------------------
        //		Constructors
        //---------------------------------------------------------------------
        public EndView()
        {
            InitializeComponent();
            BuildProgressBar();
            SetBackground();
        }

        public EndView(MainWindow window, string output) : this()
        {
            this.window = window;

            SetOutputPath(output);
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SetBackground()
        {
            UserControl pnlDone = this.FindControl<UserControl>("pnlDone");

            pnlDone.Background = ColorBrushFactory.Black();
        }

        private void SetOutputPath(string path)
        {
            TextBlock lblOutputPath = this.FindControl<TextBlock>("lblOutputPath");
            lblOutputPath.Text = path;
        }

        private void BuildProgressBar()
        {
            ProgressBar progressBar = this.FindControl<ProgressBar>("progressBar");

            progressBar.Background = ColorBrushFactory.Theme();
            progressBar.Foreground = ColorBrushFactory.ThemeAccent();
        }

        private async void OnGoBackToHome(object sender, RoutedEventArgs e)
        {
            window.NavigateToHomeView();
        }

        private void OnQuit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}

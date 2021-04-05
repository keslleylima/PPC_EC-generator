using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PpcEcGenerator.Controllers;
using PpcEcGenerator.Style.Color;

namespace PpcEcGenerator.Views
{
    public class HomeView : UserControl
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private TextBox inMetricsRootPath;
        private TextBox inTRPPCFilePrefix;
        private TextBox inTRECFilePrefix;
        private TextBox inTPFilePrefix;
        private TextBox inINFFilePrefix;
        private Button btnGenerate;
        private HomeController homeController;


        //---------------------------------------------------------------------
        //		Constructors
        //---------------------------------------------------------------------
        public HomeView()
        {
            InitializeComponent();
            BuildProgressBar();
            BuildChooseMetricsRootPathButton();
            BuildTRPPCPrefixInput();
            BuildTRECPrefixInput();
            BuildTPPrefixInput();
            BuildINFPrefixInput();
            BuildGenerateButton();
            FetchInputFields();
            SetBackground();
        }

        public HomeView(MainWindow window) : this()
        {
            homeController = new HomeController(window);
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        private void BuildProgressBar()
        {
            ProgressBar progressBar = this.FindControl<ProgressBar>("progressBar");

            progressBar.Background = ColorBrushFactory.Theme();
            progressBar.Foreground = ColorBrushFactory.ThemeAccent();
        }

        private void BuildTRPPCPrefixInput()
        {
            inTRPPCFilePrefix = this.FindControl<TextBox>("inTRPPCFilePrefix");
            inTRPPCFilePrefix.SelectionBrush = ColorBrushFactory.ThemeAccent();
            inTRPPCFilePrefix.CaretBrush = ColorBrushFactory.ThemeAccent();
            inTRPPCFilePrefix.KeyUp += (o, e) => { CheckIfGenerationIsAvailable(); };

            BuildClearPrefixButton("btnClearTRPPCFilePrefix");
        }

        private void BuildClearPrefixButton(string id)
        {
            Button btnClear = this.FindControl<Button>(id);

            btnClear.Background = ColorBrushFactory.ThemeAccent();
        }

        private void BuildTRECPrefixInput()
        {
            inTRECFilePrefix = this.FindControl<TextBox>("inTRECFilePrefix");
            inTRECFilePrefix.SelectionBrush = ColorBrushFactory.ThemeAccent();
            inTRECFilePrefix.CaretBrush = ColorBrushFactory.ThemeAccent();
            inTRECFilePrefix.KeyUp += (o, e) => { CheckIfGenerationIsAvailable(); };

            BuildClearPrefixButton("btnClearTRECFilePrefix");
        }

        private void BuildTPPrefixInput()
        {
            inTPFilePrefix = this.FindControl<TextBox>("inTPFilePrefix");
            inTPFilePrefix.SelectionBrush = ColorBrushFactory.ThemeAccent();
            inTPFilePrefix.CaretBrush = ColorBrushFactory.ThemeAccent();
            inTPFilePrefix.KeyUp += (o, e) => { CheckIfGenerationIsAvailable(); };

            BuildClearPrefixButton("btnClearTPFilePrefix");
        }

        private void BuildINFPrefixInput()
        {
            inINFFilePrefix = this.FindControl<TextBox>("inINFFilePrefix");
            inINFFilePrefix.SelectionBrush = ColorBrushFactory.ThemeAccent();
            inINFFilePrefix.CaretBrush = ColorBrushFactory.ThemeAccent();
            inINFFilePrefix.KeyUp += (o, e) => { CheckIfGenerationIsAvailable(); };

            BuildClearPrefixButton("btnClearINFFilePrefix");
        }

        private void BuildChooseMetricsRootPathButton()
        {
            Button btnChooseMetricsRootPath = this.FindControl<Button>("btnChooseMetricsRootPath");

            btnChooseMetricsRootPath.Background = ColorBrushFactory.ThemeAccent();
        }

        private void BuildGenerateButton()
        {
            btnGenerate = this.FindControl<Button>("btnGenerate");

            btnGenerate.Click += OnGenerate;
            btnGenerate.Background = ColorBrushFactory.ThemeAccent();
        }

        private void FetchInputFields()
        {
            inMetricsRootPath = this.FindControl<TextBox>("inMetricsRootPath");
    }

        private void SetBackground()
        {
            UserControl pnlHome = this.FindControl<UserControl>("pnlHome");

            pnlHome.Background = ColorBrushFactory.Black();
        }

        private void CheckIfGenerationIsAvailable()
        {
            btnGenerate.IsEnabled = AreAllRequiredFieldsProvided();
        }

        private bool AreAllRequiredFieldsProvided()
        {
            return true;
            //return (inMetricsRootPath.Text != "")
            //    && (inMetricsRootPath.Text != null)
            //    && (inTRFilePrefix.Text != "")
            //    && (inTRFilePrefix.Text != null)
            //    && (inTPFilePrefix.Text != "")
            //    && (inTPFilePrefix.Text != null);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void OnChooseMetricsRootPath(object sender, RoutedEventArgs e)
        {
            inMetricsRootPath.Text = await homeController.AskUserForDirectoryPath();
            CheckIfGenerationIsAvailable();
        }

        private void OnClearTRPPCFilePrefix(object sender, RoutedEventArgs e)
        {
            inTRPPCFilePrefix.Text = "";

            CheckIfGenerationIsAvailable();
        }

        private void OnClearTRECFilePrefix(object sender, RoutedEventArgs e)
        {
            inTRECFilePrefix.Text = "";

            CheckIfGenerationIsAvailable();
        }

        private void OnClearTPFilePrefix(object sender, RoutedEventArgs e)
        {
            inTPFilePrefix.Text = "";

            CheckIfGenerationIsAvailable();
        }

        private void OnClearINFFilePrefix(object sender, RoutedEventArgs e)
        {
            inINFFilePrefix.Text = "";

            CheckIfGenerationIsAvailable();
        }

        private void OnClear(object sender, RoutedEventArgs e)
        {
            inMetricsRootPath.Text = "";
            inTRPPCFilePrefix.Text = "";
            inTRECFilePrefix.Text = "";
            inTPFilePrefix.Text = "";
            inINFFilePrefix.Text = "";
            btnGenerate.IsEnabled = false;
        }

        private void OnGenerate(object sender, RoutedEventArgs e)
        {
            homeController.OnGenerate(
                inMetricsRootPath.Text, 
                inTRPPCFilePrefix.Text,
                inTRECFilePrefix.Text,
                inTPFilePrefix.Text, 
                inINFFilePrefix.Text
            );
        }
    }
}

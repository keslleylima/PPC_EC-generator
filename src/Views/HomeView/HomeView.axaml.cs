using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PpcEcGenerator.Controllers;
using PpcEcGenerator.Style.Color;
using System;

namespace PpcEcGenerator.Views
{
    public class HomeView : UserControl
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private TextBox inMetricsRootPath;
        private TextBox inTrPpcFilePrefix;
        private TextBox inTrEcFilePrefix;
        private TextBox inTpFilePrefix;
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
            BuildTrPpcPrefixInput();
            BuildTrEcPrefixInput();
            BuildTpPrefixInput();
            BuildInfPrefixInput();
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

        private void BuildTrPpcPrefixInput()
        {
            inTrPpcFilePrefix = this.FindControl<TextBox>("inTRPPCFilePrefix");
            inTrPpcFilePrefix.SelectionBrush = ColorBrushFactory.ThemeAccent();
            inTrPpcFilePrefix.CaretBrush = ColorBrushFactory.ThemeAccent();
            inTrPpcFilePrefix.KeyUp += (o, e) => { CheckIfGenerationIsAvailable(); };

            BuildClearPrefixButton("btnClearTRPPCFilePrefix");
        }

        private void BuildClearPrefixButton(string id)
        {
            Button btnClear = this.FindControl<Button>(id);

            btnClear.Background = ColorBrushFactory.ThemeAccent();
        }

        private void BuildTrEcPrefixInput()
        {
            inTrEcFilePrefix = this.FindControl<TextBox>("inTRECFilePrefix");
            inTrEcFilePrefix.SelectionBrush = ColorBrushFactory.ThemeAccent();
            inTrEcFilePrefix.CaretBrush = ColorBrushFactory.ThemeAccent();
            inTrEcFilePrefix.KeyUp += (o, e) => { CheckIfGenerationIsAvailable(); };

            BuildClearPrefixButton("btnClearTRECFilePrefix");
        }

        private void BuildTpPrefixInput()
        {
            inTpFilePrefix = this.FindControl<TextBox>("inTPFilePrefix");
            inTpFilePrefix.SelectionBrush = ColorBrushFactory.ThemeAccent();
            inTpFilePrefix.CaretBrush = ColorBrushFactory.ThemeAccent();
            inTpFilePrefix.KeyUp += (o, e) => { CheckIfGenerationIsAvailable(); };

            BuildClearPrefixButton("btnClearTPFilePrefix");
        }

        private void BuildInfPrefixInput()
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
            return (inMetricsRootPath.Text != "")
                && (inMetricsRootPath.Text != null)
                && (inTrPpcFilePrefix.Text != "")
                && (inTrPpcFilePrefix.Text != null)
                && (inTrEcFilePrefix.Text != "")
                && (inTrEcFilePrefix.Text != null)
                && (inTpFilePrefix.Text != "")
                && (inTpFilePrefix.Text != null);
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
            inTrPpcFilePrefix.Text = "";

            CheckIfGenerationIsAvailable();
        }

        private void OnClearTRECFilePrefix(object sender, RoutedEventArgs e)
        {
            inTrEcFilePrefix.Text = "";

            CheckIfGenerationIsAvailable();
        }

        private void OnClearTPFilePrefix(object sender, RoutedEventArgs e)
        {
            inTpFilePrefix.Text = "";

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
            inTrPpcFilePrefix.Text = "";
            inTrEcFilePrefix.Text = "";
            inTpFilePrefix.Text = "";
            inINFFilePrefix.Text = "";
            btnGenerate.IsEnabled = false;
        }

        private void OnGenerate(object sender, RoutedEventArgs e)
        {
            try
            {
                homeController.OnGenerate(
                    inMetricsRootPath.Text,
                    inTrPpcFilePrefix.Text,
                    inTrEcFilePrefix.Text,
                    inTpFilePrefix.Text,
                    inINFFilePrefix.Text
                );
            }
            catch (Exception ex)
            {
                ErrorDialog dialog = new ErrorDialog(ex.ToString());

                dialog.Show();
            }
        }
    }
}

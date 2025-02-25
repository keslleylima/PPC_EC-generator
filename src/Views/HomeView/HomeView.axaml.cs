using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PpcEcGenerator.Controllers;
using PpcEcGenerator.Data;
using PpcEcGenerator.Style.Color;
using PpcEcGenerator.Util;
using System;

namespace PpcEcGenerator.Views
{
    public class HomeView : UserControl, IClassObserver
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private TextBox inMetricsRootPath = default!;
        private TextBox inTrPpcFilePrefix = default!;
        private TextBox inTrEcFilePrefix = default!;
        private TextBox inTpFilePrefix = default!;
        private TextBox inINFFilePrefix = default!;
        private Button btnGenerate = default!;
        private readonly HomeController homeController;
        private ProgressBar progressBar = default!;


        //---------------------------------------------------------------------
        //		Constructors
        //---------------------------------------------------------------------
        public HomeView()
        {
            homeController = default!;

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
            homeController = new HomeController(window, this);
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void Update(IClassObservable observable, object data)
        {
            ProcessingProgress progress = (ProcessingProgress) data;

            progressBar.Value = (progress.Current + 1) % 101;
        }

        private void BuildProgressBar()
        {
            progressBar = this.FindControl<ProgressBar>("progressBar");
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

        private async void OnGenerate(object sender, RoutedEventArgs e)
        {
            string output = await homeController.AskUserForSavePath();

            homeController.OnGenerate(
                inMetricsRootPath.Text,
                inTrPpcFilePrefix.Text,
                inTrEcFilePrefix.Text,
                inTpFilePrefix.Text,
                inINFFilePrefix.Text,
                output
            );
        }

        public void EnableGenerateButton()
        {
            btnGenerate.IsEnabled = true;
        }

        public void DisableGenerateButton()
        {
            btnGenerate.IsEnabled = true;
        }

        public void DisplayErrorDialog(string msg)
        {
            ErrorDialog dialog = new ErrorDialog(msg);

            dialog.Show();
        }
    }
}

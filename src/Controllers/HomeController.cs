using Avalonia.Controls;
using Avalonia.Threading;
using PpcEcGenerator.Views;
using System.Threading;
using System.Threading.Tasks;

namespace PpcEcGenerator.Controllers
{
    /// <summary>
    ///     Responsible for controlling the behavior of 'HomeView'.
    /// </summary>
    public class HomeController
    {
        //---------------------------------------------------------------------
        //		Attributes
        //---------------------------------------------------------------------
        private MainWindow window;
        private HomeView homeView;


        //---------------------------------------------------------------------
        //		Constructors
        //---------------------------------------------------------------------
        public HomeController(MainWindow window, HomeView homeView)
        {
            this.window = window;
            this.homeView = homeView;
        }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public async Task<string> AskUserForDirectoryPath()
        {
            OpenFolderDialog dialog = new OpenFolderDialog();

            string result = await dialog.ShowAsync(window);

            return ((result != null) && (result.Length > 0))
                    ? result
                    : "";
        }

        public void OnGenerate(string metricsRootPath, string trPpcFilePrefix,
                               string trEcFilePrefix, string tpFilePrefix, 
                               string infFilePrefix, string outputPath)
        {
            PpcEcGenerator generator = new PpcEcGenerator.Builder()
                .ProjectPath(metricsRootPath)
                .OutputPath(outputPath)
                .PrimePathCoveragePrefix(trPpcFilePrefix)
                .EdgeCoveragePrefix(trEcFilePrefix)
                .TestPathPrefix(tpFilePrefix)
                .InfeasiblePathPrefix(infFilePrefix)
                .WithObserver(homeView)
                .Build();
            
            _ = Dispatcher.UIThread.InvokeAsync(() =>
              {
                  string output = generator.GenerateCoverage();

                  window.NavigateToEndView(output);
              });
        }

        public async Task<string> AskUserForSavePath()
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.DefaultExtension = "csv";
            dialog.InitialFileName = "CodeCoverage";
            dialog.Title = "Save metrics file";
            dialog.Filters.Add(new FileDialogFilter()
            { 
                Name = "Metrics file", 
                Extensions = { "csv" } 
            });

            string result = await dialog.ShowAsync(window);

            return ((result != null) && (result.Length > 0))
                    ? result
                    : "";
        }
    }
}

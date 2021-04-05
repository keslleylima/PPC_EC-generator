using Avalonia.Controls;
using PpcEcGenerator.Views;
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


        //---------------------------------------------------------------------
        //		Constructors
        //---------------------------------------------------------------------
        public HomeController(MainWindow window)
        {
            this.window = window;
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

        public void OnGenerate(string metricsRootPath, string trFilePrefix,
                               string tpFilePrefix, string infFilePrefix)
        {
            window.NavigateToEndView("output");
        }
    }
}

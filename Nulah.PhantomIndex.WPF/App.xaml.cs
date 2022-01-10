using Nulah.PhantomIndex.Lib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Nulah.PhantomIndex.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public readonly static PhantomIndexManager Database = new PhantomIndexManager();

        public App()
        {
            // Ensure the application data folder for this application exists
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Directory.CreateDirectory(Path.Combine(localAppData, GetType()!.Namespace));

            // Set the default database location if empty
            if (string.IsNullOrWhiteSpace(WPF.Properties.Settings.Default.ProfileDatabaseLocation) == true)
            {
                WPF.Properties.Settings.Default.ProfileDatabaseLocation = Path.Combine(localAppData, GetType()!.Namespace, "app.db");
                WPF.Properties.Settings.Default.Save();
            }

            Database.SetConnection(WPF.Properties.Settings.Default.ProfileDatabaseLocation);
        }
    }
}

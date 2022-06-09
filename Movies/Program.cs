using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Movies
{
    class Program
    {

        private static readonly string ExecutionPath = (System.AppDomain.CurrentDomain.BaseDirectory);

        private const string Filter = "*.mkv|*.avi|*.mp4|*.flv";

        private static string FinalFileName = "ListaMovies-{0}.txt";
        private static string TimeNow = DateTime.Now.ToString("yyyyMMddHHmm");

        private static string FinalPathAndFileName = Path.Combine(ExecutionPath, String.Format(FinalFileName, TimeNow));



        private static void Main(string[] args)
        {

            string actualPath = string.Empty;
            string previousPath = string.Empty;

            // Getting the list of the files
            foreach (string fileName in Directory.GetFiles(ExecutionPath, "*.*", SearchOption.AllDirectories).Where(s => Filter.Contains(Path.GetExtension(s).ToLower())))
            {
                actualPath = Onefile(fileName, actualPath, previousPath);
                
                // Geting the path for the next turn
                previousPath = actualPath;
            }

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

        }



        private static string Onefile(string fileName, string actualPath, string previousPath)
        {
            FileInfo f = null;
            FileStream fs = null;
            string message = string.Empty;

            string path = string.Empty; ;
            string file = string.Empty; ;
            bool IsnewPath = false;

            SplitPathAndName(fileName, ref path, ref file);

            actualPath = path;

            // Priting the name of the path
            if (previousPath == string.Empty)
            {
                IsnewPath = true;
            }
            else
            {
                if (actualPath != previousPath)
                {
                    IsnewPath = true;
                }
            }

            if (IsnewPath)
            {
                Console.WriteLine(path);

                // Writting the text on the file
                f = new FileInfo(FinalPathAndFileName);
                fs = f.Open(FileMode.Append, FileAccess.Write, FileShare.Write);
                AddText(fs, path);
                AddText(fs, Environment.NewLine);
                fs.Close();
            }


            message = "--> " + file;


            // Priting the name of the file            
            Console.WriteLine(message);

            // Writting the text on the file
            f = new FileInfo(FinalPathAndFileName);
            fs = f.Open(FileMode.Append, FileAccess.Write, FileShare.Write);
            AddText(fs, message);
            AddText(fs, Environment.NewLine);
            fs.Close();


            return path;
        }


        private static void AddText(FileStream fs, string value)
        {
            try
            {
                byte[] info = new UTF8Encoding(true).GetBytes(value);
                fs.Write(info, 0, info.Length);
            }
            catch (Exception)
            {
                // Nothing to do
            }
        }


        #region Files
        /// <summary>
        /// It splits a full path into Path and FileName.
        /// </summary>
        /// <param name="originalPathName">Original Path+FileName to be splitted.</param>
        /// <param name="Path">Ref Path.</param>
        /// <param name="filename">Ref FileName.</param>
        private static void SplitPathAndName(string originalPathName, ref string Path, ref string filename)
        {
            // Searching for the last "\" (folder)
            int n = originalPathName.LastIndexOf(@"\");
            // Path
            Path = originalPathName.Substring(0, n + 1);
            // FileName 
            filename = originalPathName.Substring(n + 1);
        }
        /// <summary>
        /// It splits a file name into the name and the extension.
        /// </summary>
        /// <param name="originalName">Original filename to be splitted.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="extension">File extension.</param>
        private static void SplitNameAndExtension(string originalName, ref string fileName, ref string extension)
        {
            // Searching for the last "." (extension)
            int n = originalName.LastIndexOf(@".");
            // Path
            fileName = originalName.Substring(0, n);
            // FileName 
            extension = originalName.Substring(n + 1);
        }
        /// <summary>
        /// It splits a DomainUser into Domain and UserName.
        /// </summary>
        /// <param name="domainUserName">Domain\User.</param>
        /// <param name="domain">Ref Domain.</param>
        /// <param name="user">Ref User.</param>
        private static void SplitDomainAndUser(string domainUserName, ref string domain, ref string user)
        {
            // Searching for the last "\" (folder)
            int n = domainUserName.LastIndexOf(@"\");
            // Domain
            domain = domainUserName.Substring(0, n);
            // User 
            user = domainUserName.Substring(n + 1);
        }
        #endregion
    }
}

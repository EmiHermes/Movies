using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Text.RegularExpressions;

namespace Conciety.IQon2014.Services.Common
{
    public class Helper
    {
        #region Properties
        /// <summary>
        /// Path of the TRACE LOG FILE.
        /// </summary>
        private static string PathLog = ConfigurationManager.AppSettings["PathLog"];
        /// <summary>
        /// Name of the TRACE LOG FILE.
        /// </summary>
        private static string logFile = ConfigurationManager.AppSettings["logFile"];
        #endregion

        #region LOG
        /// <summary>
        /// It writes one line to the text file.
        /// </summary>
        /// <param name="message">Message to be written.</param>
        /// <param name="dllName">DLL Name.</param>
        /// <param name="className">Class name.</param>
        /// <param name="methodName">Method.</param>
        /// <param name="isStart">If it´s a beginning (or an end).</param>
        /// <param name="gId">GUId.</param>
        public static void WriteOneLine(string message, string dllName, string className, string methodName, bool isStart, string gId)
        {
            try
            {
                // Building the file name with actual date
                string fileLog = logFile + ".log";
                string dateTimeLog = DateTime.Now.Year.ToString("0000_") + DateTime.Now.Month.ToString("00_") + DateTime.Now.Day.ToString("00_");
                fileLog = PathLog + @"\" + dateTimeLog + fileLog;
                string startEnd = isStart ? "START " : "END   ";

                // Building the text line to be inserted to the file
                message = DateTime.Now.ToString("HH:mm:ss:fffff") + "; " + gId + "; " + startEnd + "; " + dllName + "; " + className + "; " + methodName + "; " + message;

                // Checking the directory
                if (CheckAndCreateDirectory(PathLog))
                {
                    // Writting the text on the file
                    FileInfo f = new FileInfo(fileLog);
                    FileStream fs = f.Open(FileMode.Append, FileAccess.Write, FileShare.Write);
                    AddText(fs, message);
                    AddText(fs, Environment.NewLine);
                    fs.Close();
                }
                else
                {
                    // There was an error
                }
            }
            catch (Exception)
            {
                // Nothing to do
            }
        }
        /// <summary>
        /// Add text to an open file.
        /// </summary>
        /// <param name="fs">Where.</param>
        /// <param name="value">Text to be written.</param>
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
        /// <summary>
        /// Checking the file and the directory.
        /// </summary>
        /// <param name="directory">Path to be checked.</param>
        /// <returns>
        /// True if there was no errors.
        /// </returns>
        public static bool CheckAndCreateDirectory(string directory)
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Returning it was done
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Files
        /// <summary>
        /// It splits a full path into Path and FileName.
        /// </summary>
        /// <param name="originalPathName">Original Path+FileName to be splitted.</param>
        /// <param name="Path">Ref Path.</param>
        /// <param name="filename">Ref FileName.</param>
        public static void SplitPathAndName(string originalPathName, ref string Path, ref string filename)
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
        public static void SplitNameAndExtension(string originalName, ref string fileName, ref string extension)
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
        public static void SplitDomainAndUser(string domainUserName, ref string domain, ref string user)
        {
            // Searching for the last "\" (folder)
            int n = domainUserName.LastIndexOf(@"\");
            // Domain
            domain = domainUserName.Substring(0, n);
            // User 
            user = domainUserName.Substring(n + 1);
        }
        #endregion

        #region Strings
        /// <summary>
        /// It capitalizes a sentence.
        /// </summary>
        /// <param name="sentence">Sentence to convert.</param>
        /// <returns>
        ///    Sentence Capitalized.
        /// </returns>
        public static string FirstCharacterToUpper(string sentence)
        {
            String resul = "";

            try
            {
                resul = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(sentence.ToLower());
            }
            catch
            {
                // Nothing to do
            }

            return resul;
        }
        /// <summary>
        /// It capitalizes a sentence with "Culture Info".
        /// </summary>
        /// <param name="sentence">Sentence to convert.</param>
        /// <param name="culture">Culture Info.</param>
        /// <returns>
        ///    Sentence Capitalized.
        /// </returns>
        public static string FirstCharacterToUpper(string sentence, string culture)
        {
            String resul = "";
            //string culture = "es-ES";

            try
            {
                resul = new CultureInfo(culture, false).TextInfo.ToTitleCase(sentence.ToLower());
            }
            catch
            {
                // Nothing to do
            }

            return resul;
        }
        #endregion

        #region ParameterList
        public static string [] ParameterListToArray(string ParameterList)
        {           
            List<string> Result = new List<string>();
            StringBuilder Temp = new StringBuilder();
            bool inBracket = false;
         
            foreach (char c in ParameterList)
            {
                switch (c)
                {
                    case (char)',':       //Comma
                        if (!inBracket)
                        {
                            // hier noch die Brackets entfernen
                            string AddString = Temp.ToString();
                            AddString = AddString.Replace("[", "");
                            AddString = AddString.Replace("]", "");
                           
                            Result.Add(AddString);
                            Temp = new StringBuilder();
                        }
                        break;
                    case '[':     //[
                        inBracket = true;
                        break;
                    case ']':      //]
                        inBracket = false;
                        break;
                }
                if ((c != ',') || (inBracket))
                {
                    Temp.Append(c);
                }
            }
            if (Temp.Length > 0)
            {
                string AddString = Temp.ToString();
                AddString = AddString.Replace("[", "");
                AddString = AddString.Replace("]", "");
                Result.Add(AddString);
            }

            return Result.ToArray();
        }
        #endregion

    }
}

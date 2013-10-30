using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;

// mostly lifted: http://www.switchonthecode.com/tutorials/csharp-snippet-tutorial-using-the-filesystemwatcher-class
// http://msdn.microsoft.com/en-us/library/system.io.filesystemwatcher.changed.aspx

namespace ConsoleApplication1
{
    public class Program
    {
        public static int CountOfFilesAffected;
        public static bool VerboselyWeWillGo;
        public static string DirectoryToWatch;
        public static bool NoDFSRFolders;

        public static void Main(string[] args)
        {

            //Verbose();
            //CreateFSWatcher(@"c:\windows\");



            switch (args.Length)
            {
                case 0:
                    Console.WriteLine("Give me some food.");
                    Console.WriteLine("Usage: FolderChangeWatcher.exe [-v] [directory]");
                    Environment.Exit(-1);
                    break;
                case 1:
                    if (args[0] == "-v")
                    {
                        goto case 0;
                    }
                    else if (Directory.Exists(args[0]))
                    {
                        DFSRFolders();
                        CreateFSWatcher(args[0]);
                    }
                    else
                    {
                        Console.WriteLine("Give me some food.");
                        Console.WriteLine("Usage: FolderChangeWatcher.exe [-v] [directory]");
                        Environment.Exit(-1);
                    }
                    break;
                case 2:
                    if (args[0] == "-v")
                    {
                        DFSRFolders(); //disables recording of any file with DfsrPrivate in it's path
                        Verbose();
                    }

                    if (Directory.Exists(args[1]))
                    {
                        CreateFSWatcher(args[1]);
                    }
                    else
                        goto case 0;

                    if (args[1] == "-v")
                    {
                        DFSRFolders(); //disables recording of any file with DfsrPrivate in it's path
                        Verbose();
                    }
                    if (Directory.Exists(args[1]))
                    {
                        CreateFSWatcher(args[1]);
                    }

                    else if (args[0] != "-v" || Directory.Exists(args[0]) == false || args[0] != "-v" || Directory.Exists(args[0]) == false)
                    {
                        Console.WriteLine("Give me some food.");
                        Console.WriteLine("Usage: FolderChangeWatcher.exe [-v] [directory]");
                        Environment.Exit(-1);
                    }
                    break;
                default:
                    goto case 0;
            }


        }

        public static void Verbose()
        {
            VerboselyWeWillGo = true;
            Console.WriteLine("       !!! WARNING !!!       \n\r");
            Console.WriteLine("Verbosely to stdout: enabled\n\r");
            Thread.Sleep(2000);
        }

        public static void DFSRFolders()
        {
            NoDFSRFolders = true;
            Console.WriteLine("*DfsrPrivate* path ignore: enabled\n\r");

        }
        public static void CreateFSWatcher(string directorytowatch)
        {
            //initialize filesystemwatcher
            FileSystemWatcher fswatcher = new FileSystemWatcher();
            //configure the filesystemwatcher
            DirectoryToWatch = @directorytowatch;
            fswatcher.Path = DirectoryToWatch;

            fswatcher.IncludeSubdirectories = true;
            //fswatcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;

            fswatcher.Renamed += new RenamedEventHandler(fswatcher_Renamed);
            fswatcher.Deleted += new FileSystemEventHandler(fswatcher_Deleted);
            fswatcher.Changed += new FileSystemEventHandler(fswatcher_Changed);
            fswatcher.Created += new FileSystemEventHandler(fswatcher_Created);
            fswatcher.EnableRaisingEvents = true;

            Console.WriteLine("Now Watching: " + @directorytowatch + "\n\r");
            Console.WriteLine("Hit CTRL-C to kill me or q then enter for me to end nicely.\n\r");

            while (Console.Read() != 'q') ;


        }




        public static void fswatcher_Renamed(object sender, RenamedEventArgs e)
        {

            CountOfFilesAffected = CountOfFilesAffected + 1;

            if (NoDFSRFolders == true)
            {
                if (e.FullPath.Contains("DfsrPrivate") == false)
                {

                    if (VerboselyWeWillGo == true)
                    {
                        DateTime time = DateTime.Now;
                        string timeformatted = time.ToString("yyyy-MM-dd,HH:mm:ss");
                        Console.WriteLine(timeformatted + "," + Convert.ToString(CountOfFilesAffected) + ",,RENAMED, " + e.Name + ",old name " + e.OldName + ",changetype = " + e.ChangeType + ",oldfullpath = " + e.OldFullPath + ",new full path = " + e.FullPath);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Now Watching: " + DirectoryToWatch + "\n\r");
                        Console.WriteLine("Hit CTRL-C to kill me \n\r");
                        Console.WriteLine("Count of files affected: " + CountOfFilesAffected);
                    }

                }
            }


        }

        public static void fswatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            CountOfFilesAffected = CountOfFilesAffected + 1;
            if (NoDFSRFolders == true)
            {
                if (e.FullPath.Contains("DfsrPrivate") == false)
                {
                    if (VerboselyWeWillGo == true)
                    {
                        DateTime time = DateTime.Now;
                        string timeformatted = time.ToString("yyyy-MM-dd,HH:mm:ss");
                        Console.WriteLine(timeformatted + "," + Convert.ToString(CountOfFilesAffected) + ",DELETED," + e.Name);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Now Watching: " + DirectoryToWatch + "\n\r");
                        Console.WriteLine("Hit CTRL-C to kill me \n\r");
                        Console.WriteLine("Count of files affected: " + CountOfFilesAffected);
                    }
                    
                }
            }
        }

        public static void fswatcher_Changed(object sender, FileSystemEventArgs e)
        {
            CountOfFilesAffected = CountOfFilesAffected + 1;
            if (NoDFSRFolders == true)
            {
                if (e.FullPath.Contains("DfsrPrivate") == false)
                {
                    if (VerboselyWeWillGo == true)
                    {
                        DateTime time = DateTime.Now;
                        string timeformatted = time.ToString("yyyy-MM-dd,HH:mm:ss");
                        Console.WriteLine(timeformatted + "," + Convert.ToString(CountOfFilesAffected) + ",CHANGED," + e.FullPath);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Now Watching: " + DirectoryToWatch + "\n\r");
                        Console.WriteLine("Hit CTRL-C to kill me \n\r");
                        Console.WriteLine("Count of files affected: " + CountOfFilesAffected);
                    }
                }
            }
        }

        public static void fswatcher_Created(object sender, FileSystemEventArgs e)
        {
            CountOfFilesAffected = CountOfFilesAffected + 1;
            if (NoDFSRFolders == true)
            {
                if (e.FullPath.Contains("DfsrPrivate") == false)
                {
                    if (VerboselyWeWillGo == true)
                    {
                        DateTime time = DateTime.Now;
                        string timeformatted = time.ToString("yyyy-MM-dd,HH:mm:ss");
                        Console.WriteLine(timeformatted + "," + Convert.ToString(CountOfFilesAffected) + ",CREATED," + e.Name);

                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Now Watching: " + DirectoryToWatch + "\n\r");
                        Console.WriteLine("Hit CTRL-C to kill me \n\r");
                        Console.WriteLine("Count of files affected: " + CountOfFilesAffected);
                    }
                }
            }
        }
    }
}


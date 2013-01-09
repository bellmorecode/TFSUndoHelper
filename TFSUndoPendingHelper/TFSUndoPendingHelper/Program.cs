using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TFSUndoPendingHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            var arg = new TFSUndoArgs();

            Console.WriteLine("Enter a command to Begin: ");
            Console.Write("> ");
            var input = Console.ReadLine();
            while (input != "quit" && input != "exit")
            {
                switch(input)
                {
                    case "set-user":
                        //Console.WriteLine();
                        Console.Write("User: ");
                        var username = Console.ReadLine();
                        arg.User = username;
                        break;
                    case "set-workspace":
                        //Console.WriteLine();
                        Console.Write("Workspace: ");
                        var space = Console.ReadLine();
                        arg.Workspace = space;
                        break;
                    case "set-server":
                        //Console.WriteLine();
                        Console.Write("Server: ");
                        var server = Console.ReadLine();
                        arg.Server = server;
                        break;
                    case "undo":
                        Console.Write("Path: ");
                        var path = Console.ReadLine();
                        arg.ItemPath = path;
                        
                        Undo(arg);
                        break;
                    case "echo":
                        Console.WriteLine(arg.ToString());
                        break;
                    case "?":
                        Console.WriteLine("set-user, set-workspace, set-server, undo, echo");
                        break;
                }
                Console.WriteLine();
                Console.Write("> ");
                input = Console.ReadLine();
            }

            

            Console.WriteLine("Exiting");
            Console.ReadLine();
            
        }

        static void Undo(TFSUndoArgs args)
        {
            var path = @"C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\tf.exe";
            try
            {
                var startinfo = new ProcessStartInfo();
                startinfo.FileName = path;
                startinfo.Arguments = args.ToString();
                startinfo.UseShellExecute = false;
                startinfo.RedirectStandardOutput = true;
                var p = Process.Start(startinfo);
                p.Start();
                var output = p.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
                p.WaitForExit();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failure!");
                Console.WriteLine();
                Console.WriteLine(ex.ToString());
            }
        }

       
    }

    internal class TFSUndoArgs : TFSArgs
    {
        public TFSUndoArgs()
        {
            Action = "undo";
            Recursive = true;
        }

        public override string ToString()
        {
            return string.Format("{0} /workspace:{1};{2} /server:{3} /recursive \"{4}\"",
                this.Action, this.Workspace, this.User, this.Server, this.ItemPath);
        }
    }

    internal class TFSArgs
    {
        public string Action { get; set; }
        public string Workspace { get; set; }
        public string User { get; set; }
        public string Server { get; set; }
        public bool Recursive { get; set; }
        public string ItemPath { get; set; }
    }


}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Assignment
{
    public class Handler
    {
        private const string ERROR_FILE_NOT_FOUND = "File Not Found";
        private const string ERROR_FILEFOMMAT_ILLEGAL = "File name shoud be end of .txt .dat, .bat";

        public string Read(string filename)
        {
            string content = null;

            if (!File.Exists(filename))
            {
                PrintError(ERROR_FILE_NOT_FOUND);
                return content;
            }

            try
            {
                using (BinaryReader reader = new BinaryReader(File.OpenRead(filename)))
                {
                    content = reader.ReadString();
                }
            }
            catch (IOException e)
            {
                PrintError(e.Message);
            }

            return content;
        }

        public void Write(string content, string filename)
        {
            if (!filename.Contains(".txt") && !filename.Contains(".dat") && !filename.Contains(".bat"))
            {
                PrintError(ERROR_FILEFOMMAT_ILLEGAL);
                return;
            }

            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
                {
                    writer.Write(content);                  
                }
            }
            catch (IOException e)
            {
                PrintError(e.Message);
            }

            PrintResult(filename);
        }

        public void Merge(List<string> files, string filename)
        {
            bool allExist = files.All(file => File.Exists(file));
            if (!allExist)
            {
                PrintError(ERROR_FILE_NOT_FOUND);
                return;
            }

            if (!filename.Contains(".txt") && !filename.Contains(".dat") && !filename.Contains(".bat"))
            {
                PrintError(ERROR_FILEFOMMAT_ILLEGAL);
                return;
            }

            StringBuilder builder = new StringBuilder();
            Parallel.ForEach(files, file => builder.Append(Read(file)));

            Write(builder.ToString(), filename);
        }

        public void Bundle(List<string> files, string filename)
        {

            bool allExist = files.All(file => File.Exists(file));
            if (!allExist)
            {
                PrintError(ERROR_FILE_NOT_FOUND);
                return;
            }

            List<string> contents = new List<string>(files.Count);

            try
            {
                foreach (string file in files)
                {
                    using (FileStream fs = File.Open(file, FileMode.Open))
                    {
                        using (BinaryReader br = new BinaryReader(fs))
                        {
                            contents.Add(br.ReadString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PrintError(ex.Message);
            }


            try
            {
                using (FileStream fs = new FileStream(filename + ".zip", FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs, Encoding.Default))
                    {
                        bw.Write(files.Count);
                        for (int i = 0; i < files.Count; i++)
                        {
                            bw.Write(files[i]);
                            bw.Write(contents[i]);
                        }
                    }
                }

                ColorPrint("Bundle Succes", ConsoleColor.Green);

            }
            catch (Exception ex)
            {
                PrintError(ex.Message);
            }

        }

        public void Unbundle(string filename)
        {
            if (!File.Exists(filename))
            {
                PrintError(ERROR_FILE_NOT_FOUND);
                return;
            }

            List<string> contents = new List<string>();
            List<string> files = new List<string>();
            try
            {
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        int total = br.ReadInt32();

                        for (int i = 0; i < total; i++)
                        {
                            files.Add(br.ReadString());
                            contents.Add(br.ReadString());
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                PrintError(ex.Message);
            }

            try
            {
                for (int i = 0; i < files.Count; i++)
                {
                    string unZipfile = "(unziped)" + files[i];

                    using (FileStream fs = new FileStream(unZipfile, FileMode.Create))
                    {
                        using (BinaryWriter bw = new BinaryWriter(fs, Encoding.Default))
                        {
                            bw.Write(contents[i]);
                        }
                    }

                }

                ColorPrint("Unbundle Succes", ConsoleColor.Green);

            }
            catch (Exception ex)
            {
                PrintError(ex.Message);
            }


        }

        public void ShowFileList()
        {
            try
            {
                var files = Directory.EnumerateFiles(Directory.GetCurrentDirectory());
                foreach (string file in files)
                {
                    string filename = file.Split("/").Last();
                    if (filename.Contains("unziped"))
                    {
                        ColorPrint(filename, ConsoleColor.Cyan);
                    }
                    else if (filename.Contains("zip"))
                    {
                        ColorPrint(filename, ConsoleColor.Yellow);
                    }
                    else if (filename.Contains("txt") || filename.Contains("dat") || filename.Contains("bat"))
                    {
                        ColorPrint(filename, ConsoleColor.Magenta);
                    }
                    else
                    {
                        Console.WriteLine(filename);
                    }
                }
            }
            catch (DirectoryNotFoundException e)
            {
                PrintError(e.Message);
            }

        }

        public void PrintResult(string filename)
        {
            string result = Read(filename);
            if(result != null) ColorPrint(result, ConsoleColor.DarkGreen);
        }

        public void PrintError(string error)
        {
            ColorPrint(error, ConsoleColor.Red);
        }
        public void ColorPrint(string text, ConsoleColor color)
        {
            var beforeColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = beforeColor;
        }
        public void Usage()
        {
            var beforeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("commands without args : quit or -q | help or -h | show");
            Console.WriteLine("usage:<command> <args>");
            Console.WriteLine("commands : write | read | merge | bundle | unbundle");
            Console.WriteLine("args : contents | filename");
            Console.WriteLine("ex: write | [No limit on number of text] some content enything you can write | <out of filename must include .txt|.dat|.bat>");
            Console.WriteLine("ex: read | exist<filename>");
            Console.WriteLine("ex: merge | [No limit on number of files] exist<filename> exist<filename> exist<filename> | <out of filename must include .txt|.dat|.bat> ");
            Console.WriteLine("ex: bundle | [No limit on number of files] exist<filename> exist<filename> exist<filename> | <out of filename> ");
            Console.WriteLine("ex: unbundle | exist<filename>");

            Console.ForegroundColor = beforeColor;
        }
    }
}

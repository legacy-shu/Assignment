using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment
{
    public class Assignment
    {
        private Handler handler;
        private const string ERROR_ILLEGAL_ARGUMENT = "Illegal argument exeption";

        public Assignment(Handler handler)
        {
            this.handler = handler;
        }

        public void Start(List<string> commands)
        {
            if (commands[0] == "read")
            {

                //only two arguments are valid
                if (commands.Count != 2)
                {
                    handler.PrintError(ERROR_ILLEGAL_ARGUMENT + "\nArgument shoud be one and exist file name");
                    return;
                }

                handler.PrintResult(commands[1]);
            }

            else if (commands[0] == "write")
            {
                //greater than three arguments are valid
                if (commands.Count <= 2)
                {
                    handler.PrintError(ERROR_ILLEGAL_ARGUMENT + "\nArgument shoud have one content or more and last argument shoud be file name");
                    return;
                }

                //write middle part of arguments as a content
                string contents = String.Join(" ", commands.Skip(1).SkipLast(1).ToList());

                handler.Write(contents, commands.Last());
            }

            else if (commands[0] == "merge")
            {
                //greater than three arguments are valid
                if (commands.Count <= 2)
                {
                    handler.PrintError(ERROR_ILLEGAL_ARGUMENT + "\nArgument shoud have one correct file name or more and last argument shoud be file name");
                    return;
                }

                var middleArgs = commands.Skip(1).SkipLast(1).ToList();
                handler.Merge(middleArgs, commands.Last());
            }

            else if (commands[0] == "quit" || commands[0] == "-q" || commands[0] == "-quit")
            {
                Environment.Exit(0);
            }

            else if (commands[0] == "help" || commands[0] == "-h" || commands[0] == "-help")
            {
                handler.Usage();
            }

            else if (commands[0] == "show" || commands[0] == "-s" || commands[0] == "-show")
            {
                handler.ShowFileList();
            }

            else if (commands[0] == "bundle")
            {

                if (commands.Count <= 2)
                {
                    handler.PrintError(ERROR_ILLEGAL_ARGUMENT + "\nArgument shoud have one correct file name or more and last argument shoud be file name");
                    return;
                }

                var middleArgs = commands.Skip(1).SkipLast(1).ToList();
                handler.Bundle(middleArgs, commands.Last());

            }

            else if (commands[0] == "unbundle")
            {
                if (commands.Count != 2)
                {
                    handler.PrintError(ERROR_ILLEGAL_ARGUMENT + "\nArgument shoud be one and exist file name");
                    return;
                }

                handler.Unbundle(commands[1]);
            }

            else
            {
                handler.PrintError("Unknown");
            }
        }
    }
}
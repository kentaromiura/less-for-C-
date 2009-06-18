using System;
using System.IO;
using System.Reflection;

namespace lessCompiler {
    class Program {

        static void Main(string[] args) {

            
            /*
            startConversion(@"c:\less\test1.less", @"c:\less\test1.css");
            startConversion(@"c:\less\test2.less", @"c:\less\test2.css");
            startConversion(@"c:\less\miotest.less", @"c:\less\miotest.css");
            
             */ 

            // http://lesscss.org/index.html
            if (args.Length < 1 || args.Length > 2) {
                printCorrectSyntax();
            } else {
                string inputfile = args[0], outputfile = string.Format("{0}.css", inputfile);
                if (args.Length > 1) {
                    outputfile = args[1];
                }
                
                startConversion(inputfile, outputfile);
            }
        }

        private static void startConversion(string inputfile, string outputfile) {
            string input = getInput(inputfile), output = lessConverter.Convert(input);
            writeOutput(outputfile,output);
        }

        private static void writeOutput(string outputfile, string content) {
            if (File.Exists(outputfile)) {
                File.Delete(outputfile);
            }

            using (var output = File.CreateText(outputfile)) {
                output.Write(content);
                output.Flush();
                output.Close();
            }
        }

        private static string getInput(string inputfile) {
            string input = "";

            if (File.Exists(inputfile)) {            
                using (var sr = File.OpenText(inputfile)) {
                    input = sr.ReadToEnd();
                }                
            }

            return input;
        }

        private static void printCorrectSyntax() {
            string executablename = Assembly.GetExecutingAssembly().GetName().Name;
            Console.WriteLine("{0} inputfile [outputfile]",executablename);
            Console.ReadLine();
        }
    }
}

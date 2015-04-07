namespace CodeDOM
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Diagnostics;

    using CodeDOM.CodeGraphBuilders;
    using CodeDOM.DB;

    internal static class Program
    {
        private static void Main()
        {
            RunHelloWorld();
            Console.WriteLine("Hello World is executed");

            GenerateDtos();
            Console.WriteLine("Dtos are generated");
            
            Console.WriteLine("Press the ENTER key to exit");
            Console.ReadLine();
        }

        private static void RunHelloWorld()
        {
            const string FileName = "HelloWorld";
            const string SrcFile = FileName + ".cs";
            const string ExeFile = FileName + ".exe";

            // Create code graph
            CodeCompileUnit codeGraph = HelloWorldBuilder.Build();

            // Generate file based on code graph
            FileGenerator.CreateFile(codeGraph, SrcFile);

            // Compile generated source file into an executable output file
            CodeCompiler.Compile(SrcFile, ExeFile);

            // Execute compiled app
            Process.Start(ExeFile);
        }

        private static void GenerateDtos()
        {
            IEnumerable<TableInfo> tables = DbHelper.GetTableInfos().Result;

            foreach (var table in tables)
            {
                CodeCompileUnit codeGraph = DtoBuilder.Build(table, "MyNamespace");

                var srcPath = "./Dtos/" + table.Name + ".cs";
                FileGenerator.CreateFile(codeGraph, srcPath);
            }
        }
    }
}
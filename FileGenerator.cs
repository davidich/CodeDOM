namespace CodeDOM
{
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.IO;

    static class FileGenerator
    {
        public static void CreateFile(CodeCompileUnit compileunit, string fileName)
        {
            CreateFolderIfNeeded(fileName);

            using (var writer = new StreamWriter(fileName, false))
            {
                var provider = CodeDomProvider.CreateProvider("CSharp");
                var options = new CodeGeneratorOptions { BracingStyle = "C", IndentString = "    " };
                provider.GenerateCodeFromCompileUnit(compileunit, writer, options);
            }
        }

        private static void CreateFolderIfNeeded(string fileName)
        {
            var dirPath = Path.GetDirectoryName(fileName);

            if (!string.IsNullOrWhiteSpace(dirPath) && !Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
        }
    }
}
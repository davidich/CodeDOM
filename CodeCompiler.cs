namespace CodeDOM
{
    using System.CodeDom.Compiler;

    static class CodeCompiler
    {
        public static void Compile(string sourceFile, string exeFile)
        {
            // add System.dll reference
            string[] referenceAssemblies = { "System.dll" };
            var cp = new CompilerParameters(referenceAssemblies, exeFile, false);

            // specify the result type (exe, not dll)
            cp.GenerateExecutable = true;

            // compile
            var provider = CodeDomProvider.CreateProvider("CSharp");
            provider.CompileAssemblyFromFile(cp, sourceFile);
        }
    }
}
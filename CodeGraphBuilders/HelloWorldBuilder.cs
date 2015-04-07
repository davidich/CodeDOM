namespace CodeDOM.CodeGraphBuilders
{
    using System.CodeDom;

    static class HelloWorldBuilder
    {
        public static CodeCompileUnit Build()
        {
            // Create code container
            var compileUnit = new CodeCompileUnit();

            // Declare a new namespace
            var samples = new CodeNamespace("MyNameSpace");
            compileUnit.Namespaces.Add(samples);

            // Import System namespace
            samples.Imports.Add(new CodeNamespaceImport("System"));

            // Declare class
            var myClass = new CodeTypeDeclaration("MyClass");
            samples.Types.Add(myClass);

            // Create Main method.
            var mainMethod = BuildMainMethod();
            myClass.Members.Add(mainMethod);

            return compileUnit;
        }

        private static CodeEntryPointMethod BuildMainMethod()
        {
            var method = new CodeEntryPointMethod();

            // Create a type reference for the System.Console class.
            var csSystemConsoleType = new CodeTypeReferenceExpression("System.Console");

            // Add Console.WriteLine statement
            method.Statements.Add(
                new CodeMethodInvokeExpression(
                    csSystemConsoleType,
                    "WriteLine",
                    new CodePrimitiveExpression("Hello World!")));

            // Add another Console.WriteLine statement
            method.Statements.Add(
                new CodeMethodInvokeExpression(
                    csSystemConsoleType,
                    "WriteLine",
                    new CodePrimitiveExpression("Press the Enter key to exit.")));

            // Add the ReadLine statement.
            method.Statements.Add(
                new CodeMethodInvokeExpression(
                    csSystemConsoleType,
                    "ReadLine"));

            return method;
        }
    }
}
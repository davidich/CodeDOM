namespace CodeDOM.CodeGraphBuilders
{
    using System;
    using System.CodeDom;

    using CodeDOM.DB;

    static class DtoBuilder
    {
        public static CodeCompileUnit Build(TableInfo tableInfo, string namespase)
        {
            // Create code container
            var compileUnit = new CodeCompileUnit();

            // Declare a new namespace
            var codeNamespace = new CodeNamespace(namespase);
            compileUnit.Namespaces.Add(codeNamespace);
           
            // Declare class
            var dtoClass = new CodeTypeDeclaration(tableInfo.Name);
            codeNamespace.Types.Add(dtoClass);

            // Add backup fields for properties
            foreach (var column in tableInfo.Columns)
            {
                var field = CreatePrivateField(column);
                dtoClass.Members.Add(field);
            }

            // Add properties
            foreach (var column in tableInfo.Columns)
            {
                var prop = CreateProperty(column);
                dtoClass.Members.Add(prop);
            }

            return compileUnit;
        }

        private static CodeMemberField CreatePrivateField(ColumnInfo column)
        {
            return new CodeMemberField
            {
                Type = GetType(column),
                Name = FormatPrivateFieldName(column),
            };
        }

        private static CodeMemberProperty CreateProperty(ColumnInfo column)
        {
            var prop = new CodeMemberProperty
            {
                Name = column.Name,
                Type = GetType(column),
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };

            var backupFieldName = FormatPrivateFieldName(column);
            var _this = new CodeThisReferenceExpression();
            prop.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeFieldReferenceExpression(_this, backupFieldName)));
            
            var propValue = new CodePropertySetValueReferenceExpression();
            prop.SetStatements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(_this, backupFieldName), propValue));

            return prop;
        }

        private static string FormatPrivateFieldName(ColumnInfo info)
        {
            return "_" + info.Name.ToLower();
        }

        private static CodeTypeReference GetType(ColumnInfo column)
        {
            switch (column.DataType)
            {
                case "int":
                    return column.IsNullable
                        ? new CodeTypeReference(typeof(int?))
                        : new CodeTypeReference(typeof(int));
                case "nvarchar":
                    return new CodeTypeReference(typeof(string));
                default:
                    throw new NotSupportedException("DbType '" + column.DataType + "' is not supported yet");
            }
        }
    }
}
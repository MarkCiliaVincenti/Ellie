// Code temporarily yeeted from
// https://github.com/mostmand/Cloneable/blob/master/Cloneable/CloneableGenerator.cs
// because of NRT issue
#nullable enable
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloneable
{
    [Generator]
    public class CloneableGenerator : ISourceGenerator
    {
        private const string PREVENT_DEEP_COPY_KEY_STRING = "PreventDeepCopy";
        private const string EXPLICIT_DECLARATION_KEY_STRING = "ExplicitDeclaration";

        private const string CLONEABLE_NAMESPACE = "Cloneable";
        private const string CLONEABLE_ATTRIBUTE_STRING = "CloneableAttribute";
        private const string CLONE_ATTRIBUTE_STRING = "CloneAttribute";
        private const string IGNORE_CLONE_ATTRIBUTE_STRING = "IgnoreCloneAttribute";

        private const string CLONEABLE_ATTRIBUTE_TEXT = @"// <AutoGenerated/>
using System;

namespace " + CLONEABLE_NAMESPACE + @"
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true, AllowMultiple = false)]
    public sealed class " + CLONEABLE_ATTRIBUTE_STRING + @" : Attribute
    {
        public " + CLONEABLE_ATTRIBUTE_STRING + @"()
        {
        }

        public bool " + EXPLICIT_DECLARATION_KEY_STRING + @" { get; set; }
    }
}
";

        private const string CLONE_PROPERTY_ATTRIBUTE_TEXT = @"// <AutoGenerated/>
using System;

namespace " + CLONEABLE_NAMESPACE + @"
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class " + CLONE_ATTRIBUTE_STRING + @" : Attribute
    {
        public " + CLONE_ATTRIBUTE_STRING + @"()
        {
        }

        public bool " + PREVENT_DEEP_COPY_KEY_STRING + @" { get; set; }
    }
}
";

        private const string IGNORE_CLONE_PROPERTY_ATTRIBUTE_TEXT = @"// <AutoGenerated/>
using System;

namespace " + CLONEABLE_NAMESPACE + @"
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class " + IGNORE_CLONE_ATTRIBUTE_STRING + @" : Attribute
    {
        public " + IGNORE_CLONE_ATTRIBUTE_STRING + @"()
        {
        }
    }
}
";

        private INamedTypeSymbol? _cloneableAttribute;
        private INamedTypeSymbol? _ignoreCloneAttribute;
        private INamedTypeSymbol? _cloneAttribute;

        public void Initialize(GeneratorInitializationContext context)
            => context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());

        public void Execute(GeneratorExecutionContext context)
        {
            InjectCloneableAttributes(context);
            GenerateCloneMethods(context);
        }

        private void GenerateCloneMethods(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not SyntaxReceiver receiver)
                return;

            Compilation compilation = GetCompilation(context);

            InitAttributes(compilation);

            var classSymbols = GetClassSymbols(compilation, receiver);
            foreach (var classSymbol in classSymbols)
            {
                if (!classSymbol.TryGetAttribute(_cloneableAttribute!, out var attributes))
                    continue;

                var attribute = attributes.Single();
                var isExplicit = (bool?)attribute.NamedArguments.FirstOrDefault(e => e.Key.Equals(EXPLICIT_DECLARATION_KEY_STRING)).Value.Value ?? false;
                context.AddSource($"{classSymbol.Name}_cloneable.g.cs", SourceText.From(CreateCloneableCode(classSymbol, isExplicit), Encoding.UTF8));
            }
        }

        private void InitAttributes(Compilation compilation)
        {
            _cloneableAttribute = compilation.GetTypeByMetadataName($"{CLONEABLE_NAMESPACE}.{CLONEABLE_ATTRIBUTE_STRING}")!;
            _cloneAttribute = compilation.GetTypeByMetadataName($"{CLONEABLE_NAMESPACE}.{CLONE_ATTRIBUTE_STRING}")!;
            _ignoreCloneAttribute = compilation.GetTypeByMetadataName($"{CLONEABLE_NAMESPACE}.{IGNORE_CLONE_ATTRIBUTE_STRING}")!;
        }

        private static Compilation GetCompilation(GeneratorExecutionContext context)
        {
            var options = context.Compilation.SyntaxTrees.First().Options as CSharpParseOptions;

            var compilation = context.Compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(CLONEABLE_ATTRIBUTE_TEXT, Encoding.UTF8), options)).
                AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(CLONE_PROPERTY_ATTRIBUTE_TEXT, Encoding.UTF8), options)).
                AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(IGNORE_CLONE_PROPERTY_ATTRIBUTE_TEXT, Encoding.UTF8), options));
            return compilation;
        }

        private string CreateCloneableCode(INamedTypeSymbol classSymbol, bool isExplicit)
        {
            string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            var fieldAssignmentsCode = GenerateFieldAssignmentsCode(classSymbol, isExplicit).ToList();
            var fieldAssignmentsCodeSafe = fieldAssignmentsCode.Select(x =>
            {
                if (x.isCloneable)
                    return x.line + "Safe(referenceChain)";
                return x.line;
            });
            var fieldAssignmentsCodeFast = fieldAssignmentsCode.Select(x =>
            {
                if (x.isCloneable)
                    return x.line + "()";
                return x.line;
            });

            return $@"using System.Collections.Generic;

namespace {namespaceName}
{{
    {GetAccessModifier(classSymbol)} partial class {classSymbol.Name}
    {{
        /// <summary>
        /// Creates a copy of {classSymbol.Name} with NO circular reference checking. This method should be used if performance matters.
        /// 
        /// <exception cref=""StackOverflowException"">Will occur on any object that has circular references in the hierarchy.</exception>
        /// </summary>
        public {classSymbol.Name} Clone()
        {{
            return new {classSymbol.Name}
            {{
{string.Join(",\n", fieldAssignmentsCodeFast)}
            }};
        }}

        /// <summary>
        /// Creates a copy of {classSymbol.Name} with circular reference checking. If a circular reference was detected, only a reference of the leaf object is passed instead of cloning it.
        /// </summary>
        /// <param name=""referenceChain"">Should only be provided if specific objects should not be cloned but passed by reference instead.</param>
        public {classSymbol.Name} CloneSafe(Stack<object> referenceChain = null)
        {{
            if(referenceChain?.Contains(this) == true) 
                return this;
            referenceChain ??= new Stack<object>();
            referenceChain.Push(this);
            var result = new {classSymbol.Name}
            {{
{string.Join($",\n", fieldAssignmentsCodeSafe)}
            }};
            referenceChain.Pop();
            return result;
        }}
    }}
}}";
        }

        private IEnumerable<(string line, bool isCloneable)> GenerateFieldAssignmentsCode(INamedTypeSymbol classSymbol, bool isExplicit)
        {
            var fieldNames = GetCloneableProperties(classSymbol, isExplicit);

            var fieldAssignments = fieldNames.Select(field => IsFieldCloneable(field, classSymbol))
                .OrderBy(x => x.isCloneable)
                .Select(x => (GenerateAssignmentCode(x.item.Name, x.isCloneable), x.isCloneable));
            return fieldAssignments;
        }

        private string GenerateAssignmentCode(string name, bool isCloneable)
        {
            if (isCloneable)
            {
                return $@"                {name} = this.{name}?.Clone";
            }

            return $@"                {name} = this.{name}";
        }

        private (IPropertySymbol item, bool isCloneable) IsFieldCloneable(IPropertySymbol x, INamedTypeSymbol classSymbol)
        {
            if (SymbolEqualityComparer.Default.Equals(x.Type, classSymbol))
            {
                return (x, false);
            }

            if (!x.Type.TryGetAttribute(_cloneableAttribute!, out var attributes))
            {
                return (x, false);
            }

            var preventDeepCopy = (bool?)attributes.Single().NamedArguments.FirstOrDefault(e => e.Key.Equals(PREVENT_DEEP_COPY_KEY_STRING)).Value.Value ?? false;
            return (item: x, !preventDeepCopy);
        }

        private string GetAccessModifier(INamedTypeSymbol classSymbol)
            => classSymbol.DeclaredAccessibility.ToString().ToLowerInvariant();

        private IEnumerable<IPropertySymbol> GetCloneableProperties(ITypeSymbol classSymbol, bool isExplicit)
        {
            var targetSymbolMembers = classSymbol.GetMembers().OfType<IPropertySymbol>()
                .Where(x => x.SetMethod is not null &&
                            x.CanBeReferencedByName);
            if (isExplicit)
            {
                return targetSymbolMembers.Where(x => x.HasAttribute(_cloneAttribute!));
            }
            else
            {
                return targetSymbolMembers.Where(x => !x.HasAttribute(_ignoreCloneAttribute!));
            }
        }

        private static IEnumerable<INamedTypeSymbol> GetClassSymbols(Compilation compilation, SyntaxReceiver receiver)
            => receiver.CandidateClasses.Select(clazz => GetClassSymbol(compilation, clazz));

        private static INamedTypeSymbol GetClassSymbol(Compilation compilation, ClassDeclarationSyntax clazz)
        {
            var model = compilation.GetSemanticModel(clazz.SyntaxTree);
            var classSymbol = model.GetDeclaredSymbol(clazz)!;
            return classSymbol;
        }

        private static void InjectCloneableAttributes(GeneratorExecutionContext context)
        {
            context.AddSource(CLONEABLE_ATTRIBUTE_STRING, SourceText.From(CLONEABLE_ATTRIBUTE_TEXT, Encoding.UTF8));
            context.AddSource(CLONE_ATTRIBUTE_STRING, SourceText.From(CLONE_PROPERTY_ATTRIBUTE_TEXT, Encoding.UTF8));
            context.AddSource(IGNORE_CLONE_ATTRIBUTE_STRING, SourceText.From(IGNORE_CLONE_PROPERTY_ATTRIBUTE_TEXT, Encoding.UTF8));
        }
    }
}
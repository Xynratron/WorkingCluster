using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;

namespace Esb.Tests.Helper
{

    internal class CompiledAssembly
    {
        public bool HasErrors { get; set; }
        public List<string> Output { get; set; }
        public byte[] Assembly { get; set; }
    }

    internal static class CodeCompiling
    {
        public static CompiledAssembly Compile(string sourceCode)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            
            parameters.ReferencedAssemblies.Add("System.dll");
            
            var esbAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .Where(o => o.GetName(true).Name.Equals("ESB", StringComparison.OrdinalIgnoreCase))
                .Select(o => o.Location).First();
            parameters.ReferencedAssemblies.Add(esbAssembly);

            parameters.TreatWarningsAsErrors = true;
            parameters.IncludeDebugInformation = true;
            parameters.WarningLevel = 4;
            
            //If you use InMemory, the Assembly gets loaded to your Current AppDomain
            parameters.GenerateInMemory = false;
            parameters.GenerateExecutable = false;
            parameters.TreatWarningsAsErrors = true;

            var results =  provider.CompileAssemblyFromSource(parameters, sourceCode);
            
            var errors = new List<string>();

            if (results.Errors.Count > 0)
            {
                foreach (CompilerError err in results.Errors)
                {
                    var formatedError = err.IsWarning
                        ? "Warning"
                        : "Error" + $" {err.ErrorNumber} {err.ErrorText} at ({err.Line}:{err.Column})";
                    errors.Add(formatedError);
                }
            }

            var result = new CompiledAssembly
            {
                HasErrors = results.Errors.Count > 0,
                Output = errors,
                Assembly = results.Errors.Count > 0 ? null : System.IO.File.ReadAllBytes(results.PathToAssembly)
            };

            return result;
        }
    }
}

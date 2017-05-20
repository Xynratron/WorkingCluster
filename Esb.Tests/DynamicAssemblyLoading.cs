using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Esb.Tests.Helper;
using NUnit.Framework;
using System.Reflection;

namespace Esb.Tests
{
    [TestFixture()]
    public class DynamicAssemblyLoading
    {
        [Test()]
        public void Compile()
        {
            string sourceCode = "using System;\r\n" +
                                "public class MyLib{\r\n" +
                                "   public static void Execute()" +
                                "   {" +
                                "       System.Diagnostics.Debug.WriteLine(\"In Dynamic Assembly\");" +
                                "   }\r\n" +
                                "}";

            var result = CodeCompiling.Compile(sourceCode);
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

           
        }

    }
}

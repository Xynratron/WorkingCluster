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
        public void DynamicCompileShouldNotThrowErrors()
        {
            var sourceCode = "using System;\r\n" +
                                "public class MyLib{\r\n" +
                                "   public static void Execute()" +
                                "   {" +
                                "       System.Diagnostics.Debug.WriteLine(\"In Dynamic Assembly\");" +
                                "   }\r\n" +
                                "}";

            var result = CodeCompiling.Compile(sourceCode);
            result.HasErrors.ShouldBeFalse();
        }

        [Test()]
        public void GeneratedAssemblieShouldNotBeLoadedToCurrentAppDomain()
        {
            var sourceCode = "using System;\r\n" +
                                "public class MyLib{\r\n" +
                                "   public static void Execute()" +
                                "   {" +
                                "       System.Diagnostics.Debug.WriteLine(\"In Dynamic Assembly\");" +
                                "   }\r\n" +
                                "}";

            var countOfAssemblies = AppDomain.CurrentDomain.GetAssemblies().Length;

            var result = CodeCompiling.Compile(sourceCode);

            var sameCount = countOfAssemblies == AppDomain.CurrentDomain.GetAssemblies().Length;
            sameCount.ShouldEqual(true);
        }

        [Test()]
        public void EsbProcessoresShouldCompile()
        {
            var sourceCode = @"using System;
                                using Esb.Message;
                                using Esb.Processing;

                                namespace Esb.CompileTests
                                {
                                    public class TestMessage {}

                                    public class TestMessageProcessor : BaseProcessor<TestMessage>
                                    {
                                        public override void Process(IEnvironment environment, Envelope envelope, TestMessage message)
                                        {
            
                                        }
                                    }
                                }";

            var result = CodeCompiling.Compile(sourceCode);
            result.HasErrors.ShouldBeFalse();
        }
    }
}

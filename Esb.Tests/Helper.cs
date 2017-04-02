using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Esb.Processing;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Esb.Tests
{

    #region Aliases to NUnit.Framework classes to improve our BDD syntax
    public class Be : Is { }
    public class Have : Has {  }
    public class Contain : Contains { }
    #endregion

    /// <summary>
    /// Simple extension methods allowing us to use NUnit constraints as: "foo".Should(Be.StringContaining("o"));
    /// </summary>
    /// <remarks>
    /// ShouldExtensions.Should and ShouldExtensions.ShouldNot are the only methods that are really required 
    /// to give us Should() syntax with NUnit.  We also add a number of Should*() helper methods, however, 
    /// so you can say things like list.ShouldContain("rover") instead of list.Should(Contain.Item("rover"))
    /// </remarks>
    public static partial class ShouldExtensions
    {
        public static void Should(this object o, IResolveConstraint constraint)
        {
            Assert.That(o, constraint);
        }
        public static void ShouldNot(this object o, Constraint constraint)
        {
            Assert.That(o, new NotOperator().ApplyPrefix(constraint));
        }
    }
    // NUnit.Should setups up our .Should() syntax
    //
    // These are additional .Should*() helper methods to help us 
    // write specs that may be easier to read
    public static partial class ShouldExtensions
    {

        public static void ShouldEqual<T>(this T a, T b)
        {
            a.Should(Is.EqualTo(b));
        }

        public static void ShouldNotEqual<T>(this T a, T b)
        {
            a.ShouldNot(Is.EqualTo(b));
        }

        public static void ShouldContain<T>(this IEnumerable<T> list, T item)
        {
            list.Should(Contains.Item(item));
        }

         public static void ShouldContain<T>(this IEnumerable<T> list, Func<T, bool> item)
        {
            Assert.IsTrue(list.Any());
        }

        public static void ShouldNotContain<T>(this IEnumerable<T> list, T item)
        {
            list.ShouldNot(Contains.Item(item));
        }

        public static void ShouldContain(this string full, string part)
        {
            full.Should(Does.Contain(part));
        }

        public static void ShouldNotContain(this string full, string part)
        {
            full.ShouldNot(Does.Contain(part));
        }

        public static void ShouldBeFalse(this bool b)
        {
            b.Should(Is.False);
        }

        public static void ShouldBeTrue(this bool b)
        {
            b.Should(Is.True);
        }
    }

    public static class WorkerHelper
    {
        public static Worker WaitForStartUp(this Worker worker)
        {
            var sw = new Stopwatch();
            while (worker.Status != WorkerStatus.Started)
            {
                if (sw.ElapsedMilliseconds > 30000)
                    throw new TimeoutException("Waited for 30 seconds but worker did not come up.");
                System.Threading.Thread.Sleep(1);
            }
            return worker;
        }
    }
}
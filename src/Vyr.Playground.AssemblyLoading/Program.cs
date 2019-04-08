using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

namespace Vyr.Playground.AssemblyLoading
{
    class Program
    {
        static void Main(string[] args)
        {
            _ = ExecuteAndUnload(@"C:\Users\kaept\Source\Repos\kaep7n\vyr\src\Vyr.Tests.Console\bin\Debug\netcoreapp3.0\Vyr.Tests.Console.dll", out var alcWeakRef);

            while (alcWeakRef.IsAlive)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();

                Console.WriteLine("alive");
            }

            Console.WriteLine("dead");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static int ExecuteAndUnload(string assemblyPath, out WeakReference alcWeakRef)
        {
            var alc = new TestAssemblyLoadContext();
            alcWeakRef = new WeakReference(alc, trackResurrection: true);

            var a = alc.LoadFromAssemblyPath(assemblyPath);

            var args = new object[1] { new string[] { "Hello" } };
            int result = (int)a.EntryPoint.Invoke(null, args);
            alc.Unload();

            return result;
        }

        class TestAssemblyLoadContext : AssemblyLoadContext
        {
            public TestAssemblyLoadContext() : base(isCollectible: true)
            {
            }

            protected override Assembly Load(AssemblyName name)
            {
                return null;
            }
        }
    }
}

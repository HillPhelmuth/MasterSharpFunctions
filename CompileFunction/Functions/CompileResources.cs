using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompileFunction.Services;
using Microsoft.CodeAnalysis;

namespace CompileFunction.Functions
{
    public class CompileResources
    {
        

        

        public static List<PortableExecutableReference> PortableExecutableReferences
        {
            get
            {
                var appAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                var narrowedAssemblies = appAssemblies.Where(x =>
                    !x.IsDynamic && !string.IsNullOrWhiteSpace(x.Location) &&
                    (x.FullName.Contains("System")));

                return narrowedAssemblies.Select(assembly => MetadataReference.CreateFromFile(assembly.Location)).ToList();
            }
        }
    }
}

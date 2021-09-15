using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Telegram.Bot.Commands.Attributes;

namespace Telegram.Bot.Commands.Builders
{
    internal static class ModuleBuilder
    {
        /// <summary>
        /// Scans assembly for an
        /// </summary>
        /// <param name="assembly">Assembly that contains definitions of custom modules</param>
        /// <returns>List of founded modules</returns>
        internal static IReadOnlyList<TypeInfo> Search(Assembly assembly)
        {
            var results = new List<TypeInfo>();


            foreach (var type in assembly.DefinedTypes)
            {
                if (type.IsPublic && type.ImplementedInterfaces.Contains(typeof(IModuleBase)))
                {
                    results.Add(type);
                }

            }

            return results;
        }

        internal static void TryBuild(IEnumerable<TypeInfo> typesToBuild, IServiceProvider provider, CommandMapper mapper)
        {
            foreach (var type in typesToBuild)
            {
                var constructor = type.GetConstructors().First();

                var requiredParameters = constructor.GetParameters();
                var resultedParams = new List<object>();
                foreach (var par in requiredParameters)
                {
                    var instPar = provider.GetService(par.ParameterType);
                    resultedParams.Add(instPar);
                }

                var instance = constructor.Invoke(resultedParams.ToArray());

                mapper.MapModule(instance);
            }
        }
    }
}

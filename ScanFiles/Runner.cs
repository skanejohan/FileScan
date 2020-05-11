using System;
using System.Collections.Generic;

namespace ScanFiles
{
    public static class Runner
    {
        public static IEnumerable<string> Run(Func<IEnumerable<string>> filesFunction, Func<string,IEnumerable<string>> linesFunction)
        {
            foreach(var f in filesFunction())
            {
                if (linesFunction == null)
                {
                    yield return f;
                }
                else
                {
                    foreach (var l in linesFunction(f))
                    {
                        yield return l;
                    }
                }
            }
        }
    }
}

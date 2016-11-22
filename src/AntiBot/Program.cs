using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AntiBot
{
    public class Program
    {
        public static void Main(string[] args) => new AntiBot().Run().GetAwaiter().GetResult();
    }
}

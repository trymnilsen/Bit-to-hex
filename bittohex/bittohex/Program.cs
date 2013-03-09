using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace bittohex
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Selected file:" + args[0]);
            Console.WriteLine("Output file:" + args[1]);
            byte[] fileBytes = File.ReadAllBytes(args[0]);
            StringBuilder sb = new StringBuilder();

            foreach (byte b in fileBytes)
            {
                sb.Append("0x");
                sb.Append(Convert.ToString(b, 16));
            }

            File.WriteAllText(args[1], sb.ToString());
        }
    }
}

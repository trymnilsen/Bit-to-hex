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
            string input = "";
            string output = "";
            if (args.Count<string>() == 2 && File.Exists(args[0]))
            {
                input = args[0];
                output = args[1];
            }
            else
            {
                Console.WriteLine("Enter inputfile");

                while (!File.Exists(input))
                {
                    input = Console.ReadLine();
                }
                Console.WriteLine("Enter output File");
                output = Console.ReadLine();
            }
            Console.WriteLine("Selected file:" + input);
            Console.WriteLine("Output file:" + output);
            byte[] fileBytes = File.ReadAllBytes(args[0]);
            StringBuilder sb = new StringBuilder();

            foreach (byte b in fileBytes)
            {
                sb.Append("0x");
                sb.Append(Convert.ToString(b, 16));
                if (b != fileBytes.Last<byte>()) //dont append a space after last
                {
                    sb.Append(" ");//append a space
                }
            }

            File.WriteAllText(args[1], sb.ToString());
        }
    }
}

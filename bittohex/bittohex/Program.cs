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
            string separator = ", ";
            //if run through command we have var args to use
            if (args.Count<string>() == 2 && File.Exists(args[0]))
            {
                input = args[0];
                output = args[1];
            }
            else
            {
                //ask until file is found
                while (!File.Exists(input))
                {
                    Console.WriteLine("Enter inputfile");
                    input = Console.ReadLine();
                    if (!File.Exists(input))
                    {
                        Console.WriteLine("!File not found");
                    }
                }
                //ask for output file
                Console.WriteLine("File found");
                Console.WriteLine("Enter output File");
                output = Console.ReadLine();
            }

           
            //Prints the choosen files
            Console.WriteLine("Selected file:" + input);
            Console.WriteLine("Output file:" + output);
            //checks if output exists asks for overwrite
            if (File.Exists(output))
            {
                Console.WriteLine("File: " + output + " already exists overwrite? yes/no");
                string answer = Console.ReadLine();
                if (!answer.ToLower().Equals("yes"))
                {
                    Console.WriteLine("file not overwritten, press any key to exit");
                    Console.Read();
                    return;
                }
            }
            //asks for custom seperator
            Console.WriteLine("Want custom seperator? press enter for default ', '");
            string customSeperator = Console.ReadLine();
            if (customSeperator.Length != 0)
            {
                Console.WriteLine("custom seperator set");
                separator = customSeperator;
            }
            //try to read this file and convert it
            try
            {
                //dirty read, but i works.. so hey..
                byte[] fileBytes = File.ReadAllBytes(input);
                StringBuilder sb = new StringBuilder();

                //loop through all the bytes and convert the to base16 or otherwise know as hex
                foreach (byte b in fileBytes)
                {
                    sb.Append("0x");
                    sb.Append(Convert.ToString(b, 16));
                    if (b != fileBytes.Last<byte>()) //dont append a space after last
                    {
                        sb.Append(separator);//append a space so output is 0xbyte 0xbyte  not 0xbyte0xbyte
                    }
                }
                //write all our data to file
                File.WriteAllText(output, sb.ToString());
                Console.WriteLine("Conversion complete, press any key to exit");
                //this could be removed and simply having the application close on complete but i chose to do it this way so i can se what has been done
                Console.Read();
            }
            catch (IOException ie)
            {
                Console.WriteLine("Error!! Something went wrong writing or reading to file\n"+ie.ToString());
                Console.Read();
            }

        }
    }
}

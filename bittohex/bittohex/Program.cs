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
        static string input = "";
        static string output = "";
        static string separator = ", ";
        static bool rleCompress = false;
        static bool forceFlag = false;
        static bool quietFlag =false;
        static void Main(string[] args)
        {

            //if run through command we have var args to use
            if (args.Length >=2 && File.Exists(args[0]))
            {
                //Check for parameter flags
                if (args.Length >= 3)
                    CheckFlags(args[3]);

                if (args.Length == 4)
                    CheckFlags(args[4]);

                //Gather input
                input = args[0];
                output = args[1];
                //if output value is to short set to default
                if (args[1].Length < 4)
                {
                    WriteLine("Output name to short, putting as output.txt in program directory");
                    output = "output.txt";
                }
            }
            else
            {
                //ask until file is found
                input = askForFile("Enter input file: ", true);
                output = askForFile("Enter output file: ", false);
                
                if(output.Length<4)
                {
                    WriteLine("Output name to short, putting as output.txt in program directory");
                    output = "output.txt";
                }
            }

           
            //Prints the choosen files
            WriteLine("Selected file:" + input);
            WriteLine("Output file:" + output);
            //checks if output exists asks for overwrite
            if (File.Exists(output))
            {
               if(!PromtUser("File exits, overwrite anyway?"))
               {
                   return;
               }
            }
            //asks for custom seperator
            if (PromtUser("change defualt seperator?"))
            {
                WriteLine("enter custom seperator");
                separator = Console.ReadLine();
                WriteLine("custom seperator set");
                
            }
            if (PromtUser("Rle compress?"))
            {
                rleCompress = true;
            }
            //try to read this file and convert it
            try
            {

                //dirty read, but i works.. so hey..
                byte[] fileBytes = File.ReadAllBytes(input);
                StringBuilder sb = new StringBuilder();

                byte currentByte = 0;
                byte byteNumberOfTimes = 1;
                bool writeToStringBuilder = false;
                //loop through all the bytes and convert the to base16 or otherwise know as hex
                foreach (byte b in fileBytes)
                {
                    if (rleCompress)
                    {
                        if (currentByte == b)
                        {
                            byteNumberOfTimes++;
                        }
                        if (b != currentByte || byteNumberOfTimes > 254)
                        {
                            sb.Append("0x");
                            sb.Append(Convert.ToString(byteNumberOfTimes, 16).PadLeft(2,'0'));
                            sb.Append(separator);
                            for(byte i=0; i<byteNumberOfTimes; i++)
                            {
                                sb.Append("0x");
                                sb.Append(Convert.ToString(currentByte, 16).PadLeft(2, '0'));
                                sb.Append(separator);
                            }

                            currentByte = b;
                            byteNumberOfTimes = 1;
                        }

                    }
                    else
                    {
                        sb.Append("0x");
                        sb.Append(Convert.ToString(b, 16).PadLeft(2,'0'));
                        sb.Append(separator);//append a space so output is 0xbyte 0xbyte  not 0xbyte0xbyte
                    }
                }
                //write all our data to file
                bool fileNotWritten=true;
                do
                {
                    try
                    {
                        File.WriteAllText(output, sb.ToString());
                        fileNotWritten = false;
                    }
                    catch (UnauthorizedAccessException uae)
                    {
                        Console.WriteLine("File has no write access at this location");
                        if (PromtUser("Show error?"))
                            Console.WriteLine(uae.ToString());

                        if (PromtUser("Write to another location?"))
                        {
                            output = askForFile("Enter other output path:", false);
                        }
                        else
                        {
                            return;
                        }
                        
                    }
                }while(fileNotWritten);
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
        public static bool PromtUser(string promtString)
        {
            if(!forceFlag)
            {
                Console.WriteLine(promtString + " y/n");
                string answer = Console.ReadLine();
                if (answer.ToLower().Equals("y") || answer.ToLower().Equals("yes"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public static void WriteLine(string line)
        {
            if (!quietFlag)
            {
                Console.WriteLine(line);
            }
        }
        public static void CheckFlags(string args)
        {
            if (args.Equals("-q"))
                quietFlag = true;

            if (args.Equals("-f"))
                forceFlag = true;
        }
        public static string askForFile(string askMessage, bool mustExist)
        {
            string returnString="";
            do
            {
                if (returnString != "")
                    Console.WriteLine("File \""+returnString+"\" not found");

                Console.WriteLine(askMessage);
                returnString = Console.ReadLine();

                if (File.Exists(returnString))
                    break;

            } while (mustExist);

            return returnString; 
        }
    }
}

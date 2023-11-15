using System.Net.NetworkInformation;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace RM1_TestCSharp
{
    internal class Program
    {

        static int[] KMPnext(int[] next_table, string pattern)
        {
            int i = 0;
            int j = 2;

            next_table[0] = -1;
            next_table[1] = 0;
            while (true)
            {
                if(pattern.Length > j)
                {
                    if (pattern[j - 1] == pattern[i])
                    {
                        next_table[j] = i + 1;
                        //Console.WriteLine(next_table[j]);
                        i++;
                        //Console.WriteLine(i);

                        j++;
                       // Console.WriteLine(j);

                    }
                    else if (i != 0)
                    {
                        //Console.WriteLine(i);

                        i = next_table[i];
                        //Console.WriteLine(i);

                    }
                    else
                    {
                        //Console.WriteLine(j);

                        next_table[j] = 0;
                        j++;
                        //Console.WriteLine(j);

                    }
                }
             else if( pattern.Length <= j)
                {
                    break;
                }
            }

            return next_table;
        }
        static List<int> KnuttMorrisPrattAlgorithm(string text, string pattern)
        {
            int[] n_array = new int[pattern.Length];
            n_array = KMPnext(n_array, pattern);

            int text_length = text.Length;
            int pattern_length = pattern.Length;
            int j = 0;
            int i = 0;
            string pattern_find = pattern;
            string input_text = text;
            List<int> index_match = new List<int>();

            for (i = 0; i < pattern_length; i++)
            {
                if(j + i >= input_text.Length)
                {
                    break;
                }
                if(i < pattern_length)
                {
                    if (pattern_find[i] != input_text[j + i])
                    {
                        //Console.WriteLine(j);

                        j = j + i - n_array[i];
                        //Console.WriteLine(i);
                        //Console.WriteLine(j);

                        i = 0;
                    }
                    if (pattern_find[i] == input_text[j + i] && i == pattern_length - 1)
                    {
                        //Console.WriteLine(j);

                        i = 0;
                        index_match.Add(j);
                        j = j + pattern_length;
                        //Console.WriteLine(j);

                    }
                    if (j >= text_length)
                    {
                        //Console.WriteLine("break");

                        break;
                    }
                }
                else if(i >= pattern_length)
                {
                    //Console.WriteLine("break2");

                    break;
                }
                
            }
            return index_match;
        }

        static string readFile(string file)
        {
            byte[] fileBytes = File.ReadAllBytes(file);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in fileBytes)
            {
                sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            return sb.ToString();
        }
        static void writeToFile(string text, string extn)
        {


            int numOfBytes = text.Length / 8;
            byte[] bytes = new byte[numOfBytes];
            for (int i = 0; i < numOfBytes; ++i)
            {
                bytes[i] = Convert.ToByte(text.Substring(8 * i, 8), 2);
            }
            File.WriteAllBytes("test_out" + extn, bytes);
        }
        static void writeToFile2(string text)
        {


            int numOfBytes = text.Length / 8;
            byte[] bytes = new byte[numOfBytes];
            for (int i = 0; i < numOfBytes; ++i)
            {
                bytes[i] = Convert.ToByte(text.Substring(8 * i, 8), 2);
            }
            File.WriteAllBytes("test2.bin", bytes);
        }

        static string replace(string text, string pattern, List<int> indeces, string pattern_old)
        {

            var aStringBuilder = new StringBuilder(text);
            foreach (int b in indeces)
            {
                aStringBuilder.Remove(b, pattern_old.Length);
                aStringBuilder.Insert(b, pattern);

            }

            text = aStringBuilder.ToString();


            return text;
        }
        static void Main(string[] args)
        {
            string command = args[1];
            Console.WriteLine(command);
            if (command == "fr")
            {

                string input_file = args[0];
                FileInfo fi = new FileInfo(input_file);

                string pattern_to_insert = args[3];
                string pattern_to_replace = args[2];
                string input = readFile(input_file);
                List<int> indeces = KnuttMorrisPrattAlgorithm(input, pattern_to_replace);
                input = replace(input, pattern_to_insert, indeces, pattern_to_replace);
                string extn = fi.Extension;
                writeToFile(input ,extn);


            }
            else if(command == "f")
            {
                string input_file = args[0];
                Console.WriteLine(input_file);
                string input = readFile(input_file);
                Console.WriteLine(input_file);

                string pattern_to_find = args[2];
                List<int> indeces = KnuttMorrisPrattAlgorithm(input, pattern_to_find);

                foreach (var index in indeces)
                {
                    Console.WriteLine(index + ", ");
                }

            }
            //string input = readFile("test2.bin");
            //string pattern = "000000000000000011111111";
            //List<int> indeces = kmp(input, "111");
            //input = replace(input, "111", indeces);
            //writeToFile2(input);

            //writeToFile(input);


        }
    }
}
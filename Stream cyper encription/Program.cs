using SkiaSharp;
using System;
using System.IO;

namespace cypher_encyrption
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "Cipher")
                {
                    if(args.Length < 2)
                    {
                        help();
                    }else if (args.Length > 4)
                    {
                        help();
                    }
                    int tap = 0;
                    try
                    {
                        tap = int.Parse(args[2]);
                    }
                    catch (Exception e)
                    {
                        help();
                    }
                    var (newSeed, rightmostBit) = LFSRStep(args[1], tap);
                    Console.WriteLine($"New Seed: {newSeed}, Output Bit: {rightmostBit}");

                } else if (args[0] == "GenerateKeystream")
                {
                    if (args.Length < 3)
                    {
                        help();
                    }else if (args.Length > 5)
                    {
                        help();
                    }
                    int tap = 0;
                    int step = 0;
                    try
                    {
                        tap = int.Parse(args[2]);
                        step = int.Parse(args[3]);
                    }
                    catch (Exception e)
                    {
                        help();
                    }
                    GenerateKeystream(args[1], tap, step);
                }
                else if(args[0] == "Encrypt")
                {

                }else if (args[0] == "Decrypt")
                {

                }else if (args[0] == "TripleBits")
                {

                }else if (args[0] == "EncryptImage")
                {

                }else if (args[0] == "DecryptImage")
                {

                }
                else
                {
                    help();
                }

            }
        }
        public static void help()
        {
            Console.WriteLine("Cipher seed tap: takes the initial seed and " +
                "a tap position from the user and simulates one step of the LFSR cipher. " +
                "This returns and prints the new seed and the recent rightmost bit\r\n" +
                "GenerateKeystream seed tap step: steps For each step, the LFSR cipher " +
                "simulation prints the new seed and the rightmost bit. At the end of the iteration," +
                " keystream is saved in a file called “keystream” in the same directory\r\n" +
                "Encrypt plaintext: accepts plaintext in bits; perform an XOR" +
                " operation of the plaintext with the saved “keystream”; and return a set of encrypted bits\r\n" +
                "Decrypt ciphertext: accepts ciphertext in bits; perform an XOR operation with the " +
                "retrieved keystream from the file; and return a set of decrypted bits\r\n" +
                "TripleBit seed tap step iteration: accepts an initial seed, tap, step - a positive integer" +
                " prints the seed at the end of the last step and prints an accumulated integer value " +
                "obtained after the arithmetic operation is done at the last step\r\n" +
                "EncyrptImage imagefile seed tap: Given an image with a seed and a tap position , generate a rowencrypted image." +
                "Encode the row-encrypted bitmap image to an image file and save it in the same directory " +
                ". Name : “File_NameENCRYPTED”\r\n" +
                "DecyrptImage imagefile seed tap. Given an encrypted image, a seed and tap position," +
                " generate the original image and save it with a different name in the same directory." +
                " Name:“File_NameNEW”.\r\n");
        }
        public static (string newSeed, int outputBit) LFSRStep(string seed, int tapPosition)
        {
            if (seed.Length == 0 || tapPosition < 0 || tapPosition >= seed.Length)
            {
                Console.WriteLine("Invalid seed or tap position.");
                return ("0", 0);
            }
            int rightmostBit = seed[^1] - '0'; 
            int tapBit = seed[tapPosition] - '0';
            int newBit = rightmostBit ^ tapBit;
            string newSeed = newBit + seed[..^1];
            return (newSeed, rightmostBit);
        }
        public static void GenerateKeystream(string seed, int tapPosition, int steps)
        {
            string currentSeed = seed;
            string keystream = "";
            for (int i = 0; i < steps; i++)
            {
                var (newSeed, outputBit) = LFSRStep(currentSeed, tapPosition);
                keystream += outputBit.ToString();
                currentSeed = newSeed;
            }
            File.WriteAllText(Directory.GetCurrentDirectory()+"/keystream", keystream);
            Console.WriteLine($"\nKeystream saved to file: keystream");
        }
    }
}
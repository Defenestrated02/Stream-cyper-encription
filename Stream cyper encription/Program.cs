using SkiaSharp;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                    if (args.Length != 3)
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
                    var (newSeed, newBit) = LFSRStep(args[1], tap);
                    Console.WriteLine($"{args[1]} -seed");
                    Console.WriteLine($"{newSeed} {newBit}");

                }
                else if (args[0] == "GenerateKeystream")
                {
                    if (args.Length != 4)
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
                    Console.WriteLine($"{args[1]} -seed");
                    GenerateKeystream(args[1], tap, step);
                }
                else if (args[0] == "Encrypt")
                {
                    if (args.Length != 2)
                    {
                        help();
                    }
                    string[] path = Environment.CurrentDirectory.Split("bin");
                    StreamReader sr = new StreamReader(path[0] + "keystream.txt");
                    string keystream = sr.ReadLine();
                    Encrypt(args[1],keystream);
                }else if (args[0] == "Decrypt")
                {
                    if (args.Length != 2)
                    {
                        help();
                    }
                    string[] path = Environment.CurrentDirectory.Split("bin");
                    StreamReader sr = new StreamReader(path[0] + "keystream.txt");
                    string keystream = sr.ReadLine();
                    Decrypt(args[1], keystream);
                }
                else if (args[0] == "TripleBits")
                {
                    if (args.Length != 5)
                    {
                        help();
                    }
                    int tap = 0;
                    int step = 0;
                    int iteration = 0;
                    try
                    {
                        tap = int.Parse(args[2]);
                        step = int.Parse(args[3]);
                        iteration = int.Parse(args[4]);
                    }
                    catch (Exception e)
                    {
                        help();
                    }
                    Console.WriteLine($"{args[1]} -seed");
                    TripleBit(args[1], tap, step, iteration);
                }
                else if (args[0] == "EncryptImage")
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
        public static (string seen, int outputBit) LFSRStep(string seed, int tap)
        {
            int length = seed.Length;
            if (tap < 0 || tap >= length)
            {
                Console.WriteLine($"Tap must be between 0 and {length - 1}.");
                return ("",0);
            }
            int lsb = seed[0] - '0';
            int tapBit = seed[length - tap] - '0';
            int newBit = lsb ^ tapBit;
            string newSeed = seed.Substring(1, length - 1) + newBit;
            return (newSeed, newBit);
        }
        public static void GenerateKeystream(string seed, int tapPosition, int steps)
        {
            string currentSeed = seed;
            string keystream = "";
            for (int i = 0; i < steps; i++)
            {
                var (newSeed, newBit) = LFSRStep(currentSeed, tapPosition);
                Console.WriteLine($"{newSeed} {newBit}");
                keystream += newBit.ToString();
                currentSeed = newSeed;
            }
            string[] path = Environment.CurrentDirectory.Split("bin");
            Console.WriteLine($"The Keystream: {keystream}");
            File.WriteAllText(path[0] +"keystream.txt", keystream);
        }
        public static void Encrypt(string plaintext, string keystream)
        {
            while(keystream.Length < plaintext.Length)
            {
                keystream = '0' + keystream;
            }
            string ciphertext = "";
            while(keystream.Length > plaintext.Length)
            {
                plaintext = '0'+ plaintext;
            }
            for (int i = 0; i < plaintext.Length; i++)
            {
                int ptBit = plaintext[i] - '0';
                int ksBit = keystream[i] - '0';
                int cipherBit = ptBit ^ ksBit;
                ciphertext += cipherBit.ToString();
            }
            Console.WriteLine($"The ciphertext is: : {ciphertext}");

        }
        public static void Decrypt(string ciphertext, string keystream)
        {
            while (keystream.Length < ciphertext.Length)
            {
                keystream = '0' + keystream;
            }
            string plaintext = "";
            while (keystream.Length > ciphertext.Length)
            {
                ciphertext = '0' + ciphertext;
            }
            for (int i = 0; i < ciphertext.Length; i++)
            {
                int ptBit = ciphertext[i] - '0';
                int ksBit = keystream[i] - '0';
                int plainBit = ptBit ^ ksBit;
                plaintext += plainBit.ToString();
            }
            Console.WriteLine($"The plaintext is: : {plaintext}");

        }
        public static void TripleBit(string seed, int tap, int step, int iteration)
        {
            for (int i = 0; i < iteration; i++)
            {
                int value = 1;
                for (int j = 0; j < step; j++)
                {
                    var result = LFSRStep(seed, tap);
                    seed = result.Item1;
                    int rightmostBit = result.Item2;

                    value = value * 3 + rightmostBit;
                }
                Console.WriteLine($"{seed} {value}");
            }
        }
        public static void EncryptImage(string imagePath, string seed, int tap)
        {
            // Load the image into a bitmap using SkiaSharp
            using (SKBitmap bitmap = SKBitmap.Decode(imagePath))
            {
                // Create a new bitmap to store the encrypted image
                SKBitmap encryptedBitmap = new SKBitmap(bitmap.Width, bitmap.Height);

                // Use the seed and tap to generate the new seed for LFSR
                string currentSeed = seed;

                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        // Get the current pixel color (red, green, blue)
                        SKColor pixelColor = bitmap.GetPixel(x, y);

                        // Generate the new seed and get the rightmost bit
                        var lfsrResult = LFSRStep(currentSeed, tap);
                        currentSeed = lfsrResult.Item1;
                        int rightmostBit = lfsrResult.Item2;

                        // Generate a random 8-bit unsigned integer using the rightmost bit (simulate random behavior)
                        Random rand = new Random(rightmostBit);
                        byte randomByte = (byte)rand.Next(0, 256);

                        // XOR the color components with the random byte
                        byte newRed = (byte)(pixelColor.Red ^ randomByte);
                        byte newGreen = (byte)(pixelColor.Green ^ randomByte);
                        byte newBlue = (byte)(pixelColor.Blue ^ randomByte);

                        // Create a new color with the XORed components
                        SKColor newColor = new SKColor(newRed, newGreen, newBlue);

                        // Set the pixel in the encrypted bitmap
                        encryptedBitmap.SetPixel(x, y, newColor);
                    }
                }

                // Save the encrypted bitmap to a new file in the same directory
                string[] path = Environment.CurrentDirectory.Split("bin");
                string encryptedFilePath = path[0]+Path.GetFileNameWithoutExtension(imagePath) + "ENCRYPTED";
                using (SKImage img = SKImage.FromBitmap(encryptedBitmap))
                using (SKData data = img.Encode())
                {
                    File.WriteAllBytes(encryptedFilePath, data.ToArray());
                }
            }
        }

    }
}
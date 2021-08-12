using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Nolan_McAfee_Unit07_IT481
{
    class Program
    {
        static void Main(string[] args)
        {
            bool doItAgain = true;
            while (doItAgain == true)
            {
                Iterate();
                Console.WriteLine("\n\nDo it again? (Y/N)");
                if (Console.ReadLine() == "N")
                {
                    doItAgain = false;
                }
            }
        }
        
        static void Iterate()
        {
            //Declare variables
            int lineLength      = 9;
            int smallSetSize    = 100;
            int mediumSetSize   = 1000;
            int largeSetSize    = 10000; //Caution with values over 100,000 - exponential time growth for bubble sort. See note below

            int[] smallIntArray             = new int[smallSetSize];
            int[] smallIntArrayQuickSort    = new int[smallSetSize];
            int[] smallIntArrayBubbleSort   = new int[smallSetSize];
            int[] smallIntArrayListSort     = new int[smallSetSize];

            int[] mediumIntArray            = new int[mediumSetSize];
            int[] mediumIntArrayQuickSort   = new int[mediumSetSize];
            int[] mediumIntArrayBubbleSort  = new int[mediumSetSize];
            int[] mediumIntArrayListSort    = new int[mediumSetSize];

            int[] largeIntArray             = new int[largeSetSize];
            int[] largeIntArrayQuickSort    = new int[largeSetSize];
            int[] largeIntArrayBubbleSort   = new int[largeSetSize];
            int[] largeIntArrayListSort     = new int[largeSetSize];

            Stopwatch stopwatch             = new Stopwatch();
            long stopwatchFrequency         = Stopwatch.Frequency;
            long elapsed_small_quick, elapsed_small_bubble, elapsed_small_list;
            long elapsed_medium_quick, elapsed_medium_bubble, elapsed_medium_list;
            long elapsed_large_quick, elapsed_large_bubble, elapsed_large_list;


            string smallFile = @".\Datasets\SmallInt.txt";
            string mediumFile = @".\Datasets\MediumInt.txt";
            string largeFile = @".\Datasets\LargeInt.txt";


            //Allow user to regenerate datafiles OR force regeneration for each iteration - uncomment one or the other

            Console.WriteLine("Would you like to regenerate datafiles? (Y/N)"); //Uncomment these two lines to offer 
            string regenData = Console.ReadLine();                              //  regeneration to the user
            
            //string regenData = "Y";                                             //Uncomment this line to force regeneration
            //string regenData = "N";                                             //Uncomment this line to forcefully skip regeneration
            
            //This code will overwrite the datafiles defined above if regeneration has been requestsed
            if (regenData == "Y")
            {
                GenerateRandomData(smallFile, 'N', lineLength, smallSetSize);
                GenerateRandomData(mediumFile, 'N', lineLength, mediumSetSize);
                GenerateRandomData(largeFile, 'N', lineLength, largeSetSize);
            }

            //Get Small Dataset
            var smallFileStream = new FileStream(smallFile, FileMode.Open, FileAccess.Read);
            using (var streamreader = new StreamReader(smallFileStream, Encoding.UTF8))
            {
                string line;
                for (int i = 0; (i < smallSetSize) && (((line = streamreader.ReadLine())!=null)); i++)
                {
                    Int32.TryParse(line, out int addline);
                    smallIntArray[i] = addline;
                }
            }

            //Get Medium Dataset
            var mediumFileStream = new FileStream(mediumFile, FileMode.Open, FileAccess.Read);
            using (var streamreader = new StreamReader(mediumFileStream, Encoding.UTF8))
            {
                string line;
                for (int i = 0; (i < mediumSetSize) && (((line = streamreader.ReadLine()) != null)); i++)
                {
                    Int32.TryParse(line, out int addline);
                    mediumIntArray[i] = addline;
                }
            }

            //Get Large Dataset
            var largeFileStream = new FileStream(largeFile, FileMode.Open, FileAccess.Read);
            using (var streamreader = new StreamReader(largeFileStream, Encoding.UTF8))
            {
                string line;
                for (int i = 0; (i < largeSetSize) && (((line = streamreader.ReadLine()) != null)); i++)
                {
                    Int32.TryParse(line, out int addline);
                    largeIntArray[i] = addline;
                }
            }


            //Clone dataset to int arrays for each test - this is a shallow clone, so this should
            //     help to level the playing field for each test
            smallIntArrayQuickSort      = (int[])smallIntArray.Clone();
            smallIntArrayBubbleSort     = (int[])smallIntArray.Clone();
            smallIntArrayListSort       = (int[])smallIntArray.Clone();

            mediumIntArrayQuickSort     = (int[])mediumIntArray.Clone();
            mediumIntArrayBubbleSort    = (int[])mediumIntArray.Clone();
            mediumIntArrayListSort      = (int[])mediumIntArray.Clone();

            largeIntArrayQuickSort      = (int[])largeIntArray.Clone();
            largeIntArrayBubbleSort     = (int[])largeIntArray.Clone();
            largeIntArrayListSort       = (int[])largeIntArray.Clone();

            //This is a test block which can be uncommented and copied between each test to
            //      visually verify that sorting is occurring, and that each sort is not
            //      altering the other test arrays - this version is only appropriate for small datasets
            //foreach (int i in smallIntArrayQuickSort) { Console.Write("{0}, ", i); }
            //Console.WriteLine();
            //foreach (int i in smallIntArrayBubbleSort) { Console.Write("{0}, ", i); }
            //Console.WriteLine();
            //foreach (int i in smallIntArrayListSort) { Console.Write("{0}, ", i); }
            //Console.WriteLine();

            //Do Small File sorts
            stopwatch.Reset();
            stopwatch.Start();
            quickSortAsc(smallIntArrayQuickSort);
            stopwatch.Stop();
            elapsed_small_quick = stopwatch.ElapsedTicks;

            stopwatch.Reset();
            stopwatch.Start();
            bubbleSort(smallIntArrayBubbleSort);
            stopwatch.Stop();
            elapsed_small_bubble = stopwatch.ElapsedTicks;

            stopwatch.Reset();
            stopwatch.Start();
            listSort(smallIntArrayListSort);
            stopwatch.Stop();
            elapsed_small_list = stopwatch.ElapsedTicks;

            //This is a test block which can be uncommented and copied between each test to
            //      visually verify that sorting is occurring, and that each sort is not
            //      altering the other test arrays
            //for (int i = 0; i < mediumSetSize; i++)
            //{
            //    Console.WriteLine("{0,5:D4}: {1,10:D9}, {2,10:D9}, {3,10:D9}", i+1, mediumIntArrayQuickSort[i], mediumIntArrayBubbleSort[i], mediumIntArrayListSort[i]);
            //}
            //Console.ReadLine();

            //Do Medium File sorts
            stopwatch.Reset();
            stopwatch.Start();
            quickSortAsc(mediumIntArrayQuickSort);
            stopwatch.Stop();
            elapsed_medium_quick = stopwatch.ElapsedTicks;

            stopwatch.Reset();
            stopwatch.Start();
            bubbleSort(mediumIntArrayBubbleSort);
            stopwatch.Stop();
            elapsed_medium_bubble = stopwatch.ElapsedTicks;

            stopwatch.Reset();
            stopwatch.Start();
            listSort(mediumIntArrayListSort);
            stopwatch.Stop();
            elapsed_medium_list = stopwatch.ElapsedTicks;

            //This is a test block which can be uncommented and copied between each test to
            //      visually verify that sorting is occurring, and that each sort is not
            //      altering the other test arrays
            //for (int i = 0; i < largeSetSize; i++)
            //{
            //    Console.WriteLine("{0,5:D4}: {1,10:D9}, {2,10:D9}, {3,10:D9}", i + 1, largeIntArrayQuickSort[i], largeIntArrayBubbleSort[i], largeIntArrayListSort[i]);
            //}
            //Console.ReadLine();

            //Do Large File sorts
            stopwatch.Start();
            stopwatch.Stop();
            stopwatch.Reset();
            stopwatch.Start();
            quickSortAsc(largeIntArrayQuickSort);
            stopwatch.Stop();
            elapsed_large_quick = stopwatch.ElapsedTicks;

            stopwatch.Reset();
            stopwatch.Start();
            bubbleSort(largeIntArrayBubbleSort);  //Please note: this sort must be disabled/commented out for any dataset over ~100,000, 
            stopwatch.Stop();                               //       as the time increases exponentially. 100,000 benchmarked at 30 seconds,
            elapsed_large_bubble = stopwatch.ElapsedTicks;  //       144,269 (or 1/log(2) increase) benchmarked at 61.2 seconds

            stopwatch.Reset();
            stopwatch.Start();
            listSort(largeIntArrayListSort);
            stopwatch.Stop();
            elapsed_large_list = stopwatch.ElapsedTicks;


            //Format output in table:
            Console.WriteLine();
            Console.WriteLine(">>>>>>>>>  Output Table: Time in Elapsed Ticks (100 ns/tick)");
            Console.WriteLine("                 -------------------------------------------------------");
            Console.WriteLine("                 |      Quick Sort |     Bubble Sort |       List Sort |");
            Console.WriteLine("-----------------|-----------------|-----------------|-----------------|");
            Console.WriteLine("| {0,14} | {1,15} | {2,15} | {3,15} |", "Small Dataset", elapsed_small_quick, elapsed_small_bubble, elapsed_small_list);
            Console.WriteLine("|----------------|-----------------|-----------------|-----------------|");
            Console.WriteLine("| {0,14} | {1,15} | {2,15} | {3,15} |", "Medium Dataset", elapsed_medium_quick, elapsed_medium_bubble, elapsed_medium_list);
            Console.WriteLine("|----------------|-----------------|-----------------|-----------------|");
            Console.WriteLine("| {0,14} | {1,15} | {2,15} | {3,15} |", "Large Dataset", elapsed_large_quick, elapsed_large_bubble, elapsed_large_list);
            Console.WriteLine("------------------------------------------------------------------------");

            //Alternative output visuals

            //Console.WriteLine("Elapsed Ticks for Small Datasets, by Sort Method:");
            //Console.WriteLine("Quick Sort:  {0}", elapsed_small_quick);
            //Console.WriteLine("Bubble Sort: {0}", elapsed_small_bubble);
            //Console.WriteLine("List Sort:   {0}", elapsed_small_list);
            //Console.WriteLine();
            //Console.WriteLine("Elapsed Ticks for Medium Datasets, by Sort Method:");
            //Console.WriteLine("Quick Sort:  {0}", elapsed_medium_quick);
            //Console.WriteLine("Bubble Sort: {0}", elapsed_medium_bubble);
            //Console.WriteLine("List Sort:   {0}", elapsed_medium_list);
            //Console.WriteLine();
            //Console.WriteLine("Elapsed Ticks for Large Datasets, by Sort Method:");
            //Console.WriteLine("Quick Sort:  {0}", elapsed_large_quick);
            //Console.WriteLine("Bubble Sort: {0}", elapsed_large_bubble);
            //Console.WriteLine("List Sort:   {0}", elapsed_large_list);
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine("Elapsed Nanoseconds for Small Datasets, by Sort Method:");
            //Console.WriteLine("Quick Sort:  {0}", (long)(1000L * 1000L * 1000L * elapsed_small_quick / stopwatchFrequency));
            //Console.WriteLine("Bubble Sort: {0}", (long)(1000L * 1000L * 1000L * elapsed_small_bubble / stopwatchFrequency));
            //Console.WriteLine("List Sort:   {0}", (long)(1000L * 1000L * 1000L * elapsed_small_list / stopwatchFrequency));
            //Console.WriteLine();
            //Console.WriteLine("Elapsed Nanoseconds for Medium Datasets, by Sort Method:");
            //Console.WriteLine("Quick Sort:  {0}", (long)(1000L * 1000L * 1000L * elapsed_medium_quick / stopwatchFrequency));
            //Console.WriteLine("Bubble Sort: {0}", (long)(1000L * 1000L * 1000L * elapsed_medium_bubble / stopwatchFrequency));
            //Console.WriteLine("List Sort:   {0}", (long)(1000L * 1000L * 1000L * elapsed_medium_list / stopwatchFrequency));
            //Console.WriteLine();
            //Console.WriteLine("Elapsed Nanoseconds for Large Datasets, by Sort Method:");
            //Console.WriteLine("Quick Sort:  {0}", (long)(1000L * 1000L * 1000L * elapsed_large_quick / stopwatchFrequency));
            //Console.WriteLine("Bubble Sort: {0}", (long)(1000L * 1000L * 1000L * elapsed_large_bubble / stopwatchFrequency));
            //Console.WriteLine("List Sort:   {0}", (long)(1000L * 1000L * 1000L * elapsed_large_list / stopwatchFrequency));
            //Console.ReadLine();
        }


        //Quick Sort algorithm - reused from IT391_McAfee_Unit04.cs, my own work

        static void quickSortAsc(int[] SortArr)
        {
            quickSortAlg(SortArr, 0, SortArr.Length - 1);
        }

        static void quickSortAlg(int[] SortArr, int Low, int High)
        {
            if (Low < High)
            {
                int pivotIndex = partition(SortArr, Low, High);

                quickSortAlg(SortArr, Low, pivotIndex - 1);
                quickSortAlg(SortArr, pivotIndex + 1, High);
            }
        }

        static int partition(int[] SortArr, int Low, int High)
        {
            int pivotIndex = High;
            int pivot = SortArr[Low];
            int temp;

            for (int i = High; i >= Low + 1; i--)
            {
                if (SortArr[i] >= pivot)
                {
                    temp = SortArr[i];
                    SortArr[i] = SortArr[pivotIndex];
                    SortArr[pivotIndex] = temp;
                    pivotIndex--;
                }
            }

            SortArr[Low] = SortArr[pivotIndex];
            SortArr[pivotIndex] = pivot;

            return pivotIndex;
        }


        //Bubble Sort algorithm - reused from IT391_McAfee_Unit04.cs, my own work
        static void bubbleSort(int[] toSortArr)
        {
            bool swapped = true;
            int iter = toSortArr.Length - 1;
            int temp;

            while (swapped == true)
            {
                swapped = false;
                for (int i = 1; i <= iter; i++)
                {
                    if (toSortArr[i] < toSortArr[i - 1])
                    {
                        temp = toSortArr[i];
                        toSortArr[i] = toSortArr[i - 1];
                        toSortArr[i - 1] = temp;
                        swapped = true;
                    }
                }
                iter--;
            }
        }

        //Implemented from knowledge for the purpose of this assignment, this sort method
        //      takes advantage of the built-in list functionality in C#, which includes
        //      a Sort method. Then, to maintain consistency with the other algorithms,
        //      this method returns the list to the original array. In most applications,
        //      the whole program would likely be written to use list<T> natively
        static void listSort(int[] toSortArr)
        {
            List<int> sortList = new List<int>(toSortArr);
            sortList.Sort();
            for (int i = 0; i < toSortArr.Length; i++)
            {
                toSortArr[i] = sortList[i];
            }
        }

        //*********************This is the code for generating a custom file with random digits*************
        //***************************original work: Nolan J. McAfee, 8/11/21********************************
        static void GenerateRandomData(string fileName, char dataType, int lineLength, int countLines)
        {
            if ((lineLength > 0) && (countLines > 0))
            {
                string[] randArray = new string[countLines];

                for (int i = 0; i < countLines; i++)
                {
                    char[] line = new char[lineLength];
                    for (int j = 0; j < lineLength; j++)
                    {
                        line[j] = getRandomChar(dataType);
                    }
                    randArray[i] = new string(line);
                }
                using (StreamWriter file = new(@fileName, append: false))
                {
                    foreach (string line in randArray)
                    {
                        file.WriteLine(line);
                    }
                }
            }
        }

        static char getRandomChar(char charType)
        {
            char myChar;
            var rand = new Random();
            char[] charTable = new char[]
            {                                                                                                                   //Numbers               = 0 to 9
                (char)48, (char)49, (char)50, (char)51, (char)52, (char)53, (char)54, (char)55, (char)56, (char)57,             //  (0-9) = 0 to 9
                                                                                                                                //Uppercase letters     = 10 to 35
                (char)65, (char)66, (char)67, (char)68, (char)69, (char)70, (char)71, (char)72, (char)73, (char)74,             //  (A-J) = 10 to 19
                (char)75, (char)76, (char)77, (char)78, (char)79, (char)80, (char)81, (char)82, (char)83, (char)84,             //  (K-T) = 20 to 29
                (char)85, (char)86, (char)87, (char)88, (char)89, (char)90,                                                     //  (U-Z) = 30 to 35
                                                                                                                                //Lowercase letters     = 36 to 61
                (char)97, (char)98, (char)99, (char)100, (char)101, (char)102, (char)103, (char)104, (char)105, (char)106,      //  (a-j) = 36 to 45
                (char)107, (char)108, (char)109, (char)110, (char)111, (char)112, (char)113, (char)114, (char)115, (char)116,   //  (k-t) = 46 to 55
                (char)117, (char)118, (char)119, (char)120, (char)121, (char)122                                                //  (u-z) = 56 to 61
            };
            if (charType == 'N')
            {
                myChar = charTable[rand.Next(0, 10)];
            }
            else if (charType == 'L')
            {
                myChar = charTable[rand.Next(36, 62)];
            }
            else if (charType == 'U')
            {
                myChar = charTable[rand.Next(10, 36)];
            }
            else if (charType == 'A')
            {
                myChar = charTable[rand.Next(0, 62)];
            }
            else if (charType == 'E')
            {
                myChar = charTable[rand.Next(10, 62)];
            }
            else
            {
                myChar = charTable[rand.Next(0, 62)];
            }
            return myChar;
        }
    }
}

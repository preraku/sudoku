using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Sudoku1
{
    class Program
    {
        static void Main(string[] args)
        {
            Program aClass = new Program();
            string[,] s = new string[9, 9];
            string[,] f = new string[9, 9];
            List<string>[,] pv = new List<string>[9, 9];
            List<int> sI = new List<int> { };
            List<int> sJ = new List<int> { };
            List<string> value = new List<string> { };
            aClass.defineList(pv);
            aClass.initializeList(pv);
            aClass.readgrid(s, f);
            aClass.printgrid(s);
            int oiteration = 1;
  //          int iiteration = 1;
            int nF = 0;
  //          int sF = 0;
            int asF = 0;
            int s4sF = 0;
            // use info from the grid to reduce # of possiblities for each cell
            nF = aClass.solutionNotFound(pv, s);
            // reduce number of possibilities based on values already provided
            aClass.alterList(pv, s);
            nF = aClass.solutionNotFound(pv, s);
            asF = aClass.solutionFound(pv, s);
            aClass.saveSolution(s, pv, sI, sJ, value);
            // assign solutions found to the original grid
            aClass.alterGrid(s, pv);
            bool continueL = true;
            while (nF > 0 && continueL)
            {
                if (s4sF > 0)
                    asF = 1;
                while (asF > 0)
                {
                    // use improved grid to further reduce # of possiblities for each cell
                    aClass.alterList(pv, s);
                    asF = aClass.solutionFound(pv, s);
                    aClass.saveSolution(s, pv, sI, sJ, value);
                    // assign solutions found to the original grid
                    aClass.alterGrid(s, pv);
                }
                aClass.step4(pv, 0, 3, 0, 3);
                aClass.step4(pv, 0, 3, 3, 6);
                aClass.step4(pv, 0, 3, 6, 9);
                aClass.step4(pv, 3, 6, 0, 3);
                aClass.step4(pv, 3, 6, 3, 6);
                aClass.step4(pv, 3, 6, 6, 9);
                aClass.step4(pv, 6, 9, 0, 3);
                aClass.step4(pv, 6, 9, 3, 6);
                aClass.step4(pv, 6, 9, 6, 9);
                aClass.saveSolution(s, pv, sI, sJ, value);
                nF = aClass.solutionNotFound(pv, s);
 //               System.Console.WriteLine($"using step4 {oiteration} cells with missing values: {nF}");
                s4sF = aClass.solutionFound(pv, s);
 //               System.Console.WriteLine($"using step4 {oiteration} solution Found: {s4sF}");
                oiteration++;
//                System.Console.ReadKey(true);
                aClass.alterGrid(s, pv);
                if (nF > 0 && asF == 0 && s4sF == 0)
                {
                    continueL = false;
                }
            }
                Console.WriteLine("Press \"F\" for final solution, or any key for a step-by-step solution");
                if (Char.ToUpper((char)Console.Read()) != 'F'){
                    aClass.printSolution2(f, sI, sJ, value);
                }
                if (nF == 0)
                {
                    System.Console.WriteLine($"\nHere is the Sudoku completely solved:");
                }
                else
                {
                // print cells where possibilities are two ..
                    aClass.printMiniSquarePossibilities2(pv, 0, 3, 0, 3);
                    System.Console.WriteLine($"\nThis is the best I could do. {nF} values are unknown:");
                }
                aClass.printSolution(s, pv);
            //aClass.printListCount(pv);
            //aClass.printList(pv);
            System.Console.Write("\nPress any key to exit...");
            System.Console.ReadKey(true);
        }
        void readgrid(string[,] grid, string[,] grid2)
        {
            for (int i = 0; i < 9; i++)
            {
                System.Console.Write($"Enter row {i + 1}: ");
                string textInput = Console.ReadLine();
                int j = 0;
                foreach (char c in textInput)
                {
                    grid[i, j] = c.ToString();
                    grid2[i, j] = c.ToString();
                    j++;
                }
            }
        }
        void readgridbackup(string[,] grid)
        {
            for (int i = 0; i < 9; i++)
            {
                System.Console.Write($"Enter row {i + 1}: ");
                string textInput = Console.ReadLine();
                int j = 0;
                foreach (char c in textInput)
                {
                    grid[i, j++] = c.ToString();
                }
            }
        }
        void printgrid(string[,] grid)
        {
            System.Console.WriteLine();
            System.Console.WriteLine($"printing grid:");
            System.Console.WriteLine();
            for (int i = 0; i < 9; i++)
            {
                if (i % 3 == 0 && i != 0)
                {
                    System.Console.Write("\t");
                    System.Console.WriteLine("----------------------------");
                }
                System.Console.Write("\t");
                for (int j = 0; j < 9; j++)
                {
                    if (j % 3 == 0 && j != 0)
                    {
                        System.Console.Write("|");
                    }
                    System.Console.Write(" " + grid[i, j] + " ");
                }
                System.Console.WriteLine();
            }
        }
        void printRowColMini(string[,] s)
        {
            bool missing = false;
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    if (s[row, column] == "0")
                    {
                        System.Console.WriteLine($"number at row={row + 1}, column={column + 1} is {s[row, column]}");
                        System.Console.Write($"Printing contents of ********row {row + 1}: ");
                        for (int i = 0; i < 9; i++)
                            System.Console.Write(" " + s[row, i] + " ");
                        System.Console.WriteLine();
                        System.Console.Write($"Printing contents of *****column {column + 1}: ");
                        for (int i = 0; i < 9; i++)
                            System.Console.Write(" " + s[i, column] + " ");
                        System.Console.WriteLine();
                        System.Console.Write($"Printing contents of mini-square 1: ");
                        if ((row + 1 <= 3) && (column + 1 <= 3))
                        {
                            for (int i = 0; i < 3; i++)
                                for (int j = 0; j < 3; j++)
                                    System.Console.Write(" " + s[i, j] + " ");
                        }
                        System.Console.WriteLine();
                        missing = true;
                        break;
                    }
                }
                if (missing == true)
                    break;
            }
        }
        void PrintValues(List<string> members)
        {
            // Method code.
            foreach (var member in members)
            {
                System.Console.Write($"{member} ");
            }
            //          System.Console.WriteLine($"Possible values are: {members.Count}");
        }
        void PrintValues2(List<int> members)
        {
            // Method code.
            foreach (var member in members)
            {
                System.Console.WriteLine($"{member}");
            }
        }
        void defineList(List<string>[,] aList)
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    aList[i, j] = new List<string>();
                }
        }
        void printList(List<string>[,] aList)
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    System.Console.Write($"{i},{j}: ");
                    PrintValues(aList[i, j]);
                    System.Console.WriteLine();
                }
        }
        void initializeList(List<string>[,] aList)
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    for (int n = 1; n <= 9; n++)
                        aList[i, j].Add(Convert.ToString(n));
                }
        }
        // modify possible values in aList based on bList
        void alterList(List<string>[,] aList, string[,] bList)
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    if (bList[i, j] != "0")
                    {
                        // step 0. set aList[i,j] = bList[i,j]. clear all other values
                        aList[i, j].Clear();
                        aList[i, j].Add(bList[i, j]);
                        // remove blist[i,j] as a possible value from 
                        // 1. aList[i] row, 
                        // 2. aList[j] column, 
                        // 3. and [i,j] mini-square
                        // step 1. remove from row i
                        removeMembersRow(aList, bList, i, j);
                        // step 2. remove from column j
                        removeMembersColumn(aList, bList, i, j);
                        // step 3. remove from [i,j] mini squares
                        removeMembersMiniSquare(aList, bList, i, j);
                    }
                }
        }
        void printListCount(List<string>[,] aList)
        {
            System.Console.WriteLine();
            System.Console.WriteLine($"printing count of possible values:");
            System.Console.WriteLine();
            for (int i = 0; i < 9; i++)
            {
                if (i % 3 == 0 && i != 0)
                {
                    System.Console.Write("\t");
                    System.Console.WriteLine("----------------------------");
                }
                System.Console.Write("\t");
                for (int j = 0; j < 9; j++)
                {
                    if (j % 3 == 0 && j != 0)
                    {
                        System.Console.Write("|");
                    }
                    System.Console.Write(" " + aList[i, j].Count + " ");
                }
                System.Console.WriteLine();
            }
        }
        void printMiniSquarePossibilities(List<string>[,] aList, int startI, int endI, int startJ, int endJ)
        {
            for (int i = startI; i < endI; i++)
                for (int j = startJ; j < endJ; j++)
                {
                    System.Console.Write($"pv[{i},{j}]: ");
                    PrintValues(aList[i, j]);
                    System.Console.WriteLine();
                }
        }
        void printMiniSquarePossibilities2(List<string>[,] aList, int startI, int endI, int startJ, int endJ)
        {
            for (int i = startI; i < endI; i++)
                for (int j = startJ; j < endJ; j++)
                {
                    if (aList[i, j].Count == 2)
                    {
                        System.Console.Write($"pv[{i},{j}]: ");
                        PrintValues(aList[i, j]);
                        System.Console.WriteLine();
                    }
                }
        }
        int solutionFound(List<string>[,] aList, string[,] bList)
        {
            int solutionsFound = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if ((aList[i, j].Count == 1) && bList[i, j] == "0")
                        solutionsFound++;
                }
            }
            //if (solutionsFound > 0)
            //    System.Console.Write($"\nsolutions (listed within * * below) found: {solutionsFound}");
            return (solutionsFound);
        }
        int solutionNotFound(List<string>[,] aList, string[,] bList)
        {
            int solutionsNotFound = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if ((aList[i, j].Count > 1) && bList[i, j] == "0")
                        solutionsNotFound++;
                }
            }
            //if (solutionsNotFound > 0)
            //    System.Console.WriteLine($"\nsolutions not found for: {solutionsNotFound}");
            return (solutionsNotFound);
        }
        void printSolution(string[,] grid, List<string>[,] aList)
        {
            // System.Console.WriteLine($", listed within * * below:");
            //System.Console.WriteLine();
            for (int i = 0; i < 9; i++)
            {
                if (i % 3 == 0 && i != 0)
                {
                    System.Console.Write("\t");
                    System.Console.WriteLine("----------------------------");
                }
                System.Console.Write("\t");
                for (int j = 0; j < 9; j++)
                {
                    if (j % 3 == 0 && j != 0)
                    {
                        System.Console.Write("|");
                    }
                    if ((aList[i, j].Count == 1) && grid[i, j] == "0")
                        System.Console.Write("*" + aList[i, j][0] + "*");
                    else
                        System.Console.Write(" " + grid[i, j] + " ");
                }
                System.Console.WriteLine();
            }
        }
        void printSolution2(string[,] grid, List<int> sI, List<int> sJ, List<string> value)
        {
            // System.Console.WriteLine($", listed within * * below:");
            System.Console.WriteLine();
            char readFromUser = 'P';
            for (int iteration = 0; iteration < sI.Count; iteration++)
            {
                System.Console.WriteLine($"\niteration: {iteration + 1}; solution is shown within * *");
                for (int i = 0; i < 9; i++)
                {
                    if (i % 3 == 0 && i != 0)
                    {
                        System.Console.Write("\t");
                        System.Console.WriteLine("----------------------------");
                    }
                    System.Console.Write("\t");
                    for (int j = 0; j < 9; j++)
                    {
                        if (j % 3 == 0 && j != 0)
                        {
                            System.Console.Write("|");
                        }
                        if ((sI[iteration] == i) && (sJ[iteration] == j))
                        {
                            System.Console.Write("*" + value[iteration] + "*");
                            grid[i, j] = value[iteration];
                        }
                        else
                        {
                            System.Console.Write(" " + grid[i, j] + " ");
                        }
                    }
                    System.Console.WriteLine();
                }
                if (readFromUser != 'S') {
                    System.Console.Write("\nPress \"S\" to print all steps at once or any other key to continue:");
                    readFromUser = Char.ToUpper((char)Console.Read());
                }

                // System.Console.ReadKey(true);
            }
        }
        void saveSolution(string[,] grid, List<string>[,] aList, List<int> sI, List<int> sJ, List<string> value)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if ((aList[i, j].Count == 1) && grid[i, j] == "0")
                    {
                        sI.Add(i);
                        sJ.Add(j);
                        value.Add(aList[i, j][0]);
                        //                       System.Console.Write("*" + aList[i, j][0] + "*");
                    }
                }
            }
        }
        void alterGrid(string[,] grid, List<string>[,] aList)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (aList[i, j].Count == 1 && grid[i, j] == "0")
                        grid[i, j] = aList[i, j][0];
                }
            }
        }
        void removeMembersRow(List<string>[,] aList, string[,] bList, int i, int j)
        {
            for (int column = 0; column < 9; column++)
                if (column != j)
                    aList[i, column].Remove(bList[i, j]);
        }
        void removeMembersColumn(List<string>[,] aList, string[,] bList, int i, int j)
        {
            for (int row = 0; row < 9; row++)
                if (row != i)
                    aList[row, j].Remove(bList[i, j]);
        }
        void removeMembersMiniSquare(List<string>[,] aList, string[,] bList, int i, int j)
        {
            // step 3.1 remove from top left mini square
            if ((i < 3) && (j < 3))
            {
                for (int row = 0; row < 3; row++)
                    for (int column = 0; column < 3; column++)
                    {
                        if (((row == i) && (column == j)) != true)
                        {
                            aList[row, column].Remove(bList[i, j]);
                        }
                    }
            }
            // step 3.2 remove from top middle mini square
            if ((i < 3) && (j > 2 && j < 6))
            {
                for (int row = 0; row < 3; row++)
                    for (int column = 3; column < 6; column++)
                    {
                        if (((row == i) && (column == j)) != true)
                        {
                            aList[row, column].Remove(bList[i, j]);
                        }
                    }
            }
            // step 3.3 remove from top right mini square
            if ((i < 3) && (j > 5 && j < 9))
            {
                for (int row = 0; row < 3; row++)
                    for (int column = 6; column < 9; column++)
                    {
                        if (((row == i) && (column == j)) != true)
                        {
                            aList[row, column].Remove(bList[i, j]);
                        }
                    }
            }
            // step 3.4 remove from middle left mini square
            if ((i > 2 && i < 6) && (j < 3))
            {
                for (int row = 3; row < 6; row++)
                    for (int column = 0; column < 3; column++)
                    {
                        if (((row == i) && (column == j)) != true)
                        {
                            aList[row, column].Remove(bList[i, j]);
                        }
                    }
            }
            // step 3.5 remove from center mini square
            if ((i > 2 && i < 6) && (j > 2 && j < 6))
            {
                for (int row = 3; row < 6; row++)
                    for (int column = 3; column < 6; column++)
                    {
                        if (((row == i) && (column == j)) != true)
                        {
                            aList[row, column].Remove(bList[i, j]);
                        }
                    }
            }
            // step 3.6 remove from middle right mini square
            if ((i > 2 && i < 6) && (j > 5 && j < 9))
            {
                for (int row = 3; row < 6; row++)
                    for (int column = 6; column < 9; column++)
                    {
                        if (((row == i) && (column == j)) != true)
                        {
                            aList[row, column].Remove(bList[i, j]);
                        }
                    }
            }
            // step 3.7 remove from bottom left mini square
            if ((i > 5 && i < 9) && (j < 3))
            {
                for (int row = 6; row < 9; row++)
                    for (int column = 0; column < 3; column++)
                    {
                        if (((row == i) && (column == j)) != true)
                        {
                            aList[row, column].Remove(bList[i, j]);
                        }
                    }
            }
            // step 3.8 remove from bottom middle mini square
            if ((i > 5 && i < 9) && (j > 2 && j < 6))
            {
                for (int row = 6; row < 9; row++)
                    for (int column = 3; column < 6; column++)
                    {
                        if (((row == i) && (column == j)) != true)
                        {
                            aList[row, column].Remove(bList[i, j]);
                        }
                    }
            }
            // step 3.9 remove from bottom right mini square
            if ((i > 5 && i < 9) && (j > 5 && j < 9))
            {
                for (int row = 6; row < 9; row++)
                    for (int column = 6; column < 9; column++)
                    {
                        if (((row == i) && (column == j)) != true)
                        {
                            aList[row, column].Remove(bList[i, j]);
                        }
                    }
            }
        }
        void step4(List<string>[,] aList, int startI, int endI, int startJ, int endJ)
        {
            /*
            step 4. check if in a mini-square list of possible values is only 1 and no more than 1 when you look at all the squares
                0,0: 6
                0,1: 3
                0,2: 1 8
                1,0: 4
                1,1: 2 5
                1,2: 2 8
                2,0: 9
                2,1: 2 5
                2,2: 7
            Here you can see that in this entire mini-square 1 is only possible at 0, 2, hence 0, 2 is definitely 1.
            */
            //step 4.1 concat all lists with more than one possibilities in mini - square 1
            //System.Console.WriteLine("step 4");
            List<string>[,] m = new List<string>[1, 1];
            m[0, 0] = new List<string>();
            int numLists = 0; // number of cells that have more than one possibilities, in our example, 4 (0,2; 1,1; 1,2; 2,1) 
            for (int i = startI; i < endI; i++)
                for (int j = startJ; j < endJ; j++)
                {
                    if (aList[i, j].Count > 1)
                    {
                        m[0, 0] = m[0, 0].Concat(aList[i, j]).ToList(); // merge all the possibilities (1, 8; 2, 5; 2, 8; 2, 5) into one m
                        numLists++;
                    }
                }
            if (numLists > 1)
            {
                //print before step 4
                //System.Console.WriteLine($"before step 4: ");
                //printMiniSquarePossibilities(aList, startI, endI, startJ, endJ);
                // result contains only those values which appeared only once in all the lists from all the cells. In our example, 1
                var result =
                    from x in m[0, 0]
                    group m[0, 0] by x into grp
                    where grp.Count() == 1
                    select grp.Key;
                // let us clear all other values from the cell that contains the value that appeared only once in all the lists from all the cells
                // in our example, 0,2 would be set to 1
                foreach (var member in result.ToList())
                {
                    for (int i = startI; i < endI; i++)
                        for (int j = startJ; j < endJ; j++)
                        {
                            if (aList[i, j].Count > 1)
                            {
                                if (aList[i, j].Exists(x => x == member))
                                {
                                    aList[i, j].Clear();
                                    aList[i, j].Add(member);
                                }
                            }
                        }
                }
                //print after step 4
                //System.Console.WriteLine($"after step 4: ");
                //printMiniSquarePossibilities(aList, startI, endI, startJ, endJ);
            }
        }
    }


}
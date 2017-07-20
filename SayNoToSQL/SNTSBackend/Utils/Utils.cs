﻿using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Compiler;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Learning.Logging;
using Microsoft.ProgramSynthesis.Learning.Strategies;
using Microsoft.ProgramSynthesis.Specifications;
using Microsoft.ProgramSynthesis.VersionSpace;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SNTSBackend.Utils
{
    public class Utils
    {
        public static Grammar LoadGrammar(string grammarFile)
        {
            var compilationResult = DSLCompiler.ParseGrammarFromFile(grammarFile);
            if (compilationResult.HasErrors)
            {
                WriteColored(ConsoleColor.Magenta, compilationResult.TraceDiagnostics);
                throw new Exception("Grammar file has compilation errors!!");
            }
            if (compilationResult.Diagnostics.Count > 0)
            {
                WriteColored(ConsoleColor.Yellow, compilationResult.TraceDiagnostics);
            }

            return compilationResult.Value;
        }

        public static void WriteColored(ConsoleColor color, object obj) => WriteColored(color, () => Console.WriteLine(obj));

        private static void WriteColored(ConsoleColor color, Action write)
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            write();
            Console.ForegroundColor = currentColor;
        }

        public static DataTable[] GeneratePowerSet(DataTable table)
        {
            List<DataTable> outputTables = new List<DataTable>();
            int n = table.Rows.Count;

            // Power set contains 2^N subsets.
            int powerSetCount = 1 << n;

            for (int setMask = 0; setMask < powerSetCount; setMask++)
            {
                DataTable outputTable = table.Clone();
                for (int i = 0; i < n; i++)
                {
                    // Checking whether i'th element of input collection should go to the current subset.
                    if ((setMask & (1 << i)) > 0)
                    {
                        outputTable.ImportRow(table.Rows[i]);
                    }
                }
                outputTables.Add(outputTable);
            }

            return outputTables.ToArray();
        }
    }
}

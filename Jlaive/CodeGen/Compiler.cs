﻿using System;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;

namespace Jlaive
{
    internal struct JCompilerResult
    {
        public CompilerResults CompilerResults;
        public byte[] AssemblyBytes;

        public JCompilerResult(CompilerResults CompilerResults, byte[] AssemblyBytes)
        {
            this.CompilerResults = CompilerResults;
            this.AssemblyBytes = AssemblyBytes;
        }
    }

    internal class Compiler
    {
        public CSharpCodeProvider CSC { get; }
        public string[] References { get; set; }
        public string[] Resources { get; set; }

        public Compiler()
        {
            CSC = new CSharpCodeProvider();
        }

        public JCompilerResult Build(string source)
        {
            string tempfile = $"{AppDomain.CurrentDomain.BaseDirectory}\\{new RandomString(new Random()).Get(6)}.tmp";
            var parameters = new CompilerParameters(References, tempfile)
            {
                GenerateExecutable = true,
                CompilerOptions = "-optimize",
                IncludeDebugInformation = false
            };
            CompilerResults results = CSC.CompileAssemblyFromSource(parameters, source);
            byte[] bytes = Array.Empty<byte>();
            if (File.Exists(tempfile))
            {
                bytes = File.ReadAllBytes(tempfile);
                File.Delete(tempfile);
            }
            return new JCompilerResult(results, bytes);
        }
    }
}

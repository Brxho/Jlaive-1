using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Jlaive
{
    internal class Patcher
    {
        public static byte[] Fix(byte[] input)
        {
            ModuleDefMD module = ModuleDefMD.Load(input);
            foreach (TypeDef type in module.GetTypes())
            {
                if (type.IsGlobalModuleType) continue;
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody) continue;
                    IList<Instruction> instr = method.Body.Instructions;
                    for (var i = 0; i < instr.Count; i++)
                    {
                        if (instr[i].ToString().Contains(".bat.exe"))
                        {
                            instr.Insert(i + 1, OpCodes.Ldstr.ToInstruction(".bat.exe"));
                            instr.Insert(i + 2, OpCodes.Ldstr.ToInstruction(".bat"));
                            instr.Insert(i + 3, OpCodes.Callvirt.ToInstruction(method.Module.Import(GetSystemMethod(typeof(string), "Replace", 1))));
                            i += 3;
                        }
                        else if (instr[i].ToString().Contains("System.Diagnostics.ProcessModule::get_FileName()"))
                        {
                            instr.Insert(i + 1, OpCodes.Ldstr.ToInstruction(".bat.exe"));
                            instr.Insert(i + 2, OpCodes.Ldstr.ToInstruction(".bat"));
                            instr.Insert(i + 3, OpCodes.Callvirt.ToInstruction(method.Module.Import(GetSystemMethod(typeof(string), "Replace", 1))));
                            i += 3;
                        }
                        else if (instr[i].ToString().Contains("System.Reflection.Assembly::get_Location()"))
                        {
                            instr.Insert(i + 1, OpCodes.Ldstr.ToInstruction(".bat.exe"));
                            instr.Insert(i + 2, OpCodes.Ldstr.ToInstruction(".bat"));
                            instr.Insert(i + 3, OpCodes.Callvirt.ToInstruction(method.Module.Import(GetSystemMethod(typeof(string), "Replace", 1))));
                            i += 3;
                        }
                        else if (instr[i].ToString().Contains("System.Reflection.Assembly::GetEntryAssembly()"))
                        {
                            instr[i] = OpCodes.Call.ToInstruction(method.Module.Import(GetSystemMethod(typeof(Assembly), "GetExecutingAssembly")));
                        }
                    }
                    method.Body.SimplifyBranches();
                }
            }
            MemoryStream ms = new MemoryStream();
            module.Write(ms);
            byte[] output = ms.ToArray();
            ms.Dispose();
            return output;
        }

        private static MethodDef GetSystemMethod(Type type, string name, int idx = 0)
        {
            string filename = type.Module.FullyQualifiedName;
            ModuleDefMD module = ModuleDefMD.Load(filename);
            TypeDef[] types = module.GetTypes().ToArray();
            List<MethodDef> methods = new List<MethodDef>();
            foreach (TypeDef t in types)
            {
                if (t.Name != type.Name) continue;
                foreach (var m in t.Methods)
                {

                    if (m.Name != name) continue;
                    methods.Add(m);
                }
            }
            if (methods.Count > 0) return methods[idx];
            return null;
        }
    }
}

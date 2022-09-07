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
        public static void AddResources(ref byte[] input, PatcherResource[] resources)
        {
            ModuleDefMD module = ModuleDefMD.Load(input);
            foreach (PatcherResource resource in resources)
            {
                module.Resources.Add(new EmbeddedResource(resource.Name, resource.Bytes));
            }
            MemoryStream ms = new MemoryStream();
            module.Write(ms);
            input = ms.ToArray();
            ms.Dispose();
        }

        public static void Fix(ref byte[] input)
        {
            ModuleDefMD module = ModuleDefMD.Load(input);
            MethodDef replace = GetSystemMethod(typeof(string), "Replace", 1);
            MethodDef getexecutingassembly = GetSystemMethod(typeof(Assembly), "GetExecutingAssembly");
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
                            instr.Insert(i + 3, OpCodes.Callvirt.ToInstruction(method.Module.Import(replace)));
                            i += 3;
                        }
                        else if (instr[i].ToString().Contains("System.Diagnostics.ProcessModule::get_FileName()"))
                        {
                            instr.Insert(i + 1, OpCodes.Ldstr.ToInstruction(".bat.exe"));
                            instr.Insert(i + 2, OpCodes.Ldstr.ToInstruction(".bat"));
                            instr.Insert(i + 3, OpCodes.Callvirt.ToInstruction(method.Module.Import(replace)));
                            i += 3;
                        }
                        else if (instr[i].ToString().Contains("System.Reflection.Assembly::get_Location()"))
                        {
                            instr.Insert(i + 1, OpCodes.Ldstr.ToInstruction(".bat.exe"));
                            instr.Insert(i + 2, OpCodes.Ldstr.ToInstruction(".bat"));
                            instr.Insert(i + 3, OpCodes.Callvirt.ToInstruction(method.Module.Import(replace)));
                            i += 3;
                        }
                        else if (instr[i].ToString().Contains("System.Reflection.Assembly::GetEntryAssembly()"))
                        {
                            instr[i] = OpCodes.Call.ToInstruction(method.Module.Import(getexecutingassembly));
                        }
                    }
                    method.Body.SimplifyBranches();
                }
            }
            MemoryStream ms = new MemoryStream();
            module.Write(ms);
            input = ms.ToArray();
            ms.Dispose();
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

    internal struct PatcherResource
    {
        public string Name;
        public byte[] Bytes;

        public PatcherResource(string Name, byte[] Bytes)
        {
            this.Name = Name;
            this.Bytes = Bytes;
        }
    }
}

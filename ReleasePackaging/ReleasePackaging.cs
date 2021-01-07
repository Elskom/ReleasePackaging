// Copyright (c) 2018-2020, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;
    using System.IO;
    using System.IO.Compression;

    /// <summary>
    /// Handles application's release packaging command line.
    /// </summary>
    public static class ReleasePackaging
    {
        /// <summary>
        /// Packages an application's Release build to a zip file.
        /// </summary>
        /// <param name="args">The command line arguments passed into the calling process.</param>
        public static void PackageRelease(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            string outfilename;
            if (args[1].StartsWith(".\\", StringComparison.Ordinal))
            {
                // Replace spaces with periods.
                outfilename = ReplaceStr(args[1], " ", ".", StringComparison.Ordinal);
                args[1] = ReplaceStr(ReplaceStr(args[1], " ", ".", StringComparison.Ordinal), ".\\", Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar, StringComparison.Ordinal);
            }
            else if (args[1].StartsWith("./", StringComparison.Ordinal))
            {
                // Replace spaces with periods.
                outfilename = ReplaceStr(args[1], " ", ".", StringComparison.Ordinal);
                args[1] = ReplaceStr(ReplaceStr(args[1], " ", ".", StringComparison.Ordinal), "./", Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar, StringComparison.Ordinal);
            }
            else
            {
                // Replace spaces with periods.
                outfilename = ReplaceStr(args[1], " ", ".", StringComparison.Ordinal);
                args[1] = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + ReplaceStr(args[1], " ", ".", StringComparison.Ordinal);
            }

            if (args[0].Equals("-p", StringComparison.Ordinal))
            {
                Console.WriteLine("Writing build files and debug symbol files to " + outfilename + ".");
                if (File.Exists(args[1]))
                {
                    File.Delete(args[1]);
                }

                using (var zipFile = ZipFile.Open(args[1], ZipArchiveMode.Update))
                {
                    var di1 = new DirectoryInfo(Directory.GetCurrentDirectory());
                    foreach (var fi1 in di1.GetFiles("*.exe"))
                    {
                        var exe_file = fi1.Name;
                        _ = zipFile.CreateEntryFromFile(exe_file, exe_file);
                    }

                    foreach (var fi2 in di1.GetFiles("*.dll"))
                    {
                        var dll_file = fi2.Name;
                        _ = zipFile.CreateEntryFromFile(dll_file, dll_file);
                    }

                    foreach (var fi3 in di1.GetFiles("*.xml"))
                    {
                        var xml_file = fi3.Name;
                        if (!xml_file.EndsWith(".CodeAnalysisLog.xml", StringComparison.Ordinal))
                        {
                            _ = zipFile.CreateEntryFromFile(xml_file, xml_file);
                        }
                    }

                    foreach (var fi4 in di1.GetFiles("*.txt"))
                    {
                        var txt_file = fi4.Name;
                        _ = zipFile.CreateEntryFromFile(txt_file, txt_file);
                    }

                    foreach (var fi5 in di1.GetFiles("*.pdb"))
                    {
                        var pdb_file = fi5.Name;
                        _ = zipFile.CreateEntryFromFile(pdb_file, pdb_file);
                    }

                    foreach (var di2 in di1.GetDirectories())
                    {
                        foreach (var fi6 in di2.GetFiles("*.pdb"))
                        {
                            var pdb_file1 = fi6.Name;
                            _ = zipFile.CreateEntryFromFile(di2.Name + Path.DirectorySeparatorChar + pdb_file1, di2.Name + Path.DirectorySeparatorChar + pdb_file1);
                        }

                        foreach (var fi7 in di2.GetFiles("*.dll"))
                        {
                            var dll_file1 = fi7.Name;
                            _ = zipFile.CreateEntryFromFile(di2.Name + Path.DirectorySeparatorChar + dll_file1, di2.Name + Path.DirectorySeparatorChar + dll_file1);
                        }

                        foreach (var fi8 in di2.GetFiles("*.xml"))
                        {
                            var xml_file1 = fi8.Name;
                            if (!xml_file1.EndsWith(".CodeAnalysisLog.xml", StringComparison.Ordinal))
                            {
                                _ = zipFile.CreateEntryFromFile(di2.Name + Path.DirectorySeparatorChar + xml_file1, di2.Name + Path.DirectorySeparatorChar + xml_file1);
                            }
                        }

                        foreach (var fi9 in di2.GetFiles("*.txt"))
                        {
                            var txt_file1 = fi9.Name;
                            _ = zipFile.CreateEntryFromFile(di2.Name + Path.DirectorySeparatorChar + txt_file1, di2.Name + Path.DirectorySeparatorChar + txt_file1);
                        }
                    }
                }
            }
        }

        private static string ReplaceStr(string str1, string str2, string str3, StringComparison comp)
        {
#if NETSTANDARD2_1 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1
            return str1.Replace(str2, str3, comp);
#else
            if (comp == StringComparison.Ordinal)
            {
            }

            return str1.Replace(str2, str3);
#endif
        }
    }
}

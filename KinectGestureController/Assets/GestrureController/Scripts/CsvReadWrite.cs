using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

public class CsvReadWrite
{

    public static void SaveCsv<T>(IEnumerable<T> objectlist, string path)
    {
        string filePath = path;

        using (StreamWriter outStream = File.CreateText(filePath))
        {
            outStream.WriteLine(RemoveEmptyLines(ToCsv(",", objectlist)));
            outStream.Close();
        }
    }

    private static string ToCsv<T>(string separator, IEnumerable<T> objectlist)
    {
        Type t = typeof(T);
        FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        string header = String.Join(separator, fields.Select(f => f.Name).ToArray());

        StringBuilder csvdata = new StringBuilder();
        csvdata.AppendLine(header);

        foreach (var o in objectlist)
            csvdata.AppendLine(ToCsvFields(separator, fields, o));

        return csvdata.ToString();
    }

    private static string ToCsvFields(string separator, FieldInfo[] fields, object o)
    {
        StringBuilder linie = new StringBuilder();

        foreach (var f in fields)
        {
            if (linie.Length > 0)
                linie.Append(separator);

            var x = f.GetValue(o);

            if (x != null)
                linie.Append(x.ToString());
        }

        return linie.ToString();
    }

    private static string RemoveEmptyLines(string lines)
    {
        return Regex.Replace(lines, @"^\s*$\n|\r", "", RegexOptions.Multiline).TrimEnd();
    }
}
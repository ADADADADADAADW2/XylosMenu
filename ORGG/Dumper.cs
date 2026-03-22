using System;
using System.Reflection;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Assembly asm = Assembly.LoadFrom(@"C:\Program Files (x86)\Steam\steamapps\common\Gorilla Tag\Gorilla Tag_Data\Managed\Assembly-CSharp.dll");
            foreach (Type t in asm.GetTypes())
            {
                if (t.Name.Contains("Barrel") || t.Name.Contains("Experiment") || t.Name.Contains("Cannon"))
                {
                    Console.WriteLine("Type: " + t.Name);
                    foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                    {
                        if (m.Name.Contains("Barrel") || m.Name.Contains("Projectile") || m.Name.Contains("Fire"))
                            Console.WriteLine("  Method: " + m.Name);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}

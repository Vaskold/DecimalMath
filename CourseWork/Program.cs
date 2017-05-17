using System;
using dm;
using Newton;

namespace CourseWork
{
    internal class Program
    {
        private static void Main()
        {
            var a = new Infinitely(10, 16);
            var b = new Infinitely(10, 16);
            var c = new Infinitely(125, 120);
            c = Idm.Exp(Infinitely.One(c.Adn, c.Fdn));
            Infinitely.Show(c);
            Infinitely.Show(Idm.E(c.Adn, c.Fdn));
            Infinitely.Show(c);
            Console.ReadLine();
        }
    }
}
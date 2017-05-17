/*
    BruteMind Framework for .Net
    Copyright (C) 2016  Valeriy Garnaga

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*/

using System;
using System.Threading.Tasks;
using Newton;

namespace dm
{
    public static class Idm
    {
        public const decimal Precision = 1E-28m;
        public const decimal SmallPrecision = 1E-14m;

        public static Infinitely E(long a, long b)
        {
            var t = new Infinitely(a, b);
            Infinitely.ToInfinitely(
                "2,7182818284590452353602874713526624977572470936999595749669676277240766303535475945713821785251664274",
                t);
            return t;
        }

        public static Infinitely Pi(long a, long b)
        {
            var t = new Infinitely(a, b);
            Infinitely.ToInfinitely(
                "3,1415926535897932384626433832795028841971693993751058209749445923078164062862089986280348253421170679",
                t);
            return t;
        }


        public static decimal Fact(int z)
        {
            try
            {
                return z == 0 ? 1m : z * Fact(z - 1);
            }
            catch (Exception)
            {
                return decimal.MaxValue;
            }
        }

        public static long Gcd(long a, long b)
        {
            while (true)
            {
                if (b == 0) return a;
                var a1 = a;
                a = b;
                b = a1 % b;
            }
        }

        public static decimal Sqrt(decimal x)
        {
            var curr = (decimal) Math.Sqrt((double) x);
            checked
            {
                try
                {
                    decimal prev;
                    do
                    {
                        prev = curr;
                        curr = (prev + x / prev) / 2.0m;
                    } while (Math.Abs(curr - prev) > Precision);
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("Cannot divide by zero");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Overflow");
                }
            }
            return curr;
        }

        public static Infinitely P(PowArg zz)
        {
            var result = new Infinitely(zz.X.Adn, zz.X.Fdn);
            Infinitely.ToInfinitely(1.0m, result);

            for (uint i = 0; i < zz.Y; i++)
                result *= zz.X;
            return result;
        }

        public static Infinitely APow(Infinitely xx, uint z)
        {
            PowArg x1, x2;

            if (z % 2 == 0)
            {
                x1 = new PowArg(xx, z / 2);
                x2 = new PowArg(xx, z / 2);
            }
            else
            {
                x1 = new PowArg(xx, z / 2);
                x2 = new PowArg(xx, z / 2 + 1);
            }

            var t1 = new Task<Infinitely>(x => P((PowArg) x), x1);
            var t2 = new Task<Infinitely>(x => P((PowArg) x), x2);
            t1.Start();
            t2.Start();
            return t1.Result * t2.Result;
        }

        /*public static Infinitely Exp1(Infinitely x)
        {
            var result = Infinitely.One(x.Adn, x.Fdn);
            var pow = Infinitely.One(x.Adn, x.Fdn);
            var i = Infinitely.One(x.Adn, x.Fdn);
            var j = Infinitely.One(x.Adn, x.Fdn) + 1;
            var tpow = x * x;

            checked
            {
                try
                {
                    for (var ii = 0; ii < x.Adn; ii++)
                    {
                        pow *= tpow / (i * j);
                        result += pow;
                        i += 2;
                        j += 2;
                    }
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("zero division");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("overflow");
                }
            }

            return result;
        }

        public static Infinitely Exp2(Infinitely x)
        {
            var result = x;
            var pow = x;
            var i = Infinitely.One(x.Adn, x.Fdn) + 1;
            var j = Infinitely.One(x.Adn, x.Fdn) + 2;

            checked
            {
                try
                {
                    for (var ii = 0; ii < x.Adn; ii++)
                    {
                        pow *= x * x / (i * j);
                        result += pow;
                        i += 2;
                        j += 2;
                    }
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("zero division");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("overflow");
                }
            }

            return result;
        }*/

        public static Infinitely Exp1(Infinitely x)
        {
            var result = Infinitely.One(x.Adn, x.Fdn);
            var pow = Infinitely.One(x.Adn, x.Fdn);
            var i = Infinitely.One(x.Adn, x.Fdn);
            var j = Infinitely.One(x.Adn, x.Fdn) + 1.0m;
            var k = Infinitely.One(x.Adn, x.Fdn) + 2.0m;
            var tpow = APow(x, 3);
            

            checked
            {
                try
                {
                    for (var ii = 0; ii < x.Adn; ii++)
                    {
                        var div = i * j * k;
                        pow *= tpow / div;
                        result += pow;
                        i += 3.0m;
                        j += 3.0m;
                        k += 3.0m;
                    }
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("zero division");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("overflow");
                }
            }

            return result;
        }

        public static Infinitely Exp2(Infinitely x)
        {
            var result = x;
            var pow = x;
            var tpow = APow(x, 3);
            var i = Infinitely.One(x.Adn, x.Fdn) + 1;
            var j = Infinitely.One(x.Adn, x.Fdn) + 2;
            var k = Infinitely.One(x.Adn, x.Fdn) + 3;

            checked
            {
                try
                {
                    for (var ii = 0; ii < x.Adn; ii++)
                    {
                        var div = i * j * k;
                        pow *= tpow / div;
                        result += pow; 
                        i += 3.0m;
                        j += 3.0m;
                        k += 3.0m;
                    }
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("zero division");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("overflow");
                }
            }

            return result;
        }

        public static Infinitely Exp3(Infinitely x)
        {
            var result = x * x / 2;
            var pow = x * x;
            var tpow = APow(x, 3);
            var i = Infinitely.One(x.Adn, x.Fdn) + 2;
            var j = Infinitely.One(x.Adn, x.Fdn) + 3;
            var k = Infinitely.One(x.Adn, x.Fdn) + 4;
            var c = true;

            checked
            {
                try
                {
                    for (var ii = 0; ii < x.Adn; ii++)
                    {
                        var div = i * j * k;
                        if (c)
                        {
                            div *= 2m;
                            c = false;
                        }
                        pow *= tpow / div;
                        result += pow;
                        i += 3.0m;
                        j += 3.0m;
                        k += 3.0m;
                    }
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("zero division");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("overflow");
                }
            }

            return result;
        }

        public static Infinitely Exp(Infinitely z)
        {
            var t1 = new Task<Infinitely>(x => Exp1((Infinitely) x), z);
            var t2 = new Task<Infinitely>(x => Exp2((Infinitely) x), z);
            var t3 = new Task<Infinitely>(x => Exp3((Infinitely) x), z);
            t1.Start();
            t2.Start();
            t3.Start();
            return t1.Result + t2.Result + t3.Result;
        }

        /*  public static Infinitely NthRoot(Infinitely power, Infinitely root)
        {
            /* (C) John Gabriel #1#
            Infinitely l;
            var a = power;
            var n = root;
            var s = Infinitely.One(power.ADN, power.FDN);

            do
            {
                l = a / Pow(s, n - 1.0m);
                var r = (n - 1.0m) * s;
                s = (l + r) / n;
            } while (Infinitely.Abs(l - s) > SmallPrecision);

            return s;
        }*/

        public static Infinitely Ln(Infinitely power)
        {
            var p = power;
            var result = Infinitely.Zero(power.Adn, power.Fdn);

            while (p >= E(power.Adn, power.Fdn))
            {
                p /= E(power.Adn, power.Fdn);
                result = result + 1.0m;
            }
            result += p / E(power.Adn, power.Fdn);
            p = power;

            checked
            {
                try
                {
                    var old = new Infinitely(power.Adn, power.Fdn);
                    do
                    {
                        old = result;
                        result = p / Exp(result - 1.0m) / E(power.Adn, power.Fdn) + (result - 1.0m);
                    } while (Infinitely.Abs(result - old) > Precision);
                }
                catch (OverflowException)
                {
                }
                catch (DivideByZeroException)
                {
                }
            }

            return result;
        }

//        public static Infinitely Pow(Infinitely x, Infinitely y)
//        {
//            if (x > 0)
//                return Exp(y * Ln(x));
//            var t = y - Math.Truncate(y); // 0.xxx..
//            if (t == 0) return -1 * Exp(y * Ln(-1 * x));
//            var s = t.ToString().Substring(y < 0 ? 3 : 2); // xxx..
//            var r = Math.Pow(10, t.ToString().Substring(y < 0 ? 3 : 2).Length); // 10^
//            var m = Gcd(long.Parse(s), (long) r);
//            return x < 0 && (int) r / m % 2 != 0 ? -1 * Exp(y * Ln(-1 * x)) : Infinitely.Zero(x.ADN, x.FDN);
//        }

        public static Infinitely Log(Infinitely a, Infinitely b)
        {
            return Ln(a) / Ln(b);
        }

        public static Infinitely Log2(Infinitely a)
        {
            return Ln(a) / Ln(Infinitely.One(a.Adn, a.Fdn) + 1);
        }

        public static Infinitely Log10(Infinitely a)
        {
            return Ln(a) / Ln(Infinitely.One(a.Adn, a.Fdn) + 9);
        }

        public static Infinitely ALog(Infinitely power, Infinitely exponent)
        {
            return Exp(Ln(power) / exponent);
        }

        public class PowArg
        {
            public Infinitely X;
            public uint Y;

            public PowArg(Infinitely x, uint y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
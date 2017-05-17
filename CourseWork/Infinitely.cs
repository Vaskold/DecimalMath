/*
    BruteMind Framework for .Net
    Copyright (C) 2016  Valeriy Garnaga

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*/

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Newton
{


    public class InfinitelyException : Exception
    {
        public InfinitelyException(string message) : base(message)
        {
        }
    }

    // класс для передачи параметров в ассинхронные методы
    public class Point
    {
        public Infinitely A, B, C;
        public int I, J, Ii, Jj;

        public Point(int i, int j, int ii, int jj, Infinitely a, Infinitely b, Infinitely c)
        {
            I = i;
            J = j;
            Ii = ii;
            Jj = jj;
            A = a;
            B = b;
            C = c;
        }
    }


    public class Infinitely
    {
        // long –9,223,372,036,854,775,808 to 9,223,372,036,854,775,807
        // 64 bit
        // 4 bit per digit, 16 digits per long

        private const int IZero = 0;
        public const int Negative = -1;
        public const int Positive = 1;
        public const int PositiveInfinity = int.MaxValue;
        public const int NegativeInfinity = int.MinValue;
        public readonly long Adn;
        public readonly long Fdn;

        private int _attribute;
        private long[] _number;
        private bool _zero;

        public Infinitely(long adn, long fdn)
        {
            Adn = adn;
            Fdn = fdn;
            _number = new long[1 + adn / 2];
        }

        protected bool Equals(Infinitely other)
        {
            return Adn == other.Adn && _attribute == other._attribute &&
                   Fdn == other.Fdn && Equals(_number, other._number) &&
                   _zero == other._zero;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Infinitely) obj);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Adn.GetHashCode();
                hashCode = (hashCode * 397) ^ _attribute;
                hashCode = (hashCode * 397) ^ Fdn.GetHashCode();
                hashCode = (hashCode * 397) ^ (_number != null ? _number.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ _zero.GetHashCode();
                return hashCode;
            }
        }

     
        public static Infinitely Zero(long a, long b)
        {
            var t = new Infinitely(a, b);
            ToInfinitely(IZero, t);
            return t;
        }
        public static Infinitely One(long a, long b)
        {
            var t = new Infinitely(a, b);
            ToInfinitely(Positive, t);
            return t;
        }

        public static void ToInfinitely(int d, Infinitely a)
        {
            a._number = new long[1 + a.Adn / 2];
            if (d != 0)
            {
                int g;
                if (d > 0)
                {
                    a._attribute = 1;
                    g = d;
                }
                else
                {
                    a._attribute = -1;
                    g = -d;
                }
                int i = 0, j = 0;
                long w = 0;
                while (w < a.Fdn)
                {
                    if (++j > 15)
                    {
                        j = 0;
                        ++i;
                    }
                    ++w;
                }

                for (; i < a._number.Length; i++)
                {
                    long n = 0;
                    for (; j < 16; j++)
                    {
                        long t = g % 10;
                        n |= t << (4 * j);
                        g /= 10;
                    }
                    a._number[i] = n;
                }
            }
            else
            {
                a._attribute = 0;
                for (var i = 0; i < a._number.Length; i++)
                    a._number[i] = 0;
            }
        }
        public static void ToInfinitely(long d, Infinitely a)
        {
            a._number = new long[1 + a.Adn / 2];
            if (d != 0)
            {
                long g;
                if (d > 0)
                {
                    a._attribute = 1;
                    g = d;
                }
                else
                {
                    a._attribute = -1;
                    g = -d;
                }
                int i = 0, j = 0;
                long w = 0;
                while (w < a.Fdn)
                {
                    if (++j > 15)
                    {
                        j = 0;
                        ++i;
                    }
                    ++w;
                }

                for (; i < a._number.Length; i++)
                {
                    long n = 0;
                    for (; j < 16; j++)
                    {
                        var t = g % 10;
                        n |= t << (4 * j);
                        g /= 10;
                    }
                    a._number[i] = n;
                }
            }
            else
            {
                a._attribute = 0;
                for (var i = 0; i < a._number.Length; i++)
                    a._number[i] = 0;
            }
        }
        public static void ToInfinitely(decimal d, Infinitely a)
        {
            ToInfinitely(d.ToString(), a);
        }
        public static void ToInfinitely(string d, Infinitely a)
        {
            if (d != "0")
            {
                int k = 0, h = d.Length - 1;
                if (d[k] == '-')
                {
                    a._attribute = -1;
                    ++k;
                }
                else
                {
                    a._attribute = 1;
                }
                long n = 0, i = 0, f = a.Fdn - 1;
                var j = 0;

                if (d.IndexOf(',') > -1)
                    f -= d.Length - d.IndexOf(',') - 1;

                for (; f >= 0; f--)
                    if (++j > 15)
                    {
                        j = 0;
                        i++;
                    }

                if (d.IndexOf(',') > -1)
                {
                    for (; h > d.IndexOf(','); h--)
                    {
                        var t = Convert.ToInt64(d[h]) - 48;
                        //Console.Write(t); Console.ReadLine(); ////////////////////delete this
                        n |= t << (4 * j);
                        if (++j <= 15) continue;
                        a._number[i] = n;
                        n = 0;
                        j = 0;
                        i++;
                    }
                    --h;
                }

                for (; h >= k; h--)
                {
                    var t = Convert.ToInt64(d[h]) - 48;
                    //Console.Write(t); Console.ReadLine(); ////////////////////delete this
                    n |= t << (4 * j);
                    if (++j <= 15) continue;
                    a._number[i] = n;
                    n = 0;
                    j = 0;
                    i++;
                }
                a._number[i] = n;
            }
            else
            {
                a._attribute = 0;
                a._zero = true;
                for (var i = 0; i < a._number.Length; i++)
                    a._number[i] = 0;
            }
        }
        public static Infinitely Copy(Infinitely a)
        {
            var b = new Infinitely(a.Adn, a.Fdn) {_attribute = a._attribute};
            for (long i = 0; i < a._number.Length; ++i)
                b._number[i] = a._number[i];
            return b;
        }

        public static bool IsZero(Infinitely a)
        {
            if (a._attribute == IZero) return true;
            for (long i = 0; i < a._number.Length; ++i)
                if (a._number[i] != 0) return false;
            return true;
        }

        // поиск первой не нулевой цифры слева
        public static void FindBegin(Infinitely a, ref int i, ref int j)
        {
            if (a._zero)
            {
                i = 0;
                j = 0;
            }
            else
            {
                if (a._attribute == 0) return;
                var f = true;
                i = a._number.Length - 1;
                while (f && i >= 0)
                    if (a._number[i] != 0) f = false;
                    else --i;
                f = true;
                j = 15;
                while (f && j >= 0)
                    if (((a._number[i] >> (4 * j)) & 0x000000000000000F) > 0) f = false;
                    else --j;
            }
        }

        public static Infinitely Abs(Infinitely a)
        {
            if (a._attribute == 0) return a;
            a._attribute = 1;
            return a;
        }
        // |a| > |b|
        public static bool AbsGreater(Infinitely a, Infinitely b)
        {
            // long n = a.adn - a.fdn;
            int i = 0, j = 0, p = 0, q = 0;
            FindBegin(a, ref i, ref j);
            FindBegin(b, ref p, ref q);

            if (i > p) return true;
            if (i < p) return false;
            if (j > q) return true;
            if (j < q) return false;

            while (i >= 0 && p >= 0)
            {
                if (((a._number[i] >> (4 * j)) & 0x000000000000000F) >
                    ((b._number[p] >> (4 * q)) & 0x000000000000000F)) return true;
                if (((a._number[i] >> (4 * j)) & 0x000000000000000F) <
                    ((b._number[p] >> (4 * q)) & 0x000000000000000F)) return false;
                if (--j < 0)
                {
                    j = 15;
                    i--;
                }
                if (--q >= 0) continue;
                q = 15;
                p--;
            }
            /*while (i >= 0)
            {
                if ((a.number[i] >> 4 * j & 0x000000000000000F) > 0) return true;
                if (--j < 0)
                {
                    j = 15;
                    i--;
                }
            }
            while (p >= 0)
            {
                if ((b.number[p] >> 4 * q & 0x000000000000000F) > 0) return false;
                if (--q < 0)
                {
                    q = 15;
                    p--;
                }
            }*/
            return false;
        }

        public static bool operator ==(Infinitely a, Infinitely b)
        {
            return !(a != b);
        }
        public static bool operator ==(Infinitely a, decimal y)
        {
            if (!(a != null)) return false;
            var b = new Infinitely(a.Adn, a.Fdn);
            ToInfinitely(y, b);
            return !(a != b);
        }
        public static bool operator ==(decimal x, Infinitely b)
        {
            if (!(b != null)) return false;
            var a = new Infinitely(b.Adn, b.Fdn);
            ToInfinitely(x, a);
            return !(a != b);
        }

        public static bool operator !=(Infinitely a, Infinitely b)
        {
            if (b != null && a != null && a._attribute != b._attribute) return true;
            int i = 0, j = 0, p = 0, q = 0;
            FindBegin(a, ref i, ref j);
            FindBegin(b, ref p, ref q);

            if (i != p || j != q) return true;

            while (i >= 0)
            {
                if (b != null && a != null && ((a._number[i] >> (4 * j)) & 0x000000000000000F) !=
                    ((b._number[i] >> (4 * j)) & 0x000000000000000F)) return true;
                if (--j >= 0) continue;
                j = 15;
                i--;
            }
            return false;
        }
        public static bool operator !=(Infinitely a, decimal y)
        {
            if (!(a != null)) return false;
            var b = new Infinitely(a.Adn, a.Fdn);
            ToInfinitely(y, b);

            if (a._attribute != b._attribute) return true;
            int i = 0, j = 0, p = 0, q = 0;
            FindBegin(a, ref i, ref j);
            FindBegin(b, ref p, ref q);

            if (i != p || j != q) return true;

            while (i >= 0)
            {
                if (((a._number[i] >> (4 * j)) & 0x000000000000000F) !=
                    ((b._number[i] >> (4 * j)) & 0x000000000000000F)) return true;
                if (--j >= 0) continue;
                j = 15;
                i--;
            }
            return false;
        }
        public static bool operator !=(decimal x, Infinitely b)
        {
            if (!(b != null)) return false;
            var a = new Infinitely(b.Adn, b.Fdn);
            ToInfinitely(x, a);

            if (a._attribute != b._attribute) return true;
            int i = 0, j = 0, p = 0, q = 0;
            FindBegin(a, ref i, ref j);
            FindBegin(b, ref p, ref q);

            if (i != p || j != q) return true;

            while (i >= 0)
            {
                if (((a._number[i] >> (4 * j)) & 0x000000000000000F) !=
                    ((b._number[i] >> (4 * j)) & 0x000000000000000F)) return true;
                if (--j >= 0) continue;
                j = 15;
                i--;
            }
            return false;
        }

        public static bool operator <(Infinitely a, Infinitely b)
        {
            if (a._attribute > b._attribute) return false;
            if (a._attribute < b._attribute) return true;

            if (a == b)
                return false;
            if (a._attribute == 1)
                return !AbsGreater(a, b);
            if (a._attribute == -1)
            {
                return AbsGreater(a, b);
            }
            return false;
        }
        public static bool operator <(Infinitely a, decimal y)
        {
            var b = new Infinitely(a.Adn, a.Fdn);
            ToInfinitely(y, b);

            if (a._attribute > b._attribute) return false;
            if (a._attribute < b._attribute) return true;
            if (a == b) return false;
            if (a._attribute == 1)
                return !AbsGreater(a, b);
            if (a._attribute == -1)
                return AbsGreater(a, b);
            return false;
        }
        public static bool operator <(decimal x, Infinitely b)
        {
            var a = new Infinitely(b.Adn, b.Fdn);
            ToInfinitely(x, a);

            if (a._attribute > b._attribute) return false;
            if (a._attribute < b._attribute) return true;
            if (a == b) return false;
            if (a._attribute == 1)
                return !AbsGreater(a, b);
            if (a._attribute == -1)
                return AbsGreater(a, b);
            return false;
        }

        public static bool operator >(Infinitely a, Infinitely b)
        {
            if (a._attribute > b._attribute) return true;
            if (a._attribute < b._attribute) return false;
            if (a._attribute == 0) return false;
            if (a == b) return false;
            if (a._attribute == 1)
                return AbsGreater(a, b);
            if (a._attribute == -1)
                return !AbsGreater(a, b);
            return false;
        }
        public static bool operator >(Infinitely a, decimal y)
        {
            var b = new Infinitely(a.Adn, a.Fdn);
            ToInfinitely(y, b);

            if (a._attribute > b._attribute) return true;
            if (a._attribute < b._attribute) return false;
            if (a._attribute == 0) return false;
            if (a == b) return false;
            if (a._attribute == 1)
                return AbsGreater(a, b);
            if (a._attribute == -1)
                return !AbsGreater(a, b);
            return false;
        }
        public static bool operator >(decimal x, Infinitely b)
        {
            var a = new Infinitely(b.Adn, b.Fdn);
            ToInfinitely(x, a);

            if (a._attribute > b._attribute) return true;
            if (a._attribute < b._attribute) return false;
            if (a._attribute == 0) return false;
            if (a == b) return false;
            if (a._attribute == 1)
                return AbsGreater(a, b);
            if (a._attribute == -1)
                return !AbsGreater(a, b);
            return false;
        }

        public static bool operator >=(Infinitely a, Infinitely b)
        {
            if (a._attribute > b._attribute) return true;
            if (a._attribute < b._attribute) return false;
            //if (a.attribute == 0) return true; 
            if (a == b) return true;
            if (a._attribute == 1)
            {
                return AbsGreater(a, b);
            }
            if (a._attribute == -1)
            {
                return !(AbsGreater(a, b));
            }
            return false;
        }
        public static bool operator <=(Infinitely a, Infinitely b)
        {
            if (a._attribute > b._attribute) return false;
            if (a._attribute < b._attribute) return true;

            if (a == b) return true;
            else
            {
                if (a._attribute == 1)
                {
                    return !AbsGreater(a, b);
                }
                if (a._attribute == -1)
                {
                    return AbsGreater(a, b);
                }
            }
            return false;
        }

        //|a| + |b|
        public static Infinitely AbsPlus(Infinitely a, Infinitely b)
        {
            var c = new Infinitely(a.Adn, a.Fdn) {_attribute = a._attribute};
            long mod = 0;
            for (long i = 0; i < a._number.Length; i++)
            {
                long n = 0;
                for (var j = 0; j < 16; j++)
                {
                    var t = ((a._number[i] >> (4 * j)) & 0x000000000000000F) + mod;
                    if (i < b._number.Length) t += (b._number[i] >> (4 * j)) & 0x000000000000000F;
                    if (t > 9)
                    {
                        mod = 1;
                        t %= 10;
                    }
                    else
                    {
                        mod = 0;
                    }
                    n |= t << (4 * j);
                }
                c._number[i] = n;
            }
            return c;
        }
        //|a| - |b|
        public static Infinitely AbsMinus(Infinitely a, Infinitely b)
        {
            var c = new Infinitely(a.Adn, a.Fdn);
            long mod = 0;
            if (AbsGreater(a, b))
                for (long i = 0; i < a._number.Length; i++)
                {
                    long n = 0;
                    for (var j = 0; j < 16; j++)
                    {
                        var t = ((a._number[i] >> (4 * j)) & 0x000000000000000F) - mod;
                        if (i < b._number.Length) t -= (b._number[i] >> (4 * j)) & 0x000000000000000F;
                        if (t < 0)
                        {
                            mod = 1;
                            t += 10;
                        }
                        else
                        {
                            mod = 0;
                        }
                        n |= t << (4 * j);
                    }
                    c._number[i] = n;
                }
            else
                for (var i = 0; i < b._number.Length; i++)
                {
                    long n = 0;
                    for (var j = 0; j < 16; j++)
                    {
                        var t = ((b._number[i] >> (4 * j)) & 0x000000000000000F) - mod;
                        if (i < a._number.Length) t -= (a._number[i] >> (4 * j)) & 0x000000000000000F;
                        if (t < 0)
                        {
                            mod = 1;
                            t += 10;
                        }
                        else
                        {
                            mod = 0;
                        }
                        n |= t << (4 * j);
                    }
                    c._number[i] = n;
                }
            return c;
        }

        public static Infinitely operator +(Infinitely a, Infinitely b)
        {
            if (a._attribute == 0) return b;
            if (b._attribute == 0) return a;
            if (a._attribute == b._attribute)
            {
                var c = AbsPlus(a, b);
                c._attribute = a._attribute;
                return c;
            }
            if (AbsGreater(a, b))
            {
                var c = AbsMinus(a, b);
                c._attribute = a._attribute;
                return c;
            }
            else
            {
                var c = AbsMinus(b, a);
                c._attribute = b._attribute;
                return c;
            }
        }
        public static Infinitely operator +(Infinitely a, decimal y)
        {
            var b = new Infinitely(a.Adn, a.Fdn);
            ToInfinitely(y, b);

            if (a._attribute == 0) return b;
            if (b._attribute == 0) return a;
            if (a._attribute == b._attribute)
            {
                var c = AbsPlus(a, b);
                c._attribute = a._attribute;
                return c;
            }
            if (AbsGreater(a, b))
            {
                var c = AbsMinus(a, b);
                c._attribute = a._attribute;
                return c;
            }
            else
            {
                var c = AbsMinus(b, a);
                c._attribute = b._attribute;
                return c;
            }
        }
        public static Infinitely operator +(decimal x, Infinitely b)
        {
            var a = new Infinitely(b.Adn, b.Fdn);
            ToInfinitely(x, a);

            if (a._attribute == 0) return b;
            if (b._attribute == 0) return a;
            if (a._attribute == b._attribute)
            {
                var c = AbsPlus(a, b);
                c._attribute = a._attribute;
                return c;
            }
            if (AbsGreater(a, b))
            {
                var c = AbsMinus(a, b);
                c._attribute = a._attribute;
                return c;
            }
            else
            {
                var c = AbsMinus(b, a);
                c._attribute = b._attribute;
                return c;
            }
        }

        public static Infinitely operator -(Infinitely a, Infinitely b)
        {
            if (b._attribute == 0)
            {
                var v = new Infinitely(a.Adn, a.Fdn)
                {
                    _number = a._number,
                    _attribute = a._attribute
                };
                return v;
            }
            if (a._attribute == 0)
            {
                var v = new Infinitely(b.Adn, b.Fdn)
                {
                    _number = b._number,
                    _attribute = -b._attribute
                };
                return v;
            }
            if (a._attribute == b._attribute)
            {
                Console.WriteLine("Abs = {0}", AbsGreater(a, b));
                if (AbsGreater(a, b))
                {
                    var c = AbsMinus(a, b);
                    c._attribute = a._attribute;
                    return c;
                }
                else
                {
                    var c = AbsMinus(b, a);
                    c._attribute = -b._attribute;
                    return c;
                }
            }
            {
                var c = AbsPlus(a, b);
                c._attribute = a._attribute;
                return c;
            }
        }
        public static Infinitely operator -(Infinitely a, decimal y)
        {
            var b = new Infinitely(a.Adn, a.Fdn);
            ToInfinitely(y, b);

            if (b._attribute == 0)
            {
                var v = new Infinitely(a.Adn, a.Fdn)
                {
                    _number = a._number,
                    _attribute = a._attribute
                };
                return v;
            }
            if (a._attribute == 0)
            {
                var v = new Infinitely(b.Adn, b.Fdn)
                {
                    _number = b._number,
                    _attribute = -b._attribute
                };
                return v;
            }
            if (a._attribute == b._attribute)
            {
                Console.WriteLine("Abs = {0}", AbsGreater(a, b));
                if (AbsGreater(a, b))
                {
                    var c = AbsMinus(a, b);
                    c._attribute = a._attribute;
                    return c;
                }
                else
                {
                    var c = AbsMinus(b, a);
                    c._attribute = -b._attribute;
                    return c;
                }
            }
            {
                var c = AbsPlus(a, b);
                c._attribute = a._attribute;
                return c;
            }
        }
        public static Infinitely operator -(decimal x, Infinitely b)
        {
            var a = new Infinitely(b.Adn, b.Fdn);
            ToInfinitely(x, a);

            if (b._attribute == 0)
            {
                var v = new Infinitely(a.Adn, a.Fdn)
                {
                    _number = a._number,
                    _attribute = a._attribute
                };
                return v;
            }
            if (a._attribute == 0)
            {
                var v = new Infinitely(b.Adn, b.Fdn)
                {
                    _number = b._number,
                    _attribute = -b._attribute
                };
                return v;
            }
            if (a._attribute == b._attribute)
            {
                Console.WriteLine("Abs = {0}", AbsGreater(a, b));
                if (AbsGreater(a, b))
                {
                    var c = AbsMinus(a, b);
                    c._attribute = a._attribute;
                    return c;
                }
                else
                {
                    var c = AbsMinus(b, a);
                    c._attribute = -b._attribute;
                    return c;
                }
            }
            {
                var c = AbsPlus(a, b);
                c._attribute = a._attribute;
                return c;
            }
        }

        //метод для ассинхронного умножения
        private static void Mull(object state)
        {
            var g = (Point)state;
            //...


            var t = (g.A._number[g.I] >> (4 * g.J)) & 0x000000000000000F;
            int m = g.J - g.Jj, p = g.I - g.Ii;
            if (m < -16)
            {
                p--;
                m += 15;
            }
            long n = 0, mod = 0;
            if (p >= 0)
                for (var l = 0; l < m; l++)
                    n |= ((g.C._number[p] >> (4 * l)) & 0x000000000000000F) << (4 * l);
            for (var k = 0; k < g.B._number.Length; k++)
            for (var h = 0; h < 16 && p < g.C._number.Length; h++)
                if (p >= 0)
                {
                    var f = ((g.B._number[k] >> (4 * h)) & 0x000000000000000F) * t + mod;
                    if (p < g.C._number.Length) f += (g.C._number[p] >> (4 * m)) & 0x000000000000000F;
                    if (f > 9)
                    {
                        mod = f / 10;
                        f %= 10;
                    }
                    else
                    {
                        mod = 0;
                    }

                    n |= f << (4 * m);
                    if (++m <= 15) continue;
                    g.C._number[p] = n;
                    n = 0;
                    m = 0;
                    p++;
                }
                else
                {
                    if (++m <= 15) continue;
                    m = 0;
                    p++;
                }
        }

        // асинхронное умножение через ThreadPool.QueueUserWorkItem
        /*public static Infinitely AsyncMull(Infinitely a, Infinitely b)
        {
            var c = new Infinitely(a.ADN, a.FDN)
            {
                _attribute = a._attribute * b._attribute
            };

            if (c._attribute != iZero)
            {
                int ii = 0, jj = 0;
                for (var fd = c.FDN; fd > 0; fd--)
                    if (++jj == 16)
                    {
                        jj = 0;
                        ii++;
                    }
                for (var i = 0; i < a._number.Length; i++)
                for (var j = 0; j < 16; j++)
                {
                    var p = new Point(i, j, ii, jj, a, b, c);
                    ThreadPool.QueueUserWorkItem(Mull, p);
                }
            }
            else
            {
                for (var i = 0; i <= c._number.Length; i++)
                    c._number[i] = 0;
            }
            //Thread.Sleep(3);
            return c;
        }*/

        // асинхронное умножение через Task
        public static Infinitely AsyncMul1(Infinitely a, Infinitely b)
        {
            var c = new Infinitely(a.Adn, a.Fdn)
            {
                _attribute = a._attribute * b._attribute
            };

            if (c._attribute != IZero)
            {
                int ii = 0, jj = 0;
                for (var fd = c.Fdn; fd > 0; fd--)
                    if (++jj == 16)
                    {
                        jj = 0;
                        ii++;
                    }
                for (var i = 0; i < a._number.Length; i++)
                for (var j = 0; j < 16; j++)
                {
                    var p = new Point(i, j, ii, jj, a, b, c);
                    new Task(Mull, p).Start();
                }
            }
            else
            {
                for (var i = 0; i <= c._number.Length; i++)
                    c._number[i] = 0;
            }
            Task.WaitAll();

            return c;
        }

        public static Infinitely operator *(Infinitely a, Infinitely b)
        {
            var c = new Infinitely(a.Adn, a.Fdn)
            {
                _attribute = a._attribute * b._attribute
            };

            if (c._attribute == IZero) return c;
            int ii = 0, jj = 0;
            for (var fd = c.Fdn; fd > 0; fd--)
                if (++jj == 16)
                {
                    jj = 0;
                    ii++;
                }
            for (var i = 0; i < a._number.Length; i++)
            for (var j = 0; j < 16; j++)
            {
                var t = (a._number[i] >> (4 * j)) & 0x000000000000000F;
                int m = j - jj, p = i - ii;
                if (m < -16)
                {
                    p--;
                    m += 15;
                }
                long n = 0, mod = 0;
                if (p >= 0)
                    for (var l = 0; l < m; l++)
                        n |= ((c._number[p] >> (4 * l)) & 0x000000000000000F) << (4 * l);
                for (var k = 0; k < b._number.Length; k++)
                for (var h = 0; h < 16 && p < c._number.Length; h++)
                    if (p >= 0)
                    {
                        var f = ((b._number[k] >> (4 * h)) & 0x000000000000000F) * t + mod;
                        if (p < c._number.Length) f += (c._number[p] >> (4 * m)) & 0x000000000000000F;
                        if (f > 9)
                        {
                            mod = f / 10;
                            f %= 10;
                        }
                        else
                        {
                            mod = 0;
                        }

                        n |= f << (4 * m);
                        if (++m <= 15) continue;
                        c._number[p] = n;
                        n = 0;
                        m = 0;
                        p++;
                    }
                    else
                    {
                        if (++m <= 15) continue;
                        m = 0;
                        p++;
                    }
            }
            return c;
        }
        public static Infinitely operator *(Infinitely a, decimal y)
        {
            var b = new Infinitely(a.Adn, a.Fdn);
            ToInfinitely(y, b);
            var c = new Infinitely(a.Adn, a.Fdn)
            {
                _attribute = a._attribute * b._attribute
            };

            if (c._attribute == IZero) return c;
            int ii = 0, jj = 0;
            for (var fd = c.Fdn; fd > 0; fd--)
                if (++jj == 16)
                {
                    jj = 0;
                    ii++;
                }
            for (var i = 0; i < a._number.Length; i++)
            for (var j = 0; j < 16; j++)
            {
                var t = (a._number[i] >> (4 * j)) & 0x000000000000000F;
                int m = j - jj, p = i - ii;
                if (m < -16)
                {
                    p--;
                    m += 15;
                }
                long n = 0, mod = 0;
                if (p >= 0)
                    for (var l = 0; l < m; l++)
                        n |= ((c._number[p] >> (4 * l)) & 0x000000000000000F) << (4 * l);
                for (var k = 0; k < b._number.Length; k++)
                for (var h = 0; h < 16 && p < c._number.Length; h++)
                    if (p >= 0)
                    {
                        var f = ((b._number[k] >> (4 * h)) & 0x000000000000000F) * t + mod;
                        if (p < c._number.Length) f += (c._number[p] >> (4 * m)) & 0x000000000000000F;
                        if (f > 9)
                        {
                            mod = f / 10;
                            f %= 10;
                        }
                        else
                        {
                            mod = 0;
                        }

                        n |= f << (4 * m);
                        if (++m <= 15) continue;
                        c._number[p] = n;
                        n = 0;
                        m = 0;
                        p++;
                    }
                    else
                    {
                        if (++m <= 15) continue;
                        m = 0;
                        p++;
                    }
            }
            return c;
        }
        public static Infinitely operator *(decimal x, Infinitely b)
        {
            var a = new Infinitely(b.Adn, b.Fdn);
            ToInfinitely(x, a);
            var c = new Infinitely(a.Adn, a.Fdn)
            {
                _attribute = a._attribute * b._attribute
            };

            if (c._attribute == IZero) return c;
            int ii = 0, jj = 0;
            for (var fd = c.Fdn; fd > 0; fd--)
                if (++jj == 16)
                {
                    jj = 0;
                    ii++;
                }
            for (var i = 0; i < a._number.Length; i++)
            for (var j = 0; j < 16; j++)
            {
                var t = (a._number[i] >> (4 * j)) & 0x000000000000000F;
                int m = j - jj, p = i - ii;
                if (m < -16)
                {
                    p--;
                    m += 15;
                }
                long n = 0, mod = 0;
                if (p >= 0)
                    for (var l = 0; l < m; l++)
                        n |= ((c._number[p] >> (4 * l)) & 0x000000000000000F) << (4 * l);
                for (var k = 0; k < b._number.Length; k++)
                for (var h = 0; h < 16 && p < c._number.Length; h++)
                    if (p >= 0)
                    {
                        var f = ((b._number[k] >> (4 * h)) & 0x000000000000000F) * t + mod;
                        if (p < c._number.Length) f += (c._number[p] >> (4 * m)) & 0x000000000000000F;
                        if (f > 9)
                        {
                            mod = f / 10;
                            f %= 10;
                        }
                        else
                        {
                            mod = 0;
                        }

                        n |= f << (4 * m);
                        if (++m <= 15) continue;
                        c._number[p] = n;
                        n = 0;
                        m = 0;
                        p++;
                    }
                    else
                    {
                        if (++m <= 15) continue;
                        m = 0;
                        p++;
                    }
            }
            return c;
        }

        public static Infinitely operator /(Infinitely x, Infinitely b)
        {
            var a = Copy(x);
            var c = new Infinitely(a.Adn, a.Fdn);
            checked
            {
                try
                {
                    c._attribute = a._attribute / b._attribute;
                    /*  i - номер элемента в массиве делимого
                     *  j - номер цифры в i-м элементе массива делимого
                     *  p - номер элемента в массиве делителя
                     *  q - номер цифры в p-м элементе массива делителя
                     *  ci - номер элемента в массиве результата 
                     *  cj - номер цифры в ci-м элементе массива результата
                     *  inci
                     *  incj
                     *  inc               
                    */
                    int iDvdDigPos = 0,
                        jDvdDigPos = 0,
                        iDzrDigPos = 0,
                        jDzrDigPos = 0,
                        inci = 0,
                        incj = 0;
                    var inc = false;
                    //определяем позицию первой цифры
                    FindBegin(a, ref iDvdDigPos, ref jDvdDigPos);
                    FindBegin(b, ref iDzrDigPos, ref jDzrDigPos);

                    //определяем позицию первой цифры результата деления
                    var ci = iDvdDigPos - iDzrDigPos;
                    var cj = jDvdDigPos - jDzrDigPos;
                    if (cj < 0)
                    {
                        ci--;
                        cj += 16;
                    }
                    long w = 0;
                    while (w < a.Fdn)
                    {
                        if (++cj > 15)
                        {
                            ++ci;
                            cj = 0;
                        }
                        ++w;
                    }
                    for (; iDvdDigPos >= 0; iDvdDigPos--, jDvdDigPos = 15)
                    for (; jDvdDigPos >= 0; jDvdDigPos--)
                    {
                        long res = 0;
                        var run = true;
                        while (run && ci >= 0)
                        {
                            int ii = iDvdDigPos, jj = jDvdDigPos, pp = iDzrDigPos, qq = jDzrDigPos;
                            bool g = true, l = true;
                            if (!inc)
                                while (g && l && pp >= 0 && qq >= 0 && ii >= 0 && jj >= 0)
                                {
                                    if (((a._number[ii] >> (4 * jj)) & 0x000000000000000F) >
                                        ((b._number[pp] >> (4 * qq)) & 0x000000000000000F)) l = false;
                                    if (((a._number[ii] >> (4 * jj)) & 0x000000000000000F) <
                                        ((b._number[pp] >> (4 * qq)) & 0x000000000000000F)) g = false;
                                    if (--jj < 0)
                                    {
                                        jj = 15;
                                        --ii;
                                    }
                                    if (--qq >= 0) continue;
                                    qq = 15;
                                    --pp;
                                }
                            if (g)
                            {
                                var itDvdDigPos = iDvdDigPos - iDzrDigPos;
                                var jtDvdDigPos = jDvdDigPos - jDzrDigPos;
                                if (jtDvdDigPos < 0)
                                {
                                    itDvdDigPos--;
                                    jtDvdDigPos += 16;
                                }

                                int mod = 0, itDzrDigPos = 0, jtDzrDigPos = 0;
                                // определяем разряд, если вышли за границы числа справа
                                while (itDvdDigPos < 0)
                                {
                                    if (++jtDvdDigPos > 15)
                                    {
                                        ++itDvdDigPos;
                                        jtDvdDigPos = 0;
                                    }
                                    if (++jtDzrDigPos <= 15) continue;
                                    ++itDzrDigPos;
                                    jtDzrDigPos = 0;
                                }

                                while ((itDzrDigPos <= iDzrDigPos || mod > 0) && itDvdDigPos < a._number.Length)
                                {
                                    var t = ((a._number[itDvdDigPos] >> (4 * jtDvdDigPos)) & 0x000000000000000F) - mod;
                                    if (itDzrDigPos < b._number.Length)
                                        t -= (b._number[itDzrDigPos] >> (4 * jtDzrDigPos)) & 0x000000000000000F;
                                    if (t < 0)
                                    {
                                        mod = 1;
                                        t += 10;
                                    }
                                    else
                                    {
                                        mod = 0;
                                    }

                                    long f = 0; //отнимаем от делимого числа
                                    for (var u = 0; u <= 15; u++)
                                        if (u == jtDvdDigPos) f |= t << (4 * u);
                                        else f |= ((a._number[itDvdDigPos] >> (4 * u)) & 0x000000000000000F) << (4 * u);
                                    a._number[itDvdDigPos] = f;
                                    if (++jtDvdDigPos > 15)
                                    {
                                        itDvdDigPos++;
                                        jtDvdDigPos = 0;
                                    }
                                    if (++jtDzrDigPos <= 15) continue;
                                    itDzrDigPos++;
                                    jtDzrDigPos = 0;
                                }
                                ++res;
                                inc = ((a._number[inci] >> (4 * incj)) & 0x000000000000000F) > 0;
                            }
                            else
                            {
                                if (((a._number[iDvdDigPos] >> (4 * jDvdDigPos)) & 0x000000000000000F) > 0)
                                {
                                    inc = true;
                                    inci = iDvdDigPos;
                                    incj = jDvdDigPos;
                                }
                                run = false;
                                if (res <= 9)
                                {
                                    c._number[ci] |= res << (4 * cj);
                                }
                                else
                                {
                                    long ri = ci;
                                    var rj = cj;
                                    c._number[ri] |= (res % 10) << (4 * rj);
                                    res /= 10;
                                    while (res > 0)
                                    {
                                        if (++rj > 15)
                                        {
                                            ++ri;
                                            rj = 0;
                                        }
                                        res += (c._number[ri] >> (4 * rj)) & 0x000000000000000F; // + (res % 10);
                                        c._number[ri] |= (res % 10) << (4 * rj); //res = res2;
                                        res /= 10;
                                    }
                                }
                                if (--cj >= 0) continue;
                                --ci;
                                cj = 15;
                            }
                        } // while (run && (ci >= 0))
                    } // for j                  
                    if (IsZero(c)) c._attribute = IZero;
                    return c;
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("DIVIDE BY ZERO");
                    return c;
                }
            }
        }
        public static Infinitely operator /(Infinitely x, decimal y)
        {
            var a = Copy(x);
            var c = new Infinitely(a.Adn, a.Fdn);
            var b = new Infinitely(a.Adn, a.Fdn);
            ToInfinitely(y, b);
            checked
            {
                try
                {
                    c._attribute = a._attribute / b._attribute;
                    /*  i - номер элемента в массиве делимого
                     *  j - номер цифры в i-м элементе массива делимого
                     *  p - номер элемента в массиве делителя
                     *  q - номер цифры в p-м элементе массива делителя
                     *  ci - номер элемента в массиве результата 
                     *  cj - номер цифры в ci-м элементе массива результата
                     *  inci
                     *  incj
                     *  inc               
                    */
                    int iDvdDigPos = 0,
                        jDvdDigPos = 0,
                        iDzrDigPos = 0,
                        jDzrDigPos = 0,
                        inci = 0,
                        incj = 0;
                    var inc = false;
                    //определяем позицию первой цифры
                    FindBegin(a, ref iDvdDigPos, ref jDvdDigPos);
                    FindBegin(b, ref iDzrDigPos, ref jDzrDigPos);

                    //определяем позицию первой цифры результата деления
                    var ci = iDvdDigPos - iDzrDigPos;
                    var cj = jDvdDigPos - jDzrDigPos;
                    if (cj < 0)
                    {
                        ci--;
                        cj += 16;
                    }
                    long w = 0;
                    while (w < a.Fdn)
                    {
                        if (++cj > 15)
                        {
                            ++ci;
                            cj = 0;
                        }
                        ++w;
                    }
                    for (; iDvdDigPos >= 0; iDvdDigPos--, jDvdDigPos = 15)
                    for (; jDvdDigPos >= 0; jDvdDigPos--)
                    {
                        long res = 0;
                        var run = true;
                        while (run && ci >= 0)
                        {
                            int ii = iDvdDigPos, jj = jDvdDigPos, pp = iDzrDigPos, qq = jDzrDigPos;
                            bool g = true, l = true;
                            if (!inc)
                                while (g && l && pp >= 0 && qq >= 0 && ii >= 0 && jj >= 0)
                                {
                                    if (((a._number[ii] >> (4 * jj)) & 0x000000000000000F) >
                                        ((b._number[pp] >> (4 * qq)) & 0x000000000000000F)) l = false;
                                    if (((a._number[ii] >> (4 * jj)) & 0x000000000000000F) <
                                        ((b._number[pp] >> (4 * qq)) & 0x000000000000000F)) g = false;
                                    if (--jj < 0)
                                    {
                                        jj = 15;
                                        --ii;
                                    }
                                    if (--qq >= 0) continue;
                                    qq = 15;
                                    --pp;
                                }
                            if (g)
                            {
                                var itDvdDigPos = iDvdDigPos - iDzrDigPos;
                                var jtDvdDigPos = jDvdDigPos - jDzrDigPos;
                                if (jtDvdDigPos < 0)
                                {
                                    itDvdDigPos--;
                                    jtDvdDigPos += 16;
                                }

                                int mod = 0, itDzrDigPos = 0, jtDzrDigPos = 0;
                                // определяем разряд, если вышли за границы числа справа
                                while (itDvdDigPos < 0)
                                {
                                    if (++jtDvdDigPos > 15)
                                    {
                                        ++itDvdDigPos;
                                        jtDvdDigPos = 0;
                                    }
                                    if (++jtDzrDigPos <= 15) continue;
                                    ++itDzrDigPos;
                                    jtDzrDigPos = 0;
                                }

                                while ((itDzrDigPos <= iDzrDigPos || mod > 0) && itDvdDigPos < a._number.Length)
                                {
                                    var t = ((a._number[itDvdDigPos] >> (4 * jtDvdDigPos)) & 0x000000000000000F) - mod;
                                    if (itDzrDigPos < b._number.Length)
                                        t -= (b._number[itDzrDigPos] >> (4 * jtDzrDigPos)) & 0x000000000000000F;
                                    if (t < 0)
                                    {
                                        mod = 1;
                                        t += 10;
                                    }
                                    else
                                    {
                                        mod = 0;
                                    }

                                    long f = 0; //отнимаем от делимого числа
                                    for (var u = 0; u <= 15; u++)
                                        if (u == jtDvdDigPos) f |= t << (4 * u);
                                        else f |= ((a._number[itDvdDigPos] >> (4 * u)) & 0x000000000000000F) << (4 * u);
                                    a._number[itDvdDigPos] = f;
                                    if (++jtDvdDigPos > 15)
                                    {
                                        itDvdDigPos++;
                                        jtDvdDigPos = 0;
                                    }
                                    if (++jtDzrDigPos <= 15) continue;
                                    itDzrDigPos++;
                                    jtDzrDigPos = 0;
                                }
                                ++res;
                                inc = ((a._number[inci] >> (4 * incj)) & 0x000000000000000F) > 0;
                            }
                            else
                            {
                                if (((a._number[iDvdDigPos] >> (4 * jDvdDigPos)) & 0x000000000000000F) > 0)
                                {
                                    inc = true;
                                    inci = iDvdDigPos;
                                    incj = jDvdDigPos;
                                }
                                run = false;
                                if (res <= 9)
                                {
                                    c._number[ci] |= res << (4 * cj);
                                }
                                else
                                {
                                    long ri = ci;
                                    var rj = cj;
                                    c._number[ri] |= (res % 10) << (4 * rj);
                                    res /= 10;
                                    while (res > 0)
                                    {
                                        if (++rj > 15)
                                        {
                                            ++ri;
                                            rj = 0;
                                        }
                                        res += (c._number[ri] >> (4 * rj)) & 0x000000000000000F; // + (res % 10);
                                        c._number[ri] |= (res % 10) << (4 * rj); //res = res2;
                                        res /= 10;
                                    }
                                }
                                if (--cj >= 0) continue;
                                --ci;
                                cj = 15;
                            }
                        } // while (run && (ci >= 0))
                    } // for j                  
                    if (IsZero(c)) c._attribute = IZero;
                    return c;
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("DIVIDE BY ZERO");
                    return c;
                }
            }
        }
        public static Infinitely operator /(decimal x, Infinitely b)
        {
            var a = new Infinitely(b.Adn, b.Fdn);
            ToInfinitely(x, a);
            var c = new Infinitely(a.Adn, a.Fdn);
            checked
            {
                try
                {
                    c._attribute = a._attribute / b._attribute;
                    /*  i - номер элемента в массиве делимого
                     *  j - номер цифры в i-м элементе массива делимого
                     *  p - номер элемента в массиве делителя
                     *  q - номер цифры в p-м элементе массива делителя
                     *  ci - номер элемента в массиве результата 
                     *  cj - номер цифры в ci-м элементе массива результата
                     *  inci
                     *  incj
                     *  inc               
                    */
                    int iDvdDigPos = 0,
                        jDvdDigPos = 0,
                        iDzrDigPos = 0,
                        jDzrDigPos = 0,
                        inci = 0,
                        incj = 0;
                    var inc = false;
                    //определяем позицию первой цифры
                    FindBegin(a, ref iDvdDigPos, ref jDvdDigPos);
                    FindBegin(b, ref iDzrDigPos, ref jDzrDigPos);

                    //определяем позицию первой цифры результата деления
                    var ci = iDvdDigPos - iDzrDigPos;
                    var cj = jDvdDigPos - jDzrDigPos;
                    if (cj < 0)
                    {
                        ci--;
                        cj += 16;
                    }
                    long w = 0;
                    while (w < a.Fdn)
                    {
                        if (++cj > 15)
                        {
                            ++ci;
                            cj = 0;
                        }
                        ++w;
                    }
                    for (; iDvdDigPos >= 0; iDvdDigPos--, jDvdDigPos = 15)
                    for (; jDvdDigPos >= 0; jDvdDigPos--)
                    {
                        long res = 0;
                        var run = true;
                        while (run && ci >= 0)
                        {
                            int ii = iDvdDigPos, jj = jDvdDigPos, pp = iDzrDigPos, qq = jDzrDigPos;
                            bool g = true, l = true;
                            if (!inc)
                                while (g && l && pp >= 0 && qq >= 0 && ii >= 0 && jj >= 0)
                                {
                                    if (((a._number[ii] >> (4 * jj)) & 0x000000000000000F) >
                                        ((b._number[pp] >> (4 * qq)) & 0x000000000000000F)) l = false;
                                    if (((a._number[ii] >> (4 * jj)) & 0x000000000000000F) <
                                        ((b._number[pp] >> (4 * qq)) & 0x000000000000000F)) g = false;
                                    if (--jj < 0)
                                    {
                                        jj = 15;
                                        --ii;
                                    }
                                    if (--qq >= 0) continue;
                                    qq = 15;
                                    --pp;
                                }
                            if (g)
                            {
                                var itDvdDigPos = iDvdDigPos - iDzrDigPos;
                                var jtDvdDigPos = jDvdDigPos - jDzrDigPos;
                                if (jtDvdDigPos < 0)
                                {
                                    itDvdDigPos--;
                                    jtDvdDigPos += 16;
                                }

                                int mod = 0, itDzrDigPos = 0, jtDzrDigPos = 0;
                                // определяем разряд, если вышли за границы числа справа
                                while (itDvdDigPos < 0)
                                {
                                    if (++jtDvdDigPos > 15)
                                    {
                                        ++itDvdDigPos;
                                        jtDvdDigPos = 0;
                                    }
                                    if (++jtDzrDigPos <= 15) continue;
                                    ++itDzrDigPos;
                                    jtDzrDigPos = 0;
                                }

                                while ((itDzrDigPos <= iDzrDigPos || mod > 0) && itDvdDigPos < a._number.Length)
                                {
                                    var t = ((a._number[itDvdDigPos] >> (4 * jtDvdDigPos)) & 0x000000000000000F) - mod;
                                    if (itDzrDigPos < b._number.Length)
                                        t -= (b._number[itDzrDigPos] >> (4 * jtDzrDigPos)) & 0x000000000000000F;
                                    if (t < 0)
                                    {
                                        mod = 1;
                                        t += 10;
                                    }
                                    else
                                    {
                                        mod = 0;
                                    }

                                    long f = 0; //отнимаем от делимого числа
                                    for (var u = 0; u <= 15; u++)
                                        if (u == jtDvdDigPos) f |= t << (4 * u);
                                        else f |= ((a._number[itDvdDigPos] >> (4 * u)) & 0x000000000000000F) << (4 * u);
                                    a._number[itDvdDigPos] = f;
                                    if (++jtDvdDigPos > 15)
                                    {
                                        itDvdDigPos++;
                                        jtDvdDigPos = 0;
                                    }
                                    if (++jtDzrDigPos <= 15) continue;
                                    itDzrDigPos++;
                                    jtDzrDigPos = 0;
                                }
                                ++res;
                                inc = ((a._number[inci] >> (4 * incj)) & 0x000000000000000F) > 0;
                            }
                            else
                            {
                                if (((a._number[iDvdDigPos] >> (4 * jDvdDigPos)) & 0x000000000000000F) > 0)
                                {
                                    inc = true;
                                    inci = iDvdDigPos;
                                    incj = jDvdDigPos;
                                }
                                run = false;
                                if (res <= 9)
                                {
                                    c._number[ci] |= res << (4 * cj);
                                }
                                else
                                {
                                    long ri = ci;
                                    var rj = cj;
                                    c._number[ri] |= (res % 10) << (4 * rj);
                                    res /= 10;
                                    while (res > 0)
                                    {
                                        if (++rj > 15)
                                        {
                                            ++ri;
                                            rj = 0;
                                        }
                                        res += (c._number[ri] >> (4 * rj)) & 0x000000000000000F; // + (res % 10);
                                        c._number[ri] |= (res % 10) << (4 * rj); //res = res2;
                                        res /= 10;
                                    }
                                }
                                if (--cj >= 0) continue;
                                --ci;
                                cj = 15;
                            }
                        } // while (run && (ci >= 0))
                    } // for j                  
                    if (IsZero(c)) c._attribute = IZero;
                    return c;
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("DIVIDE BY ZERO");
                    return c;
                }
            }
        }

        // вывод числа в консоль
        public static void Show(Infinitely a)
        {
            if (a._attribute == 0)
            {
                Console.Write(0);
            }
            else
            {
                if (a._attribute == -1) Console.Write("-");
                int i = a._number.Length - 1, j = 15;
                long w = 0;
                int wi = 0, wj = -1;
                //поиск позиции первой цифры дробной части
                while (w < a.Fdn)
                {
                    if (++wj > 15)
                    {
                        wj = 0;
                        ++wi;
                    }
                    ++w;
                }
                // пропускаем незначащие нули до точки
                var p = true;
                while (i >= 0 && ((a._number[i] >> (4 * j)) & 0x000000000000000F) == 0 && p)
                {
                    if (i == wi && j == wj)
                    {
                        Console.Write("0.{0}", (a._number[i] >> (4 * j)) & 0x000000000000000F);
                        p = false;
                    }
                    if (--j >= 0) continue;
                    j = 15;
                    i--;
                }
                //если первая ненулевая цифра числа находится в дробной части, то приписываем ноль к целой части
                if (i >= 0)
                {
                    if (i == wi && j == wj)
                        if (p) Console.Write("0.");
                    Console.Write("{0}", (a._number[i] >> (4 * j)) & 0x000000000000000F);
                    if (--j < 0)
                    {
                        j = 15;
                        i--;
                    }
                }
                //вывод остальных цифр числа
                while (i >= 0)
                {
                    if (i == wi && j == wj)
                        Console.Write(".");
                    Console.Write("{0}", (a._number[i] >> (4 * j)) & 0x000000000000000F);
                    if (--j >= 0) continue;
                    j = 15;
                    i--;
                }
            }
            Console.WriteLine();
        }
        public static void ShowAll(Infinitely a)
        {
            if (a._attribute == 0)
            {
                Console.Write(0);
            }
            else
            {
                int i = a._number.Length - 1, j = 15;
                int wi = 0, wj = 15;

                //вывод остальных цифр числа
                while (i >= 0)
                {
                    if (i == wi && j == wj)
                        Console.Write(".");
                    Console.Write("{0}", (a._number[i] >> (4 * j)) & 0x000000000000000F);
                    if (--j >= 0) continue;
                    j = 15;
                    i--;
                    //delete this
                    Console.Write(" ");
                }
            }
        }

        //возвращает первый разряд целой части числа
        public static void FindFrac(Infinitely a, ref int i, ref int j)
        {
            var w = a.Fdn;
            i = 0;
            j = 0;
            while (w > 0)
            {
                if (++j > 15)
                {
                    ++i;
                    j = 0;
                }
                --w;
            }
        }
    }
}
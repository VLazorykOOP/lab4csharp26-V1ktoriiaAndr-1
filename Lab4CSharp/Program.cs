using System;
using System.Linq;
using System.Collections.Generic;

class Point
{
    protected int x, y;
    protected int color;

    public Point() { x = 0; y = 0; color = 0; }
    public Point(int x, int y, int color) { this.x = x; this.y = y; this.color = color; }

    public int this[int index]
    {
        get
        {
            switch (index)
            {
                case 0: return x;
                case 1: return y;
                case 2: return color;
                default:
                    Console.WriteLine("Помилка: невірний індекс!");
                    return 0;
            }
        }
        set
        {
            switch (index)
            {
                case 0: x = value; break;
                case 1: y = value; break;
                case 2: color = value; break;
                default:
                    Console.WriteLine("Помилка: невірний індекс!");
                    break;
            }
        }
    }

    public static Point operator ++(Point p) { p.x++; p.y++; return p; }
    public static Point operator --(Point p) { p.x--; p.y--; return p; }
    public static bool operator true(Point p) => p.x == p.y;
    public static bool operator false(Point p) => p.x != p.y;
    public static Point operator +(Point p, int s) => new Point(p.x + s, p.y + s, p.color);
    public static Point operator +(Point p1, Point p2) => new Point(p1.x + p2.x, p1.y + p2.y, p1.color);
    public override string ToString() => $"({x}, {y}, колір: {color})";
    public static implicit operator string(Point p) => p.ToString();
    public static explicit operator Point(string s)
    {
        var parts = s.Split(',');
        if (parts.Length >= 3 && 
            int.TryParse(parts[0].Trim(), out int x) &&
            int.TryParse(parts[1].Trim(), out int y) &&
            int.TryParse(parts[2].Trim(), out int c))
            return new Point(x, y, c);
        throw new FormatException("Невірний формат рядка для Point. Очікуується: 'x, y, колір'");
    }

    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }
    public int Color { get => color; }
    public void Print() => Console.WriteLine($"  Точка: ({x}, {y}), колір: {color}");
    public double Distance() => Math.Sqrt(x * x + y * y);
    public void Move(int dx, int dy) { x += dx; y += dy; }
}

class VectorInt
{
    protected int[] IntArray;
    protected uint size;
    protected int codeError;
    protected static uint num_vec = 0;

    public VectorInt() { Init(1, 0); }
    public VectorInt(uint size) { Init(size, 0); }
    public VectorInt(uint size, int value) { Init(size, value); }
    ~VectorInt() { num_vec--; Console.WriteLine($"[VectorInt] Знищено. Залишилось: {num_vec}"); }

    void Init(uint s, int val) { size = s; IntArray = new int[size]; for (uint i = 0; i < size; i++) IntArray[i] = val; codeError = 0; num_vec++; }

    public uint Size { get => size; }
    public int CodeError { get => codeError; set => codeError = value; }

    public int this[uint idx]
    {
        get { if (idx < size) { codeError = 0; return IntArray[idx]; } codeError = -1; return 0; }
        set { if (idx < size) { codeError = 0; IntArray[idx] = value; } else codeError = -1; }
    }

    public void Input() { for (uint i = 0; i < size; i++) { Console.Write($"[{i}]="); IntArray[i] = int.Parse(Console.ReadLine()); } }
    public void Output() { Console.Write("["); for (uint i = 0; i < size; i++) { Console.Write(IntArray[i]); if (i < size - 1) Console.Write(", "); } Console.WriteLine("]"); }
    public void Fill(int val) { for (uint i = 0; i < size; i++) IntArray[i] = val; }
    public static uint GetNumVectors() => num_vec;

    public static VectorInt operator ++(VectorInt v) { var r = new VectorInt(v.size); for (uint i = 0; i < v.size; i++) r.IntArray[i] = v.IntArray[i] + 1; return r; }
    public static VectorInt operator --(VectorInt v) { var r = new VectorInt(v.size); for (uint i = 0; i < v.size; i++) r.IntArray[i] = v.IntArray[i] - 1; return r; }
    public static bool operator true(VectorInt v) { if (v.size == 0) return false; for (uint i = 0; i < v.size; i++) if (v.IntArray[i] == 0) return false; return true; }
    public static bool operator false(VectorInt v) { if (v.size == 0) return true; for (uint i = 0; i < v.size; i++) if (v.IntArray[i] != 0) return false; return true; }
    public static bool operator !(VectorInt v) => v.size != 0;
    public static VectorInt operator ~(VectorInt v) { var r = new VectorInt(v.size); for (uint i = 0; i < v.size; i++) r.IntArray[i] = ~v.IntArray[i]; return r; }

    static VectorInt ApplyOp(VectorInt v1, VectorInt v2, Func<int, int, int> op)
    {
        uint max = Math.Max(v1.size, v2.size);
        var r = new VectorInt(max);
        for (uint i = 0; i < max; i++)
        {
            int a = i < v1.size ? v1.IntArray[i] : 0;
            int b = i < v2.size ? v2.IntArray[i] : 0;
            r.IntArray[i] = op(a, b);
        }
        return r;
    }

    static VectorInt ApplyOpScalar(VectorInt v, int s, Func<int, int, int> op)
    {
        var r = new VectorInt(v.size);
        for (uint i = 0; i < v.size; i++)
            r.IntArray[i] = op(v.IntArray[i], s);
        return r;
    }

    public static VectorInt operator +(VectorInt a, VectorInt b) => ApplyOp(a, b, (x, y) => x + y);
    public static VectorInt operator +(VectorInt v, int s) => ApplyOpScalar(v, s, (x, y) => x + y);
    public static VectorInt operator -(VectorInt a, VectorInt b) => ApplyOp(a, b, (x, y) => x - y);
    public static VectorInt operator -(VectorInt v, int s) => ApplyOpScalar(v, s, (x, y) => x - y);
    public static VectorInt operator *(VectorInt a, VectorInt b) => ApplyOp(a, b, (x, y) => x * y);
    public static VectorInt operator *(VectorInt v, int s) => ApplyOpScalar(v, s, (x, y) => x * y);
    public static VectorInt operator /(VectorInt a, VectorInt b) => ApplyOp(a, b, (x, y) => y != 0 ? x / y : throw new DivideByZeroException("Ділення на нуль у векторі!"));
    public static VectorInt operator /(VectorInt v, int s) => ApplyOpScalar(v, s, (x, y) => y != 0 ? x / y : throw new DivideByZeroException("Ділення на нуль у векторі!"));
    public static VectorInt operator %(VectorInt a, VectorInt b) => ApplyOp(a, b, (x, y) => y != 0 ? x % y : throw new DivideByZeroException("Ділення на нуль у векторі!"));
    public static VectorInt operator %(VectorInt v, int s) => ApplyOpScalar(v, s, (x, y) => y != 0 ? x % y : throw new DivideByZeroException("Ділення на нуль у векторі!"));

    public static VectorInt operator |(VectorInt a, VectorInt b) => ApplyOp(a, b, (x, y) => x | y);
    public static VectorInt operator |(VectorInt v, int s) => ApplyOpScalar(v, s, (x, y) => x | y);
    public static VectorInt operator &(VectorInt a, VectorInt b) => ApplyOp(a, b, (x, y) => x & y);
    public static VectorInt operator &(VectorInt v, int s) => ApplyOpScalar(v, s, (x, y) => x & y);
    public static VectorInt operator ^(VectorInt a, VectorInt b) => ApplyOp(a, b, (x, y) => x ^ y);
    public static VectorInt operator ^(VectorInt v, int s) => ApplyOpScalar(v, s, (x, y) => x ^ y);
    public static VectorInt operator >>(VectorInt a, VectorInt b) => ApplyOp(a, b, (x, y) => (y >= 0 && y < 32) ? x >> y : x);
    public static VectorInt operator >>(VectorInt v, int shift) { var r = new VectorInt(v.size); for (uint i = 0; i < v.size; i++) r.IntArray[i] = (shift >= 0 && shift < 32) ? v.IntArray[i] >> shift : v.IntArray[i]; return r; }
    public static VectorInt operator <<(VectorInt a, VectorInt b) => ApplyOp(a, b, (x, y) => (y >= 0 && y < 32) ? x << y : x);
    public static VectorInt operator <<(VectorInt v, int shift) { var r = new VectorInt(v.size); for (uint i = 0; i < v.size; i++) r.IntArray[i] = (shift >= 0 && shift < 32) ? v.IntArray[i] << shift : v.IntArray[i]; return r; }

    static bool Compare(VectorInt a, VectorInt b, Func<int, int, bool> op) { uint min = Math.Min(a.size, b.size); for (uint i = 0; i < min; i++) if (!op(a.IntArray[i], b.IntArray[i])) return false; return true; }
    static bool CompareStrict(VectorInt a, VectorInt b, Func<int, int, bool> op) { if (a.size != b.size) return false; for (uint i = 0; i < a.size; i++) if (!op(a.IntArray[i], b.IntArray[i])) return false; return true; }
    public static bool operator ==(VectorInt a, VectorInt b) => a.size == b.size && Compare(a, b, (x, y) => x == y);
    public static bool operator !=(VectorInt a, VectorInt b) => !(a == b);
    public static bool operator >(VectorInt a, VectorInt b) => CompareStrict(a, b, (x, y) => x > y);
    public static bool operator >=(VectorInt a, VectorInt b) => CompareStrict(a, b, (x, y) => x >= y);
    public static bool operator <(VectorInt a, VectorInt b) => CompareStrict(a, b, (x, y) => x < y);
    public static bool operator <=(VectorInt a, VectorInt b) => CompareStrict(a, b, (x, y) => x <= y);

    public override bool Equals(object? obj) => obj is VectorInt v && this == v;
    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 31 + size.GetHashCode();
        for (uint i = 0; i < size; i++)
            hash = hash * 31 + IntArray[i].GetHashCode();
        return hash;
    }
}

public struct ApplicantStruct
{
    public string LastName, FirstName, Patronymic;
    public int BirthYear;
    public int[] ExamGrades;
    public double AvgCertificate;

    public ApplicantStruct(string ln, string fn, string pn, int by, int[] grades, double avg)
    { LastName = ln; FirstName = fn; Patronymic = pn; BirthYear = by; ExamGrades = grades; AvgCertificate = avg; }

    public override string ToString() => $"{LastName} {FirstName} {Patronymic}, {BirthYear}, оцінки: [{string.Join(", ", ExamGrades)}], середній: {AvgCertificate}";
}

public class ApplicantTuple
{
    public (string LastName, string FirstName, string Patronymic, int BirthYear, int[] ExamGrades, double AvgCertificate) Data;
    public ApplicantTuple(string ln, string fn, string pn, int by, int[] grades, double avg)
    { Data = (ln, fn, pn, by, grades, avg); }
    public override string ToString() => $"{Data.LastName} {Data.FirstName} {Data.Patronymic}, {Data.BirthYear}, оцінки: [{string.Join(", ", Data.ExamGrades)}], середній: {Data.AvgCertificate}";
}

public record ApplicantRecord(string LastName, string FirstName, string Patronymic, int BirthYear, int[] ExamGrades, double AvgCertificate)
{
    public override string ToString() => $"{LastName} {FirstName} {Patronymic}, {BirthYear}, оцінки: [{string.Join(", ", ExamGrades)}], середній: {AvgCertificate}";
}

public class ApplicantManager<T>
{
    private List<T> items = new();
    private Func<T, string> getLastName;
    private Func<T, string> toString;

    public ApplicantManager(Func<T, string> getLn, Func<T, string> toStr) { getLastName = getLn; toString = toStr; }
    public void Add(T item) => items.Add(item);
    public bool DeleteByIndex(int idx) { if (idx < 0 || idx >= items.Count) return false; items.RemoveAt(idx); return true; }
    public bool AddAfterSurname(string surname, T newItem)
    {
        for (int i = 0; i < items.Count; i++)
            if (getLastName(items[i]).Equals(surname, StringComparison.OrdinalIgnoreCase)) { items.Insert(i + 1, newItem); return true; }
        return false;
    }
    public void PrintAll(string title)
    {
        Console.WriteLine($"\n=== {title} ===");
        for (int i = 0; i < items.Count; i++) Console.WriteLine($"[{i}] {toString(items[i])}");
    }
}

class MatrixInt
{
    protected int[,] IntArray;
    protected int n, m;
    protected int codeError;
    protected static int num_mat = 0;

    public MatrixInt() { Init(1, 1, 0); }
    public MatrixInt(int rows, int cols) { Init(rows, cols, 0); }
    public MatrixInt(int rows, int cols, int val) { Init(rows, cols, val); }
    ~MatrixInt() { num_mat--; Console.WriteLine($"[MatrixInt] Знищено. Залишилось: {num_mat}"); }

    void Init(int rows, int cols, int val) { n = rows; m = cols; IntArray = new int[n, m]; for (int i = 0; i < n; i++) for (int j = 0; j < m; j++) IntArray[i, j] = val; codeError = 0; num_mat++; }

    public int Rows { get => n; }
    public int Cols { get => m; }
    public int CodeError { get => codeError; set => codeError = value; }
    public static int GetNumMatrices() => num_mat;

    public int this[int i, int j]
    {
        get { if (i >= 0 && i < n && j >= 0 && j < m) { codeError = 0; return IntArray[i, j]; } codeError = -1; return 0; }
        set { if (i >= 0 && i < n && j >= 0 && j < m) { codeError = 0; IntArray[i, j] = value; } else codeError = -1; }
    }

    public int this[int k]
    {
        get { int i = k / m, j = k % m; if (i >= 0 && i < n && j >= 0 && j < m) { codeError = 0; return IntArray[i, j]; } codeError = -1; return 0; }
        set { int i = k / m, j = k % m; if (i >= 0 && i < n && j >= 0 && j < m) { codeError = 0; IntArray[i, j] = value; } else codeError = -1; }
    }

    public void Input() { for (int i = 0; i < n; i++) for (int j = 0; j < m; j++) { Console.Write($"[{i},{j}]="); IntArray[i, j] = int.Parse(Console.ReadLine()); } }
    public void Output() { for (int i = 0; i < n; i++) { for (int j = 0; j < m; j++) Console.Write($"{IntArray[i, j],5}"); Console.WriteLine(); } }
    public void Fill(int val) { for (int i = 0; i < n; i++) for (int j = 0; j < m; j++) IntArray[i, j] = val; }

    public static MatrixInt operator ++(MatrixInt mat) { var r = new MatrixInt(mat.n, mat.m); for (int i = 0; i < mat.n; i++) for (int j = 0; j < mat.m; j++) r.IntArray[i, j] = mat.IntArray[i, j] + 1; return r; }
    public static MatrixInt operator --(MatrixInt mat) { var r = new MatrixInt(mat.n, mat.m); for (int i = 0; i < mat.n; i++) for (int j = 0; j < mat.m; j++) r.IntArray[i, j] = mat.IntArray[i, j] - 1; return r; }
    public static bool operator true(MatrixInt mat) { if (mat.n == 0 || mat.m == 0) return false; for (int i = 0; i < mat.n; i++) for (int j = 0; j < mat.m; j++) if (mat.IntArray[i, j] == 0) return false; return true; }
    public static bool operator false(MatrixInt mat) { if (mat.n == 0 || mat.m == 0) return true; for (int i = 0; i < mat.n; i++) for (int j = 0; j < mat.m; j++) if (mat.IntArray[i, j] != 0) return false; return true; }
    public static bool operator !(MatrixInt mat) => mat.n != 0 && mat.m != 0;
    public static MatrixInt operator ~(MatrixInt mat) { var r = new MatrixInt(mat.n, mat.m); for (int i = 0; i < mat.n; i++) for (int j = 0; j < mat.m; j++) r.IntArray[i, j] = ~mat.IntArray[i, j]; return r; }

    static MatrixInt ApplyOp(MatrixInt a, MatrixInt b, Func<int, int, int> op)
    {
        if (a.n != b.n || a.m != b.m) return new MatrixInt(a.n, a.m);
        var r = new MatrixInt(a.n, a.m);
        for (int i = 0; i < a.n; i++) for (int j = 0; j < a.m; j++) r.IntArray[i, j] = op(a.IntArray[i, j], b.IntArray[i, j]);
        return r;
    }
    static MatrixInt ApplyOpScalar(MatrixInt a, int s, Func<int, int, int> op) { var r = new MatrixInt(a.n, a.m); for (int i = 0; i < a.n; i++) for (int j = 0; j < a.m; j++) r.IntArray[i, j] = op(a.IntArray[i, j], s); return r; }

    public static MatrixInt operator +(MatrixInt a, MatrixInt b) => ApplyOp(a, b, (x, y) => x + y);
    public static MatrixInt operator +(MatrixInt a, int s) => ApplyOpScalar(a, s, (x, y) => x + y);
    public static MatrixInt operator -(MatrixInt a, MatrixInt b) => ApplyOp(a, b, (x, y) => x - y);
    public static MatrixInt operator -(MatrixInt a, int s) => ApplyOpScalar(a, s, (x, y) => x - y);
    public static MatrixInt operator *(MatrixInt a, MatrixInt b)
    {
        if (a.m != b.n) { a.codeError = -1; return new MatrixInt(a.n, a.m); }
        var r = new MatrixInt(a.n, b.m);
        for (int i = 0; i < a.n; i++) for (int j = 0; j < b.m; j++) { r.IntArray[i, j] = 0; for (int k = 0; k < a.m; k++) r.IntArray[i, j] += a.IntArray[i, k] * b.IntArray[k, j]; }
        return r;
    }
    public static MatrixInt operator *(MatrixInt a, int s) => ApplyOpScalar(a, s, (x, y) => x * y);
    public static VectorInt operator *(MatrixInt a, VectorInt v)
    {
        if (a.m != v.Size) { a.codeError = -1; return new VectorInt(0); }
        var r = new VectorInt((uint)a.n);
        for (int i = 0; i < a.n; i++)
        {
            int sum = 0;
            for (uint k = 0; k < v.Size; k++)
                sum += a.IntArray[i, k] * (int)v[k];
            r[(uint)i] = sum;
        }
        return r;
    }
    public static MatrixInt operator /(MatrixInt a, MatrixInt b) => ApplyOp(a, b, (x, y) => y != 0 ? x / y : throw new DivideByZeroException("Ділення на нуль у матриці!"));
    public static MatrixInt operator /(MatrixInt a, int s) => ApplyOpScalar(a, s, (x, y) => y != 0 ? x / y : throw new DivideByZeroException("Ділення на нуль у матриці!"));
    public static MatrixInt operator %(MatrixInt a, MatrixInt b) => ApplyOp(a, b, (x, y) => y != 0 ? x % y : throw new DivideByZeroException("Ділення на нуль у матриці!"));
    public static MatrixInt operator %(MatrixInt a, int s) => ApplyOpScalar(a, s, (x, y) => y != 0 ? x % y : throw new DivideByZeroException("Ділення на нуль у матриці!"));

    public static MatrixInt operator |(MatrixInt a, MatrixInt b) => ApplyOp(a, b, (x, y) => x | y);
    public static MatrixInt operator |(MatrixInt a, int s) => ApplyOpScalar(a, s, (x, y) => x | y);
    public static MatrixInt operator &(MatrixInt a, MatrixInt b) => ApplyOp(a, b, (x, y) => x & y);
    public static MatrixInt operator &(MatrixInt a, int s) => ApplyOpScalar(a, s, (x, y) => x & y);
    public static MatrixInt operator ^(MatrixInt a, MatrixInt b) => ApplyOp(a, b, (x, y) => x ^ y);
    public static MatrixInt operator ^(MatrixInt a, int s) => ApplyOpScalar(a, s, (x, y) => x ^ y);
    public static MatrixInt operator >>(MatrixInt a, int shift) { var r = new MatrixInt(a.n, a.m); for (int i = 0; i < a.n; i++) for (int j = 0; j < a.m; j++) r.IntArray[i, j] = (shift >= 0 && shift < 32) ? a.IntArray[i, j] >> shift : a.IntArray[i, j]; return r; }
    public static MatrixInt operator <<(MatrixInt a, int shift) { var r = new MatrixInt(a.n, a.m); for (int i = 0; i < a.n; i++) for (int j = 0; j < a.m; j++) r.IntArray[i, j] = (shift >= 0 && shift < 32) ? a.IntArray[i, j] << shift : a.IntArray[i, j]; return r; }

    static bool Compare(MatrixInt a, MatrixInt b, Func<int, int, bool> op)
    {
        if (a.n != b.n || a.m != b.m) return false;
        for (int i = 0; i < a.n; i++) for (int j = 0; j < a.m; j++) if (!op(a.IntArray[i, j], b.IntArray[i, j])) return false;
        return true;
    }
    public static bool operator ==(MatrixInt a, MatrixInt b) => Compare(a, b, (x, y) => x == y);
    public static bool operator !=(MatrixInt a, MatrixInt b) => !(a == b);
    public static bool operator >(MatrixInt a, MatrixInt b) => Compare(a, b, (x, y) => x > y);
    public static bool operator >=(MatrixInt a, MatrixInt b) => Compare(a, b, (x, y) => x >= y);
    public static bool operator <(MatrixInt a, MatrixInt b) => Compare(a, b, (x, y) => x < y);
    public static bool operator <=(MatrixInt a, MatrixInt b) => Compare(a, b, (x, y) => x <= y);

    public override bool Equals(object? obj) => obj is MatrixInt m && this == m;
    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 31 + n.GetHashCode();
        hash = hash * 31 + m.GetHashCode();
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                hash = hash * 31 + IntArray[i, j].GetHashCode();
        return hash;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== ЛАБОРАТОРНІ РОБОТИ: C# ===\n");
        
        Console.WriteLine("1 - Point");
        Console.WriteLine("2 - VectorInt");
        Console.WriteLine("3 - Applicant (Struct/Tuple/Record)");
        Console.WriteLine("4 - MatrixInt");
        Console.Write("\nОберіть завдання: ");
        int choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case 1: TestPoint(); break;
            case 2: TestVectorInt(); break;
            case 3: TestApplicant(); break;
            case 4: TestMatrixInt(); break;
            default: Console.WriteLine("Невірний вибір!"); break;
        }
    }

    static void TestPoint()
    {
        Console.WriteLine("\n=== Тестування Point ===");
        var p = new Point(3, 5, 10);
        Console.WriteLine($"Початкова точка: {p}");
        Console.WriteLine($"Індексатор [0]={p[0]}, [1]={p[1]}, [2]={p[2]}");
        
        Console.WriteLine("\nТестування ++:");
        p++; 
        Console.WriteLine($"Після ++: {p}");
        
        Console.WriteLine("\nТестування --:");
        p--;
        Console.WriteLine($"Після --: {p}");
        
        Console.WriteLine("\nТестування true/false:");
        var p1 = new Point(5, 5, 1);
        Console.WriteLine($"Point(5,5,1) == true: {(p1 ? true : false)}");
        var p2 = new Point(3, 5, 1);
        Console.WriteLine($"Point(3,5,1) == false: {(p2 ? false : true)}");
        
        Console.WriteLine("\nТестування + зі скаляром:");
        var p3 = p + 10;
        Console.WriteLine($"Point + 10: {p3}");
        
        Console.WriteLine("\nПереведення в string:");
        string s = (string)p;
        Console.WriteLine($"ToString: {s}");
        
        Console.WriteLine("\nПереведення string в Point:");
        var p4 = (Point)"7, 8, 15";
        Console.WriteLine($"Зі string: {p4}");
        
        Console.WriteLine("\nТестування помилки індексу:");
        Console.WriteLine($"[5] (невірний): {p[5]}");
    }

    static void TestVectorInt()
    {
        Console.WriteLine("\n=== Тестування VectorInt ===");
        var v1 = new VectorInt(4, 2);
        var v2 = new VectorInt(3, 3);
        Console.WriteLine("v1: "); v1.Output();
        Console.WriteLine("v2: "); v2.Output();
        
        Console.WriteLine("\n--- Арифметичні операції ---");
        Console.WriteLine("v1 + v2: "); (v1 + v2).Output();
        Console.WriteLine("v1 + 10: "); (v1 + 10).Output();
        Console.WriteLine("v1 - v2: "); (v1 - v2).Output();
        Console.WriteLine("v1 - 1: "); (v1 - 1).Output();
        Console.WriteLine("v1 * v2: "); (v1 * v2).Output();
        Console.WriteLine("v1 * 3: "); (v1 * 3).Output();
        Console.Write("v1 / v2: "); 
        try { (v1 / v2).Output(); }
        catch (DivideByZeroException ex) { Console.WriteLine($"Помилка: {ex.Message}"); }
        Console.WriteLine("v1 / 2: "); (v1 / 2).Output();
        Console.Write("v1 % v2: ");
        try { (v1 % v2).Output(); }
        catch (DivideByZeroException ex) { Console.WriteLine($"Помилка: {ex.Message}"); }
        Console.WriteLine("v1 % 3: "); (v1 % 3).Output();
        
        Console.WriteLine("\n--- Побітові операції ---");
        Console.WriteLine("v1 | v2: "); (v1 | v2).Output();
        Console.WriteLine("v1 | 5: "); (v1 | 5).Output();
        Console.WriteLine("v1 & v2: "); (v1 & v2).Output();
        Console.WriteLine("v1 & 1: "); (v1 & 1).Output();
        Console.WriteLine("v1 ^ v2: "); (v1 ^ v2).Output();
        Console.WriteLine("v1 ^ 7: "); (v1 ^ 7).Output();
        Console.WriteLine("v1 >> 1: "); (v1 >> 1).Output();
        Console.WriteLine("v1 >> v2: "); (v1 >> v2).Output();
        Console.WriteLine("v1 << 1: "); (v1 << 1).Output();
        Console.WriteLine("v1 << v2: "); (v1 << v2).Output();
        
        Console.WriteLine("\n--- Унарні операції ---");
        Console.WriteLine("++v1: "); (++v1).Output();
        Console.WriteLine("--v1: "); (--v1).Output();
        Console.WriteLine("~v1: "); (~v1).Output();
        Console.WriteLine($"!v1 (size!=0): {!v1}");
        Console.WriteLine($"v1 true: {(v1 ? true : false)}");
        
        Console.WriteLine("\n--- Порівняння ---");
        Console.WriteLine($"v1 == v2: {v1 == v2}");
        Console.WriteLine($"v1 != v2: {v1 != v2}");
        var v3 = new VectorInt(4, 2);
        var v4 = new VectorInt(4, 3);
        Console.WriteLine($"v3(4,2) > v4(4,3): {v3 > v4}");
        Console.WriteLine($"v3(4,2) < v4(4,3): {v3 < v4}");
        Console.WriteLine($"v3(4,2) >= v4(4,3): {v3 >= v4}");
        Console.WriteLine($"v3(4,2) <= v4(4,3): {v3 <= v4}");
        
        Console.WriteLine("\n--- Індексатор з помилкою ---");
        Console.WriteLine($"v1[10] (невірний): {v1[10]}, codeError: {v1.CodeError}");
        v1[10] = 5;
        Console.WriteLine($"Після запису в v1[10], codeError: {v1.CodeError}");
        
        Console.WriteLine($"Кількість векторів: {VectorInt.GetNumVectors()}");
    }

    static void TestApplicant()
    {
        var mgrStruct = new ApplicantManager<ApplicantStruct>(a => a.LastName, a => a.ToString());
        mgrStruct.Add(new ApplicantStruct("Коваленко", "Іван", "Петрович", 2005, new[] { 180, 175, 190 }, 10.5));
        mgrStruct.Add(new ApplicantStruct("Шевченко", "Олена", "Миколаївна", 2004, new[] { 195, 188, 192 }, 11.0));
        mgrStruct.PrintAll("STRUCT");
        mgrStruct.DeleteByIndex(0);
        mgrStruct.AddAfterSurname("Шевченко", new ApplicantStruct("Новий", "Тест", "Тестович", 2006, new[] { 150, 160, 170 }, 9.0));
        mgrStruct.PrintAll("STRUCT після змін");

        var mgrTuple = new ApplicantManager<ApplicantTuple>(a => a.Data.LastName, a => a.ToString());
        mgrTuple.Add(new ApplicantTuple("Петренко", "Марія", "Ігорівна", 2005, new[] { 185, 180, 188 }, 10.8));
        mgrTuple.PrintAll("TUPLE");

        var mgrRecord = new ApplicantManager<ApplicantRecord>(a => a.LastName, a => a.ToString());
        mgrRecord.Add(new ApplicantRecord("Бондаренко", "Андрій", "Олексійович", 2003, new[] { 200, 195, 198 }, 11.2));
        mgrRecord.PrintAll("RECORD");
    }

    static void TestMatrixInt()
    {
        Console.WriteLine("\n=== Тестування MatrixInt ===");
        var m1 = new MatrixInt(2, 3, 2);
        var m2 = new MatrixInt(2, 3, 3);
        Console.WriteLine("Matrix m1:"); m1.Output();
        Console.WriteLine("Matrix m2:"); m2.Output();
        
        Console.WriteLine("\n--- Арифметичні операції ---");
        Console.WriteLine("m1 + m2:"); (m1 + m2).Output();
        Console.WriteLine("m1 + 10:"); (m1 + 10).Output();
        Console.WriteLine("m1 - m2:"); (m1 - m2).Output();
        Console.WriteLine("m1 - 1:"); (m1 - 1).Output();
        Console.WriteLine("m1 * m2 (помилка розмірів):"); (m1 * m2).Output();
        Console.WriteLine("m1 * 5:"); (m1 * 5).Output();
        Console.WriteLine("m1 / m2:"); (m1 / m2).Output();
        Console.WriteLine("m1 / 2:"); (m1 / 2).Output();
        Console.WriteLine("m1 % m2:"); (m1 % m2).Output();
        Console.WriteLine("m1 % 3:"); (m1 % 3).Output();
        
        Console.WriteLine("\n--- Множення матриць ---");
        var m3 = new MatrixInt(2, 3, 1);
        var m4 = new MatrixInt(3, 2, 2);
        Console.WriteLine("m3 (2x3):"); m3.Output();
        Console.WriteLine("m4 (3x2):"); m4.Output();
        Console.WriteLine("m3 * m4 (2x2):"); (m3 * m4).Output();
        
        Console.WriteLine("\n--- Множення матриці на вектор ---");
        var v = new VectorInt(3, 2);
        Console.WriteLine("Matrix (2x3):"); m3.Output();
        Console.WriteLine("Vector (3):"); v.Output();
        var result = m3 * v;
        Console.WriteLine("Result Vector (2):"); result.Output();
        
        Console.WriteLine("\n--- Побітові операції ---");
        Console.WriteLine("m1 | m2:"); (m1 | m2).Output();
        Console.WriteLine("m1 | 7:"); (m1 | 7).Output();
        Console.WriteLine("m1 & m2:"); (m1 & m2).Output();
        Console.WriteLine("m1 & 1:"); (m1 & 1).Output();
        Console.WriteLine("m1 ^ m2:"); (m1 ^ m2).Output();
        Console.WriteLine("m1 ^ 5:"); (m1 ^ 5).Output();
        Console.WriteLine("m1 >> 1:"); (m1 >> 1).Output();
        Console.WriteLine("m1 << 1:"); (m1 << 1).Output();
        
        Console.WriteLine("\n--- Унарні операції ---");
        Console.WriteLine("++m1:"); (++m1).Output();
        Console.WriteLine("--m1:"); (--m1).Output();
        Console.WriteLine("~m1:"); (~m1).Output();
        Console.WriteLine($"!m1: {!m1}");
        Console.WriteLine($"m1 true: {(m1 ? true : false)}");
        
        Console.WriteLine("\n--- Порівняння ---");
        Console.WriteLine($"m1 == m2: {m1 == m2}");
        Console.WriteLine($"m1 != m2: {m1 != m2}");
        var m5 = new MatrixInt(2, 3, 2);
        var m6 = new MatrixInt(2, 3, 3);
        Console.WriteLine($"m5(2,3,2) > m6(2,3,3): {m5 > m6}");
        Console.WriteLine($"m5(2,3,2) < m6(2,3,3): {m5 < m6}");
        Console.WriteLine($"m5(2,3,2) >= m6(2,3,3): {m5 >= m6}");
        Console.WriteLine($"m5(2,3,2) <= m6(2,3,3): {m5 <= m6}");
        
        Console.WriteLine("\n--- Індексатори ---");
        Console.WriteLine($"m1[0,1]: {m1[0, 1]}");
        Console.WriteLine($"m1[4] (1D, k=4): {m1[4]}");
        Console.WriteLine($"m1[10,10] (невірний): {m1[10, 10]}, codeError: {m1.CodeError}");
        Console.WriteLine($"m1[100] (невірний): {m1[100]}, codeError: {m1.CodeError}");
        
        Console.WriteLine($"\nКількість матриць: {MatrixInt.GetNumMatrices()}");
    }
}

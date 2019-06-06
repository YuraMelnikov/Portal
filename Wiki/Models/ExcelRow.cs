namespace Wiki.Models
{
    public struct ExcelRow
    {
        string a;
        string b;
        string c;
        string d;
        string e;
        string f;
        string g;
        string h;
        string i;
        string j;
        string k;
        string l;
        string m;
        string n;
        string o;
        string p;
        string q;
        string r;
        string s;
        string t;
        string u;
        string v;
        string w;
        string x;
        string y;
        string z;
        int countData;

        public ExcelRow(string a, string b, string c, string d, string e, string f, string g, string h, string i, string j, string k, string l, string m, string n, string o, string p, string q, string r, string s, string t, string u, string v, string w, string x, string y, string z, int countData) : this()
        {
            this.A = a;
            this.B = b;
            this.C = c;
            this.D = d;
            this.E = e;
            this.F = f;
            this.G = g;
            this.H = h;
            this.I = i;
            this.J = j;
            this.K = k;
            this.L = l;
            this.M = m;
            this.N = n;
            this.O = o;
            this.P = p;
            this.Q = q;
            this.R = r;
            this.S = s;
            this.T = t;
            this.U = u;
            this.V = v;
            this.W = w;
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.countData = countData;
        }

        public int CountData { get => countData; set => countData = value; }
        public string A { get => a; set => a = value; }
        public string B { get => b; set => b = value; }
        public string C { get => c; set => c = value; }
        public string D { get => d; set => d = value; }
        public string E { get => e; set => e = value; }
        public string F { get => f; set => f = value; }
        public string G { get => g; set => g = value; }
        public string H { get => h; set => h = value; }
        public string I { get => i; set => i = value; }
        public string J { get => j; set => j = value; }
        public string K { get => k; set => k = value; }
        public string L { get => l; set => l = value; }
        public string M { get => m; set => m = value; }
        public string N { get => n; set => n = value; }
        public string O { get => o; set => o = value; }
        public string P { get => p; set => p = value; }
        public string Q { get => q; set => q = value; }
        public string R { get => r; set => r = value; }
        public string S { get => s; set => s = value; }
        public string T { get => t; set => t = value; }
        public string U { get => u; set => u = value; }
        public string V { get => v; set => v = value; }
        public string W { get => w; set => w = value; }
        public string X { get => x; set => x = value; }
        public string Y { get => y; set => y = value; }
        public string Z { get => z; set => z = value; }

        public string GetData(int step)
        {
            if (step == 0)
                return A;
            else if (step == 1)
                return B;
            else if (step == 2)
                return C;
            else if (step == 3)
                return D;
            else if (step == 4)
                return E;
            else if (step == 5)
                return F;
            else if (step == 6)
                return G;
            else if (step == 7)
                return H;
            else if (step == 8)
                return I;
            else if (step == 9)
                return J;
            else if (step == 10)
                return K;
            else if (step == 11)
                return L;
            else if (step == 12)
                return M;
            else if (step == 13)
                return N;
            else if (step == 14)
                return O;
            else if (step == 15)
                return P;
            else if (step == 16)
                return Q;
            else if (step == 17)
                return R;
            else if (step == 18)
                return S;
            else if (step == 19)
                return T;
            else if (step == 20)
                return U;
            else if (step == 20)
                return V;
            else if (step == 20)
                return W;
            else if (step == 20)
                return X;
            else if (step == 20)
                return Y;
            else if (step == 20)
                return Z;
            else
                return "";
        }
    }
}
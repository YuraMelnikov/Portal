namespace Wiki.Models
{
    public class ExcelRow
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

        public ExcelRow(string a, string b, string c, string d, string e, string f, string g, string h, string i, string j, string k, string l, string m, string n, string o, string p, string q, string r, string s, string t, string u, string v, string w, string x, string y, string z, int countData)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.e = e;
            this.f = f;
            this.g = g;
            this.h = h;
            this.i = i;
            this.j = j;
            this.k = k;
            this.l = l;
            this.m = m;
            this.n = n;
            this.o = o;
            this.p = p;
            this.q = q;
            this.r = r;
            this.s = s;
            this.t = t;
            this.u = u;
            this.v = v;
            this.w = w;
            this.x = x;
            this.y = y;
            this.z = z;
            this.countData = countData;
        }

        public int CountData { get => countData; set => countData = value; }
    }
}
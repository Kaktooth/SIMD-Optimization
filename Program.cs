using System;
using System.Diagnostics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace SSE
{
    class Program
    {
        static void Main(string[] args)
        {
            bool avx = Avx.IsSupported;
            bool sse = Sse.IsSupported;
            bool sse2 = Sse2.IsSupported;
            bool sse3 = Sse3.IsSupported;
            bool ssse3 = Ssse3.IsSupported;
            bool sse41 = Sse41.IsSupported;
            bool sse42 = Sse42.IsSupported;

            Console.WriteLine($"{avx} {sse} {sse2} {sse3} {ssse3} {sse41} {sse42} ");
            Console.WriteLine("Matrix4x4");
            int[,] a = new int[4,4];
            int[,] b = new int[4, 4];
            int c = 0;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    a[i, j] = c;
                    b[i, j] = i+j;
                    c++;
                }
            }
            Stopwatch s = new Stopwatch();
            s.Start();
            var m = MultiplyMatrix(a, b);
            s.Stop();
            ShowMatrix(m);
           
            Console.WriteLine("RunTime " + s.Elapsed);

            Stopwatch s2 = new Stopwatch();
            s2.Start();
            var m2 = MultiplyMatrixSSE(a, b);
            s2.Stop();
            ShowMatrixSSE(m2);
           
            Console.WriteLine("RunTime with SSE41 " + s2.Elapsed);

            Console.WriteLine("Matrix128x128");
            int[,] a2 = new int[128, 128];
            int[,] b2 = new int[128, 128];
            c = 0;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    a[i, j] = c;
                    b[i, j] = i + j;
                    c++;
                }
            }
            Stopwatch s3 = new Stopwatch();
            s3.Start();
            var m3 = MultiplyMatrix(a2, b2);
            s3.Stop();
            
            Console.WriteLine("RunTime " + s3.Elapsed);

            Stopwatch s4 = new Stopwatch();
            s4.Start();
            var m4 = MultiplyMatrixSSE(a2, b2);
            s4.Stop();
           
            Console.WriteLine("RunTime with SSE41 " + s4.Elapsed);

            Console.WriteLine("Matrix512x512");
            int[,] a3 = new int[512, 512];
            int[,] b3 = new int[512, 512];
            c = 0;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    a[i, j] = c;
                    b[i, j] = i + j;
                    c++;
                }
            }
            Stopwatch s5 = new Stopwatch();
            s5.Start();
            var m5 = MultiplyMatrix(a3, b3);
            s5.Stop();

            Console.WriteLine("RunTime " + s5.Elapsed);

            Stopwatch s6 = new Stopwatch();
            s6.Start();
            var m6 = MultiplyMatrixSSE(a3, b3);
            s6.Stop();

            Console.WriteLine("RunTime with SSE41 " + s6.Elapsed);


            Console.WriteLine("Matrix1536x1536");
            int[,] a4 = new int[1536, 1536];
            int[,] b4 = new int[1536, 1536];
            c = 0;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    a[i, j] = c;
                    b[i, j] = i + j;
                    c++;
                }
            }
            Stopwatch s7 = new Stopwatch();
            s7.Start();
            var m7 = MultiplyMatrix(a4, b4);
            s7.Stop();

            Console.WriteLine("RunTime " + s7.Elapsed);

            Stopwatch s8 = new Stopwatch();
            s8.Start();
            var m8 = MultiplyMatrixSSE(a4, b4);
            s8.Stop();

            Console.WriteLine("RunTime with SSE41 " + s8.Elapsed);


            Console.WriteLine("Matrix10000x10000");
            int[,] a5 = new int[10000, 10000];
            int[,] b5 = new int[10000, 10000];
            c = 0;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    a[i, j] = c;
                    b[i, j] = i + j;
                    c++;
                }
            }
            Stopwatch s9 = new Stopwatch();
            s9.Start();
            var m9 = MultiplyMatrix(a5, b5);
            s9.Stop();

            Console.WriteLine("RunTime " + s9.Elapsed);

            Stopwatch s10 = new Stopwatch();
            s10.Start();
            var m10 = MultiplyMatrixSSE(a5, b5);
            s10.Stop();

            Console.WriteLine("RunTime with SSE41 " + s10.Elapsed);

            Console.WriteLine("2) Result: ");
            ShowMatrixSSE(Procedure(a,b,3,Vector128.Create(1,2,3,4), Vector128.Create(2, 2, 2, 5)));

        }
        static void ShowMatrix(int[,] m)
        {
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    Console.Write(" " + m[i, j]);
                }

                Console.WriteLine();
            }
        }
        static void ShowMatrixSSE(Vector128<int>[,] m)
        {
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    Console.Write(" " + m[i, j]);
                }

                 Console.WriteLine();
            }
        }

        static int[,] MultiplyMatrix(int[,] a, int[,] b)
        {
            int[,] c = new int[a.GetLength(0), a.GetLength(1)];
            
            for (int i = 0; i < a.GetLength(0); i++)
            {
              
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    c[i, j] = a[i, j] * b[i, j];
                }
            }           
            return c;
        }

        public static Vector128<int>[,] MultiplyMatrixSSE(int[,] a, int[,] b)
        {
            unsafe
            {
                int length = a.GetLength(0);
                int length2 = a.GetLength(1);
                int length3 = 0;
                int length4 = 0;
                try
                {
                    length3 = length;
                   length4 = length2 / 4;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error");
                }

                Vector128<int>[,] A = new Vector128<int>[length3,length4];
                
                int c = 0;
              
                
                fixed (int* ptr = a)
                {
                    fixed (int* ptr2 = b)
                    {
                        for (int i = 0; i < length3; i++)
                        {

                            for (int j = 0; j < length4; j++)
                            {
                                var v = Sse41.LoadVector128(ptr + c);
                                var v2 = Sse41.LoadVector128(ptr2 + c);
                                c += 4;
                                A[i, j] = Sse41.MultiplyLow(v, v2);
                            }
                        }
                    }
                }
                return A;
        
            }
        }

        public static Vector128<int>[,] Procedure(int[,] A, int[,] B, int a, Vector128<int> x, Vector128<int> y)
        {
            unsafe
            {
                int length = A.GetLength(0);
                int length2 = B.GetLength(1);
                int length3 = 0;
                int length4 = 0;
                try
                {
                    length3 = length;
                    length4 = length2 / 4;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error");
                }
                Vector128<int>[,] MatrixA = new Vector128<int>[length3, length4];
                Vector128<int>[,] MatrixB = new Vector128<int>[length3, length4];
                Vector128<int>[,] Matrix = new Vector128<int>[length3, length4];

                int c = 0;


                fixed (int* ptr = A)
                {
                    fixed (int* ptr2 = B)
                    {
                        for (int i = 0; i < length3; i++)
                        {

                            for (int j = 0; j < length4; j++)
                            {
                                var v = Sse41.LoadVector128(ptr + c);
                                var v2 = Sse41.LoadVector128(ptr2 + c);
                                c += 4;
                                MatrixA[i, j] = Sse41.MultiplyLow(v, x);
                                MatrixA[i, j] = Sse41.MultiplyLow(MatrixA[i,j], Vector128.Create(a));
                                MatrixB[i, j] = Sse41.MultiplyLow(v2, y);
                                Matrix[i, j] = Sse41.Add(MatrixA[i, j], MatrixB[i, j]);
                            }
                        }
                        
                    }


                }
            
                return Matrix;

            }
        }

    }
}

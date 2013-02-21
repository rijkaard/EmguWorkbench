using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Emgu;
using Emgu.CV;
using System.Runtime.InteropServices;
using System.Windows.Media.Media3D;

namespace EmguWorkbench
{
    class EmguController
    {
        public MemStorage storage = null;
        public EmguController()
        {
            storage = new MemStorage();
        }

        public void testCvSolve1()
        {
            Vector3D[] planeVecs = new Vector3D[3];
            planeVecs[0] = new Vector3D(.0, .0, .0);
            planeVecs[1] = new Vector3D(.0, 1.0, .0);
            planeVecs[2] = new Vector3D(1.0, .0, .0);

            Matrix<float> A = new Matrix<float>(4, 3);
            A.SetZero();
            Matrix<float> x = new Matrix<float>(3, 1);
            x.SetZero();
            Matrix<float> b = new Matrix<float>(4, 1);
            //b.SetValue(1.0);
            b.SetZero();

            for (int i = 0; i < planeVecs.Length; ++i)
            {
                Vector3D v = planeVecs[i];
                A[0, 0] += (float)(v.X * v.X);
                A[0, 1] += (float)(v.X * v.Y);
                A[0, 2] += (float)(v.X * v.Z);
                A[1, 1] += (float)(v.Y * v.Y);
                A[1, 2] += (float)(v.Y * v.Z);
                A[2, 2] += (float)(v.Z * v.Z);

                b[0, 0] += (float)v.X;
                b[1, 0] += (float)v.Y;
                b[2, 0] += (float)v.Z;
                b[3, 0] += (float)1.0;
            }
            A[1, 0] = A[0, 1];
            A[2, 0] = A[0, 2];
            A[2, 1] = A[1, 2];

            A[3, 0] = b[0, 0];
            A[3, 1] = b[1, 0];
            A[3, 2] = b[2, 0];

            Emgu.CV.CvInvoke.cvSolve(A, b, x, Emgu.CV.CvEnum.SOLVE_METHOD.CV_SVD);
            Vector3D bestNormal = new Vector3D();
            bestNormal.X = x[0, 0];
            bestNormal.Y = x[1, 0];
            bestNormal.Z = x[2, 0];

        }
        public void testCvSolve2()
        {
            Vector3D[] planeVecs = new Vector3D[3];
            planeVecs[0] = new Vector3D(.0, .0, .0);
            planeVecs[1] = new Vector3D(.0, 1.0, .0);
            planeVecs[2] = new Vector3D(1.0, .0, .0);

            Matrix<float> A = new Matrix<float>(3, 3);
            A.SetZero();
            Matrix<float> x = new Matrix<float>(3, 1);
            x.SetZero();
            Matrix<float> b = new Matrix<float>(3, 1);
            //b.SetValue(1.0);
            b.SetZero();

            for (int i = 0; i < planeVecs.Length; ++i)
            {
                Vector3D v = planeVecs[i];
                A[0, 0] += (float)(v.X * v.X);
                A[0, 1] += (float)(v.X * v.Y);
                A[0, 2] += (float)(v.X);
                A[1, 1] += (float)(v.Y * v.Y);
                A[1, 2] += (float)(v.Y);
                A[2, 2] += (float)(1.0);

                b[0, 0] += (float)(v.X * v.Z);
                b[1, 0] += (float)(v.Y * v.Z);
                b[2, 0] += (float)v.Z;
            }
            A[1, 0] = A[0, 1];
            A[2, 0] = A[0, 2];
            A[2, 1] = A[1, 2];

            Emgu.CV.CvInvoke.cvSolve(A, b, x, Emgu.CV.CvEnum.SOLVE_METHOD.CV_SVD);
            Vector3D bestNormal = new Vector3D();
            bestNormal.X = x[0, 0];
            bestNormal.Y = x[1, 0];
            bestNormal.Z = x[2, 0];

        }

        public void testSVD()
        {
            Vector3D[] planeVecs = new Vector3D[3];
            planeVecs[0] = new Vector3D(.0, .0, 2.0);
            planeVecs[1] = new Vector3D(.0, 2.0, .0);
            planeVecs[2] = new Vector3D(2.0, .0, .0);

            Matrix<float> A = new Matrix<float>(4, 4);
            A.SetZero();
            Matrix<float> x = new Matrix<float>(4, 1);
            x.SetZero();
            Matrix<float> U = new Matrix<float>(4, 4);
            //b.SetValue(1.0);
            U.SetZero(); 
            Matrix<float> V = new Matrix<float>(4, 4);
            //b.SetValue(1.0);
            U.SetZero();

            for (int i = 0; i < planeVecs.Length; ++i)
            {
                Vector3D v = planeVecs[i];
                A[0, 0] += (float)(v.X * v.X);
                A[0, 1] += (float)(v.X * v.Y);
                A[0, 2] += (float)(v.X * v.Z);
                A[1, 1] += (float)(v.Y * v.Y);
                A[1, 2] += (float)(v.Y * v.Z);
                A[2, 2] += (float)(v.Z * v.Z);

                A[0, 3] -= (float)(v.X);
                A[1, 3] -= (float)(v.Y);
                A[2, 3] -= (float)(v.Z);
                A[3, 3] -= (float)1.0;

            }
            A[1, 0] = A[0, 1];
            A[2, 0] = A[0, 2];
            A[2, 1] = A[1, 2];

            A[3, 0] = -A[0, 3];
            A[3, 1] = -A[1, 3];
            A[3, 2] = -A[2, 3];

            CvInvoke.cvSVD(A, x, U, V, Emgu.CV.CvEnum.SVD_TYPE.CV_SVD_DEFAULT );
        }
        public void testEigen()
        {
            Vector3D[] planeVecs = new Vector3D[3];
            planeVecs[0] = new Vector3D(.0, .0, 1.0);
            planeVecs[1] = new Vector3D(.0, 1.0, .0);
            planeVecs[2] = new Vector3D(.0, .0, .0);

            Matrix<float> A = new Matrix<float>(4, 4);
            A.SetZero();
            Matrix<float> x = new Matrix<float>(4, 1);
            x.SetZero();
            Matrix<float> ev = new Matrix<float>(4, 4);
            //b.SetValue(1.0);
            ev.SetZero();

            for (int i = 0; i < planeVecs.Length; ++i)
            {
                Vector3D v = planeVecs[i];
                A[0, 0] += (float)(v.X * v.X);
                A[0, 1] += (float)(v.X * v.Y);
                A[0, 2] += (float)(v.X * v.Z);
                A[1, 1] += (float)(v.Y * v.Y);
                A[1, 2] += (float)(v.Y * v.Z);
                A[2, 2] += (float)(v.Z * v.Z);

                A[0, 3] -= (float)(v.X);
                A[1, 3] -= (float)(v.Y);
                A[2, 3] -= (float)(v.Z);
                A[3, 3] -= (float)1.0;

            }
            A[1, 0] = A[0, 1];
            A[2, 0] = A[0, 2];
            A[2, 1] = A[1, 2];

            A[3, 0] = - A[0, 3];
            A[3, 1] = - A[1, 3];
            A[3, 2] = - A[2, 3];

            CvInvoke.cvEigenVV(A, ev, x, 1e-4, -1, -1);
        }
        public void testPolygonTest()
        {
            // CvArray<float> m = new CvArray<float>(); // = new CvArray<float>;
            //Matrix<PointF> m = new Matrix<PointF>(1,3);
            //m[0,0] = new PointF(.0f, .0f);
            //m[0,1] = new PointF(1.0f, .0f);
            //m[0,2] = new PointF(.0f, 1.0f);
            // Matrix<float> m = new Matrix<float>(3, 2);
            //float[] m = new float[6];
            //m[0] = .0f; m[1] = .0f;
            //m[2] = 1.0f; m[3] = .0f;
            //m[4] = .0f; m[5] = 1.0f;
            //List<PointF> m = new List<PointF>(3);
            //m[0] = new PointF(.0f, .0f);
            //m[0] = new PointF(1.0f, .0f);
            //m[0] = new PointF(.0f, 1.0f);
            // IntPtr m = CvInvoke.cvCreateMat(3,2,Emgu.CV.CvEnum.MAT_DEPTH.CV_32F);
            //CvInvoke.cvSet2D(m, 0, 0, (Emgu.CV.Structure.MCvScalar).0f);
            //CvInvoke.cvSet2D(m, 0, 1, .0f);
            //CvInvoke.cvSet2D(m, 0, 0, .0f);
            //unsafe
            //{
            //    float* p = (float*)m.ToPointer();
            //    p[0] = .0f; p[1] = .0f;
            //    p[2] = 1.0f; p[3] = .0f;
            //    p[4] = .0f; p[5] = 1.0f;
            //}
            //PointF[] m = new PointF[3];
            //m[0] = new PointF(.0f, .0f);
            //m[1] = new PointF(1.0f, .0f);
            //m[2] = new PointF(.0f, 1.0f);

            // Matrix<PointF> m = new Matrix<PointF>(3,1);
            //Matrix<float> m = new Matrix<float>(3, 2, 2);
            //m[0, 0] = .0f; m[0, 1] = .0f;
            //m[1, 0] = 1.0f; m[1, 1] = .0f;
            //m[2, 0] = .0f; m[2, 1] = 1.0f;
            //PointF p0 = new PointF(.0f, .0f);
            //PointF p1 = new PointF(1.0f, .0f);
            //PointF p2 = new PointF(.0f, 1.0f);
            //PointF[] m = new PointF[3];
            //m[0] = p0;
            //m[1] = p1;
            //m[2] = p2;
            //Matrix<PointF> m = new Matrix<PointF>(3, 1, 4);
            //Matrix<PointF> m = new Matrix<PointF>(3, 1);
            //m[0, 0] = new PointF(.0f, .0f);
            //m[1, 0] = new PointF(1.0f, .0f);
            //m[2, 0] = new PointF(.0f, 1.0f);
            //Contour<PointF> m = new Contour<PointF>(storage);
            //Contour<PointF> m = new Contour<PointF>(storage);
            Seq<PointF> m = new Seq<PointF>(storage);
            m.Push(new PointF(.0f, .0f));
            m.Push(new PointF(1.0f, .0f));
            m.Push(new PointF(.0f, 1.0f));

            //double asd = CvInvoke.cvPointPolygonTest(Marshal.UnsafeAddrOfPinnedArrayElement(m, 0), new PointF(.2f, .3f), false);
            double asd = CvInvoke.cvPointPolygonTest((IntPtr)(m), new PointF(.2f, .3f), false);
           
        }
        void printMatrix(Matrix<float> m)
        {
            for (int i = 0; i < m.Rows; ++i)
            {
                string s;
                s = (m[i,0]).ToString();
                for (int j = 1; j < m.Cols; ++j)
                {
                    s = s + ",\t" + (m[i,j]).ToString();
                }
                //s = s + "\n" ;
                System.Diagnostics.Debug.Print(s);
            }
        }
    }

}

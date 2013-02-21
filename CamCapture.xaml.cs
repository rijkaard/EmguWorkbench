using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Emgu;
using Emgu.CV;
using System.Runtime.InteropServices;
using System.Windows.Media.Media3D;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Windows.Interop;
using System.Drawing.Imaging;

namespace EmguWorkbench
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CamCaptureWindow : Window
    {
        private Capture camera = null;
        private bool captureInProgress = false;
        private BitmapImage ImageSrc = null;
        private MemoryStream stream = null;
        public CamCaptureWindow()
        {
            InitializeComponent();
        }
        private void CamCapture_Loaded(object sender, RoutedEventArgs e)
        {
            camera = new Capture();
            //camera.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, );
            //camera.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, );
            if (camera != null)
            {
                if (captureInProgress)
                {  //if camera is getting frames then stop the capture and set button Text
                    // "Start" for resuming capture
                    //Application.Idle -= ProcessFrame;
                    ComponentDispatcher.ThreadIdle -= ProcessFrame;
                }
                else
                {
                    //if camera is NOT getting frames then start the capture and set button
                    // Text to "Stop" for pausing capture
                    ComponentDispatcher.ThreadIdle += ProcessFrame;
                }

                captureInProgress = !captureInProgress;
            }

            ImageSrc = new BitmapImage();
            ImageBox.Source = ImageSrc;

            stream = new MemoryStream();
        }
        public void testCamera()
        {
        }
        private void ProcessFrame(object sender, EventArgs arg)
        {
            if (null == camera)
                return;
            using (Image<Bgr, Byte> ImageFrame = camera.QueryFrame())  //line 1
            {
                //stream = new MemoryStream();
                if(null != ImageFrame)
                {
                    Image<Gray, Byte> ImageGrey = ImageFrame.Convert<Gray, Byte>().PyrDown().PyrUp();
                    Image<Gray, float> grad_x = new Image<Gray, float>(ImageGrey.Size);
                    Image<Gray, float> grad_y = new Image<Gray, float>(ImageGrey.Size);
                    Image<Gray, Byte> grad_x_abs = new Image<Gray, Byte>(ImageGrey.Size);
                    Image<Gray, Byte> grad_y_abs = new Image<Gray, Byte>(ImageGrey.Size);
                    Image<Gray, Byte> grad = new Image<Gray, Byte>(ImageGrey.Size);
                    //System.Drawing.Bitmap pixels = ImageFrame.ToBitmap();
                    //System.Drawing.Bitmap pixels;
                    //Emgu.CV.Matrix<Int32> pixels, grey_pixels;
                    //var pixels = new Emgu.CV.Image<Rgba, Int32>(ImageFrame.ToBitmap());
                    //var pixels_grey = new Emgu.CV.Image<Gray, Int32>(ImageFrame.ToBitmap());
                    //Matrix<Int32> grad_x;
                    //Matrix<Int32> grad_y;
                    //Emgu.CV.CvInvoke.cvCvtColor(pixels, pixels_grey, COLOR_CONVERSION.CV_RGB2GRAY);
                    //Emgu.CV.CvInvoke.cvSobel(pixels_grey, (IntPtr)(grad_x), 1, 0, 3);
                    //Emgu.CV.CvInvoke.cvSobel(pixels_grey, grad_y, 0, 1, 3);
                    //CvInvoke.cvCanny(ImageGrey, ImageGrey, 10, 60, 3);
                    CvInvoke.cvSobel(ImageGrey, grad_x, 1, 0, 3);
                    CvInvoke.cvConvertScaleAbs(grad_x, grad_x_abs, 1, 0);
                    CvInvoke.cvSobel(ImageGrey, grad_y, 0, 1, 3);
                    CvInvoke.cvConvertScaleAbs(grad_y, grad_y_abs, 1, 0);
                    CvInvoke.cvAddWeighted(grad_x_abs, 0.5, grad_y_abs, 0.5, 0, grad);
                    double mn = .0,mx = .0;
                    var mnpnt = new System.Drawing.Point();
                    var mxpnt = new System.Drawing.Point();
                    CvInvoke.cvMinMaxLoc(grad, ref mn, ref mx, ref mnpnt, ref mxpnt, IntPtr.Zero);
                    //CvInvoke.cvThreshold(grad, grad, mx - (mx - mn)/1.2 , 0, THRESH.CV_THRESH_TOZERO);
                    grad = grad.SmoothGaussian(17);
                    CvInvoke.cvAdaptiveThreshold(grad, grad, mx, ADAPTIVE_THRESHOLD_TYPE.CV_ADAPTIVE_THRESH_MEAN_C, THRESH.CV_THRESH_BINARY, 3, mn);
                    //Image<Gray, byte> grad_x = ImageGrey.Sobel(1, 0, 3);
                    //Image<Gray, byte> grad_y = ImageGrey.Sobel(0, 1, 3);

                    //ImageFrame.Bitmap.Save(stream, ImageFormat.Bmp);
                    //ImageGrey.Bitmap.Save(stream, ImageFormat.Bmp);
                    grad.Bitmap.Save(stream, ImageFormat.Bmp);

                    ImageSrc = new BitmapImage();
                    ImageSrc.BeginInit();
                    ImageSrc.StreamSource = new MemoryStream( stream.ToArray() );        //line 2
                    ImageSrc.EndInit();

                    ImageBox.Source = ImageSrc;
                }
            }
        }

        private void CamCapture_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != camera)
                camera.Dispose();
            if (null != stream)
                stream.Dispose();
        }
    }
}

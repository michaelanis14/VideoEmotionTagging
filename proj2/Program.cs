using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proj2
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string inputfile="";
            Affdex.VideoDetector detector = new Affdex.VideoDetector(30, 1, Affdex.FaceDetectorMode.LARGE_FACES);
            Form1 f1 = new Form1();
            ProcessVideo feed = new ProcessVideo(detector);
            System.Console.WriteLine("ranunaaa");
            detector.setClassifierPath("C:\\Program Files\\Affectiva\\AffdexSDK\\data");
            detector.setDetectAllEmotions(true);
            detector.setDetectAllEmojis(true);
            detector.setDetectAllExpressions(true);
            detector.setDetectAge(true);
            detector.setDetectGender(true);
            detector.setDetectGlasses(true);
            detector.setDetectLipCornerDepressor(false);
            detector.setDetectLipPress(false);
            detector.setDetectLipPucker(false);
            detector.setDetectLipStretch(false);
            detector.setDetectLipSuck(false);
            detector.setDetectMouthOpen(false);
            detector.setDetectSmile(false);
            detector.setDetectSmirk(false);
            detector.setDetectUpperLipRaise(false);
            System.Console.WriteLine("all functions on");
            Application.Run(f1);
            using (Form1 form2 = new Form1())
            {
                    inputfile = Form1.getFilename();
                    Console.WriteLine("hey from program");
                    Console.WriteLine(inputfile);
            }
            detector.start();
            System.Console.WriteLine("detector started");
            //System.Diagnostics.Process.Start("C:\\Users\\Rana\\Desktop\\video\\test.avi");
            //var dir = new DirectoryInfo(@"C:\Users\Rana\Desktop\video\enterface database");
            //foreach (var file in dir.EnumerateFiles("*.avi", SearchOption.AllDirectories))
            {
                // dynamic filename = file.FullName;
               
                detector.process(inputfile);
                Application.Run(feed);
                //feed.ShowDialog();
               // System.Console.WriteLine("detector reset");
                detector.reset();
            }
            System.Console.WriteLine("detector stopped");
            detector.stop();
          
        }

        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Threading;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;
using Affdex;
using System.Diagnostics;

namespace proj2
{

    public partial class ProcessVideo : Form, Affdex.ImageListener, Affdex.ProcessStatusListener
    {

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string command, StringBuilder retstring, int ReturnLength, IntPtr callBack);
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource = null;
        private static long count = 0;
        private string prefixSad = "save recsound C:\\Users\\Rana\\Desktop\\data\\Sadness\\";
        private string prefixJoy = "save recsound C:\\Users\\Rana\\Desktop\\data\\Joy\\";
        private string prefixAnger = "save recsound C:\\Users\\Rana\\Desktop\\data\\Anger\\";
        private string prefixDisgust = "save recsound C:\\Users\\Rana\\Desktop\\data\\Disgust\\";
        private string prefixSurprise = "save recsound C:\\Users\\Rana\\Desktop\\data\\Surprise\\";
        private string prefixFear = "save recsound C:\\Users\\Rana\\Desktop\\data\\Fear\\";
        private string path;
        private string filepath = @"C:\Users\Rana\Desktop\video\test.mp3";
        private static string inputfile = "C:\\Users\\Rana\\Desktop\\video\\s23_fe_5.avi";
        private ArrayList f = new ArrayList();
        private ArrayList a = new ArrayList();
        private ArrayList su = new ArrayList();
        private ArrayList sa = new ArrayList();
        private ArrayList h = new ArrayList();
        private ArrayList d = new ArrayList();
        private static Form1 form1 = new Form1();
        private string result2 = "The Facial classification shows that the dominant emotion is";
        private string[] result3=new string [5];
        private string[] result4;
        private Boolean audio = false;
        private int facialResult;
        private int audioResult;
        private string facialEmotion;
        private string audioEmotion;
        //private Boolean[] bools;

        //average values
        private int fv;
        private int av;
        private int dv;
        private int hv;
        private int sav;
        private int suv;

        public ProcessVideo(Affdex.VideoDetector detector)
        {

            using (Form1 form2 = new Form1())
            {
                form2.Visible = false;
                if (form2.ShowDialog() == DialogResult.Cancel)
                {
                    form2.Dispose();
                    form2.Close();
                    inputfile = Form1.getFilename();
                    Console.WriteLine(inputfile);
                }
            }

            //Thread camThread = new  Thread(camStart);
            //camThread.Start();
            detector.setImageListener(this);
            detector.setProcessStatusListener(this);
            InitializeComponent();
            // label2.Text = generateSentence();
            //mciSendString("open new Type waveaudio alias recsound", null, 0, IntPtr.Zero);
            //button1.Click += new EventHandler(this.button1_Click_1);
            /*var dir = new DirectoryInfo(@"C:\Users\Rana\Desktop\video\enterface database");
            foreach (var file in dir.EnumerateFiles("*.avi", SearchOption.AllDirectories))
            {
                inputfile = file.FullName;
                changed = !changed;
            }*/
            label11.Visible = false;
        }
        public void camStart()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            CloseVideoSource();
            videoSource.Start();
        }
        public void onImageCapture(Affdex.Frame frame)
        {
            DrawCapturedImage(frame);
            frame.Dispose();
        }
        public void onImageResults(Dictionary<int, Affdex.Face> faces, Affdex.Frame frame)
        {
            //if (ready)
            {
                System.Console.WriteLine("faces count");
                System.Console.WriteLine(faces.Count);

                //inputfile = detector.getName();

                foreach (KeyValuePair<int, Affdex.Face> pair in faces)
                {


                    Affdex.Face face = pair.Value;

                    //adding values to the arraylist
                    f.Add(face.Emotions.Fear);
                    a.Add(face.Emotions.Anger);
                    h.Add(face.Emotions.Joy);
                    d.Add(face.Emotions.Disgust);
                    sa.Add(face.Emotions.Sadness);
                    su.Add(face.Emotions.Surprise);


                    float[] emo = new float[6];
                    emo[0] = face.Emotions.Fear;
                    emo[1] = face.Emotions.Anger;
                    emo[2] = face.Emotions.Surprise;
                    emo[3] = face.Emotions.Joy;
                    emo[4] = face.Emotions.Sadness;
                    emo[5] = face.Emotions.Disgust;

                    progressBar1.Value = (int)face.Emotions.Anger;
                    progressBar2.Value = (int)face.Emotions.Fear;
                    progressBar4.Value = (int)face.Emotions.Surprise;
                    progressBar5.Value = (int)face.Emotions.Joy;
                    progressBar6.Value = (int)face.Emotions.Sadness;
                    progressBar7.Value = (int)face.Emotions.Disgust;



                    float engagement = face.Emotions.Engagement;

                    float dominantEmotion = emo.Max();
                    int index = emo.ToList().IndexOf(dominantEmotion);
                    if ((index == 0) && (emo[index] > 10))
                    {
                        filepath = @"C:\Users\Rana\Desktop\data\Fear\" + inputfile.Substring(28, 8) + ".mp3";
                        pictureBox2.Image = Image.FromFile("C:\\Users\\Rana\\Documents\\Visual Studio 2015\\Projects\\proj4 - Copy\\proj2\\fear.png");
                        label3.Text = "Afraid";
                    }
                    else
                    {
                        if ((index == 1) && (emo[index] > 10))
                        {
                            filepath = @"C:\Users\Rana\Desktop\data\Anger\" + inputfile.Substring(28, 8) + ".mp3";
                            pictureBox2.Image = Image.FromFile("C:\\Users\\Rana\\Documents\\Visual Studio 2015\\Projects\\proj4 - Copy\\proj2\\angry.png");
                            label3.Text = "Angry";
                        }
                        else
                        {
                            if ((index == 2) && (emo[index] > 10))
                            {

                                System.Console.WriteLine(inputfile.Substring(28, 4));
                                filepath = @"C:\Users\Rana\Desktop\data\Surprise\" + inputfile.Substring(28, 8) + ".mp3";
                                pictureBox2.Image = Image.FromFile("C:\\Users\\Rana\\Documents\\Visual Studio 2015\\Projects\\proj4 - Copy\\proj2\\surprise.png");
                                label3.Text = "Surprised";
                            }
                            else
                            {
                                if ((index == 3) && (emo[index] > 10))
                                {
                                    filepath = @"C:\Users\Rana\Desktop\data\Joy\" + inputfile.Substring(28, 8) + ".mp3";
                                    pictureBox2.Image = Image.FromFile("C:\\Users\\Rana\\Documents\\Visual Studio 2015\\Projects\\proj4 - Copy\\proj2\\happy.png");
                                    label3.Text = "Happy";
                                }
                                else
                                {
                                    if ((index == 4) && (emo[index] > 10))
                                    {
                                        filepath = @"C:\Users\Rana\Desktop\data\Sadness\" + inputfile.Substring(28, 8) + ".mp3";
                                        pictureBox2.Image = Image.FromFile("C:\\Users\\Rana\\Documents\\Visual Studio 2015\\Projects\\proj4 - Copy\\proj2\\sad.png");
                                        label3.Text = "Sad";
                                    }
                                    else
                                    {
                                        if ((index == 5) && (emo[index] > 10))
                                        {
                                            filepath = @"C:\Users\Rana\Desktop\data\Disgust\" + inputfile.Substring(28, 8) + ".mp3";
                                            pictureBox2.Image = Image.FromFile("C:\\Users\\Rana\\Documents\\Visual Studio 2015\\Projects\\proj4 - Copy\\proj2\\disgust.png");
                                            label3.Text = "Disgusted";
                                        }
                                        else
                                        {
                                            System.Console.WriteLine(inputfile);
                                            filepath = @"C:\Users\Rana\Desktop\data\Neutral\" + inputfile.Substring(28, 8) + ".mp3";
                                            pictureBox2.Image = Image.FromFile("C:\\Users\\Rana\\Documents\\Visual Studio 2015\\Projects\\proj4 - Copy\\proj2\\neutral.png");
                                            label3.Text = "Neutral";
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (faces != null)
                    {
                        foreach (PropertyInfo prop in typeof(Affdex.Emotions).GetProperties())
                        {

                            float value = (float)prop.GetValue(face.Emotions, null);
                            string output = string.Format("{0}:{1:N2}", prop.Name, value);
                            System.Console.WriteLine(output);
                        }
                    }
                }
                frame.Dispose();
            }
        }

        /// <summary>
        /// Draws the image captured from the camera.
        /// </summary>
        /// <param name="image">The image captured.</param>
        private void DrawCapturedImage(Affdex.Frame image)
        {
            // Update the Image control from the UI thread

            try
            {
                // Update the Image control from the UI thread
                //cameraDisplay.Source = rtb;
                pictureBox1.Image = BitmapFromSource(ConstructImage(image.getBGRByteArray(), image.getWidth(), image.getHeight()));

                // Allow N successive OnCapture callbacks before the FacePoint drawing canvas gets cleared.


                if (image != null)
                {
                    image.Dispose();
                }
            }
            catch (Exception ex)
            {
                String message = String.IsNullOrEmpty(ex.Message) ? "AffdexMe error encountered." : ex.Message;
                // ShowExceptionAndShutDown(message);
            }

        }

        Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())

            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }

        private BitmapSource ConstructImage(byte[] imageData, int width, int height)
        {
            try
            {
                if (imageData != null && imageData.Length > 0)
                {
                    var stride = (width * PixelFormats.Bgr24.BitsPerPixel + 7) / 8;
                    var imageSrc = BitmapSource.Create(width, height, 96d, 96d, PixelFormats.Bgr24, null, imageData, stride);
                    return imageSrc;
                }
            }
            catch (Exception ex)
            {
                String message = String.IsNullOrEmpty(ex.Message) ? "AffdexMe error encountered." : ex.Message;
                //   ShowExceptionAndShutDown(message);
            }

            return null;
        }
        private void button1_Click(object sender, EventArgs e)
        {

            mciSendString("record recsound", null, 0, IntPtr.Zero);
            //button2.Click += new EventHandler(this.button2_Click);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (label3.Text == "Happy")
            {
                path = prefixJoy + "JOY_" + count + ".wav";
                mciSendString(path, null, 0, IntPtr.Zero);
                mciSendString("close recsound", null, 0, IntPtr.Zero);
            }
            else
            {
                if (label3.Text == "Angry")
                {

                    path = prefixAnger + "ANGER_" + count + ".wav";
                    mciSendString(path, null, 0, IntPtr.Zero);
                    mciSendString("close recsound", null, 0, IntPtr.Zero);
                }
                else
                {
                    if (label3.Text == "Sad")
                    {
                        path = prefixSad + "SAD_" + count + ".wav";
                        mciSendString(path, null, 0, IntPtr.Zero);
                        mciSendString("close recsound", null, 0, IntPtr.Zero);
                    }
                    else
                    {
                        if (label3.Text == "Afraid")
                        {

                            path = prefixFear + "FEAR_" + count + ".wav";
                            mciSendString(path, null, 0, IntPtr.Zero);
                            mciSendString("close recsound", null, 0, IntPtr.Zero);
                        }
                        else
                        {
                            if (label3.Text == "Disgusted")
                            {

                                path = prefixDisgust + "Disgust_" + count + ".wav";
                                mciSendString(path, null, 0, IntPtr.Zero);
                                mciSendString("close recsound", null, 0, IntPtr.Zero);
                            }
                            else
                            {
                                if (label3.Text == "Surprised")
                                {

                                    path = prefixSurprise + "SURPRISE_" + count + ".wav";
                                    mciSendString(path, null, 0, IntPtr.Zero);
                                    mciSendString("close recsound", null, 0, IntPtr.Zero);
                                }
                            }
                        }
                    }
                }
            }

            mciSendString("open new Type waveaudio alias recsound", null, 0, IntPtr.Zero);
            //button1.Click += new EventHandler(this.button1_Click);
            count++;
        }

        public string generateSentence()
        {
            Random rnd = new Random();
            int emotion = rnd.Next(0, 5);
            int sent = rnd.Next(0, 4);
            //label11.MaximumSize = new Size(200, 280);
            //label11.AutoSize = true;

            if (emotion == 0)
            {
                //label11.Text = "“You are in a foreign city. A city that contains only one bank, which is open today until 4pm.You need to get 200$ from the bank, in order to buy a flight ticket to go home.You absolutely need your money today. There is no ATM cash machine and you don’t know anyone else in the city. You arrive at the bank at 3pm and see a big queue. After 45 minutes of queuing, when you finally arrive at the counter, the employee tells you to come back the day after because he wants to have a coffee before leaving the bank. You tell him that you need the money today and that the bank should be open for 15 more minutes, but he is just repeating that he does not care about anything else than his coffee...”";
                string[] anger = new string[30];
                anger[0] = "What??? No, no, no, listen! I need this money!";
                anger[1] = "I don't care about your coffee! Please serve me!";
                anger[2] = "I can have you fired you know!";
                anger[3] = "Is your coffee more important than my money?";
                anger[4] = "You're getting paid to work, not drink coffee!";

                return anger[sent];
            }
            else
            {
                if (emotion == 1)
                {
                    //label11.Text = "“You are in a restaurant. You are already a bit sick and the restaurant looks quite dirty, but it is the only restaurant in the village, so you don’t really have the choice...When you finally receive your plate, which is a sort of noodle soup, you take your spoon, ready to eat.Although you are very hungry, the soup does not taste very good.It seems that it is not very fresh...Suddenly you see a huge cockroach swimming in your plate!You’re first surprised and you jump back out of your chair.Then, you look again at your plate, really disgusted. ”";
                    string[] disgust = new string[30];
                    disgust[0] = "That's horrible! I'll never eat noodles again.";
                    disgust[1] = "Something is moving inside my plate!";
                    disgust[2] = "Aaaaah a cockroach!!!";
                    disgust[3] = "Eeeek, this is disgusting!!!";
                    disgust[4] = "That's gross!";

                    return disgust[sent];
                }
                else
                {
                    if (emotion == 2)
                    {
                        //label11.Text = "“You are alone in your bedroom at night, in your bed. You cannot sleep because you are nervous.Your bedroom is located on the second floor of your house. You are the only person living there.Suddenly, you start hearing some noise downstairs. You go on listening and realize that there is definitely someone in the house, probably a thief...or maybe even a murderer!He’s now climbing up the stairs, you are really scared.”";
                        string[] fear = new string[30];
                        fear[0] = "Oh my god, there is someone in the house!";
                        fear[1] = "Someone is climbing up the stairs";
                        fear[2] = "Please don't kill me...";
                        fear[3] = "I'm not alone! Go away!";
                        fear[4] = "I have nothing to give you! Please don't hurt me!";

                        return fear[sent];
                    }
                    else
                    {
                        if (emotion == 3)
                        {
                            //label11.Text = "“You learned this morning that you won the big prize of 5.000.000€ at the lottery!You’re in a very happy mood of course, because you realize that some of your dreams will now become true!After the surprise to learn that you have won, comes the happy state of mind when you start dreaming about your new projects.You are in a restaurant, inviting your friends for a good meal, and telling them how happy you feel.”";
                            string[] joy = new string[30];
                            joy[0] = "That's great, I'm rich now!!!";
                            joy[1] = "I won: this is great!I’m so happy!!";
                            joy[2] = "I'm so lucky!";
                            joy[3] = "I'm so excited!";
                            joy[4] = "Wahoo...This is so great.";

                            return joy[sent];
                        }
                        else
                        {

                            if (emotion == 4)
                            {
                                //label11.Text = "“You just came back from an exhausting day at work. You are in a neutral state of mind when suddenly the telephone rings. You take the phone call and realize that it is your boy(girl) friend.He(she) announces you that he (she)doesn’t want to go on the relationship with you.You first don’t believe it, but after a while you start realizing what just happened.When you think about all the good moments you spent with your boy (girl) friend, and associate these memories with the fact that the relationship just finished, you start feeling really sad”";
                                string[] sadness = new string[30];
                                sadness[0] = "Life won't be the same now";
                                sadness[1] = "Oh no, tell me this is not true, please!";
                                sadness[2] = "Everything was so perfect! I just don't understand!";
                                sadness[3] = "I still loved him (her)";
                                sadness[4] = "He (she) was my life";

                                return sadness[sent];
                            }
                            else
                            {
                                if (emotion == 5)
                                {
                                    //label11.Text = "“Your best friend invites you for a drink after your day at work. You join him on the Grand Place of Mons2 , for a beer. Then, he suddenly tells you that he robbed a bank!You are very surprised about it, you really didn’t expect that!”";
                                    string[] surprise = new string[30];
                                    surprise[0] = "You have never told me that!";
                                    surprise[1] = " I didn't expect that!";
                                    surprise[2] = "Wahoo, I would never have believed this!";
                                    surprise[3] = "I never saw that coming!";
                                    surprise[4] = "Oh my God, that’s so weird!";

                                    return surprise[sent];

                                }
                                else
                                {
                                    return "";
                                }
                            }
                        }
                    }
                }
            }
        }


        private void frm_menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap img = (Bitmap)eventArgs.Frame.Clone();
            //do processing here
            pictureBox1.Image = img;
        }

        //close the device safely
        private void CloseVideoSource()
        {
            if (!(videoSource == null))
                if (videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource = null;
                }
        }

        public void onProcessingException(AffdexException ex)
        {
            throw new NotImplementedException();
        }

        public void onProcessingFinished()
        {
            {

                //Get Arraylists average
                Console.WriteLine(f);
                fv = calculateAverage(f);
                av = calculateAverage(a);
                dv = calculateAverage(d);
                hv = calculateAverage(h);
                suv = calculateAverage(su);
                sav = calculateAverage(sa);

                Console.WriteLine(fv - av);
                Console.WriteLine(fv - hv);
                Console.WriteLine(fv - dv);
                Console.WriteLine(fv - sav);
                Console.WriteLine(fv - suv);

                Console.WriteLine(av);
                Console.WriteLine(hv);
                Console.WriteLine(dv);
                Console.WriteLine(sav);
                Console.WriteLine(suv);
                Console.WriteLine(fv);


                float[] emo1 = new float[6];
                emo1[0] = fv;
                emo1[1] = av;
                emo1[2] = dv;
                emo1[3] = hv;
                emo1[4] = suv;
                emo1[5] = sav;
                float dominantEmotion1 = emo1.Max();
                int index1 = emo1.ToList().IndexOf(dominantEmotion1);

                if ((index1 == 0) && (emo1[index1] != 0))
                {
                    result2 += " Fear with accuracy of " + fv + " %";

                    if ((fv - av <= 20) || (fv - hv <= 20) || (fv - dv <= 20) || (fv - sav <= 20) || (fv - suv <= 20))
                    {
                        Boolean[] bools = new Boolean[5];
                        bools[0] = fv - av <= 20;
                        bools[1] = fv - hv <= 20;
                        bools[2] = fv - dv <= 20;
                        bools[3] = fv - sav <= 20;
                        bools[4] = fv - suv <= 20;

                        result3[0] = "av";
                        result3[1] = "hv";
                        result3[2] = "dv";
                        result3[3] = "sav";
                        result3[4] = "suv";

                        MessageBox.Show(overlapping(bools,"fv",result3));
                        label11.Visible = true;
                        label11.Refresh();
                        audio = true;
                        filepath = @"C:\Users\Rana\Desktop\avgdata\audio-classification\" + inputfile.Substring(28, 8) + ".mp3";
                        convertToMP3(filepath);
                        File.WriteAllText("C:\\Users\\Rana\\Documents\\Visual Studio 2015\\Projects\\new vid\\filepath.txt", filepath);
                        run_cmd("C:\\Python27\\python.exe", "C:\\Users\\Rana\\Downloads\\ali.py  filepath");
                    }
                    else
                    {
                        label1.Text = "FEAR";
                        filepath = @"C:\Users\Rana\Desktop\avgdata\Fear\" + inputfile.Substring(28, 8) + ".mp3";
                        label2.Text = result2;
                    }
                }
                else
                {
                    if ((index1 == 1) && (emo1[index1] != 0))
                    {
                        result2 += " Anger with accuracy of " + av + " %";

                        if ((av - fv <= 20) || (av - hv <= 20) || (av - dv <= 20) || (av - sav <= 20) || (av - suv <= 20))
                        {
                            Boolean[] bools = new Boolean[5];
                            bools[0] = av - fv <= 20;
                            bools[1] = av - hv <= 20;
                            bools[2] = av - dv <= 20;
                            bools[3] = av - sav <= 20;
                            bools[4] = av - suv <= 20;

                            result3[0] = "fv";
                            result3[1] = "hv";
                            result3[2] = "dv";
                            result3[3] = "sav";
                            result3[4] = "suv";

                            MessageBox.Show(overlapping(bools, "av", result3));
                            label11.Visible = true;
                            label11.Refresh();
                            audio = true;
                            filepath = @"C:\Users\Rana\Desktop\avgdata\audio-classification\" + inputfile.Substring(28, 8) + ".mp3";
                            convertToMP3(filepath);
                            File.WriteAllText("C:\\Users\\Rana\\Documents\\Visual Studio 2015\\Projects\\new vid\\filepath.txt", filepath);
                            run_cmd("C:\\Python27\\python.exe", "C:\\Users\\Rana\\Downloads\\ali.py  filepath");
                        }
                        else
                        {
                            label1.Text = "ANGER";
                            filepath = @"C:\Users\Rana\Desktop\avgdata\Anger\" + inputfile.Substring(28, 8) + ".mp3";
                            label2.Text = result2;
                        }
                    }
                    else
                    {
                        if ((index1 == 2) && (emo1[index1] != 0))
                        {
                            result2 += " Disgust with accuracy of " + dv + " %";

                            if ((dv - av <= 20) || (dv - hv <= 20) || (dv - fv <= 20) || (dv - sav <= 20) || (dv - suv <= 20))
                            {
                                Boolean[] bools = new Boolean[5];
                                bools[0] = dv - av <= 20;
                                bools[1] = dv - hv <= 20;
                                bools[2] = dv - fv <= 20;
                                bools[3] = dv - sav <= 20;
                                bools[4] = dv - suv <= 20;

                                result3[0] = "av";
                                result3[1] = "hv";
                                result3[2] = "fv";
                                result3[3] = "sav";
                                result3[4] = "suv";

                                MessageBox.Show(overlapping(bools, "dv", result3));


                                label11.Visible = true;
                                label11.Refresh();
                                audio = true;
                                filepath = @"C:\Users\Rana\Desktop\avgdata\audio-classification\" + inputfile.Substring(28, 8) + ".mp3";
                                convertToMP3(filepath);
                                File.WriteAllText("C:\\Users\\Rana\\Documents\\Visual Studio 2015\\Projects\\new vid\\filepath.txt", filepath);
                                run_cmd("C:\\Python27\\python.exe", "C:\\Users\\Rana\\Downloads\\ali.py  filepath");
                            }
                            else
                            {
                                label1.Text = "DISGUST";
                                filepath = @"C:\Users\Rana\Desktop\avgdata\Disgust\" + inputfile.Substring(28, 8) + ".mp3";
                                label2.Text = result2;
                            }
                        }
                        else
                        {
                            if ((index1 == 3) && (emo1[index1] != 0))
                            {
                                result2 += " Happy with accuracy of " + hv + " %";

                                if ((hv - av <= 20) || (hv - fv <= 20) || (hv - dv <= 20) || (hv - sav <= 20) || (hv - suv <= 20))
                                {
                                    Boolean[] bools = new Boolean[5];
                                    bools[0] = hv - av <= 20;
                                    bools[1] = hv - fv <= 20;
                                    bools[2] = hv - dv <= 20;
                                    bools[3] = hv - sav <= 20;
                                    bools[4] = hv - suv <= 20;

                                    result3[0] = "av";
                                    result3[1] = "fv";
                                    result3[2] = "dv";
                                    result3[3] = "sav";
                                    result3[4] = "suv";

                                    MessageBox.Show(overlapping(bools, "hv", result3));
                                    label11.Visible = true;
                                    label11.Refresh();
                                    audio = true;
                                    filepath = @"C:\Users\Rana\Desktop\avgdata\audio-classification\" + inputfile.Substring(28, 8) + ".mp3";
                                    convertToMP3(filepath);
                                    File.WriteAllText("C:\\Users\\Rana\\Documents\\Visual Studio 2015\\Projects\\new vid\\filepath.txt", filepath);
                                    run_cmd("C:\\Python27\\python.exe", "C:\\Users\\Rana\\Downloads\\ali.py  filepath");
                                }
                                else
                                {
                                    label1.Text = "HAPPY";
                                    filepath = @"C:\Users\Rana\Desktop\avgdata\Joy\" + inputfile.Substring(28, 8) + ".mp3";
                                    label2.Text = result2;
                                }
                            }
                            else
                            {
                                if ((index1 == 4) && (emo1[index1] != 0))
                                {
                                    result2 += " Surprise with accuracy of " + suv + " %";

                                    if ((suv - av <= 20) || (suv - hv <= 20) || (suv - dv <= 20) || (suv - sav <= 20) || (suv - fv <= 20))
                                    {
                                        Boolean[] bools = new Boolean[5];
                                        bools[0] = suv - av <= 20;
                                        bools[1] = suv - hv <= 20;
                                        bools[2] = suv - dv <= 20;
                                        bools[3] = suv - sav <= 20;
                                        bools[4] = suv - fv <= 20;

                                        result3[0] = "av";
                                        result3[1] = "hv";
                                        result3[2] = "dv";
                                        result3[3] = "sav";
                                        result3[4] = "fv";

                                        MessageBox.Show(overlapping(bools, "suv", result3));
                                        label11.Visible = true;
                                        label11.Refresh();
                                        audio = true;
                                        filepath = @"C:\Users\Rana\Desktop\avgdata\audio-classification\" + inputfile.Substring(28, 8) + ".mp3";
                                        convertToMP3(filepath);
                                        File.WriteAllText("C:\\Users\\Rana\\Documents\\Visual Studio 2015\\Projects\\new vid\\filepath.txt", filepath);
                                        run_cmd("C:\\Python27\\python.exe", "C:\\Users\\Rana\\Downloads\\ali.py  filepath");
                                    }
                                    else
                                    {
                                        label1.Text = "SURPRISE";
                                        filepath = @"C:\Users\Rana\Desktop\avgdata\Surprise\" + inputfile.Substring(28, 8) + ".mp3";
                                        label2.Text = result2;
                                    }
                                }
                                else
                                {
                                    if ((index1 == 5) && (emo1[index1] != 0))
                                    {
                                        result2 += " Sad with accuracy of " + sav + " %";
                                        if ((sav - av <= 20) || (sav - hv <= 20) || (sav - dv <= 20) || (sav - fv <= 20) || (sav - suv <= 20))
                                        {
                                            Boolean[] bools=new Boolean[5];
                                            bools[0] = sav - av <= 20;
                                            bools[1] = sav - hv <= 20;
                                            bools[2] = sav - dv <= 20;
                                            bools[3] = sav - fv <= 20;
                                            bools[4] = sav - suv <= 20;

                                            result3[0] = "av";
                                            result3[1] = "hv";
                                            result3[2] = "dv";
                                            result3[3] = "fv";
                                            result3[4] = "suv";

                                            MessageBox.Show(overlapping(bools, "sav", result3));
                                            label11.Visible = true;
                                            label11.Refresh();
                                            audio = true;
                                            filepath = @"C:\Users\Rana\Desktop\avgdata\audio-classification\" + inputfile.Substring(28, 8) + ".mp3";
                                            convertToMP3(filepath);
                                            File.WriteAllText("C:\\Users\\Rana\\Documents\\Visual Studio 2015\\Projects\\new vid\\filepath.txt", filepath);
                                            run_cmd("C:\\Python27\\python.exe", "C:\\Users\\Rana\\Downloads\\ali.py filepath");
                                        }
                                        else
                                        {
                                            label1.Text = "SAD";
                                            filepath = @"C:\Users\Rana\Desktop\avgdata\Sadness\" + inputfile.Substring(28, 8) + ".mp3";
                                            label2.Text = result2;
                                        }
                                    }
                                    else
                                    {
                                        result2 += "Neutral";
                                        label1.Text = "Neutral";
                                        filepath = @"C:\Users\Rana\Desktop\avgdata\Neutral\" + inputfile.Substring(28, 8) + ".mp3";
                                        label2.Text = result2;

                                    }
                                }
                            }
                        }
                    }
                }

                //Clear Arraylistsq
                f.Clear();
                a.Clear();
                d.Clear();
                su.Clear();
                sa.Clear();
                h.Clear();

                System.Console.WriteLine("done");
                System.Console.WriteLine(filepath);

            }


            if (audio)
            {
                Console.WriteLine("working");
                result4 = result2.Split(new char[0]);
                facialEmotion = result4[9];
                facialResult = Int32.Parse(result4[13]);
                result4 = label2.Text.Split(new char[0]);
                audioEmotion = result4[9];
                audioResult = Int32.Parse(result4[13]);

                if (audioResult > facialResult)
                {
                    saveAsPerEmo(audioEmotion);
                }
                else
                {
                    saveAsPerEmo(facialEmotion);
                }

            }
            else
            {
                convertToMP3(filepath);
                MessageBox.Show("saved to " + filepath);
            }
            File.WriteAllText("C:\\Users\\Rana\\Documents\\Visual Studio 2015\\Projects\\new vid\\filepath.txt", String.Empty);
        }

        public void convertToMP3(string outpath)
        {
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            ffMpeg.ConvertMedia(inputfile, outpath, "mp3");
        }

        public int calculateAverage(ArrayList y)
        {
            float sum = 0;
            for (int i = 0; i < y.Count; i++)
            {
                sum += (float)y[i];
            }
            return (int)sum / y.Count;
        }

        private void run_cmd(string cmd, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = cmd;//cmd is full path to python.exe
            start.Arguments = args;//args is path to .py file and any cmd line args
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.WriteLine("i'm the result");
                    Console.Write(result);
                    label2.Text = result;
                    label12.Text = result2;
                }
            }
        }

        private void saveAsPerEmo(String emo)
        {
            if (emo == "Sad")
            {
                filepath = @"C:\Users\Rana\Desktop\avgdata\Sadness\" + inputfile.Substring(28, 8) + ".mp3";

            }
            else
            {
                if (emo == "Surprise")
                {
                    filepath = @"C:\Users\Rana\Desktop\avgdata\Surprise\" + inputfile.Substring(28, 8) + ".mp3";
                }
                else
                {
                    if (emo == "Happy")
                    {
                        filepath = @"C:\Users\Rana\Desktop\avgdata\Joy\" + inputfile.Substring(28, 8) + ".mp3";
                    }
                    else
                    {
                        if (emo == "Disgust")
                        {
                            filepath = @"C:\Users\Rana\Desktop\avgdata\Disgust\" + inputfile.Substring(28, 8) + ".mp3";
                        }
                        else
                        {
                            if (emo == "Anger")
                            {
                                filepath = @"C:\Users\Rana\Desktop\avgdata\Anger\" + inputfile.Substring(28, 8) + ".mp3";
                            }
                            else
                            {
                                if (emo == "Fear")
                                {
                                    filepath = @"C:\Users\Rana\Desktop\avgdata\Fear\" + inputfile.Substring(28, 8) + ".mp3";
                                }
                                else
                                {
                                    if ((emo == "Bored") || (emo == "Neutral"))
                                    {
                                        filepath = @"C:\Users\Rana\Desktop\avgdata\Neutral\" + inputfile.Substring(28, 8) + ".mp3";
                                    } else
                                    {
                                        Console.WriteLine("Emotion Error");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            convertToMP3(filepath);
            MessageBox.Show("saved to " + filepath);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("hi baby");
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                Console.WriteLine(openFileDialog1.ShowDialog());
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string strfilename = openFileDialog1.FileName;
                    MessageBox.Show(strfilename);
                    inputfile = strfilename;
                }
            }
            catch (Exception e1)
            { Console.Write("error baby"); }
        }

        private string overlapping(Boolean [] x,string main,string [] y)
        {
            int index=0;
            for(int i = 0; i < x.Length; ++i)
           {
                if (x[i])
                {
                    index = i;
                    break;
                }
            }
 
            return "Inaccuracy of interpretation between "+ getEmotion(main)+ " and "+ getEmotion(y[index]);
        }

        private string getEmotion(string x)
        {
            if(x=="av")
            return "Anger";
            else
            {

                if (x == "hv")
                return "Happy";
                else
                {

                    if (x == "dv")
                    return "Disgust";
                    else
                    {

                        if (x == "fv")
                        return "Fear";
                        else
                        {

                            if (x == "sav")
                            return "Sad";
                            else
                            {

                                if (x == "suv")
                                return "Surprise";
                                else
                                {
                                    return "";
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

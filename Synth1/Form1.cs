using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using System.Timers;
using System.Windows;
using Xamarin.Forms;

namespace Synth1
{
    public partial class Form1 : Form
    {

        private double amp = 0.01;
        private int sustain = 500;
        private DirectSoundOut output = null;
        private BlockAlignReductionStream stream = null;
        WaveTone tone;


        public Form1()
        {
            InitializeComponent();
        }
        

        private void GenerateSound(double freq, double amp)
        {
            if (output != null)
            {
                output.Stop();
            }

            if(nudAmplifier.Value == 2)
            {
                amp += 0.02;
            }
            else if(nudAmplifier.Value == 3)
            {
                amp += 0.1;
            }
            else
            {
                amp = 0.001;
            }

            if (nudPitch.Value == 2)
            {
               tone = new WaveTone(freq/2, amp);
            }
            else if(nudPitch.Value == 4)
            {
               tone = new WaveTone(freq*2, amp);
            }
            else if(nudPitch.Value == 5)
            {
               tone = new WaveTone(freq * 4, amp);
            }else
            {
                tone = new WaveTone(freq, amp);
            }

            stream = new BlockAlignReductionStream(tone);
            output = new DirectSoundOut();
            output.Init(stream);
            output.Play();

            Task.Delay(new TimeSpan(0, 0, 0, 0, sustain)).ContinueWith(o => { output.Stop(); });
            tone.Dispose();
        }






        private void btnC3_Click(object sender, EventArgs e)
        {
            GenerateSound(130.8128, amp);
        }
        private void btnCs3_Click(object sender, EventArgs e)
        {
            GenerateSound(138.5913, amp);
        }
        private void btnD3_Click(object sender, EventArgs e)
        {
            GenerateSound(146.8324, amp);
        }
        private void btnDs3_Click(object sender, EventArgs e)
        {
            GenerateSound(155.5635, amp);
        }
        private void btnE3_Click(object sender, EventArgs e)
        {
            GenerateSound(164.8138, amp);
        }
        private void btnF3_Click(object sender, EventArgs e)
        {
            GenerateSound(174.6141, amp);
        }
        private void btnFs3_Click(object sender, EventArgs e)
        {
            GenerateSound(184.9972, amp);
        }
        private void btnG3_Click(object sender, EventArgs e)
        {
            GenerateSound(195.9977, amp);
        }
        private void btnGs3_Click(object sender, EventArgs e)
        {
            GenerateSound(207.6523, amp);
        }
        private void btnA3_Click(object sender, EventArgs e)
        {
            GenerateSound(220.0, amp);
        }
        private void btnAs3_Click(object sender, EventArgs e)
        {
            GenerateSound(233.0819, amp);
        }
        private void btnB3_Click(object sender, EventArgs e)
        {
            GenerateSound(246.9417, amp);
        }
        private void btnC4_Click(object sender, EventArgs e)
        {
            GenerateSound(261.6256, amp);

        }

        private void nudSustain_ValueChanged(object sender, EventArgs e)
        {
            this.sustain = ((int)nudSustain.Value);
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (output != null)
            {
                output.Dispose();
                output = null;
            }
            if (stream != null)
            {
                stream.Dispose();
                stream = null;
            }
        }

        private void nudAmplifier_ValueChanged(object sender, EventArgs e)
        {
            
        }
    }


    public class WaveTone : WaveStream
    {
        private double frequency;
        private double amplitude;
        private double time;
        private int sampleRate = 44100;
        private int bitRate = 16;
        private int channel = 2;

        public WaveTone(double f, double a)
        {
            this.time = 0;
            this.frequency = f;
            this.amplitude = a;
        }
        ~WaveTone() { }
        public override long Position
        {
            get;
            set;
        }


        // Brojot na bajti od audioto vo stream-ot
        public override long Length
        {
            get { return long.MaxValue; }
        }

        public override WaveFormat WaveFormat
        {
            get { return new WaveFormat(sampleRate, bitRate, channel); }
        }



        //count e brojot na bajti sto go bara NAudio, baferot se polni so podatoci za da svirat
        public override int Read(byte[] buffer, int offset, int count)
        {
            int samples = count / 2;
            for (int i = 0; i < samples; i++)
            {
                
                double sine = amplitude * Math.Sin(Math.PI * 2 * frequency * time);
                time += 1.0 / 44100;

                short truncated = (short)Math.Round(sine * (Math.Pow(2, 15) - 1));
                buffer[i * 2] = (byte)(truncated & 0x00ff);
                buffer[i * 2 + 1] = (byte)((truncated & 0xff00) >> 8);
            }

            return count;
        }


    }
}

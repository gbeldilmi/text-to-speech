using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;

namespace TextToSpeech
{
    public partial class MainWindow : Window
    {
        private SpeechSynthesizer synthesizer;
        public MainWindow()
        {
            synthesizer = new SpeechSynthesizer();
            InitializeComponent();
            InitializeSynthesizer();
        }
        private void InitializeSynthesizer()
        {
            synthesizer.SetOutputToDefaultAudioDevice();
            xVoices.Items.Clear();
            foreach (InstalledVoice v in synthesizer.GetInstalledVoices().ToArray())
            {
                xVoices.Items.Add(v.VoiceInfo.Name);
            }
            xVoices.SelectedIndex = 0;
            Rate_ValueChanged(null, null);
            Volume_ValueChanged(null, null);
        }
        private void Rate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            synthesizer.Rate = (int)xRate.Value;
        }
        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            synthesizer.SpeakAsyncCancelAll();
            synthesizer.SpeakAsync(xText.Text);
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            FileStream fs = File.Create("rec" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".wav");
            SpeechSynthesizer fsynth = new SpeechSynthesizer();
            fsynth.SelectVoice(xVoices.SelectedItem.ToString());
            fsynth.Volume = (int)xVolume.Value;
            fsynth.Rate = (int)xRate.Value;
            fsynth.SetOutputToWaveStream(fs);
            fsynth.Speak(xText.Text);
            fsynth.Dispose();
            fs.Close();
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            synthesizer.SpeakAsyncCancelAll();
        }
        private void Voices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            synthesizer.SelectVoice(xVoices.SelectedItem.ToString());
        }
        private void Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            synthesizer.Volume = (int)xVolume.Value;
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            synthesizer.Dispose();
        }
    }
}

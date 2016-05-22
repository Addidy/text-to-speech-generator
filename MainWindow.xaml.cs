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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech;
using System.Speech.Synthesis;
using System.Diagnostics;


namespace TTS {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private SpeechSynthesizer speech;

        protected override void OnInitialized(EventArgs e) {
            base.OnInitialized(e);

            speech = new SpeechSynthesizer();
            speech.SpeakProgress += new EventHandler<SpeakProgressEventArgs>(speech_SpeakProgress);
        }

        private void speech_SpeakProgress(object sender, SpeakProgressEventArgs e) {
            txtText.Select(e.CharacterPosition, e.CharacterCount);
        }

        private void cmdSpeak_Click(object sender, RoutedEventArgs e) {
            // Start speech without blocking the UI.
            speech.SpeakAsync(txtText.Text);
        }

        private void cmdStop_Click(object sender, RoutedEventArgs e) {
            // Stop the speech
            speech.SpeakAsyncCancelAll();
        }

        private void txtText_LostFocus(object sender, RoutedEventArgs e) {
            e.Handled = true; // This will prevent selection from being hidden
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e) {
            // Direct speech to a file.
            speech.SetOutputToWaveFile("Speech.wav");
            try {
                // Generate speech
                speech.SpeakAsync(txtText.Text);
            } finally {
                speech.SetOutputToNull();
            }

            // Now let's play it...

            Process.Start("Speech.wav");
        }
    }
}

using Microsoft.Win32;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace Client;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void SendImageButton_Click(object sender, RoutedEventArgs e)
    {
        string ip = IpTextBox.Text;
        int port = int.Parse(PortTextBox.Text);

        var openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

        if (openFileDialog.ShowDialog() == true)
        {
            string filePath = openFileDialog.FileName;
            var imageBytes = File.ReadAllBytes(filePath);

            try
            {
                using var client = new TcpClient(ip, port);
                using var networkStream = client.GetStream();

                networkStream.Write(imageBytes, 0, imageBytes.Length);
                MessageBox.Show("Image sent successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Mistake: {ex.Message}");
            }
        }
    }
}
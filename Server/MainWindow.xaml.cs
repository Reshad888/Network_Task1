using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Server
{
    public partial class MainWindow : Window
    {
        private TcpListener listener;
        private string serverIP = "192.168.1.151";
        private int port = 27001;

        public MainWindow()
        {
            InitializeComponent();

            StartServer();
        }

        private void StartServer()
        {
            IPAddress iPAddress = IPAddress.Parse(serverIP);

            listener = new TcpListener(iPAddress, port);
            listener.Start();

            ListenForClientsAsync();
        }

        private async Task ListenForClientsAsync()
        {
            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();

                await ReceiveImageAsync(client);
            }
        }

        private async Task ReceiveImageAsync(TcpClient client)
        {
            using var networkStream = client.GetStream();
            using var memoryStream = new MemoryStream();

            await networkStream.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();

            Dispatcher.Invoke(() => DisplayImage(imageBytes));
        }

        private void DisplayImage(byte[] imageBytes)
        {
            using var ms = new MemoryStream(imageBytes);

            var bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            image.Source = bitmapImage;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            listener?.Stop();
        }
    }
}
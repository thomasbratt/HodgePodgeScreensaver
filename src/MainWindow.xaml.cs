using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace HodgePodge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point? firstMousePosition = null;
        private HodgePodgeMachine hodgePodge;
        private int[] palette;
        private RenderToBitmap renderer;
        private DispatcherTimer timer;

        private readonly TimeSpan Interval = TimeSpan.FromMilliseconds(200);
        private readonly int numberOfStates;
        private const double K1 = 1.0;
        private const double K2 = 1.5;
        private const double G = 34.0;

        public MainWindow()
        {
            InitializeComponent();

            // 96 produces organic looking concentric rings.
            // 100 produces fairly round concentric circles.
            this.numberOfStates = new Random().Next(70, 101);

            this.palette = FalseColourPalette.Generate();
            // this.palette = MonochromeBluePalette.Generate();
            // this.palette = IceBluePalette.Generate();
            
            base.Loaded += new RoutedEventHandler(this.OnWindowLoaded);
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Display in normal sized window when debugging.
            if (Debugger.IsAttached)
            {
                this.ChangeWindowForDebugging();
            }

            int width = (int)base.Width;
            int height = (int)base.Height;

            this.renderer = new RenderToBitmap( width,
                                                height,
                                                numberOfStates,
                                                palette);

            this.hodgePodge = new HodgePodgeMachine(    width,
                                                        height,
                                                        K1,
                                                        K2,
                                                        G,
                                                        numberOfStates);

            // Draw the first frame.
            this.DisplayFrame(null, null);

            // Schedule the successive frames.
            this.timer = new DispatcherTimer();
            this.timer.Interval = Interval;
            this.timer.Tick += new EventHandler(this.DisplayFrame);
            this.timer.Start();
        }

        // Save a screenshot if a key is pressed.
        protected override void OnKeyDown(KeyEventArgs e)
        {
            var image = this.Display.Source as BitmapSource;

            string filename = GenerateScreenshotFilename();

            image.SaveAsPng(filename);

            base.OnKeyDown(e);            
        }

        // Close the window if the mouse is clicked.
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            base.Close();
        }

        // Close the window if the mouse is moved.
        // A mouse move event is raised at the start of the application, so
        // the first movement is ignored.
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            Point current = e.GetPosition(null);

            if (this.firstMousePosition == null)
            {
                this.firstMousePosition = current;
            }
            else if (this.firstMousePosition != current)
            {
                base.Close();
            }
        }

        private void ChangeWindowForDebugging()
        {
            base.Topmost = false;
            base.WindowState = WindowState.Normal;
            base.WindowStartupLocation = WindowStartupLocation.Manual;

            // Order of operations matters in this code.
            base.Height = 400;
            base.Width = 600;
            this.Left = (SystemParameters.WorkArea.Width - base.Width) / 2 + SystemParameters.WorkArea.Left;
            this.Top = (SystemParameters.WorkArea.Height - base.Height) / 2 + SystemParameters.WorkArea.Top;
        }

        private void DisplayFrame(object sender, EventArgs e)
        {
            // Update the hodge podge machine.
            int[] states = this.hodgePodge.Next();

            // Render the states to an image.
            BitmapSource image = this.renderer.Render(states);

            // Update the display with the image.
            this.Display.Source = image;
        }

        private static string GenerateScreenshotFilename()
        {
            string filename = "HodgePodge.png";

            // Insert timestamp into filename.
            filename = filename.Replace(".",
                                            DateTime.Now.ToString("s") + ".")
                               .Replace(":", "-");

            string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            filename = Path.Combine(desktopFolder, filename);

            return filename;
        }
    }
}

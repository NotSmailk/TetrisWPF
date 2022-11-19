using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TetrisWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] m_tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Sprites/Tiles/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/Tiles/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/Tiles/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/Tiles/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/Tiles/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/Tiles/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/Tiles/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/Tiles/TileRed.png", UriKind.Relative))
        };
        private readonly ImageSource[] m_blockImages = new ImageSource[]
        { 
            new BitmapImage(new Uri("Assets/Sprites/BlockImages/BLock-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/BlockImages/BLock-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/BlockImages/BLock-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/BlockImages/BLock-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/BlockImages/BLock-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/BlockImages/BLock-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/BlockImages/BLock-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Sprites/BlockImages/BLock-Z.png", UriKind.Relative))
        };
        private readonly Image[,] m_imageControls;
        private readonly int m_maxDelay = 1000;
        private readonly int m_minDelay = 75;
        private readonly int m_delayDecrease = 25;

        private GameState m_gameState = new GameState();

        public MainWindow()
        {
            InitializeComponent();

            m_imageControls = SetupGameCanvas(m_gameState.GameGrid);
        }

        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }

            return imageControls;
        }

        private void Pause()
        {
            if (!m_gameState.GameStarted || m_gameState.GameOver)
                return;

            m_gameState.Pause();

            GamePausedMenu.Visibility = m_gameState.GamePaused ? Visibility.Visible : Visibility.Hidden;
        }

        #region DrawMethods

        private void DrawGrid(GameGrid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    m_imageControls[r, c].Opacity = 1f;
                    m_imageControls[r, c].Source = m_tileImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePosition())
            {
                m_imageControls[p.Row, p.Column].Opacity = 1f;
                m_imageControls[p.Row, p.Column].Source = m_tileImages[block.Id];
            }
        }

        private void DrawNextBlock(BlockQueue blockQueue)
        {
            if (blockQueue == null)
                return;

            Block block = blockQueue.NextBlock!;
            NextImage.Source = m_blockImages[block.Id];
        }

        private void DrawHeldBlock(Block heldBlock)
        {
            HoldImage.Source = heldBlock == null ? m_blockImages[0] : m_blockImages[heldBlock.Id];
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = m_gameState.BlockDropDistance();

            foreach (Position p in block.TilePosition())
            {
                m_imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25f;
                m_imageControls[p.Row + dropDistance, p.Column].Source = m_tileImages[block.Id];
            }
        }

        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);

            DrawGhostBlock(m_gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);

            DrawNextBlock(m_gameState.BlockQueue);
            DrawHeldBlock(m_gameState.HeldBlock);

            ScoreText.Text = $"Score: {m_gameState.Score}";
        }

        #endregion DrawMethods

        private async Task GameLoop()
        {
            Draw(m_gameState);

            while (!m_gameState.GameOver)
            {
                int delay = Math.Max(m_minDelay, m_maxDelay - (m_gameState.Score * m_delayDecrease));
                await Task.Delay(delay);
                m_gameState.MoveBlockDown();
                Draw(m_gameState);
            }

            GameOverMenu.Visibility = Visibility.Visible;
            FinaleScoreText.Text = $"Score: {m_gameState.Score}";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    m_gameState.MoveBlockLeft();
                    break;

                case Key.Right:
                    m_gameState.MoveBlockRight();
                    break;

                case Key.Down:
                    m_gameState.MoveBlockDown();
                    break;

                case Key.Q:
                    m_gameState.RotateBlockCCW();
                    break;

                case Key.E:
                    m_gameState.RotateBlockCW();
                    break;

                case Key.H:
                    m_gameState.HoldBlock();
                    break;

                case Key.Space:
                    m_gameState.DropBlock();
                    break;

                case Key.P:
                    Pause();
                    break;

                case Key.Escape:
                    Close();
                    break;

                case Key.F11:
                    WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                    break;

                default:
                    return;
            }

            Draw(m_gameState);
        }

        private void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            m_gameState = new GameState();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            m_gameState = new GameState();

            m_gameState.GameStarted = true;

            GameOverMenu.Visibility = Visibility.Hidden;

            await GameLoop();
        }

        private async void Start_Click(object sender, RoutedEventArgs e)
        {
            m_gameState.GameStarted = true;

            StartMenu.Visibility = Visibility.Hidden;

            await GameLoop();
        }
    }
}

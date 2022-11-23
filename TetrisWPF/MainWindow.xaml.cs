using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TetrisWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Image[,] _imageControls;

        private GameState _gameState = new GameState();

        public MainWindow()
        {
            InitializeComponent();

            _imageControls = SetupGameCanvas(_gameState.GameGrid);
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
            if (!_gameState.GameStarted || _gameState.GameOver)
                return;

            _gameState.Pause();

            GamePausedMenu.Visibility = _gameState.GamePaused ? Visibility.Visible : Visibility.Hidden;
        }

        #region DrawMethods

        private void DrawGrid(GameGrid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    _imageControls[r, c].Opacity = 1f;
                    _imageControls[r, c].Source = GameData.TileImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePosition())
            {
                _imageControls[p.Row, p.Column].Opacity = 1f;
                _imageControls[p.Row, p.Column].Source = GameData.TileImages[block.Id];
            }
        }

        private void DrawNextBlock(BlockQueue blockQueue)
        {
            if (blockQueue == null)
                return;

            Block block = blockQueue.NextBlock!;
            NextImage.Source = GameData.BlockImages[block.Id];
        }

        private void DrawHeldBlock(Block heldBlock)
        {
            HoldImage.Source = heldBlock == null ? GameData.BlockImages[0] : GameData.BlockImages[heldBlock.Id];
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = _gameState.BlockDropDistance();

            foreach (Position p in block.TilePosition())
            {
                _imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25f;
                _imageControls[p.Row + dropDistance, p.Column].Source = GameData.TileImages[block.Id];
            }
        }

        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);

            DrawGhostBlock(_gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);

            DrawNextBlock(_gameState.BlockQueue);
            DrawHeldBlock(_gameState.HeldBlock);

            ScoreText.Text = $"Score: {_gameState.Score}";
        }

        #endregion DrawMethods

        private async Task GameLoop()
        {
            Draw(_gameState);

            while (!_gameState.GameOver)
            {
                int delay = Math.Max(GameData.MinDelay, GameData.MaxDelay - (_gameState.Score * GameData.DelayDecrease));
                await Task.Delay(delay);
                _gameState.MoveBlockDown();
                Draw(_gameState);
            }

            GameOverMenu.Visibility = Visibility.Visible;
            FinaleScoreText.Text = $"Score: {_gameState.Score}";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    _gameState.MoveBlockLeft();
                    break;

                case Key.Right:
                    _gameState.MoveBlockRight();
                    break;

                case Key.Down:
                    _gameState.MoveBlockDown();
                    break;

                case Key.Q:
                    _gameState.RotateBlockCCW();
                    break;

                case Key.E:
                    _gameState.RotateBlockCW();
                    break;

                case Key.H:
                    _gameState.HoldBlock();
                    break;

                case Key.Space:
                    _gameState.DropBlock();
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

            Draw(_gameState);
        }

        private void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            _gameState = new GameState();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            _gameState.GameStarted = true;

            GameOverMenu.Visibility = Visibility.Hidden;

            await GameLoop();
        }

        private async void Start_Click(object sender, RoutedEventArgs e)
        {
            _gameState = new GameState();

            _gameState.GameStarted = true;

            StartMenu.Visibility = Visibility.Hidden;

            await GameLoop();
        }
    }
}

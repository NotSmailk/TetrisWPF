namespace TetrisWPF
{
    public class GameState
    {
        private Block m_currentBlock;

        public Block CurrentBlock 
        {
            get => m_currentBlock!;

            private set
            {
                m_currentBlock = value;
                m_currentBlock.Reset();

                for (int i = 0; i < 2; i++)
                {
                    m_currentBlock.Move(1, 0);

                    if (!BlockFits())
                        m_currentBlock.Move(-1, 0);
                }
            }
        }
        public Block HeldBlock { get; private set; }
        public GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameStarted { get; set; }
        public bool GameOver { get; private set; }
        public bool GamePaused { get; private set; }
        public bool CanHold { get; private set; }
        public int Score { get; private set; }

        public GameState()
        {
            Score = 0;
            GameGrid = new GameGrid(22, 10);
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetAnyUpdate();
            GameStarted = false;
            GamePaused = false;
            CanHold = true;
        }

        #region Commands

        public void HoldBlock()
        {
            if (!CanHold || GameOver || GamePaused)
                return;

            if (HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = BlockQueue.GetAnyUpdate();
            }
            else
            {
                Block temp = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = temp;
            }

            CanHold = false;
        }

        public void RotateBlockCW()
        {
            if (GameOver || GamePaused)
                return;

            CurrentBlock.RotateCW();

            if (!BlockFits())
                CurrentBlock.RotateCCW();
        }

        public void RotateBlockCCW()
        {
            if (GameOver || GamePaused)
                return;

            CurrentBlock.RotateCCW();

            if (!BlockFits())
                CurrentBlock.RotateCW();
        }

        public void MoveBlockLeft()
        {
            if (GameOver || GamePaused)
                return;

            CurrentBlock.Move(0, -1);

            if (!BlockFits())
                CurrentBlock.Move(0, 1);
        }

        public void MoveBlockRight()
        {
            if (GameOver || GamePaused)
                return;

            CurrentBlock.Move(0, 1);

            if (!BlockFits())
                CurrentBlock.Move(0, -1);
        }

        public void MoveBlockDown()
        {
            if (GameOver || GamePaused)
                return;

            CurrentBlock.Move(1, 0);

            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }

        public void DropBlock()
        {
            if (GameOver || GamePaused)
                return;

            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }

        #endregion Commands

        private bool BlockFits()
        {
            foreach (Position p in CurrentBlock.TilePosition())
            {
                if (!GameGrid.IsEmpty(p.Row, p.Column))
                    return false;
            }

            return true;
        }

        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        private void PlaceBlock()
        {
            foreach (Position p in CurrentBlock.TilePosition())
            {
                GameGrid[p.Row, p.Column] = CurrentBlock.Id;
            }

            Score += GameGrid.ClearFullRows();

            if (IsGameOver())
            {
                GameOver = true;
                GameStarted = false;
            }
            else
            {
                CurrentBlock = BlockQueue.GetAnyUpdate();
                CanHold = true;
            }
        }

        private int TileDropInstance(Position p)
        {
            int drop = 0;

            while (GameGrid.IsEmpty(p.Row + drop + 1, p.Column))
                drop++;

            return drop;
        }

        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows;

            foreach (Position p in CurrentBlock.TilePosition())
                drop = System.Math.Min(drop, TileDropInstance(p));

            return drop;
        }

        public void Pause()
        {
            GamePaused = !GamePaused;
        }
    }
}

﻿namespace TetrisWPF
{
    public class GameGrid
    {
        private readonly int[,] m_grid;

        public int Rows { get; }
        public int Columns { get; }

        public int this[int r, int c]
        { 
            get => m_grid[r, c];
            set => m_grid[r, c] = value;
        }

        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            m_grid = new int[rows, columns];
        }

        public bool IsInside(int r, int c)
        { 
            return r >= 0 && r < Rows && c >= 0 && c < Columns;
        }

        public bool IsEmpty(int r, int c)
        {
            return IsInside(r, c) && m_grid[r, c] == 0;
        }

        public bool IsRowFull(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (m_grid[r, c] == 0)
                    return false;
            }

            return true;
        }

        public bool IsRowEmpty(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (m_grid[r, c] != 0)
                    return false;
            }

            return true;    
        }

        private void ClearRow(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                m_grid[r, c] = 0;
            }
        }

        private void MoveDownRow(int r, int numRows)
        {
            if (r + numRows > Rows)
                return;

            for (int c = 0; c < Columns; c++)
            {
                m_grid[r + numRows, c] = m_grid[r, c];
                m_grid[r, c] = 0;
            }
        }

        public int ClearFullRows()
        {
            int cleared = 0;

            for (int r = Rows - 1; r >= 0; r--)
            {
                if (IsRowFull(r))
                {
                    ClearRow(r);
                    cleared++;
                }
                else if (cleared > 0)
                {
                    MoveDownRow(r, cleared);
                }
            }

            return cleared;
        }
    }
}

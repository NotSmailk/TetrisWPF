using System.Collections.Generic;

namespace TetrisWPF
{
    public abstract class Block
    {
        protected abstract Position[][] Tiles { get; }
        protected abstract Position StartOffset { get; }
        public abstract int Id { get; }

        private int m_rotationState;
        private Position m_offset;

        public Block()
        { 
            m_offset = new Position(StartOffset.Row, StartOffset.Column);
        }

        public IEnumerable<Position> TilePosition()
        {
            foreach (Position p in Tiles[m_rotationState])
            {
                yield return new Position(p.Row + m_offset.Row, p.Column + m_offset.Column);
            }
        }

        public void RotateCW()
        { 
            m_rotationState = (m_rotationState + 1) % Tiles.Length;
        }

        public void RotateCCW()
        {
            m_rotationState = m_rotationState == 0 ? Tiles.Length - 1 : --m_rotationState;
        }

        public void Move(int rows, int columns)
        {
            m_offset.Row += rows;
            m_offset.Column += columns;
        }

        public void Reset()
        {
            m_rotationState = 0;
            m_offset.Row = StartOffset.Row;
            m_offset.Column = StartOffset.Column;
        }
    }
}

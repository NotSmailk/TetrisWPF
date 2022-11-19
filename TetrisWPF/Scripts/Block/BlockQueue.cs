using System;

namespace TetrisWPF
{
    public class BlockQueue
    {
        private readonly Block[] m_blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBLock(),
            new TBLock(),
            new ZBlock()
        };
        private readonly Random m_random = new Random();

        public Block? NextBlock { get; private set; }

        public BlockQueue()
        {
            NextBlock = RandomBlock();
        }

        private Block RandomBlock()
        {
            return m_blocks[m_random.Next(m_blocks.Length)];
        }

        public Block GetAnyUpdate()
        {
            Block? block = NextBlock;

            do NextBlock = RandomBlock();
            while (block!.Id == NextBlock.Id);

            return block;
        }
    }
}

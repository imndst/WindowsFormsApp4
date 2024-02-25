using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        private Blockchain blockchain;
        public Form1()
        {
            InitializeComponent();
            blockchain = new Blockchain(2);
            CreateDynamicButtons();
        }
        private void CreateDynamicButtons()
        {
            // Create button on the left side
            Button leftButton = new Button();
            leftButton.Text = "Left Button"; // Set button text
            leftButton.Location = new System.Drawing.Point(20, this.ClientSize.Height / 2 - leftButton.Height / 2); // Set button location on the left side
            leftButton.Click += DynamicButtonL_Click; // Attach click event handler
            this.Controls.Add(leftButton); // Add button to form

            // Create button on the right side
            Button rightButton = new Button();
            rightButton.Text = "Right Button"; // Set button text
            rightButton.Location = new System.Drawing.Point(this.ClientSize.Width - rightButton.Width - 20, this.ClientSize.Height / 2 - rightButton.Height / 2); // Set button location on the right side
            rightButton.Click += DynamicButtonR_Click; // Attach click event handler
            this.Controls.Add(rightButton); // Add button to form
        }


        private void DynamicButtonL_Click(object sender, EventArgs e)
        {
            string data = "imndst coin";
            blockchain.AddBlock(data);

            Block latestBlock = blockchain.GetLatestBlock();


            LogBlockInformation(latestBlock);
        }



        private void LogBlockInformation(Block block)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Block Index: {block.Index}");
            sb.AppendLine($"Timestamp: {block.Timestamp}");
            sb.AppendLine($"Data: {block.Data}");
            sb.AppendLine($"Previous Hash: {block.PreviousHash}");
            sb.AppendLine($"Hash: {block.Hash}");
            sb.AppendLine($"none: {block.Nonce}");
            sb.AppendLine($"dificlut: {block.Difficulty}");

            sb.AppendLine(); // Add an empty line for separation
            // Append the new block information to the existing text in the text area
            richTextBox1.AppendText(sb.ToString());
        }













        private void DynamicButtonR_Click(object sender, EventArgs e)
        {
            // Example event handler for dynamic buttons
            MessageBox.Show(" ROght Dynamic button clicked!");
        }




        public class Block
        {
            public int Index { get; }
            public DateTime Timestamp { get; }
            public string PreviousHash { get; }
            public string Data { get; }
            public string Hash { get; }
            public int Nonce { get; } // Nonce used in mining
            public int Difficulty { get; } // Difficulty level for mining

            public Block(int index, DateTime timestamp, string previousHash, string data, string hash, int nonce, int difficulty)
            {
                Index = index;
                Timestamp = timestamp;
                PreviousHash = previousHash;
                Data = data;
                Hash = hash;
                Nonce = nonce;
                Difficulty = difficulty;
            }
        }



        public class Blockchain
        {
            private List<Block> chain;
            private int difficulty; 

            public Blockchain(int difficulty)
            {
                chain = new List<Block>();
                // Create the genesis block
                this.difficulty = difficulty;
                CreateGenesisBlock();
            }

            // Create the genesis block
            private void CreateGenesisBlock()
            {
                AddBlock("Genesis Block");
            }

            // Add a new block to the chain
            public void AddBlock(string data)
            {
                int index = chain.Count;
                string previousHash = index > 0 ? chain[index - 1].Hash : null;
                DateTime timestamp = DateTime.Now;
                int nonce = MineBlock(index, timestamp, previousHash, data);
                string hash = CalculateHash(index, timestamp, previousHash, data, nonce);

                Block block = new Block(index, timestamp, previousHash, data, hash, nonce, difficulty);
                chain.Add(block);
            }
            private int MineBlock(int index, DateTime timestamp, string previousHash, string data)
            {
                int nonce = 0;
                string hash = CalculateHash(index, timestamp, previousHash, data, nonce);
                string difficultyPattern = new string('a', difficulty);
                while (!hash.StartsWith(difficultyPattern))
                {
                    nonce++;
                    hash = CalculateHash(index, timestamp, previousHash, data, nonce);
                }
                return nonce;
            }

            private bool IsHashValid(string hash)
            {
                // In a real blockchain implementation, you would check if the hash meets the difficulty requirement
                // For simplicity, this method always returns true in this example
                return true;
            }

                // Calculate the hash of a block
                private string CalculateHash(int index, DateTime timestamp, string previousHash, string data, int nonce)
            {
                string rawData = $"{index}-{timestamp}-{previousHash ?? ""}-{data}-{nonce}-{difficulty}";
                // In a real blockchain implementation, you would use a hashing algorithm (e.g., SHA256) to calculate the hash
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            }

            // Get the latest block in the chain
            // Get the latest block in the chain
            public Block GetLatestBlock()
            {
                if (chain.Count > 0)
                {
                    return chain[chain.Count - 1];
                }
                else
                {
                    // Return null or throw an exception, depending on your requirements
                    return null;
                }
            }

        }




    }
}

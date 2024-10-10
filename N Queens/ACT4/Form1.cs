using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace ACT4
{
    public partial class Form1 : Form
    {
        
        int side; 
        int n = 6; 
        SixState startState; 
        SixState currentState; 
        int moveCounter; 
        List<SixState> beam; 
        private readonly Random random = new Random(); 
        int beamWidth = 3; 

        public Form1()
        {
            InitializeComponent();
            side = pictureBox1.Width / n;
            startState = randomSixState();
            currentState = new SixState(startState);
            beam = new List<SixState> { new SixState(startState) };
            updateUI();
            label1.Text = "Attacking pairs: " + currentState.GetAttackingPairs();
        }

        
        private void updateUI()
        {
            pictureBox2.Refresh();
            label3.Text = "Attacking pairs: " + currentState.GetAttackingPairs();
            label4.Text = "Moves: " + moveCounter;

                
            listBox1.Items.Clear();
            foreach (var state in beam)
            {
                listBox1.Items.Add(state.ToString());
            }

            label2.Text = "Current State: " + currentState.ToString();
        }

        
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            DrawBoard(e, startState, Brushes.Blue);
        }

        
        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            DrawBoard(e, currentState, Brushes.Black);
        }

        
        private void DrawBoard(PaintEventArgs e, SixState state, Brush cellBrush)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        e.Graphics.FillRectangle(cellBrush, i * side, j * side, side, side);
                    }
                    if (j == state.Y[i])
                    {
                        e.Graphics.FillEllipse(Brushes.Fuchsia, i * side, j * side, side, side);
                    }
                }
            }
        }

        
        private SixState randomSixState()
        {
            return new SixState(n, random);
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        // Resets the board to a new random state
        private void button3_Click(object sender, EventArgs e)
        {
            startState = randomSixState();
            currentState = new SixState(startState);
            beam = new List<SixState> { new SixState(startState) };
            moveCounter = 0;
            updateUI();
            pictureBox1.Refresh();
            label1.Text = "Attacking pairs: " + currentState.GetAttackingPairs();
        }

        
        private async void button4_Click(object sender, EventArgs e)
        {
            
            beamWidth = (int)numericBeamWidth.Value;

            
            moveCounter = 0;
            startState = randomSixState();
            currentState = new SixState(startState);
            beam = new List<SixState> { new SixState(startState) };
            updateUI();
            pictureBox1.Refresh();
            label1.Text = "Attacking pairs: " + currentState.GetAttackingPairs();

            
            await BeamSearch(beamWidth);
        }
        
        // Implementation of BeamSearch
        private async Task BeamSearch(int beamWidth, int maxIterations = 1000)
        {
            beam = new List<SixState> { new SixState(startState) };
            moveCounter = 0;
            updateUI();

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                List<SixState> allNeighbors = new List<SixState>();

               
                foreach (var state in beam)
                {
                    allNeighbors.AddRange(state.GenerateNeighbors(random));
                }

                
                allNeighbors = allNeighbors
                    .GroupBy(s => string.Join(",", s.Y))
                    .Select(g => g.First())
                    .ToList();

                
                var sortedNeighbors = allNeighbors
                    .OrderBy(s => s.GetAttackingPairs())
                    .ToList();

               
                beam = sortedNeighbors
                    .Take(beamWidth)
                    .ToList();

                
                currentState = beam[0];
                moveCounter++;
                updateUI();
                await Task.Delay(500); 

                
                if (currentState.GetAttackingPairs() == 0)
                {
                    MessageBox.Show($"Solution found in {moveCounter} iterations!");
                    return;
                }

            }

            MessageBox.Show("Beam Search completed without finding a solution.");
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
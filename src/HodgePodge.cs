using System;

namespace HodgePodge
{
    public class HodgePodgeMachine
    {
        private int width;
        private int height;

        private int[] statesCurrent;
        private int[] statesNext;
        private int numberOfStates;
        private double k1;
        private double k2;
        private double g;

        public HodgePodgeMachine(int width, int height, double k1, double k2, double g, int numberOfStates)
        {
            this.width = width;
            this.height = height;
            this.k1 = k1;
            this.k2 = k2;
            this.g = g;
            this.numberOfStates = numberOfStates;
            this.statesCurrent = new int[width * height];
            this.statesNext = new int[width * height];

            this.IntialiseStates();
        }

        public int[] Next()
        {
            this.UpdateStates();

            this.SwapStateBuffers();

            return this.statesCurrent;
        }

        private void IntialiseStates()
        {
            var random = new Random();

            // Don't seed edges to prevent fixed border states from dominating.
            for (int y = 1; y < height - 1; ++y)
            {
                for (int x = 1; x < width - 1; ++x)
                {
                    statesCurrent[x + y * width] = random.Next(numberOfStates);
                }
            }
        }

        private void UpdateStates()
        {
            for (int y = 1; y < height-1; ++y)
            {
                for (int x = 1; x < width-1; ++x)
                {
                    int current = statesCurrent[x + y * width];
                    int numInfected = 0;
                    int numIll = 0;
                    int sum = current;

                    var neighbours = new int[]
                    {
                        statesCurrent[(x+0) + (y-1) * width],
                        statesCurrent[(x-1) + (y+0) * width],
                        statesCurrent[(x+1) + (y+0) * width],
                        statesCurrent[(x+0) + (y+1) * width],
                    };

                    foreach (int cell in neighbours)
                    {
                        numIll      += cell == (numberOfStates - 1) ? 1 : 0;
                        numInfected += cell != 0 ? 1 : 0;
                        sum         += cell;
                    }
                    
                    double nextState;

                    // Healthy.
                    if (current == 0)
                    {
                        nextState = Math.Floor(numInfected/k1) + Math.Floor(numIll/k2);
                    }
                    // Ill.
                    else if (current == numberOfStates - 1)
                    {
                        nextState = 0.0;
                    }
                    // Infected.
                    else
                    {
                        nextState = Math.Floor(sum / (numInfected + 1.0)) + g;  // Copied from C implementation. Not in book!
                    }

                    // Ensure state is valid.
                    nextState = Math.Min(nextState, numberOfStates - 1);

                    // Update state.
                    statesNext[x + y * width] = (int)nextState;
                }
            }
        }

        private void SwapStateBuffers()
        {
            int[] temp = this.statesCurrent;
            this.statesCurrent = this.statesNext;
            this.statesNext = temp;
        }
    }
}

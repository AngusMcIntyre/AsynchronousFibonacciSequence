namespace ThreadingExample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Generates elements of the fibonacci sequence.
    /// </summary>
    public class FibonacciSequence
    {
        /// <summary>
        /// Notifies that a sequence element has been generated.
        /// </summary>
        public event EventHandler<StepEventArgs> OnStepAdvance;

        /// <summary>
        /// Calculates the sequence to a specified length.
        /// </summary>
        /// <param name="length">The length to calculate.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the work.</param>
        /// <returns>
        /// Sequence results.
        /// </returns>
        public async Task CalculateAsync(int length, System.Threading.CancellationToken cancellationToken)
        {
            List<int> results = new List<int>();

            for (int loopVariable = 0; loopVariable < length; loopVariable++)
            {
                int index = loopVariable;

                // create a continuation to run asynchronously (probably the thread pool)
                await Task.Run(
                    () =>
                    {
                        // check the cancellation token to see if cancellation is required
                        cancellationToken.ThrowIfCancellationRequested();

                        // the first two elements are not calculated
                        if (index < 2)
                        {
                            results.Add(index);
                        }
                        else
                        {
                            // the new element is (i-1 + i-2)
                            results.Add(results[results.Count - 1] + results[results.Count - 2]);
                        }

                        if (results.Last() % 2 == 0)
                        {
                            //throw new InvalidOperationException("Test Exception");
                        }

                        System.Threading.Thread.Sleep(500);

                        // check the cancellation token to see if cancellation is required
                        cancellationToken.ThrowIfCancellationRequested();
                    });

                var handler = this.OnStepAdvance;
                if (handler != null)
                {
                    handler(this, new StepEventArgs(results.Last()));
                }
            }
        }
    }
}

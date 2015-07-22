using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadingExample
{
    public class StepEventArgs
    {
        public int StepResult { get; set; }

        public StepEventArgs(int stepResult)
        {
            this.StepResult = stepResult;
        }
    }
}

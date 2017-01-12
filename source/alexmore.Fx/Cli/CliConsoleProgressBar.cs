#region License
/******************************************************************************
DanielSWolf/Program.cs
https://gist.github.com/DanielSWolf/0ab6a96899cc5377bf54
Console progress bar. Code is under the MIT License: http://opensource.org/licenses/MIT
******************************************************************************/
#endregion

using System;
using System.Text;
using System.Threading;

namespace alexmore.Fx.Cli
{
    public class CliConsoleProgressBar : ICliOutputProgressBar
    {
        private const int blockCount = 20;

        private readonly TimeSpan animationInterval = TimeSpan.FromSeconds(1.0 / 8);
        private const string animation = @"|/-\";

        private readonly Timer timer;

        private readonly int maxValue;
        private int currentValue = 0;
        private string currentText = string.Empty;

        private bool disposed = false;
        private int animationIndex = 0;


        public CliConsoleProgressBar(int maxValue)
        {
            this.maxValue = maxValue;
            timer = new Timer(TimerHandler, null, 0, animationInterval.Milliseconds);

            if (!Console.IsOutputRedirected) ResetTimer();
        }

        public void SetValue(int value)
        {
            value = Math.Max(0, Math.Min(maxValue, value));
            Interlocked.Exchange(ref currentValue, value);

        }

        public void Increase(int count = 1)
        {
            SetValue(currentValue + count);
        }

        private void TimerHandler(object state)
        {
            lock (timer)
            {
                if (disposed) return;

                int percent = currentValue * 100 / maxValue;
                int progressBlockCount = (int)(((double)percent / 100) * blockCount);

                string text = string.Format(" {3} [{0}{1}] {2,3}%",
                    new string('#', progressBlockCount), new string('-', blockCount - progressBlockCount),
                    percent,
                    animation[animationIndex++ % animation.Length]);

                UpdateText(text);

                ResetTimer();
            }
        }

        private void UpdateText(string text)
        {
            // Get length of common portion
            int commonPrefixLength = 0;
            int commonLength = Math.Min(currentText.Length, text.Length);
            while (commonPrefixLength < commonLength && text[commonPrefixLength] == currentText[commonPrefixLength])
            {
                commonPrefixLength++;
            }

            // Backtrack to the first differing character
            StringBuilder outputBuilder = new StringBuilder();
            outputBuilder.Append('\b', currentText.Length - commonPrefixLength);

            // Output new suffix            
            outputBuilder.Append(text.Substring(commonPrefixLength));

            // If the new text is shorter than the old one: delete overlapping characters
            int overlapCount = currentText.Length - text.Length;
            if (overlapCount > 0)
            {
                outputBuilder.Append(' ', overlapCount);
                outputBuilder.Append('\b', overlapCount);
            }

            Console.Write(outputBuilder);
            currentText = text;
        }

        private void ResetTimer()
        {
            timer.Change(animationInterval, TimeSpan.FromMilliseconds(-1));
        }

        public void Dispose()
        {
            lock (timer)
            {
                disposed = true;
                UpdateText(string.Empty);
            }
        }
    }
}

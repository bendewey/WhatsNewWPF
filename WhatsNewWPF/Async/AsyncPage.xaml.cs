using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WhatsNewWPF.Async
{
    public partial class AsyncPage : Page
    {
        public AsyncPage()
        {
            InitializeComponent();
        }


        private async void RunAsync_OnClick(object sender, RoutedEventArgs e)
        {
            BasicAsyncFlow.Text = "";
            BasicAsyncFlow.Text += "Started RunAsync_OnClick...\r\n";
            FireAndForgetAsync();
            await BlockingAsync();
            await BlockingAsyncTranslated();
            BasicAsyncFlow.Text += "Ended RunAsync_OnClick...\r\n";
        }

        private async void FireAndForgetAsync()
        {
            BasicAsyncFlow.Text += "Started FireAndForgetAsync...\r\n";
            await Task.Delay(2000);
            BasicAsyncFlow.Text += "Ended FireAndForgetAsync...\r\n";
        }

        private async Task BlockingAsync()
        {
            BasicAsyncFlow.Text += "Started BlockingAsync...\r\n";
            await Task.Delay(500);
            BasicAsyncFlow.Text += "Ended BlockingAsync...\r\n";
        }

        #region Logical Translated Code
        private Task BlockingAsyncTranslated()
        {
            BasicAsyncFlow.Text += "Started BlockingAsyncTranslated...\r\n";
            return Task.Delay(500).ContinueWith(t =>
            {
                BasicAsyncFlow.Text += "Ended BlockingAsyncTranslated...\r\n";    
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion






        private async void RunAsyncStateMachine_OnClick(object sender, RoutedEventArgs e)
        {
            AsyncStateMachine.Text = "";
            RunYieldReturnStateMachine();
            await RunAsyncCode();
        }

        private void RunYieldReturnStateMachine()
        {
            AsyncStateMachine.Text += "Starting RunYieldReturnStateMachine\r\n";
            foreach (var i in GetYieldNumbers())
            {
                AsyncStateMachine.Text += "Writing " + i + "\r\n";    
            }
            AsyncStateMachine.Text += "Ending RunYieldReturnStateMachine\r\n";
        }

        public IEnumerable<int> GetYieldNumbers()
        {
            AsyncStateMachine.Text += "Returning 100\r\n";
            yield return 100;
            AsyncStateMachine.Text += "Returning 200\r\n";
            yield return 200;
            AsyncStateMachine.Text += "Returning 300\r\n";
            yield return 300;
            AsyncStateMachine.Text += "Returning 400\r\n";
            yield return 400;
        }

        #region Translated CompilerCode
        public IEnumerable<int> GetYieldNumbersTranslated()
        {
            return new GetYeildNumbersEnumerable()
            {
                ClosureReference = this
            };
        }
        #endregion

        public async Task RunAsyncCode()
        {
            AsyncStateMachine.Text += "Starting RunNestedAsyncCode\r\n";
            await Task.Delay(100);
            AsyncStateMachine.Text += "Delayed 100\r\n";
            await Task.Delay(200);
            AsyncStateMachine.Text += "Delayed 200\r\n";
            await Task.Delay(300);
            AsyncStateMachine.Text += "Delayed 300\r\n";
            await Task.Delay(400);
            AsyncStateMachine.Text += "Delayed 400\r\n";
            AsyncStateMachine.Text += "Ending RunNestedAsyncCode\r\n";
        }

        #region Translated CompilerCode
        public Task RunAsyncCodeTranslated()
        {
            var taskResult = AsyncTaskMethodBuilder.Create();
            var stateMachine = new NestedAsyncCompilerGeneratedClass();
            stateMachine.Source = this;
            stateMachine.Builder = taskResult;
            taskResult.Start(ref stateMachine);
            return taskResult.Task;
        }
        #endregion




        private void NavigateWeb_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as ContentControl;
            if (button != null)
            {
                Process.Start(button.Content as string ?? string.Empty);
            }
        }
    }

    #region Translated CompilerCode
    public class GetYeildNumbersEnumerable : IEnumerable<int>, IEnumerator<int>
    {
        public AsyncPage ClosureReference { get; set; }

        private int _state = 0;
        public IEnumerator<int> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            switch (_state)
            {
                case 0:
                    ClosureReference.AsyncStateMachine.Text += "Returning 100\r\n";
                    Current = 100;
                    break;
                case 1:
                    ClosureReference.AsyncStateMachine.Text += "Returning 200\r\n";
                    Current = 200;
                    break;
                case 2:
                    ClosureReference.AsyncStateMachine.Text += "Returning 300\r\n";
                    Current = 300;
                    break;
                case 3:
                    ClosureReference.AsyncStateMachine.Text += "Returning 400\r\n";
                    Current = 400;
                    break;
                default:
                    return false;
            }

            _state++;
            return true;
        }

        public void Reset()
        {
            _state = 0;
        }

        public int Current { get; private set; }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }

    public class NestedAsyncCompilerGeneratedClass : IAsyncStateMachine
    {
        public AsyncTaskMethodBuilder Builder { get; set; }
        public TaskAwaiter Awaiter { get; set; }
        public int State { get; set; }
        public AsyncPage Source { get; set; }

        public void MoveNext()
        {
            switch (State)
            {
                case 0:
                    State++;
                    Source.AsyncStateMachine.Text += "Starting RunNestedAsyncCode\r\n";
                    var task100 = Task.Delay(100);
                    if (!task100.IsCompleted)
                    {
                        Awaiter = task100.GetAwaiter();
                        Awaiter.OnCompleted(MoveNext);
                    }
                    else
                    {
                        MoveNext();
                    }
                    break;
                case 1:
                    State++;
                    Source.AsyncStateMachine.Text += "Delayed 100\r\n";
                    var task200 = Task.Delay(200);
                    if (!task200.IsCompleted)
                    {
                        Awaiter = task200.GetAwaiter();
                        Awaiter.OnCompleted(MoveNext);
                    }
                    else
                    {
                        MoveNext();
                    }
                    break;
                case 2:
                    State++;
                    Source.AsyncStateMachine.Text += "Delayed 200\r\n";
                    var task300 = Task.Delay(300);
                    if (!task300.IsCompleted)
                    {
                        Awaiter = task300.GetAwaiter();
                        Awaiter.OnCompleted(MoveNext);
                    }
                    else
                    {
                        MoveNext();
                    }
                    break;
                case 3:
                    State++;
                    Source.AsyncStateMachine.Text += "Delayed 300\r\n";
                    var task400 = Task.Delay(400);
                    if (!task400.IsCompleted)
                    {
                        Awaiter = task400.GetAwaiter();
                        Awaiter.OnCompleted(MoveNext);
                    }
                    else
                    {
                        MoveNext();
                    }
                    break;
                case 4:
                    State++;
                    Source.AsyncStateMachine.Text += "Delayed 400\r\n";
                    Source.AsyncStateMachine.Text += "Ending RunNestedAsyncCode\r\n";
                    break;
            }
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            Builder.SetStateMachine(stateMachine);
        }
    }
    #endregion
}

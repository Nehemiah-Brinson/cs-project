using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.Diagnostics;

class App : Form
{
    Label Label1;
    TextBox Input;
    TextBox Output;
    TextBox Timer;
    Button MyStartButton;
    Button MyCancelButton;
    Thread SieveThread;
    Thread thread2;
    int fact;
    static ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true);
    App ()
    {
       
        Text = "Factorial machine";
        ClientSize = new System.Drawing.Size (340, 158);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        
        Label1 = new Label ();
        Input = new TextBox ();
        Timer = new TextBox ();
        Output = new TextBox ();
        MyStartButton = new Button ();
        MyCancelButton = new Button ();

        
        Label1.Location = new Point (24, 28);
        Label1.Size = new Size (200, 16);
        Label1.Text = "Number you wish to find the factorial of";

        Input.Location = new Point (224, 24);
        Input.Size = new Size (96, 20);
        Input.Name = "Input";
        Input.TabIndex = 0;

        Output.Location = new Point (24, 64);
        Output.Size = new Size (240, 20);
        Output.Name = "Output";
        Output.ReadOnly = true;
        Output.TabStop = false;

        Timer.Location = new Point (280, 64);
        Timer.Size = new Size (40, 20);
        Timer.Name = "Timer";
        Timer.ReadOnly = true;
        Timer.TabStop = false;

        MyStartButton.Location = new Point (24, 104);
        MyStartButton.Size = new Size (104, 32);
        MyStartButton.Text = "Start";
        MyStartButton.TabIndex = 1;
        MyStartButton.Click += new EventHandler (OnStart);

        MyCancelButton.Location = new Point (160, 104);
        MyCancelButton.Size = new Size (104, 32);
        MyCancelButton.Text = "Cancel";
        MyCancelButton.TabIndex = 2;
        MyCancelButton.Enabled = false;
        MyCancelButton.Click += new EventHandler (OnCancel);

       
        Controls.Add (Label1);
        Controls.Add (Input);
        Controls.Add (Output);
        Controls.Add (MyStartButton);
        Controls.Add (MyCancelButton);
        Controls.Add (Timer);
    }

    void OnStart (object sender, EventArgs e)
    {
        try {
            fact = Convert.ToInt32(Input.Text);
        }
        catch (FormatException) {
            MessageBox.Show ("Please enter a number greater than 2");
            return;
        }

        if (fact < 3) {
            MessageBox.Show ("Please enter a number greater than 2");
            return;
        }

        MyStartButton.Enabled = false;
        MyCancelButton.Enabled = true;
        Output.Text = "";
        SieveThread = new Thread (new ThreadStart (ThreadFunc));
        thread2 = new Thread(new ThreadStart(Threadbully));
        SieveThread.IsBackground = true;
        
        SieveThread.Start ();
        thread2.Start ();
    }

    void OnCancel (object sender, EventArgs e)
    {
        if (SieveThread != null && SieveThread.IsAlive) {
           
            SieveThread.Abort ();
            thread2.Abort ();
            SieveThread.Join ();
            MyStartButton.Enabled = true;
            MyCancelButton.Enabled = false;
            SieveThread = null;
            thread2 = null;
        }
    }

    Int64 CountPrimes (int fact)
    {
        Int64 number = 1;

    for (int i = fact; i > 0; i--)
     {
      pauseEvent.Wait();
      System.Threading.Thread.Sleep(10);
      number = number * fact;
  }

  return number;
    }

      void Threadbully()
     {
       Stopwatch stopwatch = Stopwatch.StartNew();
       TimeSpan elapsed =  stopwatch.Elapsed;
      while (SieveThread.IsAlive)
      {
         pauseEvent.Reset();
        System.Threading.Thread.Sleep(100);
          pauseEvent.Set();
        Timer.Text = Convert.ToString(elapsed.TotalSeconds);
         elapsed = stopwatch.Elapsed;
      }
      stopwatch.Stop();
        while (true){}
  }

    void ThreadFunc ()
    {
        // Do the computation
        Int64 count = CountPrimes (fact);

        // Update the UI
        Output.Text = count.ToString ();
        MyStartButton.Enabled = true;
        MyCancelButton.Enabled = false;
    }

    static void Main () 
    {
        Application.Run (new App ());
    }
}
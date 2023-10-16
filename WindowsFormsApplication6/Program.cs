using System;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication6
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew;
            Mutex dup = new Mutex(true, "WIA_DIO_COM", out createdNew);
            if (createdNew)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
                dup.ReleaseMutex();
            }
            else
            {
                ////중복실행에 대한 처리
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show("이미 프로그램이 실행 중입니다.");
                Application.Exit();
            }
        }
    }
}

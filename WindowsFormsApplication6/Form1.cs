using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using Microsoft.Win32;
using System.Diagnostics;
using System.Management;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Xml;

namespace WindowsFormsApplication6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            FormClosing += Form1_FormClosing;
            InitializeComponent();
            GetNicIds();
            //SP = new SoundPlayer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PreventScreenAndSleep();
            loadingWritingIp();
            computerNameTextBox.Text = SystemInformation.ComputerName;
            programVersionText.Text = Convert.ToString(now_version);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show("정말로 종료하시겠습니까?", "종료", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    /*
                    if (File.Exists(MessageListFile)) // 파일 존재 유무, 있으면 처리한다.
                    {
                        File.Delete(MessageListFile);
                    }

                    StreamWriter wr = new StreamWriter(@MessageListFile, true);
                    for (int i = 0; i < 24; i++)
                    {
                        wr.WriteLine(TimeForDelayDataGridView.Rows[i].Cells[0].Value.ToString() + " " + TimeForDelayDataGridView.Rows[i].Cells[1].Value.ToString());
                    }
                    wr.Close();
                    
                    if (Serv != null)
                    {
                        this.Serv.Close();
                        this.Serv = null;
                        for (int i = 0; i < this.ClnSocket.Count; i++)
                        {
                            this.ClnSocket[i].Close();
                        }
                        this.ClnSocket.Clear();
                    }
                    */
                    AllowMonitorPowerdown();
                    Environment.Exit(0);
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void loadingTimeForDelayDataGridView()
        {
            string loadingPopURL = saveUrl + "loadingDelay.php?"
                 + "database=" + textBoxDBName.Text + "&username=" + textBoxID.Text + "&password=" + textBoxPW.Text + "&code=" + delayCodeTextBox.Text + "&tablename=writedelay";
            TimeForDelayDataGridView.Rows.Clear();
            try
            {
                textBox4.Text = loadingPage(loadingPopURL, null, "GET", null, Encoding.GetEncoding("EUC-KR"));

                string[] strTexts = textBox4.Text.Split(new Char[] { '\'' }, StringSplitOptions.None);
                TimeForDelayDataGridView.Rows.Add("0", strTexts[1]);
                TimeForDelayDataGridView.Rows.Add("1", strTexts[3]);
                TimeForDelayDataGridView.Rows.Add("2", strTexts[5]);
                TimeForDelayDataGridView.Rows.Add("3", strTexts[7]);
                TimeForDelayDataGridView.Rows.Add("4", strTexts[9]);
                TimeForDelayDataGridView.Rows.Add("5", strTexts[11]);
                TimeForDelayDataGridView.Rows.Add("6", strTexts[13]);
                TimeForDelayDataGridView.Rows.Add("7", strTexts[15]);
                TimeForDelayDataGridView.Rows.Add("8", strTexts[17]);
                TimeForDelayDataGridView.Rows.Add("9", strTexts[19]);
                TimeForDelayDataGridView.Rows.Add("10", strTexts[21]);
                TimeForDelayDataGridView.Rows.Add("11", strTexts[23]);
                TimeForDelayDataGridView.Rows.Add("12", strTexts[25]);
                TimeForDelayDataGridView.Rows.Add("13", strTexts[27]);
                TimeForDelayDataGridView.Rows.Add("14", strTexts[29]);
                TimeForDelayDataGridView.Rows.Add("15", strTexts[31]);
                TimeForDelayDataGridView.Rows.Add("16", strTexts[33]);
                TimeForDelayDataGridView.Rows.Add("17", strTexts[35]);
                TimeForDelayDataGridView.Rows.Add("18", strTexts[37]);
                TimeForDelayDataGridView.Rows.Add("19", strTexts[39]);
                TimeForDelayDataGridView.Rows.Add("20", strTexts[41]);
                TimeForDelayDataGridView.Rows.Add("21", strTexts[43]);
                TimeForDelayDataGridView.Rows.Add("22", strTexts[45]);
                TimeForDelayDataGridView.Rows.Add("23", strTexts[47]);
                systemMessageBox_AppendText("딜레이 불러오기에 성공하였습니다.", Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241))))));
            }
            catch (Exception ex)
            {
                systemMessageBox_AppendText("딜레이를 불어오는 도중 오류가 발생!", Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241))))));

                //textBoxComment.AppendText(ex.StackTrace);
                Console.WriteLine(ex.StackTrace);
            }
            /*
            if (File.Exists(MessageListFile)) // 파일 존재 유무, 있으면 처리한다.
            {
                FileStream MessageFile = File.Open(MessageListFile, FileMode.OpenOrCreate, FileAccess.Read); // 파일을 열되 없다면 생성하고 권한은 읽기 전용으로 생성
                StreamReader MessageStream = new StreamReader(MessageFile); // 스트림 생성

                MessageStream.BaseStream.Seek(0, SeekOrigin.Begin); // 파일을 읽어가기 시작한다.

                while (MessageStream.Peek() > -1)
                {
                    String ReadLine = MessageStream.ReadLine(); // String형 변수에 파일 내의 한 줄을 읽어온다.

                    string[] strTexts = ReadLine.Split(new Char[] { ' ' }, StringSplitOptions.None);
                    if (Convert.ToInt16(strTexts[0]) > 23)
                    {
                        break;
                    }
                    TimeForDelayDataGridView.Rows.Add(
                        strTexts[0],
                        strTexts[1]
                    );
                }

                MessageStream.Close();
            }


            else // 파일이 없으면 찾을 수 없다는 메세지 출력
            {
                StreamWriter wr = new StreamWriter(@MessageListFile, true);
                for (int i = 0; i < 24; i++)
                {
                    wr.WriteLine(i + " " + 90);
                    TimeForDelayDataGridView.Rows.Add(
                         i,
                         90
                     );
                }
                wr.Close();
            }
            */
        }

        private void loadingWritingIp()
        {
            if (File.Exists(writingIpTextFile)) // 파일 존재 유무, 있으면 처리한다.
            {
                FileStream _file = File.Open(writingIpTextFile, FileMode.OpenOrCreate, FileAccess.Read); // 파일을 열되 없다면 생성하고 권한은 읽기 전용으로 생성
                StreamReader MessageStream = new StreamReader(_file); // 스트림 생성

                MessageStream.BaseStream.Seek(0, SeekOrigin.Begin); // 파일을 읽어가기 시작한다.               
                while (MessageStream.Peek() > -1)
                {

                    String ReadLine = MessageStream.ReadLine(); // String형 변수에 파일 내의 한 줄을 읽어온다.
                    string[] strTexts = ReadLine.Split(new Char[] { '|' }, StringSplitOptions.None);
                    String _ip = strTexts[0];
                    if (strTexts[2].IndexOf("success") > -1)
                    {
                        success_writing++;
                        completedataGridView.Rows.Add(
                            success_writing,
                            strTexts[3],
                            strTexts[1]
                        );
                    }

                    writingIpdataGridView.Rows.Add(
                            _ip // 아이피, 시간, 타입, 아이디
                            );
                    _writingIp.Add(strTexts[0]);
                }
                MessageStream.Close();
            }
        }

        private void LoadingPopLinId_Click(object sender, EventArgs e)
        {
            loadingPop();
        }
        private void loadingPop()
        {
            DateTime dt = DateTime.Now;
            int sDayofYear = dt.DayOfYear;

            string loadingPopURL = saveUrl + "loadingPop2.php?"
                 + "database=" + textBoxDBName.Text + "&username=" + textBoxID.Text + "&password=" + textBoxPW.Text + "&limit=" + textBoxLoadingCount.Text
                 + "&tablename=" + textBoxTableName.Text
                  + "&todaynumber=1000"; // + sDayofYear;
            
            popLinIDdataGridView.Rows.Clear();
            //systemMessageBox_AppendText(loadingPopURL,Color.Beige);
            idCount = 0;
            try
            {
                textBox4.Text = loadingPage(loadingPopURL, null, "GET", null, Encoding.GetEncoding("EUC-KR"));
                //XElement 요소 Load
                XElement root = XElement.Load(loadingPopURL);
                /*
                if (root.Element("root"))
                {
                    MessageBox.Show("리스트를 불러 올 수 없습니다. 정보를 확인하여 주시기 바랍니다.");
                    return;
                }
                 */
                //하위 노드를 읽는 방법 (XML 원본 참고)
                ArrayList array1 = new ArrayList();
                list = new Dictionary<int, popIDPW>();
                foreach (XElement item in root.Descendants("items").Descendants("item"))
                {
                    string popid = item.Attribute("popid").Value;
                    string pw = item.Attribute("pw").Value;

                    if (!array1.Contains(popid))
                    {
                        list.Add(idCount, new popIDPW { _popid = popid, _pw = pw });

                        popLinIDdataGridView.Rows.Add(
                        popid
                        );
                        idCount++;
                    }
                    array1.Add(popid);
                };
                array1 = null;
                systemMessageBox_AppendText("로딩 아이디 : " + idCount + "개가 로딩 완료되었습니다.", Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241))))));
                //systemMessageBox_AppendText(list[i][0] + " / " + list[i][1], Color.Pink);
            }
            catch (KeyNotFoundException ex)
            {
                textBoxComment.AppendText(ex.StackTrace);
            }
            catch (Exception ex)
            {
                //textBoxComment.AppendText(ex.StackTrace);
                Console.WriteLine(ex.StackTrace);
            }
        }
        class popIDPW
        {
            public string _popid { get; set; }
            public string _pw { get; set; }
        }
        /*
        class proxyIpList
        {
            public string _ip { get; set; }
            public string _port { get; set; }
        }
        */
        
        int now_version = 2016030100;
        //Socket Serv;
       // List<Socket> ClnSocket = new List<Socket>();
        ArrayList _writingIp = new ArrayList();
        //String MessageListFile = @"delay.txt"; // 메세지가 들어 있는 파일을 지정
        String writingIpTextFile = @"writingIpTextFile.txt"; // 메세지가 들어 있는 파일을 지정
        //String proxyFile = "proxy.txt"; // 메세지가 들어 있는 파일을 지정
        Dictionary<int, popIDPW> list;
        int _popNumber = 0;
        //SoundPlayer SP;
        String popid = null;
        String pw = null;
        Boolean isStart = false;

        String saveUrl = "";
        String url = "http://popall.com";
        String loginUrl = "https://popall.com/login2.php";
        String freeBoardUrl = "http://popall.com/lin/bbs.htm?code=talking";
        String writeUrl = "http://popall.com/lin/bbs.htm?code=talking&mode=1#main";
        String writePostUrl = "http://popall.com/pboard_write_ok.php?code=talking";
        String logoutUrl = "http://popall.com/logout2.php?part=0";
        String _referer = "popall.com";
        int delay = 0;
        private step level = 0;
        //String idp = null;
        String funny = null;
        String sobj = null;
        String imageCodeUrl = null;
        int success_writing = 0;
        int 금일_남은_글쓰기_횟수 = 0;
        int 금일_아이피_남은_글쓰기_횟수 = 0;
        int hasConnectionCount = 0;
        Boolean isLogIn = false;
        Boolean idChage = false;
        int idCount = 0;
        int writingDelay = 90;
        int abnormality = 0;
        int oddCount = 0;
        int writelevel = 0;
        /*
        int proxyNumber = 0;
        string MyProxyHostString = "192.168.1.200";
        int MyProxyPort = 8080;
        Dictionary<int, proxyIpList> proxyList;
        */
        private enum step
        {
            start = 1,
            b = 2,
            c = 3,
            d = 4,
            e = 5,
            f = 6,
            g = 7,
            //h = 8
        }

        private void hongboStart_Click(object sender, EventArgs e)
        {
            if (isStart)
            {
                MessageBox.Show("이미 홍보기가 돌아가는 상태입니다. 혹시라도 돌아가지 않는다면 종료 후 재실행하여 주시기 바랍니다.");
            }
            if (serverNameTextBox.Text.Length < 2)
            {
                MessageBox.Show("서버 이름을 입력하여 주시기 바랍니다.");
                serverNameTextBox.Focus();
                return;
            }
            if (textBoxSubject.Text.Length < 2)
            {
                MessageBox.Show("제목을 입력하여 주시기 바랍니다.");
                textBoxSubject.Focus();
                return;
            }
            if (textBoxComment.Text.Length < 2)
            {
                MessageBox.Show("내용을 입력하여 주시기 바랍니다.");
                textBoxComment.Focus();
                return;
            }

            if (textBoxSubject1.Text.Length < 2)
            {
                MessageBox.Show("제목을 입력하여 주시기 바랍니다.");
                textBoxSubject1.Focus();
                return;
            }
            if (textBoxComment1.Text.Length < 2)
            {
                MessageBox.Show("내용을 입력하여 주시기 바랍니다.");
                textBoxComment1.Focus();
                return;
            }

            if (textBoxSubject2.Text.Length < 2)
            {
                MessageBox.Show("제목을 입력하여 주시기 바랍니다.");
                textBoxSubject2.Focus();
                return;
            }
            if (textBoxComment2.Text.Length < 2)
            {
                MessageBox.Show("내용을 입력하여 주시기 바랍니다.");
                textBoxComment2.Focus();
                return;
            }
            if (delayCodeTextBox.Text.Length < 3)
            {
                MessageBox.Show("코드를 입력하여 주시기 바랍니다.");
                delayCodeTextBox.Focus();
                return;
            }
            
            loadingTimeForDelayDataGridView();
            
            if (list == null)
            {
                MessageBox.Show("아이디를 불러와 주시기 바랍니다.");
                return;
                /*
                while (list == null)
                {
                    loadingPop();
                    Thread.Sleep(60000);
                }
                */
            }
            
            textBox3.Text = success_writing.ToString();

            systemMessageBox_AppendText(" 홍 보 를  시 작 합 니 다 .", Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241))))));

            level = step.start;

            isStart = true;

            startTimeTextBox.Text = getDateTime();

            _popNumber = 0;

            hongBo();

            //proxyNumber = 0;
            //최소값 지정(기본값)
            /*
            while (true)
            {
                if (!isStart)
                {
                    break;
                }
                if (!sCheck)
                {
                    break;
                }
                if (delay <= 0)
                {
                    break;
                }
            }
            */

        }

        bool sCheck = false;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (sCheck)
            {
                delay--;
                label1.Text = delay.ToString();

                if (delay > 59
                    && delay % 60 == 0)
                {
                    pcSave();
                }
                if (delay <= 0)
                {
                    sCheck = false;
                    timer1.Enabled = false;
                    try
                    {
                        hongBo();
                    }
                    catch (KeyNotFoundException)
                    {
                        return;
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
            }
            else
            {
                timer1.Enabled = false;  // case실행중 Tick의 접근을 막겠다.
            }

        }

        private void hongBo()
        {
            if (!isStart)
            {
                systemMessageBox_AppendText("홍보가 종료되었습니다. 이용해 주셔서 감사합니다.", Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241))))));
                return;
            }

            DateTime dt = DateTime.Now;

            int nowtime = Convert.ToInt16(dt.ToString("HHmm"));

            int sDayofYear = dt.DayOfYear;

            int writingFullDay = sDayofYear - 1;

            if (nowtime > 2330 && 2359 >= nowtime)
            {
                success_writing = 0;
                _writingIp.Clear();
                completedataGridView.Rows.Clear();
                writingIpdataGridView.Rows.Clear();
                if (File.Exists(writingIpTextFile)) // 파일 존재 유무, 있으면 처리한다.
                {
                    File.Delete(writingIpTextFile);
                }
            }
            
            if (nowtime % 10 == 0)
            {
                if (needupdateversionCheck())
                {
                    systemMessageBox_AppendText("버젼이 달라 홍보가 종료되었습니다. 업데이트 하십시요.", Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241))))));
                    isStart = false;
                    return;
                }
            }

            if (getMyIp().StartsWith("169.254") || getMyIp().StartsWith("127.0")) // 해당 A,B클래스로 시작한다면 맥주소 변경
            {
                macChange();
            }

            while (!HasConnection())
            {
                hasConnectionCount++;

                if (hasConnectionCount > 30)
                {
                    hasConnectionCount = 0;
                    macChange();
                    break;
                }
                Thread.Sleep(1000);
            }

            /*
            MyProxyHostString = proxyList[proxyNumber]._ip;
            MyProxyPort = Convert.ToInt16(proxyList[proxyNumber]._port);
            */

            if (containsIp(getMyIp()))
            {
                //systemMessageBox_AppendText(string.Format("{0} 이미 사용한 아이피입니다. ", getMyIp()), Color.Chocolate);
                statusMessageBox_AppendText(string.Format("{0} 이미 사용", getMyIp()), Color.Chocolate);
                delay = 10;
                level = step.g;
            }
            else if (getMyIp().StartsWith("169"))
            {
                systemMessageBox_AppendText(string.Format("{0} 응답이 없습니다.", "169.254."), Color.DarkRed);
                delay = 10;
                level = step.g;
            }
            else if (getMyIp().StartsWith("127")) // 해당 A,B클래스로 시작한다면 맥주소 변경
            {
                macChange();
            }
            else
            {
                try
                {
                    if (list.Count <= _popNumber)
                    {
                        _popNumber = 0;

                        loadingPop();
                        while (list.Count <= 0)
                        {
                            loadingPop();
                            Thread.Sleep(60000);
                        }

                        systemMessageBox_AppendText(string.Format("{0}", new object[] { "아이디를 이미 다 사용하여 아이디를 새롭게 불러옵니다.." }), Color.DarkRed);
                        statusMessageBox_AppendText("아이디를 이미 다 사용하여 아이디를 새롭게 불러옵니다.", Color.DarkRed);
                    }
                }
                catch (KeyNotFoundException)
                {
                    return;
                }
                catch (Exception)
                {
                    return;
                }
                
                try
                {
                    popid = list[_popNumber]._popid;
                    pw = list[_popNumber]._pw;
                } catch (KeyNotFoundException)
                {
                    return;
                } catch (Exception)
                {
                    return;
                }

                textBox6.Text = popid;

                idCountTextBox.Text = _popNumber + " / " + idCount;

                //popLinIDdataGridView.Rows[_popNumber].Selected = true;
                //popLinIDdataGridView.CurrentCell = popLinIDdataGridView.Rows[_popNumber].Cells[0];

                writingDelay = Convert.ToInt16(TimeForDelayDataGridView.Rows[Convert.ToInt16(getTimeHour())].Cells[1].Value.ToString());

                nowDelayLabel.Text = writingDelay.ToString();
            }

            switch (this.level)
            {
                case step.start:
                    SPEEDCDN = null;
                    pcSave();
                    //systemMessageBox.Text = "";
                    statusMessageBox_AppendText("팝린 홈페이지 접속 중....", Color.DarkOrange);
                    try
                    {
                        textBox4.Text = loadingPage(url, null, "GET", null, Encoding.GetEncoding("EUC-KR"));

                        //MessageBox.Show(textBox4.Text);
                        if (textBox4.Text.IndexOf("기본 연결이") > -1)
                        {
                            try
                            {
                                this.delay = 60;
                                this.level = step.f;
                                systemMessageBox_AppendText(string.Format("{0} > {1}", new object[] { getMyIp(), "기본 연결이 닫혔습니다." }), Color.DarkRed);
                                statusMessageBox_AppendText("기본 연결이 닫혔습니다.", Color.DarkRed);
                                break;
                            }
                            catch (FormatException formatexception)
                            {
                                statusMessageBox_AppendText("기본 연결이 닫혔습니다.", Color.DarkOrange);
                                this.delay = 60;
                                this.level = step.f;
                                Console.WriteLine(formatexception);
                                break;
                            }
                        }

                        if (textBox4.Text.IndexOf("전송 연결 ") > -1)
                        {
                            try
                            {
                                this.delay = 60;
                                this.level = step.f;
                                systemMessageBox_AppendText(string.Format("{0} > {1}", new object[] { getMyIp(), "전송 연결 현재 연결은 원격 호스트에 의해 강제로 끊겼습니다에서 데이터를 읽을 수 없습니다." }), Color.PaleVioletRed);
                                statusMessageBox_AppendText("전송 연결 현재 연결은 원격 호스트에 의해 강제로 끊겼습니다에서 데이터를 읽을 수 없습니다.", Color.PaleVioletRed);
                                break;
                            }
                            catch (FormatException formatexception)
                            {
                                statusMessageBox_AppendText("전송 연결 현재 연결은 원격 호스트에 의해 강제로 끊겼습니다에서 데이터를 읽을 수 없습니다.", Color.DarkOrange);
                                this.delay = 60;
                                this.level = step.f;
                                Console.WriteLine(formatexception);
                                break;
                            }
                        }
                    }
                    catch (FormatException formatexception)
                    {
                        statusMessageBox_AppendText("팝린 접속 문제....", Color.DarkOrange);
                        this.delay = 3;
                        Console.WriteLine(formatexception);
                        break;
                    }
                    /*
                    if (idp == null)
                    {
                        oddCount++;
                        if (oddCount >= 3)
                        {
                            oddCount = 0;
                            this.delay = 5;
                            this.level = step.f;
                            statusMessageBox_AppendText("idp 인식 오류", Color.DarkOrange);
                            break;
                        }
                        this.delay = 5;
                        this.level = step.d;
                        statusMessageBox_AppendText("idp 인식 오류", Color.DarkOrange);
                        break;
                    }
                    */
                    statusMessageBox_AppendText("로그인 중....", Color.IndianRed);
                    try
                    {
                        textBox4.Text = loadingPage(loginUrl, _referer, "POST", string.Format("popid={0}&pw={1}&loginbtn.x=20&loginbtn.y=20&idp={2}", popid, pw, "748"), Encoding.GetEncoding("EUC-KR"));
                    }
                    catch (FormatException formatexception)
                    {
                        statusMessageBox_AppendText("로그인 중 문제 발생", Color.DarkOrange);
                        this.delay = 5;
                        this.level = step.g;
                        Console.WriteLine(formatexception);
                        break;
                    }

                    if (textBox4.Text.IndexOf("전송 연결 ") > -1)
                    {
                        try
                        {
                            this.delay = 60;
                            this.level = step.f;
                            systemMessageBox_AppendText(string.Format("{0} > {1}", new object[] { getMyIp(), "전송 연결 현재 연결은 원격 호스트에 의해 강제로 끊겼습니다에서 데이터를 읽을 수 없습니다." }), Color.PaleVioletRed);
                            statusMessageBox_AppendText("전송 연결 현재 연결은 원격 호스트에 의해 강제로 끊겼습니다에서 데이터를 읽을 수 없습니다.", Color.PaleVioletRed);
                            break;
                        }
                        catch (FormatException formatexception)
                        {
                            statusMessageBox_AppendText("전송 연결 현재 연결은 원격 호스트에 의해 강제로 끊겼습니다에서 데이터를 읽을 수 없습니다.", Color.DarkOrange);
                            this.delay = 60;
                            this.level = step.f;
                            Console.WriteLine(formatexception);
                            break;
                        }
                    }
                    if (this.textBox4.Text == null)
                    {

                        try
                        {
                            this.delay = 60;
                            this.level = step.f;
                            statusMessageBox_AppendText("응답이 없습니다.", Color.DarkOrange);
                            systemMessageBox_AppendText(string.Format("{0} > {1}", new object[] { getMyIp(), textBox4.Text }), Color.DarkOrange);
                            break;
                        }
                        catch (FormatException formatexception)
                        {
                            statusMessageBox_AppendText("응답이 없습니다.....", Color.DarkOrange);
                            this.delay = 3;
                            Console.WriteLine(formatexception);
                            break;
                        }
                    }
                    else if (textBox4.Text.IndexOf("기본 연결이") > -1)
                    {
                        try
                        {
                            this.delay = 60;
                            this.level = step.f;
                            systemMessageBox_AppendText(string.Format("{0} > {1}", new object[] { getMyIp(), "기본 연결이 닫혔습니다." }), Color.DarkRed);
                            statusMessageBox_AppendText("기본 연결이 닫혔습니다.", Color.DarkRed);
                            break;
                        }
                        catch (FormatException formatexception)
                        {
                            statusMessageBox_AppendText("기본 연결이 닫혔습니다.", Color.DarkOrange);
                            this.delay = 60;
                            this.level = step.f;
                            Console.WriteLine(formatexception);
                            break;
                        }
                    }
                    else if (this.textBox4.Text.IndexOf("todayDate.toGMTString()") > -1)
                    {
                        isLogIn = true;
                        this.delay = 3;
                        this.level = step.b;
                        try
                        {
                            systemMessageBox_AppendText(string.Format("{0} > {1} 로그인 성공 ", new object[] { getMyIp(), popid }), Color.Chocolate);
                        }
                        catch (FormatException formatexception)
                        {
                            this.delay = 3;
                            Console.WriteLine(formatexception);
                            break;
                        }
                    }
                    else if (this.textBox4.Text.IndexOf("meta http-equiv=refresh content='0; url=http://popall.com") > -1)
                    {
                        try
                        {
                            systemMessageBox_AppendText(string.Format("{1} > {2}", new object[] { getMyIp(), "meta http-equiv=refresh content='0" }), Color.DarkRed);
                            this.delay = 60;
                            this.level = step.f;
                        }
                        catch (FormatException formatexception)
                        {
                            statusMessageBox_AppendText("meta http-equiv=refresh content='0", Color.DarkOrange);
                            this.delay = 60;
                            this.level = step.f;
                            Console.WriteLine(formatexception);
                            break;
                        }
                    }
                    else if (textBox4.Text.IndexOf("로그인이 실패했습니다. 잠시후 접속해주세요.") > -1) // mac adress, ip 변화가 필요한 상황
                    {
                        try
                        {
                            idChage = true;
                            this.delay = 3;
                            this.level = step.g;
                            systemMessageBox_AppendText(string.Format("{0} > {1}", new object[] { getMyIp(), "로그인이 실패했습니다. 잠시후 접속해주세요." }), Color.DarkRed);
                        }
                        catch (FormatException formatexception)
                        {
                            statusMessageBox_AppendText("로그인이 실패했습니다. 잠시후 접속해주세요.", Color.DarkOrange);
                            this.delay = 3;
                            Console.WriteLine(formatexception);
                            break;
                        }
                    }
                    else if (textBox4.Text.IndexOf("최근 1분이내 같은 장소에서 로그인하셨습니다.") > -1)
                    {
                        try
                        {
                            //textBox4.Text = d.a().b(bn, null, "GET", null);
                            this.delay = 60;
                            systemMessageBox_AppendText(string.Format("{0} > {1}", new object[] { getMyIp(), "최근 1분이내 같은 장소에서 로그인하셨습니다." }), Color.DarkRed);
                        }
                        catch (FormatException formatexception)
                        {
                            statusMessageBox_AppendText("최근 1분이내 같은 장소에서 로그인하셨습니다.", Color.DarkOrange);
                            this.delay = 3;
                            Console.WriteLine(formatexception);
                            break;
                        }
                    }
                    else if (textBox4.Text.IndexOf("최근 1분이내 로그인 한 적이 있습니다. 잠시후 접속해주세요.") > -1)
                    {
                        try
                        {
                            //textBox4.Text = d.a().b(bn, null, "GET", null);
                            this.delay = 60;
                            systemMessageBox_AppendText(string.Format("{0} > {1}", new object[] { getMyIp(), "최근 1분이내 로그인 한 적이 있습니다. 잠시후 접속해주세요." }), Color.DarkRed);

                        }
                        catch (FormatException formatexception)
                        {
                            statusMessageBox_AppendText("최근 1분이내 로그인 한 적이 있습니다. 잠시후 접속해주세요.", Color.DarkOrange);
                            this.delay = 3;
                            Console.WriteLine(formatexception);
                            break;
                        }
                    }
                    else if (textBox4.Text.IndexOf("아이디를 입력하여 주십시요") > -1)
                    {
                        try
                        {
                            idChage = true;
                            this.delay = 3;
                            this.level = step.f;
                            systemMessageBox_AppendText(string.Format("{0} > {1}", new object[] { getMyIp(), "아이디를 입력하여 주십시요" }), Color.DarkRed);
                        }
                        catch (FormatException formatexception)
                        {
                            statusMessageBox_AppendText("아이디를 입력하여 주십시요.", Color.DarkOrange);
                            this.delay = 3;
                            Console.WriteLine(formatexception);
                            break;
                        }
                    }
                    else if (textBox4.Text.IndexOf("비밀번호를 입력하여 주십시요") > -1)
                    {
                        try
                        {
                            idChage = true;
                            this.delay = 3;
                            this.level = step.f;
                            systemMessageBox_AppendText(string.Format("{0} > {1}", new object[] { getMyIp(), "비밀번호를 입력하여 주십시요" }), Color.DarkRed);
                        }
                        catch (FormatException formatexception)
                        {
                            statusMessageBox_AppendText("비밀번호를 입력하여 주십시요.", Color.DarkOrange);
                            this.delay = 3;
                            Console.WriteLine(formatexception);
                            break;
                        }
                    }
                    else if (textBox4.Text.IndexOf("패스워드가 일치하지 않습니다.") > -1)
                    {
                        try
                        {
                            idChage = true;
                            this.delay = 60;
                            this.level = step.f;
                            systemMessageBox_AppendText(string.Format("{0} > {1}", new object[] { getMyIp(), "패스워드가 일치하지 않습니다." }), Color.DarkRed);

                            try
                            {
                                this.delay = 60;
                                this.level = step.f;
                                String updateUrl = saveUrl + "saveSql.php?";
                                updateUrl
                                    += "username=" + textBoxID.Text
                                    + "&password=" + textBoxPW.Text
                                    + "&database=" + textBoxDBName.Text
                                    + "&tablename=" + textBoxTableName.Text
                                    + "&lawbanday=" + 1000
                                    + "&popid=" + popid;
                               // statusMessageBox_AppendText("접근금지 " + str + "일", Color.DarkOrange);
                                generateURL(updateUrl);
                                idChage = true;
                            }
                            catch (FormatException formatexception)
                            {
                                Console.WriteLine(formatexception);
                                break;
                            }

                            //e.a(Application.StartupPath, "비밀번호틀린아이디", popid + " " + pw, false);
                        }
                        catch (FormatException formatexception)
                        {
                            statusMessageBox_AppendText("패스워드가 일치하지 않습니다.", Color.DarkOrange);
                            this.delay = 3;
                            Console.WriteLine(formatexception);
                            break;
                        }
                    }
                    else if (textBox4.Text.IndexOf("로그인이 실패했습니다. 해당 아이디가 존재하지 않습니다.") > -1)
                    {
                        try
                        {
                            idChage = true;
                            this.delay = 60;
                            this.level = step.f;
                            systemMessageBox_AppendText(string.Format("{0} > {1}", new object[] { getMyIp(), "로그인이 실패했습니다. 해당 아이디가 존재하지 않습니다." }), Color.DarkRed);

                            try
                            {
                                this.delay = 60;
                                this.level = step.f;
                                String updateUrl = saveUrl + "saveSql.php?";
                                updateUrl
                                    += "username=" + textBoxID.Text
                                    + "&password=" + textBoxPW.Text
                                    + "&database=" + textBoxDBName.Text
                                    + "&tablename=" + textBoxTableName.Text
                                    + "&lawbanday=" + 1000
                                    + "&popid=" + popid;
                                // statusMessageBox_AppendText("접근금지 " + str + "일", Color.DarkOrange);
                                generateURL(updateUrl);
                                idChage = true;
                            }
                            catch (FormatException formatexception)
                            {
                                Console.WriteLine(formatexception);
                                break;
                            }
                        }
                        catch (FormatException formatexception)
                        {
                            statusMessageBox_AppendText("로그인이 실패했습니다. 해당 아이디가 존재하지 않습니다.", Color.DarkOrange);
                            this.delay = 3;
                            Console.WriteLine(formatexception);
                            break;
                        }
                    }
                    else
                    {
                        try
                        {
                            this.delay = 60;
                            this.level = step.f;
                            statusMessageBox_AppendText("알 수 없는 이유로 로그인 실패", Color.DarkOrange);
                            systemMessageBox_AppendText(string.Format("{0} > {1}", new object[] { getMyIp(), textBox4.Text }), Color.DarkOrange);
                        }
                        catch (FormatException formatexception)
                        {
                            statusMessageBox_AppendText("알 수 없는 이유로 로그인 실패", Color.DarkOrange);
                            this.delay = 3;
                            Console.WriteLine(formatexception);
                            break;
                        }
                    }
                    break;
                case step.b:
                    statusMessageBox_AppendText("접근금지 상태 점검 중....", Color.DarkOrange);
                    textBox4.Text = loadingPage(freeBoardUrl, url, "GET", null, Encoding.GetEncoding("EUC-KR"));
                    richTextBox1.Text = textBox4.Text;
                    
                    if (textBox4.Text.IndexOf("기본 연결이") > -1)
                    {
                        try
                        {
                            this.delay = 60;
                            this.level = step.f;
                            systemMessageBox_AppendText(string.Format("{0} > {1}", new object[] { getMyIp(), "기본 연결이 닫혔습니다." }), Color.DarkRed);
                            statusMessageBox_AppendText("기본 연결이 닫혔습니다.", Color.DarkRed);
                            break;
                        }
                        catch (FormatException formatexception)
                        {
                            statusMessageBox_AppendText("기본 연결이 닫혔습니다.", Color.DarkOrange);
                            this.delay = 60;
                            this.level = step.f;
                            Console.WriteLine(formatexception);
                            break;
                        }
                    }

                    if (textBox4.Text.IndexOf("전송 연결 ") > -1)
                    {
                        try
                        {
                            this.delay = 60;
                            this.level = step.f;
                            systemMessageBox_AppendText(string.Format("{0} > {1}", new object[] { getMyIp(), "전송 연결 현재 연결은 원격 호스트에 의해 강제로 끊겼습니다에서 데이터를 읽을 수 없습니다." }), Color.PaleVioletRed);
                            statusMessageBox_AppendText("전송 연결 현재 연결은 원격 호스트에 의해 강제로 끊겼습니다에서 데이터를 읽을 수 없습니다.", Color.PaleVioletRed);
                            break;
                        }
                        catch (FormatException formatexception)
                        {
                            statusMessageBox_AppendText("전송 연결 현재 연결은 원격 호스트에 의해 강제로 끊겼습니다에서 데이터를 읽을 수 없습니다.", Color.DarkOrange);
                            this.delay = 60;
                            this.level = step.f;
                            Console.WriteLine(formatexception);
                            break;
                        }
                    }

                    string str = "";
                    try
                    {
                        str = textBox4.Text.Split(new string[] { "d_on" }, StringSplitOptions.None)[1];
                        if (str.IndexOf("lawban=") > 0)
                        {
                            try
                            {
                                str = _split(str, "lawban=", ";", 0);
                                this.delay = 60;
                                this.level = step.f;
                                systemMessageBox_AppendText(string.Format("접근금지({0}일)", str), Color.DarkOrange);

                                String updateUrl = saveUrl + "saveSql.php?";
                                updateUrl
                                    += "username=" + textBoxID.Text
                                    + "&password=" + textBoxPW.Text
                                    + "&database=" + textBoxDBName.Text
                                    + "&tablename=" + textBoxTableName.Text
                                    + "&lawbanday=" + str
                                    + "&popid=" + popid;
                                statusMessageBox_AppendText("접근금지 " + str + "일", Color.DarkOrange);
                                generateURL(updateUrl);
                                idChage = true;
                            }
                            catch (FormatException formatexception)
                            {
                                Console.WriteLine(formatexception);
                                break;
                            }

                        }
                        else if (this.textBox4.Text.IndexOf("<td align=right width=100 style=\"color:rgb(255,255,255);\">접근금지<tr align=center><td colspan=3>") > -1)
                        {
                            try
                            {
                                this.delay = 60;
                                this.level = step.f;
                                systemMessageBox_AppendText(string.Format("접근금지({0}일)", str), Color.DarkOrange);

                                String updateUrl = saveUrl + "saveSql.php?";
                                updateUrl
                                    += "username=" + textBoxID.Text
                                    + "&password=" + textBoxPW.Text
                                    + "&database=" + textBoxDBName.Text
                                    + "&tablename=" + textBoxTableName.Text
                                    + "&lawbanday=" + str
                                    + "&popid=" + popid;

                                generateURL(updateUrl);
                                idChage = true;
                            }
                            catch (FormatException formatexception)
                            {
                                Console.WriteLine(formatexception);
                                break;
                            }
                        }
                        else if (this.textBox4.Text.IndexOf("팝콘") > -1)
                        {
                            try
                            {
                                this.delay = 5;
                                this.level = step.d;
                                systemMessageBox_AppendText(string.Format("접근금지 0일 사용가능", new object[0]), Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241))))));

                                statusMessageBox_AppendText("정상적인 아이디...", Color.DarkOrange);
                                String cleaIdSaveUrl = saveUrl + "cleanIdSave.php?";
                                cleaIdSaveUrl
                                     += "database=" + textBoxDBName.Text
                                    + "&username=" + textBoxID.Text
                                    + "&password=" + textBoxPW.Text
                                    + "&tablename=" + textBoxTableName.Text
                                    + "&popid=" + popid
                                    + "&pw=" + pw
                                    ;
                                generateURL(cleaIdSaveUrl);
                            }
                            catch (FormatException formatexception)
                            {
                                Console.WriteLine(formatexception);
                                break;
                            }
                        }
                        else
                        {
                            try
                            {
                                oddCount++;
                                if (oddCount >= 3)
                                {
                                    this.delay = 60;
                                    this.level = step.f;
                                    systemMessageBox_AppendText("oddCount 3회 이상 발생!", Color.DarkOrange);
                                    break;
                                }
                                else
                                {
                                    idChage = true;
                                    this.delay = 5;
                                    this.level = step.b;
                                    break;
                                }
                            }
                            catch (FormatException formatexception)
                            {
                                Console.WriteLine(formatexception);
                                break;
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        idChage = true;
                        this.delay = 3;
                        this.level = step.g;
                        systemMessageBox_AppendText(string.Format(exception.ToString() + "\r\n", new object[0]), Color.DarkOrange);
                    }
                    break;
                case step.c:

                    break;
                case step.d:
                    oddCount = 0;
                    textBox4.Text = loadingPage(writeUrl, null, "GET", null, Encoding.GetEncoding("EUC-KR"));
                    statusMessageBox_AppendText("글쓰기 상태로...", Color.DarkOrange);
                    if (this.textBox4.Text.IndexOf("로그인하지 않으셨습니다.") > -1)
                    {
                        this.delay = 3;
                        this.level = step.start;
                    }
                    else if (this.textBox4.Text.IndexOf("</table><script>alert(") > -1)
                    {
                        if (this.textBox4.Text.IndexOf("실명확인이 되지 않은 회원은 글을 쓰실 수 없습니다. 실명인증해주세요") > -1)
                        {
                            idChage = true;
                            this.delay = 60;
                            this.level = step.f;
                            systemMessageBox_AppendText(string.Format("{0} > {1} {2} 실명확인이 되지 않은 계정", new object[] { getMyIp(), popid, pw }), Color.DarkOrange);
                        }
                        else
                        {
                            this.delay = Convert.ToInt32(this._split(this.textBox4.Text, "</table><script>alert('", "초후에 글을 쓰실 수 있습니다.');")) + 7;
                            systemMessageBox_AppendText(string.Format("{0} > {1} {2} {3}초후에 글을 쓰실 수 있습니다.", new object[] { getMyIp(), popid, pw, delay }), Color.DarkOrange);
                        }
                    }
                    else
                    {
                        try
                        {
                            this.funny = this.a(this.textBox4.Text, "name=funny value='", "'>");
                            if (funny == null)
                            {
                                oddCount++;
                                if (oddCount >= 3)
                                {
                                    oddCount = 0;
                                    this.delay = 5;
                                    this.level = step.f;
                                    statusMessageBox_AppendText("funny 인식 오류", Color.DarkOrange);
                                    break;
                                }
                                this.delay = 5;
                                this.level = step.f;
                                statusMessageBox_AppendText("funny 인식 오류", Color.DarkOrange);
                                break;
                            }
                            this.sobj = this.a(this.textBox4.Text, "name=sobj value='", "'>");
                            if (sobj == null)
                            {
                                oddCount++;
                                if (oddCount >= 3)
                                {
                                    oddCount = 0;
                                    this.delay = 5;
                                    this.level = step.d;
                                    statusMessageBox_AppendText("sobj 인식 오류", Color.DarkOrange);
                                    break;
                                }
                                this.delay = 5;
                                this.level = step.f;
                                statusMessageBox_AppendText("sobj 인식 오류", Color.DarkOrange);
                                break;
                            }
                            this.imageCodeUrl = this.a(this.textBox4.Text, "<img border=1 src=\"", "\" align=absmiddle>");
                            if (imageCodeUrl == null)
                            {
                                oddCount++;
                                if (oddCount >= 3)
                                {
                                    oddCount = 0;
                                    this.delay = 5;
                                    this.level = step.f;
                                    statusMessageBox_AppendText("imageCodeUrl 인식 오류", Color.DarkOrange);
                                    break;
                                }
                                this.delay = 5;
                                this.level = step.f;

                                statusMessageBox_AppendText("imageCodeUrl 인식 오류", Color.DarkOrange);
                                break;
                            }

                            Bitmap bitmap = a(this.imageCodeUrl, null, "GET", (string)null);
                            imageCodePictureBox.Image = bitmap;
                            bitmap.Save("c:\\out.bmp");
                            statusMessageBox_AppendText("이미지 인식 중...", Color.DarkOrange);
                            /*
                            if (deCaptchaType.Contains("de-captcha"))
                            {
                                if (deCaptcha())
                                {
                                    if (this.imageCode.Text.Length > 3)
                                    {
                                        this.imageCode.Text = this.imageCode.Text.Substring(0, 3);
                                    }
                                    this.delay = 5;
                                    this.level = step.e;
                                }
                            }
                            else if (deCaptchaType.Contains("deathbycaptcha"))
                            {
                                if (_DeathByCaptcha())
                                {
                                    if (this.imageCode.Text.Length > 3)
                                    {
                                        this.imageCode.Text = this.imageCode.Text.Substring(0, 3);
                                    }
                                    this.delay = 5;
                                    this.level = step.e;
                                }
                            }
                            else if (deCaptchaType.Contains("ByPassCaptcha"))
                            {
                                if (_DeathByCaptcha())
                                {
                                    if (this.imageCode.Text.Length > 3)
                                    {
                                        this.imageCode.Text = this.imageCode.Text.Substring(0, 3);
                                    }
                                    this.delay = 5;
                                    this.level = step.e;
                                }
                            }
                            else if (deCaptchaType.Contains("localCaptcha"))
                            {

                            }
                            */
                            if (getLocalCaptcha())
                            {
                                if (this.imageCode.Text.Length > 3)
                                {
                                    this.imageCode.Text = this.imageCode.Text.Substring(0, 3);
                                }
                                this.delay = 5;
                                this.level = step.e;
                            }
                            statusMessageBox_AppendText("이미지 인식 완료...", Color.DarkOrange);
                        }
                        catch (FormatException exception)
                        {
                            Console.WriteLine(exception);
                            this.delay = 5;
                            this.level = step.f;
                            statusMessageBox_AppendText("이미지 인식 오류", Color.DarkOrange);
                        }
                        catch (NullReferenceException exception)
                        {
                            Console.WriteLine(exception);
                            this.delay = 5;
                            this.level = step.f;
                            statusMessageBox_AppendText("이미지 인식 오류", Color.DarkOrange);
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                            this.delay = 5;
                            this.level = step.f;
                            statusMessageBox_AppendText("이미지 인식 오류", Color.DarkOrange);
                        }
                    }
                    break;
                case step.e:
                    String textSubject = "";
                    String textComment = "";
                    if (writelevel == 0)
                    {
                        textSubject = textBoxSubject.Text;
                        textComment = textBoxComment.Text;
                    }
                    else if (writelevel == 1)
                    {
                        textSubject = textBoxSubject1.Text;
                        textComment = textBoxComment1.Text;
                    }
                    else if (writelevel == 2)
                    {
                        textSubject = textBoxSubject2.Text;
                        textComment = textBoxComment2.Text;
                    }
                    else
                    {
                        textSubject = textBoxSubject.Text;
                        textComment = textBoxComment.Text;
                    }
                    String subject = System.Web.HttpUtility.UrlEncode(textSubject, System.Text.Encoding.GetEncoding("euc-kr"));
                    String comment = System.Web.HttpUtility.UrlEncode(textComment, System.Text.Encoding.GetEncoding("euc-kr"));
                    textBox4.Text = loadingPage(writePostUrl, writeUrl, "POST", string.Format("part=0&subject={0}&comment={1}&imagecode={2}&funny={3}&sobj={4}", subject, comment, imageCode.Text, funny, sobj), Encoding.GetEncoding("EUC-KR"));

                    if (this.textBox4.Text.IndexOf("도배방지 코드가 일치하지 않습니다.") > -1)
                    {
                        try
                        {
                            statusMessageBox_AppendText("방지 코드 불일치...", Color.DarkOrange);
                            this.delay = 5;
                            this.level = step.d;
                            systemMessageBox_AppendText(string.Format("{0} > {1} {2} {3}", new object[] { this.getMyIp(), popid, pw, this.a(this.textBox4.Text, "alert(", ")") }), Color.DarkRed);
                            break;
                        }
                        catch (FormatException formatexception)
                        {
                            Console.WriteLine(formatexception);
                            break;
                        }
                    }
                    else if (this.textBox4.Text.IndexOf("비정상적인 접근입니다.") > -1)
                    {
                        try
                        {
                            statusMessageBox_AppendText("비정상적인 접근...", Color.DarkOrange);
                            /*
                            int rowIndex = -1;
                            foreach (DataGridViewRow row in completedataGridView.Rows)
                            {
                                if (row.Cells[0].Value.ToString().Equals(getMyIp()))
                                {
                                    rowIndex = row.Index;
                                    break;
                                }
                            }
                            if (rowIndex < 0)
                            {
                                writingIp(getMyIp(), "failed", popid);
                            }
                            */
                            abnormality++;
                            delay = 60;
                            level = step.g;
                            systemMessageBox_AppendText(string.Format("{0}회 {1} > {2} {3} {4}", new object[] { abnormality, getMyIp(), popid, pw, this.a(this.textBox4.Text, "alert(", ")") }), Color.DarkRed);

                            //idChage = true;
                            if (isLogIn)
                            {
                                textBox4.Text = loadingPage(logoutUrl, null, "GET", null, Encoding.GetEncoding("EUC-KR"));
                                isLogIn = false;
                            }

                            if (abnormality > 9)
                            {
                                abnormality = 0;
                                idChage = true;
                            }
                            break;
                        }
                        catch (FormatException formatexception)
                        {
                            Console.WriteLine(formatexception);
                            break;
                        }
                    }
                    else if (this.textBox4.Text.IndexOf("금일 글쓰기 횟수가 초과했습니다.") > -1)
                    {
                        try
                        {
                            writingFullDay = sDayofYear;

                            String updatePopLinID = saveUrl + "updatePopLinID.php?";
                            updatePopLinID
                                 += "database=" + textBoxDBName.Text
                                + "&username=" + textBoxID.Text
                                + "&password=" + textBoxPW.Text
                                + "&popid=" + popid
                                + "&writingFullDay=" + writingFullDay
                                ;
                            generateURL(updatePopLinID);

                            idChage = true;
                            this.delay = 60;
                            this.level = step.f;
                            systemMessageBox_AppendText(string.Format("{0} > {1} {2} {3}", new object[] { this.getMyIp(), popid, pw, this.a(this.textBox4.Text, "alert(", ")") }), Color.DarkRed);
                            break;
                        }
                        catch (FormatException formatexception)
                        {
                            Console.WriteLine(formatexception);
                            break;
                        }
                    }
                    else if (this.textBox4.Text.IndexOf("초 후에 글을 쓰실 수 있습니다.") > -1)
                    {
                        string str2 = "<script>alert('";
                        this.delay = Convert.ToInt32(this.textBox4.Text.Substring(str2.Length, this.textBox4.Text.IndexOf("초 후에 글을 쓰실 수 있습니다.") - str2.Length)) + 8;
                        this.level = step.d;
                        systemMessageBox_AppendText(string.Format("{0} > {1} {2} {3}", new object[] { this.getMyIp(), popid, pw, this.a(this.textBox4.Text, "alert(", ")") }), Color.DarkRed);
                        break;
                    }
                    else if (this.textBox4.Text.IndexOf("접근금지된 회원입니다.") > -1)
                    {
                        String updateUrl = saveUrl + "saveSql.php?";
                        updateUrl
                            += "username=" + textBoxID.Text
                            + "&password=" + textBoxPW.Text
                            + "&database=" + textBoxDBName.Text
                            + "&tablename=" + textBoxTableName.Text
                            + "&lawbanday=1000"
                            + "&popid=" + popid;
                        generateURL(updateUrl);

                        idChage = true;
                        this.delay = 60;
                        this.level = step.f;
                        systemMessageBox_AppendText(string.Format("{0} > {1} {2} {3}", new object[] { this.getMyIp(), popid, pw, this.a(this.textBox4.Text, "alert(", ")") }), Color.DarkRed);
                        break;
                    }
                    else if (this.textBox4.Text.IndexOf("규칙위반으로 접근금지 상태입니다") > -1)
                    {
                        String updateUrl = saveUrl + "saveSql.php?";
                        updateUrl
                            += "username=" + textBoxID.Text
                            + "&password=" + textBoxPW.Text
                            + "&database=" + textBoxDBName.Text
                            + "&tablename=" + textBoxTableName.Text
                            + "&lawbanday=1000"
                            + "&popid=" + popid;
                        generateURL(updateUrl);

                        idChage = true;
                        this.delay = 60;
                        this.level = step.f;
                        systemMessageBox_AppendText(string.Format("{0} > {1} {2} {3}", new object[] { this.getMyIp(), popid, pw, this.a(this.textBox4.Text, "alert(", ")") }), Color.DarkRed);
                        break;
                    }
                    else if (this.textBox4.Text.IndexOf("정상적으로 작성하여 주시기 바랍니다") > -1)
                    {
                        idChage = true;
                        this.delay = 3;
                        this.level = step.d;
                        systemMessageBox_AppendText(string.Format("{0} > {1} {2} {3}", new object[] { this.getMyIp(), popid, pw, this.a(this.textBox4.Text, "alert(", ")") }), Color.DarkRed);
                        break;
                    }
                    else if (this.textBox4.Text.IndexOf("금칙어가 포함되어 있습니다") > -1)
                    {
                        this.BackColor = System.Drawing.Color.MediumVioletRed;
                        this.delay = 180;
                        this.level = step.f;
                        systemMessageBox_AppendText(string.Format("{0} > {1} {2} {3}", new object[] { this.getMyIp(), popid, pw, this.a(this.textBox4.Text, "alert(", ")") }), Color.DarkRed);
                        break;
                    }
                    else
                    {
                        statusMessageBox_AppendText("글쓰기 완료...", Color.DarkRed);

                        this.success_writing++;
                        //this.b2++;
                        this.금일_남은_글쓰기_횟수 = Convert.ToInt32(this.a(this.textBox4.Text, "금일 남은 글쓰기 횟수:", ","));
                        this.금일_아이피_남은_글쓰기_횟수 = Convert.ToInt32(this.a(this.textBox4.Text, "아이피:", " '"));

                        //SP.SoundLocation = @"sound.wav";
                        //SP.Play();

                        if (this.금일_남은_글쓰기_횟수 <= 0)
                        {
                            //this.b2 = 0;
                            this.delay = 15;
                            this.level = step.f;
                            /*
                            if (!this.ca.ContainsKey(this.e.Items[this.bw].SubItems[0].Text))
                            {
                                a.a().e(this.e.Items[this.bw].SubItems[0].Text, this.e.Items[this.bw].SubItems[1].Text, this.cd);
                                this.ca.Add(this.e.Items[this.bw].SubItems[0].Text, this.e.Items[this.bw].SubItems[1].Text);
                                e.a(Application.StartupPath, "사용한아이디", this.e.Items[this.bw].SubItems[0].Text + " " + this.e.Items[this.bw].SubItems[1].Text, false);
                            }
                            */
                        }
                        else if (this.금일_아이피_남은_글쓰기_횟수 <= 0)
                        {
                            this.delay = 3;
                            this.level = step.g;
                            //e.a(Application.StartupPath, "사용한아이피", this.b(), false);
                        }
                        else
                        {
                            //int num2 = Convert.ToInt32(DateTime.Now.ToString("HH"));
                            int num3 = 0x5f;
                            /*
                            if ((Convert.ToInt32(this.bf.Text) <= num2) && (num2 < Convert.ToInt32(this.be.Text)))
                            {
                                num3 = Convert.ToInt32(this.bc.Text);
                                this.ae.Text = num2 + " 1 " + num3;
                            }
                            else if ((Convert.ToInt32(this.bb.Text) <= num2) && (num2 < Convert.ToInt32(this.ba.Text)))
                            {
                                num3 = Convert.ToInt32(this.a8.Text);
                                this.ae.Text = num2 + " 2 " + num3;
                            }
                            else if ((Convert.ToInt32(this.a7.Text) <= num2) && (num2 < Convert.ToInt32(this.a5.Text)))
                            {
                                num3 = Convert.ToInt32(this.a1.Text);
                                this.ae.Text = num2 + " 3 " + num3;
                            }
                            else if ((Convert.ToInt32(this.a6.Text) <= num2) && (num2 < Convert.ToInt32(this.a4.Text)))
                            {
                                num3 = Convert.ToInt32(this.a0.Text);
                                this.ae.Text = num2 + " 4 " + num3;
                            }
                            else
                            {
                                this.ae.Text = num2 + " 5 " + num3;
                            }
                            */
                            this.delay = num3;
                            this.level = step.d;
                        }
                    }
                    abnormality = 0;
                    completedataGridView.Rows.Add(
                    success_writing,
                    popid,
                    getTime()
                    );

                    // 글쓰기를 완료하였다면 arraylist 에 추가한다.
                    writingIp(textBox5.Text, "success", popid);

                    String successWritingSaveUrl = saveUrl + "SuccessWriting.php?";
                    successWritingSaveUrl
                         += "database=" + textBoxDBName.Text
                        + "&username=" + textBoxID.Text
                        + "&password=" + textBoxPW.Text
                        + "&popid=" + popid
                        + "&servername=" + serverNameTextBox.Text
                        + "&subject=" + textSubject
                        + "&pcname=" + SystemInformation.ComputerName
                        ;
                    generateURL(successWritingSaveUrl);

                    writelevel++;

                    if (writelevel > 2)
                    {
                        writelevel = 0;
                    }

                    textBox3.Text = success_writing + "회";
                    systemMessageBox_AppendText(
                        string.Format(
                            "{0} > {1} 아이피 : {2} 아이디 : {4} 갯수 {3} / {5}초",
                        new object[] {
                                getMyIp(),
                                popid,
                                금일_아이피_남은_글쓰기_횟수,
                                success_writing,
                                금일_남은_글쓰기_횟수,
                                writingDelay
                        }
                        ),
                        Color.Lime
                        );

                    if (isLogIn)
                    {
                        isLogIn = false;
                        textBox4.Text = loadingPage(logoutUrl, null, "GET", null, Encoding.GetEncoding("EUC-KR"));
                    }


                    if (금일_남은_글쓰기_횟수 <= 0)
                    {
                        idChage = true;
                    }
                    this.delay = writingDelay;
                    this.level = step.f;
                    break;
                case step.f:
                case step.g:
                    if (autoReloadBulletinCheckBox.Checked)
                    {
                        loadingSubjectAndComment();
                    }

                    if (isLogIn)
                    {
                        statusMessageBox_AppendText("로그아웃 완료...", Color.DarkOrange);

                        isLogIn = false;
                        textBox4.Text = loadingPage(logoutUrl, null, "GET", null, Encoding.GetEncoding("EUC-KR"));
                    }

                    // 홍보 완료된 gridview의 row 삭제
                    //dataGridView2.Rows.RemoveAt(dataGridView2.Rows[0].Index);
                    //int rowsCount = dataGridView2.Rows.Count;
                    //textBox7.Text = rowsCount + "개";

                    if (idChage)
                    {
                        abnormality = 0;
                        _popNumber++;
                        idChage = false;
                    }

                    if (list.Count <= _popNumber)
                    {

                        _popNumber = 0;

                        loadingPop();

                        while (list == null)
                        {
                            loadingPop();
                            Thread.Sleep(1000);
                        }

                        systemMessageBox_AppendText(string.Format("{0}", new object[] { "아이디를 이미 다 사용하여 아이디를 새롭게 불러옵니다.." }), Color.DarkRed);
                        statusMessageBox_AppendText("아이디를 이미 다 사용하여 아이디를 새롭게 불러옵니다.", Color.DarkRed);
                    }

                    //proxyNumber++;

                    pcSave();
                    macChange();

                    popid = list[_popNumber]._popid;
                    pw = list[_popNumber]._pw;

                    this.delay = 3;
                    this.level = step.start;

                    break;
            }
            sCheck = true;
            timer1.Enabled = true;
        }

        private void pcSave()
        {
            String pcSave = saveUrl + "pcsave.php?";
            pcSave
                += "username=" + textBoxID.Text
                + "&password=" + textBoxPW.Text
                + "&database=" + textBoxDBName.Text
                + "&tablename=adstatus"
                + "&pcname=proxyClient" /*+ SystemInformation.ComputerName*/
                + "&ipAddress=" + textBox5.Text
                + "&adnumber=" + success_writing
                + "&servername=" + serverNameTextBox.Text
                + "&version=" + now_version;

            generateURL(pcSave);
        }

        public string _split(string A_0, string A_1, string A_2)
        {
            string str = null;
            try
            {
                int startIndex = 0;
                int index = 0;
                startIndex = A_0.Trim().IndexOf(A_1, startIndex);
                index = A_0.Trim().IndexOf(A_2, startIndex);
                if ((startIndex > -1) && (index > -1))
                {
                    str = A_0.Substring(startIndex + A_1.Length, (index - startIndex) - A_1.Length);
                }
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Console.WriteLine(exception);
                return "";
            }
            catch (Exception)
            {
                return "";
            }
            return str;
        }

        public string _split(string A_0, string A_1, string A_2, int A_3)
        {
            int startIndex = 0;
            int index = 0;
            try
            {

                startIndex = A_0.IndexOf(A_1, A_3);
                if (startIndex < 0)
                {
                    return "";
                }
                index = A_0.IndexOf(A_2, startIndex);
                if (index < 0)
                {
                    return "";
                }

            }
            catch (ArgumentOutOfRangeException exception)
            {
                Console.WriteLine(exception);
                return "";
            }
            return A_0.Substring(startIndex + A_1.Length, (index - startIndex) - A_1.Length);
        }

        public static bool HasConnection()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string getMyIp()
        {

            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            textBox5.Text = localIP;
            return localIP;

        }

        private Boolean getLocalCaptcha()
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("id", "TTR");
            nvc.Add("btn-submit-photo", "Upload");
            HttpUploadFile("http://112.173.174.189/gsa_test.gsa",
                 @"C:\out.bmp", "file", "image/x-png", nvc);
            if (imageCode.Text.Length != 3)
            {
                return false;
            }
            return true;
        }

        public String HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                String readtoend = reader2.ReadToEnd().ToString();
                imageCode.Text = _split(readtoend, "<html><head><title>GSA CB Result</title></head><body><h1>The solution is :: <span id=\"captcha_result\">", "</span></h1></body></html>");
            }
            catch (Exception)
            {
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                    return "";
                }
            }
            finally
            {
                wr = null;
            }
            return imageCode.Text;
        }
        /*
        private Boolean _DeathByCaptcha()
        {
            string dbcUsername = "sentimentalhoon", dbcPassword = "tkfkdgo1!";
            try
            {
                // Put your ENDCAPTCHA username & password here.
                //Client client = (Client)new HttpClient(dbcUsername, dbcPassword);
                DeathByCaptcha.Client client = (DeathByCaptcha.Client)new DeathByCaptcha.SocketClient(dbcUsername, dbcPassword);


                if (client.Balance <= 0)
                {
                    Console.WriteLine("Your balance is {0:F2} US cents", client.Balance);
                    systemMessageBox_AppendText(string.Format("Your balance is {0:F2} US cents", client.Balance), Color.Tan);
                    return false;
                }
                else
                {
                    Console.WriteLine("Your balance is {0:F2} US cents", client.Balance);
                    systemMessageBox_AppendText(string.Format("Your balance is {0:F2} US cents", client.Balance), Color.Tan);
                }
                // Upload a CAPTCHA and poll for its status.  Put the CAPTCHA
                // image file name, file object, stream, or a vector of bytes,
                // and desired solving timeout (in seconds) here.  If solved,
                // you'll receive a DeathByCaptcha.Captcha object.
                DeathByCaptcha.Captcha captcha = client.Decode("c:\\out.bmp", DeathByCaptcha.Client.DefaultTimeout);
                if (captcha.Solved && captcha.Correct)
                {
                    Console.WriteLine("CAPTCHA {0:D} solved: {1}",
                                      captcha.Id, captcha.Text);
                    systemMessageBox_AppendText(string.Format("CAPTCHA {0:D} solved: {1}", captcha.Id, captcha.Text), Color.Tan);
                    if (captcha.Text.Length == 3)
                    {
                        //systemMessageBox_AppendText("자동 방지 코드 인식 완료 : " + captcha.Text, Color.Tan);
                        imageCode.Text = captcha.Text;
                    }
                    else
                    {
                        imageCode.Text = "XXX";
                        systemMessageBox_AppendText("자동 방지 코드 인식 실패 ㅠㅠ 재도전하겠습니다. : ", Color.Aqua);
                        return false;
                    }
                    // Report an incorrectly solved CAPTCHA.  Make sure the
                    // CAPTCHA was in fact incorrectly solved, do not just
                    // report them all or at random, or you might be banned
                    // as abuser.

                   
                    //if (false)
                   // {
                   //     if (client.Report(captcha))
                    //    {
                   //         Console.WriteLine("Reported as incorrectly solved");
                   //     }
                   //     else
                   //     {
                   //         Console.WriteLine("Failed reporting as incorrectly solved");
                   //     }
                  //  }
                }
                else
                {
                    Console.WriteLine("CAPTCHA was not solved");
                }
                Console.WriteLine("Your balance is {0:F2} US cents", client.Balance);
            }
            catch (DeathByCaptcha.AccessDeniedException e)
            {
                imageCode.Text = "XXX";
                systemMessageBox_AppendText("자동 방지 코드 인식 실패 ㅠㅠ 재도전하겠습니다. : ", Color.Aqua);
                Console.WriteLine(e);
                return false;
            }
            catch (DeathByCaptcha.ServiceOverloadException e)
            {
                imageCode.Text = "XXX";
                systemMessageBox_AppendText("자동 방지 코드 인식 실패 ㅠㅠ 재도전하겠습니다. : ", Color.Aqua);
                Console.WriteLine(e);
                return false;
            }
            catch (NullReferenceException e)
            {
                imageCode.Text = "XXX";
                systemMessageBox_AppendText("자동 방지 코드 인식 실패 ㅠㅠ 재도전하겠습니다. : ", Color.Aqua);
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
    */
        /*
        [STAThread]
        private Boolean deCaptcha()
        {
            try
            {

                string login = "sentimentalhoon";
                string password = "tkfkdgo11";
                int port = 3500;

                FileStream fs = new FileStream("c:\\out.bmp", FileMode.Open);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                //Construct object
                decaptcher_class dec = new decaptcher_class(login, password, port);

                //Get balance
                decimal bal = dec.Get_Balance();
                systemMessageBox_AppendText("현재 남은 밸런스 : $" + bal, Color.LightGoldenrodYellow);
                //Check error code
                if (DecaptureError.ccERR_NONE != dec.Error_Text)
                {
                    return false;
                }

                //Solve CAPTCHA
                string ans = null;
                ans = dec.RecognizePicture(buffer);
                if (ans.Length == 3)
                {
                    systemMessageBox_AppendText("자동 방지 코드 인식 완료 : " + ans, Color.Tan);
                    imageCode.Text = ans;
                }
                else
                {
                    imageCode.Text = "XXX";
                    systemMessageBox_AppendText("자동 방지 코드 인식 실패 ㅠㅠ 재도전하겠습니다. : ", Color.Aqua);
                    return false;
                }


                //Check error code
                if (DecaptureError.ccERR_NONE != dec.Error_Text)
                {
                    return false;
                }


                //If the CAPTCHA has been solved incorrectly, report it
                // dec.BadPicture(dec.major, dec.minor);

            }
            catch (System.Net.Sockets.SocketException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        */

        private Boolean needupdateversionCheck()
        {
            try
            {
                string loadingPopURL = saveUrl + "loadingVersion.php";
                Double updateVersion = Convert.ToDouble(loadingPage(loadingPopURL, null, "GET", null, Encoding.GetEncoding("EUC-KR")));
                if (updateVersion > now_version)
                {
                    return true;
                }
                return false;
            }
            catch (FormatException)
            {
                return false;
            } 
            catch (Exception)
            {
                return false;
            }
        }

        private void macAddressChangeBtn_Click(object sender, EventArgs e)
        {
            macChange();
        }

        private void macChange()
        {
            _init();
            statusMessageBox_AppendText("아이피 변경 중...", Color.MediumPurple);
            int _count = 0;
            String nicId = selectLanCard.GetItemText(selectLanCard.SelectedItem).Split('|')[0];
            //statusMessageBox_AppendText(nicId, Color.YellowGreen);
            String ip = getMyIp();
            statusMessageBox_AppendText(nicId + " [전] IP : " + ip, Color.YellowGreen);
            String rndMac = GenerateMACAddress(wireLessCheck.Checked);
            //statusMessageBox_AppendText("rndMac " + rndMac, Color.YellowGreen);
            if (SetMAC(nicId, rndMac))
            {
                while (true)
                {
                    if (HasConnection())
                    {
                        break;
                    }
                    if (_count > 20)
                    {
                        _count = 0;
                        //systemMessageBox_AppendText("맥주소 변경 후 아이피를 할당받지 못해 다시 맥주소를 설정하겠습니다.", Color.GreenYellow);
                        break;
                    }
                    Thread.Sleep(1000);
                    _count++;
                }

                ip = getMyIp();
                statusMessageBox_AppendText(nicId + " [후] IP : " + ip, Color.GreenYellow);

                textBox5.Text = ip;
                statusMessageBox_AppendText("아이피 변경 완료...", Color.MediumPurple);

                systemMessageBox.Text = "";
                statusMessageBox.Text = "";

                oddCount = 0;
            }
            else
            {
                // MessageBox.Show("맥 어드레스 변경에 실패하였습니다. 다시 변경을 하여 주시기 바랍니다.");
            }
        }

        private void statusMessageBox_AppendText(String s, Color _color)
        {
            try
            {
                statusTextBox.Text = s;
                statusMessageBox.SelectionColor = _color;
                statusMessageBox.AppendText(getTime() + s + "\r\n");
                statusMessageBox.SelectionStart = statusMessageBox.Text.Length;
                statusMessageBox.ScrollToCaret();
            }
            catch (FormatException formatexception)
            {
                Console.WriteLine(formatexception);
            }

        }
        private void systemMessageBox_AppendText(String s, Color _color)
        {
            try
            {
                systemMessageBox.SelectionColor = _color;
                systemMessageBox.AppendText(getTime() + s + "\r\n");
                systemMessageBox.SelectionStart = systemMessageBox.Text.Length;
                systemMessageBox.ScrollToCaret();
            }
            catch (FormatException formatexception)
            {
                Console.WriteLine(formatexception);
            }

        }

        private String getDateTime()
        {
            return "[" + DateTime.Now.ToString("MM-dd HH:mm:ss") + "] ";
        }

        private String getTime()
        {
            return "[" + DateTime.Now.ToString("HH:mm") + "] ";
        }

        private String getTimeHour()
        {
            return DateTime.Now.ToString("HH");
        }

        // mac address change
        private const string baseReg =
        @"SYSTEM\CurrentControlSet\Control\Class\{4D36E972-E325-11CE-BFC1-08002bE10318}\";

        public static bool SetMAC(string nicid, string newmac)
        {
            bool ret = false;
            try
            {

                using (RegistryKey bkey = GetBaseKey())
                using (RegistryKey key = bkey.OpenSubKey(baseReg + nicid, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))
                {
                    if (key != null)
                    {
                        key.SetValue("NetworkAddress", newmac, RegistryValueKind.String);

                        ManagementObjectSearcher mos = new ManagementObjectSearcher(
                            new SelectQuery("SELECT * FROM Win32_NetworkAdapter WHERE Index = " + nicid));

                        foreach (ManagementObject o in mos.Get().OfType<ManagementObject>())
                        {
                            o.InvokeMethod("Disable", null);
                            o.InvokeMethod("Enable", null);
                            ret = true;
                        }
                    }
                }
            }
            catch (System.Management.ManagementException e)
            {
                Console.WriteLine(e);
                return false;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e);
                return false;
            }
            return ret;
        }

        private void GetNicIds()
        {
            using (RegistryKey bkey = GetBaseKey())
            using (RegistryKey key = bkey.OpenSubKey(baseReg))
            {
                if (key != null)
                {
                    foreach (string name in key.GetSubKeyNames().Where(n => n != "Properties"))
                    {
                        using (RegistryKey sub = key.OpenSubKey(name))
                        {
                            if (sub != null)
                            {
                                object busType = sub.GetValue("BusType");
                                string busStr = busType != null ? busType.ToString() : string.Empty;
                                if (busStr != string.Empty)
                                {
                                    String driverDesc = sub.GetValue("DriverDesc").ToString();
                                    //textBox4_AppendText(driverDesc + " ||| " + name);
                                    selectLanCard.Items.Add(name + "|" + driverDesc);
                                }
                            }
                        }
                    }
                    selectLanCard.SelectedIndex = 0;
                }
            }
        }

        public static RegistryKey GetBaseKey()
        {
            return RegistryKey.OpenBaseKey(
                RegistryHive.LocalMachine,
                InternalCheckIsWow64() ? RegistryView.Registry64 : RegistryView.Registry32);
        }

        static bool is64BitProcess = (IntPtr.Size == 8);
        static bool is64BitOperatingSystem = is64BitProcess || InternalCheckIsWow64();

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process
        );

        public static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
                Environment.OSVersion.Version.Major >= 6)
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    bool retVal;
                    if (!IsWow64Process(p.Handle, out retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            else
            {
                return false;
            }
        }

        public static string GenerateMACAddress(Boolean wireLessCheck)
        {
            var sBuilder = new StringBuilder();
            var r = new Random();
            int number;
            byte b;
            if (wireLessCheck)
            {
                sBuilder.Append("02");
                for (int i = 0; i < 5; i++)
                {
                    number = r.Next(0, 255);
                    b = Convert.ToByte(number);
                    if (i == 0)
                    {
                        b = setBit(b, 6); //--> set locally administered
                        b = unsetBit(b, 7); // --> set unicast 
                    }
                    sBuilder.Append(number.ToString("X2"));
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    number = r.Next(0, 255);
                    b = Convert.ToByte(number);
                    if (i == 0)
                    {
                        b = setBit(b, 6); //--> set locally administered
                        b = unsetBit(b, 7); // --> set unicast 
                    }
                    sBuilder.Append(number.ToString("X2"));
                }
            }
            return sBuilder.ToString().ToUpper();
        }

        private static byte setBit(byte b, int BitNumber)
        {
            if (BitNumber < 8 && BitNumber > -1)
            {
                return (byte)(b | (byte)(0x01 << BitNumber));
            }
            else
            {
                throw new InvalidOperationException(
                "Der Wert für BitNumber " + BitNumber.ToString() + " war nicht im zulässigen Bereich! (BitNumber = (min)0 - (max)7)");
            }
        }

        private static byte unsetBit(byte b, int BitNumber)
        {
            if (BitNumber < 8 && BitNumber > -1)
            {
                return (byte)(b | (byte)(0x00 << BitNumber));
            }
            else
            {
                throw new InvalidOperationException(
                "Der Wert für BitNumber " + BitNumber.ToString() + " war nicht im zulässigen Bereich! (BitNumber = (min)0 - (max)7)");
            }
        }

        /// <summary>
        /// 로컬 IP를 구해온다.
        /// </summary>
        /// <param name="index">랜카드 인덱스</param>
        /// <returns>IP</returns>
        public static string GetLocalIp(int index)
        {
            return System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[index].ToString();
        }

        /// <summary>
        /// 첫번째 로컬 IP를 구해온다.
        /// </summary>
        /// <returns>IP</returns>
        public static string GetLocalIp()
        {
            return GetLocalIp(0);
        }


        /// <summary>
        /// IP Address에 해당하는 Mac Address를 가져온다.
        /// </summary>
        /// <param name="ip">찾고자 하는 IP Address</param>
        /// <returns>Mac Address</returns>
        public static string GetMacAddress(string ip)
        {
            string rtn = string.Empty;
            ObjectQuery oq = new ObjectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled='TRUE'");
            ManagementObjectSearcher query1 = new ManagementObjectSearcher(oq);
            foreach (ManagementObject mo in query1.Get())
            {
                string[] address = (string[])mo["IPAddress"];
                if (address[0] == ip && mo["MACAddress"] != null)
                {
                    rtn = mo["MACAddress"].ToString();
                    break;
                }
            }
            return rtn;
        }

        /// <summary>
        /// 첫번째 IPv4 값을 가져온다.
        /// </summary>
        /// <returns>못찾으면 null</returns>
        public string GetFirstIPv4()
        {
            Regex regex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");

            foreach (System.Net.IPAddress ip in System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList)
            {
                if (regex.IsMatch(ip.ToString()))
                {
                    return ip.ToString();
                }
            }

            return null;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
        [FlagsAttribute]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }
        public static void PreventScreenAndSleep()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS |
                                    EXECUTION_STATE.ES_SYSTEM_REQUIRED |
                                    EXECUTION_STATE.ES_AWAYMODE_REQUIRED |
                                    EXECUTION_STATE.ES_DISPLAY_REQUIRED);
        }
        public static void AllowMonitorPowerdown()
        {
            Console.WriteLine(SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS));
        }
        private void generateURL(string url)
        {
            try
            {
                //systemMessageBox_AppendText(url, Color.AntiqueWhite);
                string html = string.Empty;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }
                systemMessageBox_AppendText(html, Color.Ivory);
                Console.WriteLine(html);
            }
            catch (WebException e)
            {
                Console.WriteLine(e);
            }
        }

        public string a(Bitmap A_0)
        {
            string str = string.Empty;
            MemoryStream stream = new MemoryStream();
            EncoderParameters encoderParams = new EncoderParameters(1);
            EncoderParameter parameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
            encoderParams.Param[0] = parameter;
            ImageCodecInfo[] imageDecoders = ImageCodecInfo.GetImageDecoders();
            ImageCodecInfo encoder = null;
            foreach (ImageCodecInfo info2 in imageDecoders)
            {
                if (info2.FormatID == ImageFormat.Png.Guid)
                {
                    encoder = info2;
                }
            }
            A_0.Save(stream, encoder, encoderParams);
            stream.Position = 0L;
            byte[] inArray = stream.ToArray();
            stream.Close();
            str = Convert.ToBase64String(inArray);
            inArray = null;
            return str;
        }
        public string a(string A_0, string A_1, string A_2)
        {
            string str = null;
            try
            {
                int startIndex = 0;
                int index = 0;
                startIndex = A_0.Trim().IndexOf(A_1, startIndex);
                index = A_0.Trim().IndexOf(A_2, startIndex);
                if ((startIndex > -1) && (index > -1))
                {
                    str = A_0.Substring(startIndex + A_1.Length, (index - startIndex) - A_1.Length);
                }
            }
            catch (Exception)
            {
            }
            return str;
        }

        public byte[] b(Bitmap A_0)
        {
            if (A_0 == null)
            {
                return null;
            }
            MemoryStream stream = new MemoryStream();
            A_0.Save(stream, ImageFormat.Png);
            return stream.ToArray();
        }

        private int _substr1 = 0;
        private int _substr2 = 0;
        private HttpWebRequest _webRequest;
        private HttpWebResponse _webResponse;
        public static CookieContainer _cookieContainer;
        public static CookieCollection _cookieCollection;
        private Stream _responseStream;
        private StreamReader _streamReader;
        //private StreamWriter g;
        private string loadingPageHtml;

        public void _init()
        {
            _cookieContainer = new CookieContainer();
            _cookieCollection = new CookieCollection();
        }

        public Bitmap a(string A_0, string A_1, string A_2, string A_3)
        {
            Bitmap bitmap = null;
            try
            {
                if (_cookieContainer == null)
                {
                    _init();
                }
                _webRequest = WebRequest.Create(A_0) as HttpWebRequest;
                // _webRequest.Proxy = new WebProxy(MyProxyHostString, MyProxyPort);
                this._webRequest.Method = A_2;
                this._webRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
                this._webRequest.Accept = "*/*";
                this._webRequest.KeepAlive = true;
                this._webRequest.ContentType = "image/png";
                if (A_1 != null)
                {
                    this._webRequest.Referer = A_1;
                }
                this._webRequest.CookieContainer = _cookieContainer;
                
                if (SPEEDCDN != null)
                {
                    _webRequest.CookieContainer.Add(new Uri(url),
new Cookie("SPEEDCDN", SPEEDCDN.Trim()));
                }

                if (A_3 != null)
                {
                    byte[] bytes = Encoding.Default.GetBytes(A_3);
                    this._webRequest.ContentLength = bytes.Length;
                    Stream requestStream = this._webRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                _webResponse = (HttpWebResponse)_webRequest.GetResponse();
                _webResponse.Cookies = _webRequest.CookieContainer.GetCookies(_webRequest.RequestUri);
                _cookieCollection.Add(_webResponse.Cookies);
                bitmap = new Bitmap(_webResponse.GetResponseStream());
            }
            catch (WebException)
            {
            }
            catch (Exception)
            {
            }
            return bitmap;
        }

        String SPEEDCDN = null;

        public string loadingPage(string _url, string _referer, string _method, string _params, Encoding _encoding)
        {
            try
            {
                if (_cookieContainer == null)
                {
                    _init();

                }
                /*
                if (_url.Contains("https"))
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                }
                */
                _webRequest = (HttpWebRequest)WebRequest.Create(_url);
                /*
                _webRequest.Proxy = new WebProxy(MyProxyHostString, MyProxyPort);
                _webRequest.Timeout = 10000;
                _webRequest.ReadWriteTimeout = 10000;
                */
                _webRequest.Credentials = CredentialCache.DefaultCredentials;
                _webRequest.Method = _method;
                _webRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
                _webRequest.Accept = "text/html, application/xhtml+xml, */*";
                _webRequest.KeepAlive = true;
                _webRequest.ContentType = "application/x-www-form-urlencoded";
                _webRequest.ProtocolVersion = HttpVersion.Version10;


                _webRequest.AllowAutoRedirect = true;

                if (_referer != null)
                {
                    _webRequest.Referer = _referer;
                }

                _webRequest.CookieContainer = _cookieContainer;


                if (SPEEDCDN != null)
                {
                    _webRequest.CookieContainer.Add(new Uri(_url), new Cookie("SPEEDCDN", SPEEDCDN.Trim()));
                }

                if (_params != null)
                {
                    byte[] bytes = Encoding.Default.GetBytes(_params);
                    _webRequest.ContentLength = bytes.Length;
                    Stream requestStream = _webRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                /*
                using (_webResponse = (HttpWebResponse)_webRequest.GetResponse())
                {
                    Stream respPostStream = _webResponse.GetResponseStream();
                    StreamReader readerPost = new StreamReader(respPostStream, Encoding.GetEncoding("EUC-KR"), true);

                    h = readerPost.ReadToEnd();
                }
                */

                _webResponse = (HttpWebResponse)_webRequest.GetResponse();
                _webResponse.Cookies = _webRequest.CookieContainer.GetCookies(_webRequest.RequestUri);
                _cookieCollection.Add(_webResponse.Cookies);
                _responseStream = _webResponse.GetResponseStream();
                _streamReader = new StreamReader(_responseStream, _encoding, true);
                loadingPageHtml = _streamReader.ReadToEnd();

                if (_url.Equals("http://popall.com"))
                {
                    if (_webResponse.StatusCode == HttpStatusCode.OK)
                    {
                        string str1 = _split(loadingPageHtml, "X.substr(", ")", 0);
                        if (str1 != null)
                        {
                            if (!str1.Equals(""))
                            {
                                _substr1 = Convert.ToInt32(str1.Split(',')[0]);
                                _substr2 = Convert.ToInt32(str1.Split(',')[1]);

                                _cookieContainer.Add(new Cookie("pop_cnt", "done", "/", "popall.com"));
                                foreach (Cookie cookie in _webResponse.Cookies)
                                {
                                    string cookieValue = cookie.Value;
                                    string cookieName = cookie.Name;
                                    SPEEDCDN = !cookieName.Equals("SPEEDCDN") ? cookieValue : cookieValue.Substring(_substr1, _substr2);
                                    
                                    string path = "/";
                                    string domain = "popall.com";
                                    _cookieContainer.Add(new Cookie(cookieName.Trim(), SPEEDCDN.Trim(), path, domain));
                                }
                            }
                        }
                    }
                }
            }
            catch (WebException exception)
            {
                return exception.Message;
            }
            catch (Exception exception2)
            {
                return exception2.Message;
            }
            finally
            {
                if (_responseStream != null)
                {
                    _responseStream.Close();
                }
                if (_streamReader != null)
                {
                    _streamReader.Close();
                }
            }
            return loadingPageHtml;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (isStart)
            {

                list.Clear();
                isStart = false;
                popLinIDdataGridView.Rows.Clear();
                systemMessageBox_AppendText("잠시 후 종료될 예정입니다. 잠시만 기다려 주시기 바랍니다.", Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241))))));
            }
            else
            {
                systemMessageBox_AppendText("홍보 진행 상태가 아닙니다. 확인하여 주시기 바랍니다.", Color.MediumVioletRed);
            }
        }

        public static string euckr(string str)
        {
            Encoding euckr = Encoding.GetEncoding(51949);
            byte[] tmp = euckr.GetBytes(str);

            string res = "";

            foreach (byte b in tmp)
            {
                res += "%";
                res += string.Format("{0:X}", b);
            }

            return res;
        }
        private void cleanIdSelectStartButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("아직 구현되지 않은 기능입니다.");
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            systemMessageBox_AppendText("홍보 종료 예정 시간 : " + EndDateTimePicker.Text, Color.Moccasin);
        }

        private void loadingSubjectAndComment()
        {
            if (HasConnection())
            {
                string loadingPopURL = saveUrl + "loadingServerWriting.php?"
                    + "&servername=" + serverNameTextBox.Text;
                try
                {
                    string[] subjectandcomment = loadingPage(loadingPopURL, null, "GET", null, Encoding.GetEncoding("UTF-8")).Split('|');
                    textBoxSubject.Text = subjectandcomment[0];
                    textBoxComment.Text = subjectandcomment[1];
                    textBoxSubject1.Text = subjectandcomment[2];
                    textBoxComment1.Text = subjectandcomment[3];
                    textBoxSubject2.Text = subjectandcomment[4];
                    textBoxComment2.Text = subjectandcomment[5];

                    systemMessageBox_AppendText("글을 불러왔습니다.", Color.GreenYellow);
                    statusMessageBox_AppendText("글을 불러왔습니다.", Color.GreenYellow);



                }
                catch (Exception)
                {
                    systemMessageBox_AppendText("글을 불러오는 도중 에러 발생", Color.GreenYellow);
                }
            }
        }
        private void FileOpenButton_Click(object sender, EventArgs e)
        {
            if (autoReloadBulletinCheckBox.Checked)
            {
                if (serverNameTextBox.Text.Length < 2)
                {
                    MessageBox.Show("서버 이름을 입력하여 주시기 바랍니다.");
                    return;
                }
                loadingSubjectAndComment();
            }
            else
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    serverNameTextBox.Text = "";
                    textBoxSubject.Text = "";
                    textBoxComment.Text = "";
                    Encoding encode = System.Text.Encoding.GetEncoding("ks_c_5601-1987");
                    FileStream MessageFile = File.Open(openFileDialog1.FileName, FileMode.Open); // 파일을 열되 없다면 생성하고 권한은 읽기 전용으로 생성
                    StreamReader MessageStream = new StreamReader(MessageFile, encode); // 스트림 생성

                    MessageStream.BaseStream.Seek(0, SeekOrigin.Begin); // 파일을 읽어가기 시작한다.

                    int _readLine = 0;
                    while (MessageStream.Peek() > -1)
                    {
                        String ReadLine = MessageStream.ReadLine(); // String형 변수에 파일 내의 한 줄을 읽어온다.
                        if (_readLine == 0)
                        {
                            serverNameTextBox.Text = ReadLine;
                        }
                        else if (_readLine == 1)
                        {
                            textBoxSubject.Text = ReadLine;
                            textBoxSubject1.Text = ReadLine;
                            textBoxSubject2.Text = ReadLine;
                        }
                        else
                        {
                            textBoxComment.AppendText(ReadLine + "\r\n");
                            textBoxComment1.AppendText(ReadLine + "\r\n");
                            textBoxComment2.AppendText(ReadLine + "\r\n");
                        }
                        _readLine++;
                    }

                    MessageStream.Close();
                }
            }
        }

        private Boolean containsIp(String ip)
        {
            if (_writingIp.Contains(ip))
            {
                return true;
            }
            return false;
        }
        private void writingIp(String ip, String type, String id)
        {
            if (!_writingIp.Contains(ip))
            {
                _writingIp.Add(ip);
                writingIpdataGridView.Rows.Add(ip);
                StreamWriter wr = new StreamWriter(@writingIpTextFile, true);
                wr.WriteLine(ip + "|" + getTime() + "|" + type + "|" + id);
                wr.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            /*
            if (File.Exists(proxyFile)) // 파일 존재 유무, 있으면 처리한다.
            {
                proxyNumber = 0;
                FileStream _file = File.Open(proxyFile, FileMode.OpenOrCreate, FileAccess.Read); // 파일을 열되 없다면 생성하고 권한은 읽기 전용으로 생성
                StreamReader MessageStream = new StreamReader(_file); // 스트림 생성

                MessageStream.BaseStream.Seek(0, SeekOrigin.Begin); // 파일을 읽어가기 시작한다.
                proxyList = new Dictionary<int, proxyIpList>();

                while (MessageStream.Peek() > -1)
                {
                    String ReadLine = MessageStream.ReadLine(); // String형 변수에 파일 내의 한 줄을 읽어온다.
                    string[] strTexts = ReadLine.Split(new Char[] { ':' }, StringSplitOptions.None);
                        proxyList.Add(proxyNumber, new proxyIpList { _ip = strTexts[0], _port = strTexts[1] });

                        proxyDataGridView.Rows.Add(ReadLine);
                        proxyNumber++;
                                    
                }
                systemMessageBox_AppendText("총 " + proxyNumber + " 개의 프록시를 불러왔습니다", Color.AliceBlue);        
                MessageStream.Close();
            }
            proxyNumber = 0;
            */
        }
        /*
        byte[] data;
        int recv;
        MemoryStream stream;
        Image pic_image;
        Boolean is_running = false;
        int s_count = 0;
        Thread client_thread;
        Socket sock;
        */
        
        private void button2_Click(object sender, EventArgs e)
        {
            IniParser parser = new IniParser("config.ini");

            serverNameTextBox.Text = parser.GetSetting("MyConfig", "serverName");
            delayCodeTextBox.Text = parser.GetSetting("MyConfig", "delayCode");
            saveUrl = parser.GetSetting("MyConfig", "saveUrl");

            systemMessageBox_AppendText("saveUrl " + saveUrl, Color.AliceBlue);
            /*
            if (is_running == true)
            {
                MessageBox.Show("서버와 이미 접속중입니다.");
                return;
            }
            client_thread = new Thread(new ThreadStart(connect_work));

            client_thread.Start();
            */

        }

        /*
        private void connect_work()
        {

            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000);
            try
            {
                sock.Connect(iep);

            }
            catch
            {
                MessageBox.Show("서버 접속 실패");
                return;
            }

            is_running = true;
            this.Invoke(new MethodInvoker(delegate () { systemMessageBox_AppendText("connect 쓰레드 start" + s_count++.ToString(), Color.AliceBlue); }));

            while (is_running == true)
            {
                data = new byte[1000000]; // 1 Mbytes        
                stream = new MemoryStream();

                // BinaryWriter writer = new BinaryWriter(stream);
                try
                {

                    recv = sock.Receive(data);
                    stream.Write(data, 0, recv);

                    if (recv > 10000)
                    {
                       try
                        {
                            pic_image = Image.FromStream(stream);
                            //this.Invoke(new MethodInvoker(delegate () { pictureBox1.BackgroundImage = pic_image; }));

                        }
                        catch { continue; }

                    }
                }
                catch (SocketException)
                {
                    is_running = false;
                    sock.Shutdown(SocketShutdown.Both);
                    sock.Close();
                    this.Invoke(new MethodInvoker(delegate () { systemMessageBox_AppendText("connect 쓰레드 stop" + s_count--.ToString(),  Color.AliceBlue); }));
                    return;
                }

                //if (recv == 0) continue;
                if (recv < 1)
                {
                    is_running = false;
                    sock.Shutdown(SocketShutdown.Both);
                    sock.Close();
                    this.Invoke(new MethodInvoker(delegate () { systemMessageBox_AppendText("connect 쓰레드 stop" + s_count--.ToString(), Color.AliceBlue); }));
                    return;

                }
            }

            is_running = false;
            sock.Shutdown(SocketShutdown.Both);
            sock.Close();
            this.Invoke(new MethodInvoker(delegate () { systemMessageBox_AppendText("connect 쓰레드 stop" + s_count--.ToString(), Color.AliceBlue); }));
            return;


        }
        */

        private void button4_Click(object sender, EventArgs e)
        {
            loadingTimeForDelayDataGridView();
        }

        private void loadingID_Click(object sender, EventArgs e)
        {
            loadingPop();
        }
    }
}
